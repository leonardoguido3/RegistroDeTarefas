using RegistroDeTarefasWebApp.Repositorio;
using RegistroDeTarefasWebApp.Servico;

namespace RegistroDeTarefasWebApp.Helpers
{
    public class Dependencia
    {
        // função static de relacionamento pegando parametros do IServiceCollection
        public static void Registro(IServiceCollection servico)
        {
            // servicos
            servico.AddScoped<EmpresaServico, EmpresaServico>();
            servico.AddScoped<UsuarioServico, UsuarioServico>();
            servico.AddScoped<TarefaServico, TarefaServico>();

            // sessao
            servico.AddScoped<Sessao, Sessao>();

            // repositorios
            servico.AddScoped<EmpresaRepositorio, EmpresaRepositorio>();
            servico.AddScoped<UsuarioRepositorio, UsuarioRepositorio>();
            servico.AddScoped<TarefaRepositorio, TarefaRepositorio>();

            // email
            servico.AddScoped<Email, Email>();

        }
    }
}
