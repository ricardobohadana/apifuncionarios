//using ApiFuncionarios.Infra.Data.Entities;
//using ApiFuncionarios.Services.Requests;
//using Bogus;
//using Bogus.Extensions.Brazil;
//using FluentAssertions;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace ApiFuncionarios.Tests
//{
//    public class FuncionariosTest
//    {
//        //private HttpClient Initialize()
//        //{
//        //    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//        //    return new TestServer(new WebHostBuilder().UseConfiguration(configuration)).CreateClient();
//        //}
//        private HttpClient Initialize()
//        {
//            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//            var webBuilder = new WebHostBuilder();
//            webBuilder.UseConfiguration(configuration).UseStartup<Startup>();

//            return new TestServer(webBuilder).CreateClient();
//        }

//        [Fact]
//        public async Task<Resultado> PostFuncionario()
//        {
//            var faker = new Faker("pt_BR");

//            var request = new FuncionarioPostRequest()
//            {
//                Nome = faker.Person.FullName,
//                Cpf = faker.Person.Cpf(),
//                Matricula = faker.Random.AlphaNumeric(8),
//                DataAdmissao = DateTime.Now
//            };

//            // conexão com a API
//            var client = Initialize();

//            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
//            var response = await client.PostAsync("api/Funcionarios", content);

//            var resultado = JsonConvert.DeserializeObject<Resultado>(response.Content.ReadAsStringAsync().Result);

//            // verificar resposta
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//            resultado.mensagem.Should().Contain("Funcionário cadastrado com sucesso");

//            return resultado;
//        }

//        [Fact(Skip = "Não implementado")]
//        public void PutFuncionario()
//        {

//        }

//        [Fact(Skip = "Não implementado.")]
//        public void DeleteFuncionario()
//        {

//        }

//        [Fact(Skip = "Não implementado")]
//        public void GetAllFuncionarios()
//        {

//        }

//        [Fact(Skip = "Não implementado")]
//        public void GetFuncionarioById()
//        {

//        }


//    }

//    public class Resultado
//    {
//        public string mensagem { get; set; }

