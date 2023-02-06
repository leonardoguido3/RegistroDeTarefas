using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace RegistroDeTarefasWebApp.Models
{
    public class UsuarioModel
    {
        public enum PerfilAcesso
        {
            Admin = 1,
            Desenvolvedor = 2,
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o nome do usuário!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite o email do usuário!")]
        [EmailAddress(ErrorMessage = "O email informado não é válido!")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Digite a senha do usuário!")]
        public PerfilAcesso Perfil { get; set; }
        public List<TarefaModel> Tarefas { get; set; }


        //Responsabilidade de validar senha
        public bool SenhaValida(string senha)
        {
            var senhaHash = GerarSha512(senha);
            return Senha == senhaHash;
        }

        //Responsabilidade de gerar o HASH
        public void SetSenhaHash()
        {
            Senha = GerarSha512(Senha);
        }

        //Responsabilidade de gerar nova senha (recuperacao)
        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = GerarSha512(novaSenha);
            return novaSenha;
        }

        //Responsabilidade de alterar a senha do usuario
        public void SetNovaSenha(string novaSenha)
        {
            Senha = GerarSha512(novaSenha);
        }
        
        // geracao da criptografica sha512
        public static string GerarSha512(string valor)
        {
            var bytes = Encoding.UTF8.GetBytes(valor);
            using (var hash = SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString().ToLower();
            }
        }
    }
}
