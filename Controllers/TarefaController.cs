using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using Nobisoft.Core.SnowflakeId;
using RegistroDeTarefasWebApp.Filtros;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Servico;
using System;
using System.Drawing;
using System.Runtime.Intrinsics.Arm;

namespace RegistroDeTarefasWebApp.Controllers
{
    [PaginaParaUsuarioLogado]
    public class TarefaController : Controller
    {
        // variaveis privates injetando funcionalidades
        private readonly TarefaServico _servico;
        private readonly EmpresaServico _empresa;
        private readonly Sessao _sessao;

        // construtor
        public TarefaController(TarefaServico servico, Sessao sessao, EmpresaServico empresa)
        {
            _servico = servico;
            _sessao = sessao;
            _empresa = empresa;
        }

        public IActionResult Index()
        {
            // listagem index de tarefas, pegando caso dev o id do usuario e entregando somente suas tarefas

            List<TarefaModel> tarefa;

            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            if (usuarioLogado.Perfil == UsuarioModel.PerfilAcesso.Admin)
            {

                tarefa = _servico.ListarTodos();

                var tarefas = new List<TarefaModel>();

                foreach (var item in tarefa)
                {
                    while (true)
                    {
                        int id = Convert.ToInt32(item.EmpresaId);
                        var tarefaId = new TarefaModel();
                        string encontrada = _empresa.BuscarNomeEmpresaPeloId(id);

                        tarefaId.Empresa = encontrada;
                     
                        tarefas.Add(tarefaId);
                    }
                }

                return View(tarefas);
            }
            else
            {
                tarefa = _servico.ListarPorId(usuarioLogado.Id);

                foreach (TarefaModel item in tarefa)
                {
                    item.Usuario = usuarioLogado.Nome;
                    int id = Convert.ToInt32(item.EmpresaId);
                    item.Empresa = _empresa.BuscarNomeEmpresaPeloId(id);
                }

                return View(tarefa);
            }

        }

        //Funcao padrao chamando a pagina de criação de tarefas
        public IActionResult Criar()
        {
            return View();
        }

        //Função POST de criação de tarefas, pegamos todos os dados, realizamos uma tratativa, se tudo estiver correto, é enviado para cadastro
        [HttpPost]
        public IActionResult Criar(TarefaModel tarefa)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                tarefa.UsuarioId = usuarioLogado.Id;

                _servico.Inserir(tarefa);

                TempData["MensagemSucesso"] = $"{tarefa.DescricaoResumo} cadastrada com sucesso!";
                return RedirectToAction("Index");

            }
            catch (ValidacaoException erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar o cadastro, {erro.Message}";
                return RedirectToAction("Index");
            }

        }

        public IActionResult Editar(int id)
        {
            TarefaModel empresa = _servico.BuscarEspecifico(id);
            return View(empresa);
        }

        //Metodo HTTPPOST de atualização do equipamento
        [HttpPost]
        public IActionResult Atualizar(TarefaModel modelo)
        {
            //Realizando a validação, com tratativa
            if (ModelState.IsValid)
            {
                _servico.Atualizar(modelo);
                //Caso houver sucesso, mensagem será enviada
                TempData["MensagemSucesso"] = "Tarefa alterada com sucesso";
                return RedirectToAction("Index");
            }
            //Caso não seja valido usaremos o try catch para fazer as tentativas
            try
            {
                _servico.Atualizar(modelo);
                //Caso houver sucesso, mensagem será enviada
                TempData["MensagemSucesso"] = "Tarefa alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Houve um erro na atualização da tarefa";
                return RedirectToAction("Index");
            }
        }

        //Metodo de confirmação do delete
        public IActionResult ApagarConfirmacao(int id)
        {
            TarefaModel tarefa = _servico.BuscarEspecifico(id);
            return View(tarefa);
        }

        //Metodo de apagar um equipamento
        public IActionResult Apagar(int id)
        {
            // valido com booleano se foi deletado

            bool apagado = _servico.Deletar(id);
            if (apagado)
            {
                TempData["MensagemSucesso"] = "Tarefa apagada com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Ops, não foi possível apagar a tarefa!";
            }
            return RedirectToAction("Index");

        }

    }
}