//        public Funcionario funcionario { get; set; }
//    }
//}
using ApiFuncionarios.Infra.Data.Entities;
using ApiFuncionarios.Services.Requests;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiFuncionarios.Tests
{
    /// <summary>
    /// Classe de testes para Funcionario
    /// </summary>
    public class FuncionariosTest
    {
        //Servidor de testes API
        private const string apiUrl = "http://sergioapi-001-site1.etempurl.com/";

        /// <summary>
        /// Teste para o serviço de cadastro de funcionário
        /// </summary>
        [Fact]
        public async Task<ResultadoFuncionario> PostFuncionario()
        {
            var faker = new Faker("pt_BR");

            //criando o funcionário que será cadastrado
            var request = new FuncionarioPostRequest()
            {
                Nome = faker.Person.FullName,
                Cpf = faker.Person.Cpf(),
                Matricula = faker.Random.AlphaNumeric(8),
                DataAdmissao = new DateTime(2022, 04, 29)
            };

            //conectar com a API
            var client = new HttpClient();

            //enviando uma requisição POST para cadastrar o funcionário
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl + "api/Funcionarios", content);

            //capturar o resultado obtido
            var resultado = JsonConvert.DeserializeObject<ResultadoFuncionario>
                (response.Content.ReadAsStringAsync().Result);

            //verificar o resultado
            response.StatusCode.Should().Be(HttpStatusCode.Created); //HTTP 201
            resultado.mensagem.Should().Contain("Funcionário cadastrado com sucesso");

            return resultado;
        }

        /// <summary>
        /// Teste para o serviço de atualização de funcionário
        /// </summary>
        [Fact]
        public async Task<ResultadoFuncionario> PutFuncionario()
        {
            var faker = new Faker("pt_BR");

            //realizar o cadastro de um funcionário
            var cadastro = await PostFuncionario();

            //criando um objeto (request) para ser alterado
            var request = new FuncionarioPutRequest()
            {
                IdFuncionario = cadastro.funcionario.IdFuncionario,
                Nome = faker.Person.FullName,
                Cpf = faker.Person.Cpf(),
                Matricula = faker.Random.AlphaNumeric(8),
                DataAdmissao = new DateTime(2022, 04, 29)
            };

            //conectar com a API
            var client = new HttpClient();

            //enviando uma requisição PUT para atualizar o funcionário
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(apiUrl + "api/Funcionarios", content);

            //capturar o resultado obtido
            var resultado = JsonConvert.DeserializeObject<ResultadoFuncionario>
                (response.Content.ReadAsStringAsync().Result);

            //verificar o resultado
            response.StatusCode.Should().Be(HttpStatusCode.OK); //HTTP 200
            resultado.mensagem.Should().Contain("Funcionário atualizado com sucesso");

            return resultado;
        }

        /// <summary>
        /// Teste para o serviço de exclusão de funcionário
        /// </summary>
        [Fact]
        public async Task<ResultadoFuncionario> DeleteFuncionario()
        {
            //realizar o cadastro de um funcionário
            var cadastro = await PostFuncionario();

            //conectar com a API
            var client = new HttpClient();

            //enviando uma requisição DELETE para excluir o funcionário
            var response = await client.DeleteAsync(apiUrl + "api/Funcionarios/" + cadastro.funcionario.IdFuncionario);

            //capturar o resultado obtido
            var resultado = JsonConvert.DeserializeObject<ResultadoFuncionario>
                (response.Content.ReadAsStringAsync().Result);

            //verificar o resultado
            response.StatusCode.Should().Be(HttpStatusCode.OK); //HTTP 200
            resultado.mensagem.Should().Contain("Funcionário excluído com sucesso");

            return resultado;
        }

        /// <summary>
        /// Teste para o serviço de consulta de funcionários
        /// </summary>
        [Fact]
        public async Task<List<Funcionario>> GetAllFuncionarios()
        {
            //realizar o cadastro de um funcionário
            await PostFuncionario();

            //conectar com a API
            var client = new HttpClient();

            //enviando uma requisição GET para consultar os funcionários
            var response = await client.GetAsync(apiUrl + "api/Funcionarios");

            //capturar o resultado obtido
            var resultado = JsonConvert.DeserializeObject<List<Funcionario>>
                (response.Content.ReadAsStringAsync().Result);

            //verificar o resultado
            response.StatusCode.Should().Be(HttpStatusCode.OK); //HTTP 200
            resultado.Should().NotBeEmpty(); //a consulta não pode retornar vazio

            return resultado;
        }

        /// <summary>
        /// Teste para o serviço de consulta de 1 funcionário
        /// </summary>
        [Fact]
        public async Task<Funcionario> GetFuncionarioById()
        {
            //realizar o cadastro de um funcionário
            var cadastro = await PostFuncionario();

            //conectar com a API
            var client = new HttpClient();

            //enviando uma requisição GET para consultar 1 funcionário através do ID
            var response = await client.GetAsync(apiUrl + "api/Funcionarios/" + cadastro.funcionario.IdFuncionario);

            //capturar o resultado obtido
            var resultado = JsonConvert.DeserializeObject<Funcionario>
                (response.Content.ReadAsStringAsync().Result);

            //verificar o resultado
            response.StatusCode.Should().Be(HttpStatusCode.OK); //HTTP 200

            //comparando se os dados obtidos do funcionário são iguais ao que foi cadastrado
            resultado.IdFuncionario.Should().Be(cadastro.funcionario.IdFuncionario);
            resultado.Nome.Should().Be(cadastro.funcionario.Nome);
            resultado.Cpf.Should().Be(cadastro.funcionario.Cpf);
            resultado.Matricula.Should().Be(cadastro.funcionario.Matricula);
            resultado.DataAdmissao.Should().Be(cadastro.funcionario.DataAdmissao);

            return resultado;
        }
    }

    /// <summary>
    /// Classe para capturar o resultado obtido da API
    /// </summary>
    public class ResultadoFuncionario
    {
        public string mensagem { get; set; }
        public Funcionario funcionario { get; set; }
    }
}











