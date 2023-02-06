using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistroDeTarefasWebApp.Filtros;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Repositorio;
using RegistroDeTarefasWebApp.Servico;

namespace RegistroDeTarefasWebApp.Controllers
{
    public class LoginController : Controller
    {
        //Injeção de dependencias do Repositorio Usuario, Sessao e Email
        private readonly UsuarioServico _repositorio;
        private readonly Sessao _sessao;
        private readonly Email _email;

        //Construtor do Usuario, Sessao e Email
        public LoginController(UsuarioServico usuarioServico, Sessao sessao, Email email)
        {
            _repositorio = usuarioServico;
            _sessao = sessao;
            _email = email;
        }
        //Metodo index padrão, chamando a pagina prinicipal, mas antes realizando a verificação se o usuario tá logado
        public IActionResult Index()
        {
          if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        //Metodo de logar no sistema, realizando as tratativas e gerando a sessao do usuario
        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _repositorio.BuscarPorEmail(loginModel.Login);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);

                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MensagemErro"] = "Ops, senha inválida!";
                    }
                    TempData["MensagemErro"] = "Ops, usuário e/ou senha inválidos!";
                }
                return View("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, houve um erro ao realizar o login!";
                return RedirectToAction("Index");
            }
        }

        //Metodo index redefinir senha
        public IActionResult RedefinirSenha()
        {
            return View();
        }

        //Metodo para sair, removendo assim a sessao do usuario e enviando ele para tela de login
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        //Metodo POST para envio de email, onde é realizado as tratativas dos valores, e se tudo estiver correto, é enviado um email com a nova senha
        [HttpPost]
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _repositorio.BuscarPorEmail(redefinirSenhaModel.Email);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Olá, este é um email de recuperação de senha. Sua nova senha é: {novaSenha}";

                        _repositorio.Atualizar(usuario);

                        bool emailEnviado = _email.Enviar(usuario.Email, "Registro de Tarefas | Nova Senha", mensagem);
                        if (emailEnviado)
                        {
                            TempData["MensagemSucesso"] = $"Enviamos sua nova senha para: {redefinirSenhaModel.Email}!";
                        }

                        return RedirectToAction("Index", "Login");
                    }
                    TempData["MensagemErro"] = "Ops, não foi possivel redefinir sua senha!";
                }
                return View("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos redefinir sua senha!";
                return RedirectToAction("Index");
            }
        }


    }
}
