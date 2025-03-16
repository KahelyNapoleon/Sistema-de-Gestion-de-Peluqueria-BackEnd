using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Administrador
{
    public int AdministradorId { get; set; }

    public string Usuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
