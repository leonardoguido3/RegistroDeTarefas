using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Repositorio;

namespace RegistroDeTarefasWebApp.Servico
{
    public class TarefaServico
    {
        // criando uma variavel readonly private para injetar as funções do Empresa Repositorio
        private readonly TarefaRepositorio _repositorio;
        private readonly EmpresaServico _empresa;
        private readonly Sessao _sessao;

        // criando o construtor que vai receber a injecao de dependencia
        public TarefaServico(TarefaRepositorio repositorio, EmpresaServico empresa, Sessao sessao)
        {
            _repositorio = repositorio;
            _empresa = empresa;
            _sessao = sessao;
        }

        // recebo o valor quando possuir e faço uma listagem de todos as tarefas
        public List<TarefaModel> ListarTodos()
        {
            return _repositorio.ListarTodos();
        }

        public List<TarefaModel> ListarPorId(int id)
        {
            return _repositorio.ListarPorId(id);
        }

        public TarefaModel BuscarEspecifico(int id)
        {
            return _repositorio.BuscarEspecifico(id);
        }

        public void Inserir(TarefaModel modelo)
        {

            var empresaEncontrada = _empresa.BuscarIdPeloCnpj(modelo.CnpjEmpresa);
            if (empresaEncontrada == 0)
                throw new ValidacaoException("Empresa não cadastrada no banco de dados.");

            modelo.EmpresaId = empresaEncontrada;

            _repositorio.Inserir(modelo);

        }

        public void Atualizar(TarefaModel modelo)
        {

            TarefaModel tarefa = _repositorio.BuscarEspecifico(modelo.Id);

            if (tarefa == null)
                throw new ValidacaoException($"Ops. {modelo.Descricao} não cadastrada em nosso banco de dados");

            tarefa.HorarioFim = modelo.HorarioFim;
            tarefa.Descricao = modelo.Descricao;

            _repositorio.Atualizar(tarefa);
        }

        // recebo o id da tarefa a ser deletada, faço uma varredura se existe este ID
        // caso tenha realizo o delete dele
        public bool Deletar(int id)
        {
            var tarefa = BuscarEspecifico(id);

            return  _repositorio.Deletar(tarefa.Id);

        }

    }
}
