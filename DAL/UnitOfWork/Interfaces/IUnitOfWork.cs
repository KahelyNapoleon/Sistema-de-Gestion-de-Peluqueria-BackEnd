using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        ITurnoRepository TurnoRepository { get; }

        IHistorialTurnoRepository HistorialTurnoRepository { get; }

        //Metodo abre apertura en la base de datos.
        Task IniciarTransaccionAsync();

        //Siguientes dos metodos para:
        // CommitAsync y RollBackAsync.
        Task GuardarCambiosAsync();

        Task DeshacerCambiosAsync();
        
    }
}

//IPedidoRepositorio PedidoRepositorio { get; }

//IDetallePedidoRepositorio DetallePedidoRepositorio { get; }

//Task IniciarTransaccionAsync();
//Task GuardarCambiosAsync();
//Task DeshacerCambiosAsync();