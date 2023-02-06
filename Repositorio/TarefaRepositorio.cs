using RegistroDeTarefasWebApp.Data;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace RegistroDeTarefasWebApp.Repositorio
{
    public class TarefaRepositorio : Contexto
    {
        // herdando parametros do Contexto para injecao de dependencias, recebendo o IConfiguration
        public TarefaRepositorio(IConfiguration configuration) : base(configuration) { }

        // função para inserção do modelo de Usuario no banco de dados
        public void Inserir(TarefaModel modelo)
        {
            _conn.Open();

            // crio uma variavel que vai receber o comando de inserção no banco de dados
            string comandosql = @"INSERT INTO Tarefa (DescricaoResumo, HorarioInicio, TipoDeTarefa, UsuarioId, EmpresaId)
                                    VALUES
                                        (@DescricaoResumo, @HorarioInicio, @TipoDeTarefa, @UsuarioId, @EmpresaId);";

            // realizo um using na memoria para criar uma variavel recebendo o comando e o parametro do contexto
            using (var cmd = new SqlCommand(comandosql, _conn))
            {
                // parametrizando os valores para a inserção no banco sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@DescricaoResumo", modelo.DescricaoResumo);
                cmd.Parameters.AddWithValue("@HorarioInicio", modelo.HorarioInicio);
                cmd.Parameters.AddWithValue("@TipoDeTarefa", modelo.TipoTarefa);
                cmd.Parameters.AddWithValue("@UsuarioId", modelo.UsuarioId);
                cmd.Parameters.AddWithValue("@EmpresaId", modelo.EmpresaId);
                cmd.ExecuteNonQuery();
            }
            _conn.Close();
        }

        // função para listar todas as tarefas existentes na tabela, ou que contenha o valor especifico
        public List<TarefaModel> ListarTodos()
        {
            _conn.Open();

            // criar uma variavel que recebe o comando para gerar a lista de todas as tarefas cadastradas
            string comandoSql = @"SELECT DescricaoResumo, HorarioInicio, HorarioFim, Id, Descricao, TipoDeTarefa, UsuarioId, EmpresaId
                                    FROM
                                        Tarefa";

            // realizar um using, para criar uma variavel recebendo o comando e o parametro de conexão do banco
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                // crio uma variavel reader para percorrer pelo banco gerando uma lista de tarefas, retornando a lista
                using (var rdr = cmd.ExecuteReader())
                {
                    var tarefas = new List<TarefaModel>();
                    while (rdr.Read())
                    {
                        var tarefa = new TarefaModel();
                        tarefa.DescricaoResumo = Convert.ToString(rdr["DescricaoResumo"]);
                        tarefa.HorarioInicio = Convert.ToDateTime(rdr["HorarioInicio"]);
                        tarefa.HorarioFim = rdr["HorarioFim"] == DBNull.Value ? null : Convert.ToDateTime(rdr["HorarioFim"]);
                        tarefa.Id = Convert.ToInt32(rdr["Id"]);
                        tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        tarefa.TipoTarefa = (TarefaModel.Tipo)Convert.ToInt32(rdr["TipoDeTarefa"]);
                        tarefa.UsuarioId = Convert.ToInt32(rdr["UsuarioId"]);
                        tarefa.EmpresaId = Convert.ToInt32(rdr["EmpresaId"]);


                        tarefas.Add(tarefa);
                    }
                    _conn.Close();

                    return tarefas;
                }
            }

        }

        // listar tarefas por ID
        public List<TarefaModel> ListarPorId(int id)
        {
            _conn.Open();

            string commandSql = @"SELECT DescricaoResumo, HorarioInicio, HorarioFim, Id, Descricao, TipoDeTarefa, UsuarioId, EmpresaId 
                                    FROM Tarefa 
                                    WHERE UsuarioId = @usuarioId;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@UsuarioId", id);

                using (var rdr = cmd.ExecuteReader())
                {
                    var tarefas = new List<TarefaModel>();
                    while (rdr.Read())
                    {
                        var tarefa = new TarefaModel();
                        tarefa.DescricaoResumo = Convert.ToString(rdr["DescricaoResumo"]);
                        tarefa.HorarioInicio = Convert.ToDateTime(rdr["HorarioInicio"]);
                        tarefa.HorarioFim = rdr["HorarioFim"] == DBNull.Value ? null : Convert.ToDateTime(rdr["HorarioFim"]);
                        tarefa.Id = Convert.ToInt32(rdr["Id"]);
                        tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        tarefa.TipoTarefa = (TarefaModel.Tipo)Convert.ToInt32(rdr["TipoDeTarefa"]);
                        tarefa.UsuarioId = Convert.ToInt32(rdr["UsuarioId"]);
                        tarefa.EmpresaId = Convert.ToInt32(rdr["EmpresaId"]);


                        tarefas.Add(tarefa);
                    }
                    _conn.Close();

                    return tarefas;
                }
            }
        }

        // função para buscar uma empresa específica passando CNPJ
        public TarefaModel BuscarEspecifico(int id)
        {
            _conn.Open();

            string commandSql = @"SELECT DescricaoResumo, HorarioInicio, HorarioFim, Id, Descricao, TipoDeTarefa, UsuarioId, EmpresaId 
                                    FROM Tarefa 
                                    WHERE Id = @id;";

            using (var cmd = new SqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var rdr = cmd.ExecuteReader())
                {
                    var tarefa = new TarefaModel();
                    while (rdr.Read())
                    {
                        tarefa.DescricaoResumo = Convert.ToString(rdr["DescricaoResumo"]);
                        tarefa.HorarioInicio = Convert.ToDateTime(rdr["HorarioInicio"]);
                        tarefa.HorarioFim = rdr["HorarioFim"] == DBNull.Value ? null : Convert.ToDateTime(rdr["HorarioFim"]);
                        tarefa.Id = Convert.ToInt32(rdr["Id"]);
                        tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        tarefa.TipoTarefa = (TarefaModel.Tipo)Convert.ToInt32(rdr["TipoDeTarefa"]);
                        tarefa.UsuarioId = Convert.ToInt32(rdr["UsuarioId"]);
                        tarefa.EmpresaId = Convert.ToInt32(rdr["EmpresaId"]);

                    }

                    _conn.Close();

                    return tarefa;
                }
            }
        }

        public void Atualizar(TarefaModel modelo)
        {
            _conn.Open();

            // crio uma variavel que recebe o parametro de update do banco, passando o ID da tarefa
            string comandoSql = @"UPDATE Tarefa 
                                  SET 
                                    DescricaoResumo = @descricaoResumo, HorarioInicio = @horarioInicio, HorarioFim = @horarioFim, Descricao = @descricao, TipoDeTarefa = @tipoDeTarefa, UsuarioId = @usuarioId, EmpresaId = @empresaId
                                  WHERE 
                                    Id = @id;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser iserido
            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                // injeto os parametros para a inserção no banco e executo sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@DescricaoResumo", modelo.DescricaoResumo);
                cmd.Parameters.AddWithValue("@HorarioInicio", modelo.HorarioInicio);
                cmd.Parameters.AddWithValue("@HorarioFim", modelo.HorarioFim);
                cmd.Parameters.AddWithValue("@Descricao", modelo.Descricao);
                cmd.Parameters.AddWithValue("@TipoDeTarefa", modelo.TipoTarefa);
                cmd.Parameters.AddWithValue("@UsuarioId", modelo.UsuarioId);
                cmd.Parameters.AddWithValue("@EmpresaId", modelo.EmpresaId);
                cmd.Parameters.AddWithValue("@Id", modelo.Id);

                // faco uma condição, que se não houve nenhum dado afetado, estoura uma exceção. Informando que não houve alteração
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidacaoException($"Nenhum registro afetado para a empresa {modelo.DescricaoResumo}");
            }
            _conn.Close();
        }

        // recebo o id da tarefa a ser deletado, faço uma varredura se existe este ID
        public bool Deletar(int id)
        {
            _conn.Open();

            // crio uma variavel que recebe o parametro de deleção
            string comandoSql = @"DELETE FROM Tarefa 
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
