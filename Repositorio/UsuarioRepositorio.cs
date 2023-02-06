using RegistroDeTarefasWebApp.Data;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace RegistroDeTarefasWebApp.Repositorio
{
    public class UsuarioRepositorio : Contexto
    {
        // herdando parametros do Contexto para injecao de dependencias, recebendo o IConfiguration
        public UsuarioRepositorio(IConfiguration configuration) : base(configuration) { }

        // função para inserção do modelo de Usuario no banco de dados
        public void Inserir(UsuarioModel modelo)
        {
            _conn.Open();

            // crio uma variavel que vai receber o comando de inserção no banco de dados
            string comandosql = @"INSERT INTO Usuario (Nome, Email, Senha, IdentificadorCargo)
                                    VALUES
                                        (@Nome, @Email, @Senha, @identificadorCargo);";

            // realizo um using na memoria para criar uma variavel recebendo o comando e o parametro do contexto
            using (var cmd = new SqlCommand(comandosql, _conn))
            {
                // parametrizando os valores para a inserção no banco sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@Nome", modelo.Nome);
                cmd.Parameters.AddWithValue("@Email", modelo.Email);
                cmd.Parameters.AddWithValue("@Senha", modelo.Senha);
                cmd.Parameters.AddWithValue("@IdentificadorCargo", modelo.Perfil);
                cmd.ExecuteNonQuery();
            }
            _conn.Close();
        }

        // função para listar todas as Usuarios existentes na tabela, ou que contenha o valor especifico
        public List<UsuarioModel> ListarTodos()
        {
            _conn.Open();
            // criar uma variavel que recebe o comando para gerar a lista de todas as Usuarios cadastradas
            string comandoSql = @"SELECT Id, Nome, Email, IdentificadorCargo
                                    FROM
                                        Usuario";

            // realizar um using, para criar uma variavel recebendo o comando e o parametro de conexão do banco
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                // crio uma variavel reader para percorrer pelo banco gerando uma lista de Usuarios, retornando a lista
                using (var rdr = cmd.ExecuteReader())
                {
                    var Usuarios = new List<UsuarioModel>();
                    while (rdr.Read())
                    {
                        var Usuario = new UsuarioModel();
                        Usuario.Nome = Convert.ToString(rdr["Nome"]);
                        Usuario.Id= Convert.ToInt32(rdr["Id"]);
                        Usuario.Email = Convert.ToString(rdr["Email"]);
                        Usuario.Perfil = (UsuarioModel.PerfilAcesso)Convert.ToInt32(rdr["IdentificadorCargo"]);

                        Usuarios.Add(Usuario);
                    }

                    _conn.Close();

                    return Usuarios;
                }
            }

        }

        public void Atualizar(UsuarioModel modelo)
        {
            _conn.Open();
            // crio uma variavel que recebe o parametro de update do banco, passando o ID do funcionario
            string comandoSql = @"UPDATE Usuario 
                                  SET 
                                    Nome = @nome, Email = @email, Senha = @senha
                                  WHERE 
                                    Id = @id;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser iserido
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                // injeto os parametros para a inserção no banco e executo sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@Nome", modelo.Nome);
                cmd.Parameters.AddWithValue("@Email", modelo.Email);
                cmd.Parameters.AddWithValue("@Id", modelo.Id);
                cmd.Parameters.AddWithValue("@Senha", modelo.Senha);

                // faco uma condição, que se não houve nenhum dado afetado, estoura uma exceção. Informando que não houve alteração
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidacaoException($"Nenhum registro afetado para a Usuario {modelo.Nome}");

                _conn.Close();
            }
        }

        // função para buscar uma Usuario específica passando ID
        public UsuarioModel BuscarEspecifico(int id)
        {
            _conn.Open();

            string commandSql = @"SELECT Id, Nome, Email, IdentificadorCargo, Senha FROM Usuario WHERE Id = @id;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var rdr = cmd.ExecuteReader())
                {
                    var Usuario = new UsuarioModel();
                    while (rdr.Read())
                    {
                        Usuario.Nome = Convert.ToString(rdr["Nome"]);
                        Usuario.Id = Convert.ToInt32(rdr["Id"]);
                        Usuario.Email = Convert.ToString(rdr["Email"]);
                        Usuario.Perfil = (UsuarioModel.PerfilAcesso)Convert.ToInt32(rdr["IdentificadorCargo"]);
                        Usuario.Senha = Convert.ToString(rdr["Senha"]);

                    }
                    _conn.Close();

                    return Usuario;
                }
            }
        }

        // função para buscar usuario por email
        public UsuarioModel BuscarPorEmail(string email)
        {
            _conn.Open();

            string commandSql = @"SELECT Id, Nome, Email, IdentificadorCargo, Senha FROM Usuario WHERE Email = @email;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                using (var rdr = cmd.ExecuteReader())
                {
                    var Usuario = new UsuarioModel();
                    while (rdr.Read())
                    {
                        Usuario.Nome = Convert.ToString(rdr["Nome"]);
                        Usuario.Id = Convert.ToInt32(rdr["Id"]);
                        Usuario.Email = Convert.ToString(rdr["Email"]);
                        Usuario.Perfil = (UsuarioModel.PerfilAcesso)Convert.ToInt32(rdr["IdentificadorCargo"]);
                        Usuario.Senha = Convert.ToString(rdr["Senha"]);

                    }
                    _conn.Close();

                    return Usuario;
                }
            }
        }

        // função para buscar se o cadastro já existe, antes de inserir
        public bool SeExiste(string valor)
        {
            _conn.Open();
            // crio uma variavel que recebe a lista de todos os cadastros existentes no banco
            string commandSql = @"SELECT COUNT (Email) as total FROM Usuario WHERE Email = @Email;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o parametro de contexto
            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Email", valor);
                bool encontrado =  Convert.ToBoolean(cmd.ExecuteScalar());

                _conn.Close();

                return encontrado;
            }
        }

        // recebo o id do funcionario a ser deletado, faço uma varredura se existe este ID
        public bool Deletar(string email)
        {
            // crio uma variavel que recebe o parametro de deleção
            string comandoSql = @"DELETE FROM Usuario 
                                  WHERE 
                                    Email = @email;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de contexto
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                // realizo tratativa, caso não tenha afetado nenhuma Usuario da base, retorna que nenhum registro foi alterado
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidationException($"Nenhum registro afetado para a Usuario informada");
            }
            return true;
        }

        // função buscar nome pelo usuario ID
        public string BuscarNomeUsuarioPeloId(int id)
        {
            _conn.Open();

            string commandSql = @"SELECT Nome FROM Usuario WHERE Id = @id;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                var usuario = "";
                using (var rdr = cmd.ExecuteReader())
                {

                    rdr.Read();
                    usuario = rdr["Nome"] == DBNull.Value ? null : Convert.ToString(rdr["Nome"]);
                    _conn.Close();

                    return usuario;
                }
            }
        }
    }
}
