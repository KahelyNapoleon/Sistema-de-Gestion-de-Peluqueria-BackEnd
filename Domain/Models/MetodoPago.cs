using System;
using System.Collections.Generic;

namespace DomainLayer.Models;
public partial class MetodoPago
{
    public int MetodoPagoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
