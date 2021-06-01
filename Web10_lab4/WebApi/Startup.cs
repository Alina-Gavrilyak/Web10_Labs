using AutoMapper;
using Contracts;
using Contracts.Repositories;
using DataAccessContracts.Entities;
using DataAccessServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Mapper;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Models;

namespace Web10_Lab2 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<TurnoverDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<TurnoverUser, TurnoverRole>()
                .AddEntityFrameworkStores<TurnoverDbContext>()
                .AddDefaultTokenProviders();

            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AccountMappingProfile());
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new RequestDeliveryMappingProfile());
                mc.AddProfile(new ShopMappingProfile());
                mc.AddProfile(new WarehouseMappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddOptions<EmailServiceSettings>().BindConfiguration("EmailServiceSettings");
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRequestDeliveryRepository, RequestDeliveryRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();


            services.AddOptions<AuthSettings>().BindConfiguration("AuthSettings");

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });

            // JWT Authentication
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web10_Lab4", Version = "v1" });

                var securitySchema = new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { securitySchema, new[] { "Bearer" } }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            InitDatabase(app.ApplicationServices);

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web10_Lab4 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        private async void InitDatabase(IServiceProvider serviceProvider) {
            IServiceScope serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

            TurnoverDbContext context = serviceScope.ServiceProvider.GetService<TurnoverDbContext>();
            context.Database.Migrate();

            RoleManager<TurnoverRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<TurnoverRole>>();
            UserManager<TurnoverUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<TurnoverUser>>();

            foreach (var roleName in RolesHelper.Roles.Keys) {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist && !(await roleManager.CreateAsync(new TurnoverRole() { Name = roleName, Description = RolesHelper.Roles[roleName] })).Succeeded)
                    throw new Exception($"Cannot create role {roleName}.");
            }

            TurnoverUser adminUser = new TurnoverUser {
                UserName = "AdminUser",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "admin@rs.com",
                EmailConfirmed = true
            };

            TurnoverUser existingAdminUser = await userManager.FindByNameAsync(adminUser.UserName);

            if (existingAdminUser == null) {
                if ((await userManager.CreateAsync(adminUser, adminUser.UserName)).Succeeded) {
                    foreach (var roleName in RolesHelper.Roles.Keys)
                        await userManager.AddToRoleAsync(adminUser, roleName);
                } else
                    throw new Exception("Cannot create admin user.");
            }

        }
    }
}
