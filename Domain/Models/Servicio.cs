using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Models;

public partial class Servicio
{
    public int ServicioId { get; set; }
    [Required]
    public string Descripcion { get; set; } = null!;
    [Required]
    public decimal Precio { get; set; }
    [Required]
    public int Duracion { get; set; }

    public string? Observacion { get; set; }
    [Required]
    public int TipoServicioId { get; set; }

    public virtual TipoServicio TipoServicio { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
