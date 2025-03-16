using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class HistorialTurno
{
    public int HistorialTurnoId { get; set; }

    public int EstadoAnterior { get; set; }

    public int EstadoActual { get; set; }

    public DateTime? FechaCambio { get; set; }

    public int TurnoId { get; set; }

    public int AdministradorId { get; set; }

    public virtual Administradores Administrador { get; set; } = null!;

    public virtual Turno Turno { get; set; } = null!;
}
