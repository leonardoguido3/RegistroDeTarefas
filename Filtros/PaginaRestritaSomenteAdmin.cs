﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RegistroDeTarefasWebApp.Models;

namespace RegistroDeTarefasWebApp.Filtros
{
    public class PaginaRestritaSomenteAdmin : ActionFilterAttribute
    {
        //Este método é para usuarios admin, ele irá verificar a sessão do usuario
        //e irá validar o perfil do usuario se for admin será liberado, caso não for
        //redirecionará para uma pagina de restrição
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }
            else
            {

                UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

                if (usuario == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                }

                if (usuario.Perfil != UsuarioModel.PerfilAcesso.Admin)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Restrito" }, { "action", "Index" } });
                }

            }
            base.OnActionExecuting(context);
        }
    }
}
