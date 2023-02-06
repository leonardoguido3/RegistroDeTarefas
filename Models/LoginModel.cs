﻿using System.ComponentModel.DataAnnotations;

namespace RegistroDeTarefasWebApp.Models
{
    public class LoginModel
    {
        //Modelo que não vai ao banco de dados, ele apenas recebe a informação input
        //é tratada dentro do back para comparação com a que está salva
        [Required(ErrorMessage = "Digite o login!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite a senha!")]
        public string Senha { get; set; }
    }
}
