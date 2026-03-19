using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class Local
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do local é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
        [Display(Name = "Capacidade Máxima")]
        public int Capacidade { get; set; }
    }
}
