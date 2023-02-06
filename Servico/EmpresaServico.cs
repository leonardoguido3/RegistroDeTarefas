using Microsoft.AspNetCore.Mvc.ModelBinding;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Repositorio;
using System.Drawing;

namespace RegistroDeTarefasWebApp.Servico
{
    public class EmpresaServico
    {
        // criando uma variavel readonly private para injetar as funções do Empresa Repositorio
        private readonly EmpresaRepositorio _repositorio;

        // criando o construtor que vai receber a injecao de dependencia
        public EmpresaServico(EmpresaRepositorio repositorio) { _repositorio = repositorio; }

        // recebendo o modelo validado, faço a abertura da conexão com o banco, insiro e fecho
        public void Inserir(EmpresaModel modelo)
        {
            // faço uma nova try para executar uma inserção de dados, mas antes verifico se existe algum modelo já cadastrado
            // caso não haja, realizo a inserção e vou para o finally, fechando esta conexão
            var isExist = _repositorio.BuscarCNPJ(modelo.Cnpj);
            if (isExist)
            {
                throw new ValidacaoException($"Ops. {modelo.RazaoSocial} já cadastrada no banco de dados");
            }
            modelo.DataCadastro = DateTime.Now;
            _repositorio.Inserir(modelo);
        }

        // recebo o valor quando possuir e faço uma listagem de todos as empresas
        public List<EmpresaModel> ListarTodos()
        {
            return _repositorio.ListarTodos();
        }

        // buscar o cliente por valor especifico
        public EmpresaModel BuscarEspecifico(int id)
        {
            return _repositorio.BuscarEspecifico(id);
        }

        public string BuscarNomeEmpresaPeloId(int id)
        {
            return _repositorio.BuscarNomeEmpresaPeloId(id);
        }

        public string BuscarNomeEmpresaPeloCnpj(string valor)
        {
            return _repositorio.BuscarNomeEmpresaPeloCnpj(valor);
        }

        public int BuscarIdPeloCnpj(string cnpj)
        {
            var existe = _repositorio.SeExiste(cnpj);
            if (!existe)
                return 0;

            return _repositorio.BuscarIdpeloCnpj(cnpj);
            
        }

        // recebo o modelo de dados a ser atualizado, antes faço uma busca por valor existente
        // encontrado o funcionario, realizo a captura dos dados e envio para update
        public void Atualizar(EmpresaModel modelo)
        {

            EmpresaModel empresa = _repositorio.BuscarEspecifico(modelo.Id);

            if (empresa == null)
                throw new ValidacaoException($"Ops. {modelo.RazaoSocial} não cadastrada em nosso banco de dados");

            empresa.RazaoSocial = modelo.RazaoSocial;

            _repositorio.Atualizar(empresa);
        }

        public EmpresaModel Buscar(int id)
        {
            return _repositorio.BuscarEspecifico(id);
        }

        // recebo o cnpj da empresa a ser deletada, faço uma varredura se existe este ID
        // caso tenha realizo o delete dele
        public bool Deletar(int id)
        {
            var empresa = BuscarEspecifico(id);

            return _repositorio.Deletar(empresa.Id);
        }
    }
}
