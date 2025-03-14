﻿using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface ITurnoRepository
    {
        Task<IEnumerable<Turno>> ObtenerTurnoPorFecha(DateTime fechaTurno);
        Task<IEnumerable<Turno>> ObtenerTurnoPorServicio(int servicioId);
        Task<IEnumerable<Turno>> ObtenerTurnoPorCliente(int clienteId);
        Task<IEnumerable<Turno>> ObtenerTurnoPorEstadoTurno(int estadoTurnoId);
    }
}
