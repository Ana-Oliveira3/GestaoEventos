using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class Eventos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do evento é obrigatório!")]
        [StringLength(100)]

        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Data do evento")]

        public DateTime Data { get; set; }

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        [Required(ErrorMessage = "Selecione um local")]
        public int LocalId { get; set; }

        public Local? Local { get; set; }
    }
}