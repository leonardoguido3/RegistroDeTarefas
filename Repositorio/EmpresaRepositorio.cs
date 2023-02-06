using RegistroDeTarefasWebApp.Data;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace RegistroDeTarefasWebApp.Repositorio
{
    // herdo a classe de contexto na minha Empresa Repositorio
    public class EmpresaRepositorio : Contexto
    {
        // herdando parametros do Contexto para injecao de dependencias, recebendo o IConfiguration
        public EmpresaRepositorio(IConfiguration configuration) : base(configuration) { }

        // função para inserção do modelo de empresa no banco de dados
        public void Inserir(EmpresaModel modelo)
        {
            // abro a conexão com o banco
            _conn.Open();

            // crio uma variavel que vai receber o comando de inserção no banco de dados
            string comandosql = @"INSERT INTO Empresa (Cnpj, RazaoSocial, DataCadastro)
                                    VALUES
                                        (@Cnpj, @RazaoSocial, @DataCadastro);";

            // realizo um using na memoria para criar uma variavel recebendo o comando e o parametro do contexto
            using (var cmd = new SqlCommand(comandosql, _conn))
            {
                // parametrizando os valores para a inserção no banco sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@Cnpj", modelo.Cnpj);
                cmd.Parameters.AddWithValue("@RazaoSocial", modelo.RazaoSocial);
                cmd.Parameters.AddWithValue("@DataCadastro", modelo.DataCadastro);
                cmd.ExecuteNonQuery();
            }
            // fecho a conexão
            _conn.Close();
        }

        // função para listar todas as empresas existentes na tabela, ou que contenha o valor especifico
        public List<EmpresaModel> ListarTodos()
        {
            // abro conexão
            _conn.Open();

            // criar uma variavel que recebe o comando para gerar a lista de todas as empresas cadastradas
            string comandoSql = @"SELECT Cnpj, RazaoSocial, DataCadastro, Id
                                    FROM
                                        Empresa";

            // realizar um using, para criar uma variavel recebendo o comando e o parametro de conexão do banco
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                // crio uma variavel reader para percorrer pelo banco gerando uma lista de empresas, retornando a lista
                using (var rdr = cmd.ExecuteReader())
                {
                    var empresas = new List<EmpresaModel>();
                    while (rdr.Read())
                    {
                        var empresa = new EmpresaModel();
                        empresa.Cnpj = Convert.ToString(rdr["Cnpj"]);
                        empresa.RazaoSocial = Convert.ToString(rdr["RazaoSocial"]);
                        empresa.DataCadastro = Convert.ToDateTime(rdr["DataCadastro"]);
                        empresa.Id = Convert.ToInt32(rdr["id"]);

                        empresas.Add(empresa);
                    }
                    _conn.Close();

                    return empresas;
                }
            }

        }

        public void Atualizar(EmpresaModel modelo)
        {
            _conn.Open();

            // crio uma variavel que recebe o parametro de update do banco, passando o ID do funcionario
            string comandoSql = @"UPDATE Empresa 
                                  SET 
                                    RazaoSocial = @razaoSocial, Cnpj = @cnpj, DataCadastro = @dataCadastro
                                  WHERE 
                                    Id = @id;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser iserido
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                // injeto os parametros para a inserção no banco e executo sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@Cnpj", modelo.Cnpj);
                cmd.Parameters.AddWithValue("@RazaoSocial", modelo.RazaoSocial);
                cmd.Parameters.AddWithValue("@DataCadastro", modelo.DataCadastro);
                cmd.Parameters.AddWithValue("@Id", modelo.Id);

                // faco uma condição, que se não houve nenhum dado afetado, estoura uma exceção. Informando que não houve alteração
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidacaoException($"Nenhum registro afetado para a empresa {modelo.RazaoSocial}");
            }
            _conn.Close();
        }

        // função para buscar uma empresa específica passando CNPJ
        public EmpresaModel BuscarEspecifico(int id)
        {
            _conn.Open();

            string commandSql = @"SELECT Cnpj, RazaoSocial, DataCadastro, Id FROM Empresa WHERE Id = @id;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var rdr = cmd.ExecuteReader())
                {
                    var empresa = new EmpresaModel();
                    while (rdr.Read())
                    {
                        empresa.Cnpj = Convert.ToString(rdr["Cnpj"]);
                        empresa.RazaoSocial = Convert.ToString(rdr["RazaoSocial"]);
                        empresa.DataCadastro = Convert.ToDateTime(rdr["DataCadastro"]);
                        empresa.Id = Convert.ToInt32(rdr["id"]);

                    }

                    _conn.Close();

                    return empresa;
                }
            }
        }

        // função para buscar um CNPJ específico passando ID
        public string BuscarNomeEmpresaPeloId(int id)
        {
            _conn.Open();

            string commandSql = @"SELECT RazaoSocial FROM Empresa WHERE Id = @id;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                var empresa = "";
                using (var rdr = cmd.ExecuteReader())
                {

                    rdr.Read();
                    empresa = rdr["RazaoSocial"] == DBNull.Value ? null : Convert.ToString(rdr["RazaoSocial"]);
                    _conn.Close();

                    return empresa;
                }
            }
        }

        // função para buscar um ID específica passando CNPJ
        public int BuscarIdpeloCnpj(string cnpj)
        {
            _conn.Open();

            string commandSql = @"SELECT Id FROM Empresa WHERE Cnpj = @cnpj;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Cnpj", cnpj);

                int empresa = 0;
                using (var rdr = cmd.ExecuteReader())
                {

                    rdr.Read();
                    empresa = Convert.ToInt32(rdr["Id"]);
                    _conn.Close();

                    return empresa;
                }
            }
        }

        // função para buscar uma Razao Social passando CNPJ
        public string BuscarNomeEmpresaPeloCnpj(string cnpj)
        {
            if (String.IsNullOrWhiteSpace(cnpj))
            {
                return null;
            }
            _conn.Open();

            string commandSql = @"SELECT RazaoSocial FROM Empresa WHERE Cnpj = @cnpj;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Cnpj", cnpj);

                var empresa = "";
                using (var rdr = cmd.ExecuteReader())
                {

                    rdr.Read();
                    empresa = Convert.ToString(rdr["RazaoSocial"]);
                    _conn.Close();

                    return empresa;
                }
            }
        }

        // função para buscar uma empresa específica passando CNPJ
        public bool BuscarCNPJ(string valor)
        {
            _conn.Open();

            // crio uma variavel que recebe a lista de todos os cadastros existentes no banco
            string commandSql = @"SELECT COUNT (Cnpj) as total FROM Empresa WHERE Cnpj = @cnpj;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o parametro de contexto
            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Cnpj", valor);
                bool contem = Convert.ToBoolean(cmd.ExecuteScalar());

                _conn.Close();

                return contem;
            }

        }

        // função para buscar se o cadastro já existe, antes de inserir
        public bool SeExiste(string valor)
        {
            _conn.Open();

            // crio uma variavel que recebe a lista de todos os cadastros existentes no banco
            string commandSql = @"SELECT COUNT (Cnpj) as total FROM Empresa WHERE Cnpj = @cnpj;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o parametro de contexto
            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Cnpj", valor);

                bool contem = Convert.ToBoolean(cmd.ExecuteScalar());

                _conn.Close();

                return contem;
            }
        }

        // recebo o id do funcionario a ser deletado, faço uma varredura se existe este ID
        public bool Deletar(int id)
        {
            _conn.Open();

            // crio uma variavel que recebe o parametro de deleção
            string comandoSql = @"DELETE FROM Empresa 
                                  WHERE 
                                    Id = @id;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de contexto
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                // realizo tratativa, caso não tenha afetado nenhuma empresa da base, retorna que nenhum registro foi alterado
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidationException($"Nenhum registro afetado para a empresa informada");
            }
            _conn.Close();

            return true;
        }
    }
}
