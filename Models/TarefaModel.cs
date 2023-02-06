using System.ComponentModel.DataAnnotations;

namespace RegistroDeTarefasWebApp.Models
{
    public class TarefaModel
    {
        public enum Tipo
        {
            Reunião = 1,
            Contexto = 2,
            Tarefa = 3
        }
        public int Id { get; set; }
        public DateTime HorarioInicio { get; set; }
        [Required(ErrorMessage = "Digite o dia e horario de inicio")]
        public DateTime? HorarioFim { get; set; }
        public string DescricaoResumo { get; set; }
        [Required(ErrorMessage = "Descreva um titulo para tarefa")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Descreva a tarefa realizada")]
        public Tipo TipoTarefa { get; set; }
        [Required(ErrorMessage = "Selecione o tipo de tarefa")]
        public int? UsuarioId { get; set; }
        public int? EmpresaId { get; set; }
        public string CnpjEmpresa { get; set; }
        [Required(ErrorMessage = "Digite o CNPJ")]
        public string Usuario { get; set; }   
        public string  Empresa { get; set; }
        public double? Total { get; set; }
    }
}
