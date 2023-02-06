using Microsoft.AspNetCore.Mvc;
using RegistroDeTarefasWebApp.Filtros;
using RegistroDeTarefasWebApp.Helpers;
using RegistroDeTarefasWebApp.Models;
using RegistroDeTarefasWebApp.Servico;

namespace RegistroDeTarefasWebApp.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class EmpresaController : Controller
    {
        // variavel readonly privada injetando as funcionalidades da empresa
        private readonly EmpresaServico _servico;

        // criando construtor que vai receber a injeção de dependencia dos servicos
        public EmpresaController(EmpresaServico servico) { _servico = servico; }

        // metodo index padrão, chamando a pagina prinicipal, listando todas as empresas cadastradas
        public IActionResult Index()
        {
            List<EmpresaModel> empresas = _servico.ListarTodos();
            return View(empresas);
        }

        // metodo padrão chamando a pagina de criar cadastro da empresa
        public IActionResult Criar()
        {
            return View();
        }

        // metodo HTTPPOST para realizar a inserção de um modelo de empresa
        [HttpPost]
        public IActionResult Adicionar(EmpresaModel modelo)
        {
            // realizo a validação se minha modelo veio correta
            if (ModelState.IsValid)
            {
                // envio para o servico de inserção
                _servico.Inserir(modelo);

                // caso houver sucesso, retornamos para a index de empresas, passando a mensagem de sucesso 
                TempData["MensagemSucesso"] = $"{modelo.RazaoSocial} cadastrada!";
                return RedirectToAction("Index");
            }
            //Caso não seja valido usaremos o try catch para fazer as tentativas
            try
            {
                // ele tentará inserir novamente
                _servico.Inserir(modelo);

                //Caso houver sucesso, mensagem será enviada e retornaremos para a index
                TempData["MensagemSucesso"] = $"{modelo.RazaoSocial} cadastrada!";
                return RedirectToAction("Index");
            }

            catch (ValidacaoException erro)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar {modelo.RazaoSocial}: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        //Metodo de edição, recebendo ID do objeto a ser alterado
        public IActionResult Editar(int id)
        {
            //Crio uma variavel recebendo como parametro o ID
            EmpresaModel empresa = _servico.BuscarEspecifico(id);
            return View(empresa);
        }

        //Metodo HTTPPOST de atualização de uma empresa
        [HttpPost]
        public IActionResult Atualizar(EmpresaModel modelo)
        {
            //Realizando a validação, com tratativa
            if (ModelState.IsValid)
            {
                // envio para atualização dos dados
                _servico.Atualizar(modelo);

                //Caso houver sucesso, mensagem será enviada
                TempData["MensagemSucesso"] = $"{modelo.RazaoSocial} alterada com sucesso!";
                return RedirectToAction("Index");
            }
            //Caso não seja valido usaremos o try catch para fazer as tentativas
            try
            {
                _servico.Atualizar(modelo);
                //Caso houver sucesso, mensagem será enviada
                TempData["MensagemSucesso"] = $"{modelo.RazaoSocial} alterada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = $"Houve um erro na atualização da {modelo.RazaoSocial}";
                return RedirectToAction("Index");
            }
        }

        //Metodo de confirmação do delete
        public IActionResult ApagarConfirmacao(int id)
        {
            EmpresaModel empresa = _servico.BuscarEspecifico(id);
            return View(empresa);
        }

        //Metodo de apagar uma empresa cadastrada
        public IActionResult Apagar(int id)
        {
            // faço o envio para a função de apagar e recebo um booleano
            bool apagado = _servico.Deletar(id);

            if (apagado)
            {
                TempData["MensagemSucesso"] = "Empresa apagada com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Ops, não foi possível apagar a empresa!";
            }
            return RedirectToAction("Index");

        }
    }
}
