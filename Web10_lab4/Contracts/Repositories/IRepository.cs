using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories {
    public interface IRepository<TEntity, TInputEntity, TKey> {
        public IEnumerable<TEntity> GetAll();
        public TEntity Get(TKey id);
        public TKey Add(TInputEntity inputEntity);
        public void Update(TKey id, TInputEntity inputEntity);
        public void Remove(TKey id);
    }
}
