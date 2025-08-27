using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class HistorialTurno
{
    public int HistorialTurnoId { get; set; }

    public int EstadoAnterior { get; set; }

    public int EstadoActual { get; set; }

    public int TurnoId { get; set; }

    public int AdministradorId { get; set; }

    public DateOnly? FechaActual { get; set; }

    public DateOnly? FechaAnterior { get; set; }

    public TimeOnly? HoraActual { get; set; }

    public TimeOnly? HoraAnterior { get; set; }

    public virtual Administrador Administrador { get; set; } = null!;

    public virtual Turno Turno { get; set; } = null!;
}
