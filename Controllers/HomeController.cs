using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using RegistroDeTarefasWebApp.Filtros;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Servico;
using System.Diagnostics;

namespace RegistroDeTarefasWebApp.Controllers
{
    [PaginaParaUsuarioLogado]
    public class HomeController : Controller
    {
        // variaveis readonly privada injetando as funcionalidades da empresa
        private readonly ILogger<HomeController> _logger;
        private readonly EmpresaServico _empresa;
        private readonly TarefaServico _tarefa;
        private readonly UsuarioServico _usuario;
        private readonly Sessao _sessao;

        // criando construtor que vai receber a injeção de dependencia dos servicos
        public HomeController(ILogger<HomeController> logger, EmpresaServico empresa, TarefaServico tarefa, UsuarioServico usuario, Sessao sessao)
        {
            _logger = logger;
            _empresa = empresa;
            _tarefa = tarefa;
            _usuario = usuario;
            _sessao = sessao;
        }

        // metodo index padrão, chamando a pagina prinicipal, listando as funções para dashboard
        public IActionResult Index()
        {
            // geração de listagens para funções e geração de dashboard
            List<EmpresaModel> empresas = _empresa.ListarTodos();
            List<TarefaModel> tarefas = _tarefa.ListarTodos();
            List<UsuarioModel> usuarios = _usuario.ListarTodos();
            List<TarefaModel> Tarefas = new List<TarefaModel>();

            DateTime horaAtual = DateTime.Now;

            // pegando perfil do usuario logado
            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            // percorrendo as listas, pegando as informações de usuario caso não seja admin, para filtrar os registros a serem inseridos
            if (usuarioLogado.Perfil == UsuarioModel.PerfilAcesso.Admin)
            {
                foreach (var tarefa in tarefas)
                {
                    foreach (var empresa in empresas)
                    {
                        foreach (var usuario in usuarios)
                        {
                            int id = Convert.ToInt32(tarefa.EmpresaId);
                            var encontrada = _empresa.BuscarNomeEmpresaPeloId(id);
                            tarefa.Empresa = encontrada;

                            id = Convert.ToInt32(tarefa.UsuarioId);
                            encontrada = _usuario.BuscarNomeUsuarioPeloId(id);
                            tarefa.Usuario = encontrada;
                            if (tarefa.HorarioFim == null)
                            {
                                Tarefas.Add(tarefa);
                            }
                            else
                            {
                                Tarefas.Add(tarefa);
                            }
                            
                        }
                    }
                }
                return View(Tarefas);
            }
            else
            {
                List<TarefaModel> tarefasPorId = _tarefa.ListarPorId(usuarioLogado.Id);

                foreach (var tarefa in tarefasPorId)
                {
                    int id = Convert.ToInt32(tarefa.EmpresaId);
                    var encontrada = _empresa.BuscarNomeEmpresaPeloId(id);
                    tarefa.Empresa = encontrada;
                    tarefa.Usuario = usuarioLogado.Nome;
                    if (tarefa.HorarioFim == null)
                    {
                        Tarefas.Add(tarefa);
                    }
                    else
                    {
                        Tarefas.Add(tarefa);
                    }
                }
                return View(Tarefas);
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private TimeSpan DiferencaDeHora (DateTime horaInicio, DateTime horaFim)
        {
            var data1 = horaInicio;
            var data2 = horaFim;

            TimeSpan ts = data1 - data2;

            return ts;
        }
    }
}