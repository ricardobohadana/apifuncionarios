using ApiFuncionarios.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFuncionarios.Infra.Data.Interfaces
{
    public interface IFuncionarioRepository : IBaseRepository<Funcionario>
    {
        Funcionario ConsultarPorCpf(string cpf);
        Funcionario ConsultarPorMatricula(string matricula);
    }
}
