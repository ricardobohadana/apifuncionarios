using ApiFuncionarios.Infra.Data.Contexts;
using ApiFuncionarios.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFuncionarios.Infra.Data.Repositories
{
    public class BaseRepository<TEntity>: IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly SqlServerContext _sqlServerContext;

        public BaseRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext;
        }

        public void Inserir(TEntity obj)
        {
            _sqlServerContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _sqlServerContext.SaveChanges();
        }

        public void Alterar(TEntity obj)
        {
            _sqlServerContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _sqlServerContext.SaveChanges();
        }

        public void Excluir(TEntity obj)
        {
            _sqlServerContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _sqlServerContext.SaveChanges();
        }

        public List<TEntity> Consultar()
        {
            return _sqlServerContext.Set<TEntity>().ToList();
        }

        public TEntity ObterPorId(Guid id)
        {
            return _sqlServerContext.Set<TEntity>().Find(id);
        }
    }
}
