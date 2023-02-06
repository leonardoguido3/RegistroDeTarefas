using System.ComponentModel.DataAnnotations;

namespace RegistroDeTarefasWebApp.Models
{
    public class EmpresaModel
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        [Required(ErrorMessage = "Digite o CNPJ da empresa")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "Digite a Razão Social")]
        public DateTime DataCadastro { get; set; }
    }
}
