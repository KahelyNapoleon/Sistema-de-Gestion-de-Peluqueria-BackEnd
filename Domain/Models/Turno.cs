using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Turno
{
    public int TurnoId { get; set; }

    public DateTime FechaTurno { get; set; }

    public string? Detalle { get; set; }

    public int ServicioId { get; set; }

    public int ClienteId { get; set; }

    public int EstadoTurnoId { get; set; }

    public int MontoTotal { get; set; }

    public int MetodoPagoId { get; set; }

    public int AdministradorId { get; set; }

    public virtual Administradore Administrador { get; set; } = null!;

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual EstadoTurno EstadoTurno { get; set; } = null!;

    public virtual ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
