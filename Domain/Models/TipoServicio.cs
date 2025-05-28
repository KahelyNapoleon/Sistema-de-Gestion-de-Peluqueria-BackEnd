using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Models;

public partial class TipoServicio
{
    public int TipoServicioId { get; set; }
    [Required]
    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
