using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Repositorio;

namespace RegistroDeTarefasWebApp.Servico
{
    public class UsuarioServico
    {
        // criando uma variavel readonly private para injetar as funções do Usuario Repositorio
        private readonly UsuarioRepositorio _repositorio;

        // criando o construtor que vai receber a injecao de dependencia
        public UsuarioServico(UsuarioRepositorio repositorio) { _repositorio = repositorio; }

        // recebendo o modelo validado, faço a abertura da conexão com o banco, insiro e fecho
        public void Inserir(UsuarioModel modelo)
        {
            // faço uma nova try para executar uma inserção de dados, mas antes verifico se existe algum modelo já cadastrado
            // caso não haja, realizo a inserção e vou para o finally, fechando esta conexão

            var isExist = _repositorio.SeExiste(modelo.Email);
            if (isExist)
            {
                throw new ValidacaoException($"Ops. {modelo.Nome} já cadastrada no banco de dados");
            }
            modelo.SetSenhaHash();

            _repositorio.Inserir(modelo);

        }

        // recebo o valor quando possuir e faço uma listagem de todos as usuarios
        public List<UsuarioModel> ListarTodos()
        {
            return _repositorio.ListarTodos();
        }

        // buscar o cliente por valor especifico
        public UsuarioModel BuscarEspecifico(int valor)
        {
            return _repositorio.BuscarEspecifico(valor);
        }

        // recebo o modelo de dados a ser atualizado, antes faço uma busca por valor existente
        // encontrado o funcionario, realizo a captura dos dados e envio para update
        public void Atualizar(UsuarioModel modelo)
        {

            UsuarioModel usuario = _repositorio.BuscarEspecifico(modelo.Id);


            if (usuario == null)
                throw new ValidacaoException($"Ops. {modelo.Nome} não cadastrada em nosso banco de dados");

            usuario.Nome = modelo.Nome;
            usuario.Email = modelo.Email;

            bool contem = VerificarSenha(modelo.Senha);

            if (contem)
            {
                usuario.Senha = modelo.Senha;
                _repositorio.Atualizar(usuario);         
            }
            else
            {
                _repositorio.Atualizar(usuario);
            }
        }

        public UsuarioModel Buscar(int valor)
        {
            return _repositorio.BuscarEspecifico(valor);
        }

        // recebo o id da Usuario a ser deletada, faço uma varredura se existe este ID
        // caso tenha realizo o delete dele
        public bool Deletar(string email)
        {
            try
            {
                _repositorio.AbrirConexao();
                var Usuario = _repositorio.SeExiste(email);

                if (!Usuario)
                    throw new ValidacaoException($"Ops. Não foi encontrado a Usuario informada");

                var apagado = _repositorio.Deletar(email);

                if (!apagado)
                {
                    return false;
                }
                return true;
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        public UsuarioModel BuscarPorEmail(string email)
        {
            return _repositorio.BuscarPorEmail(email);
        }

        private bool VerificarSenha(string senha)
        {
            if (senha is null)
            {
                return false;
            }
            return true;
        }

        public string BuscarNomeUsuarioPeloId(int id)
        {
            return _repositorio.BuscarNomeUsuarioPeloId(id);
        }
    }
}
