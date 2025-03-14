using System;
using System.Collections.Generic;

namespace DomainLayer.Models;
public partial class Servicio
{
    public int ServicioId { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Duracion { get; set; }

    public string? Observacion { get; set; }

    public int TipoServicioId { get; set; }

    public virtual TipoServicio TipoServicio { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
