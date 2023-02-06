namespace RegistroDeTarefasWebApp.Servico
{
    public class RelatorioServico
    {
        private readonly TarefaServico _servico;

        public RelatorioServico(TarefaServico servico) { _servico= servico; }
        public Stream GerarRelatorioTarefa()
        {
            var tarefas = _servico.ListarTodos();

            string conteudoCsv = "Empresa,Titulo,Descricao;HoraInicial;HoraFinal,TipoDeTarefa,DEV" + Environment.NewLine;

            foreach (var tarefa in tarefas)
            {
                conteudoCsv += string.Format("{0};{1};{2};{3};{4};{5},{6},{7}"
                    , tarefa.Empresa
                    , tarefa.DescricaoResumo
                    , tarefa.Descricao
                    , tarefa.HorarioInicio
                    , tarefa.HorarioFim
                    , tarefa.TipoTarefa
                    , tarefa.Usuario
                    , Environment.NewLine);
            }

            return GenerateStreamFromString(conteudoCsv);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
