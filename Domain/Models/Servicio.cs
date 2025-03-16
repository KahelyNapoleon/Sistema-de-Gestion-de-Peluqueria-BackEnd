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

    public virtual ICollection<TipoServicio> TipoServicios { get; set; } = new List<TipoServicio>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
