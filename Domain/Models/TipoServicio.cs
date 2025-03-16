using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class TipoServicio
{
    public int TipoServicioId { get; set; }

    public string Descripcion { get; set; } = null!;

    public int ServicioId { get; set; }

    public virtual Servicio Servicio { get; set; } = null!;
}
