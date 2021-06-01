using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers {
    public class BaseTurnoverController : ControllerBase {

        protected int GetUserId() {
            return int.Parse(GetUserIdString());
        }

        protected string GetUserIdString() {
            return User.Claims.First(x => x.Type == "UserId").Value;
        }
    }
}
