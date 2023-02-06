using Microsoft.AspNetCore.Mvc;
using RegistroDeTarefasWebApp.Filtros;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Servico;

namespace RegistroDeTarefasWebApp.Controllers
{
    //Usuario precisa estar logado e ser administrador do sistema para ter acesso a esta funcionalidade
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        // variavel readonly privada injetando as funcionalidades da empresa
        private readonly UsuarioServico _servico;

        // criando construtor que vai receber a injeção de dependencia dos servicos
        public UsuarioController(UsuarioServico servico) { _servico = servico; }

        //Função padrão chamando a página index para a pagina de usuario, buscando todos cadastrados
        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _servico.ListarTodos();
            return View(usuarios);
        }

        //Funcao padrao chamando a pagina de criação de usuario
        public IActionResult Criar()
        {
            return View();
        }

        //Função POST de criação de usuario, pegamos todos os dados, realizamos uma tratativa, se tudo estiver correto, é enviado para cadastro
        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            _servico.Inserir(usuario);
            TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Index");
            try
            {
                if (ModelState.IsValid)
                {
                    _servico.Inserir(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(usuario);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos realizar o cadastro!";
                return RedirectToAction("Index");
            }
        }

        //Funcao edição de usuario, passamos o ID do usuario para a função de edição
        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _servico.BuscarEspecifico(id);
            return View(usuario);
        }

        //Função POST para alteração de usuario propriamente dita. Aqui pegamos os dados, fazemos uma tratativa e chamamos o metodo de alteração
        [HttpPost]
        public IActionResult Atualizar(UsuarioModel usuario)
        {
            _servico.Atualizar(usuario);
            TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";
            return RedirectToAction("Index");

            try
            {
                if (ModelState.IsValid)
                {
                    _servico.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", usuario);

            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos alterar o usuario!";
                return RedirectToAction("Index");
            }

        }

        //Metodo que confirma a deleção do usuario, não é a deleção em si, apenas confirma e pega o ID do usuario que sera deletado
        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _servico.Buscar(id);
            return View(usuario);
        }

        //Metodo para deletar o usuario propriamente dito, fazemos uma tratativa para verificar se tudo está correto, pegamos o ID capturado e é realizado a remoção
        public IActionResult Apagar(string email)
        {

            bool apagado = _servico.Deletar(email);
            if (apagado)
            {
                TempData["MensagemSucesso"] = "Usuário apagado com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Ops, não foi possível apagar o usuário!";
            }
            return RedirectToAction("Index");
        }
    }
}
