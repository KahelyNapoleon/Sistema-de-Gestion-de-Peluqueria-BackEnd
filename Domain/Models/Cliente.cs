using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string NroCelular { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
