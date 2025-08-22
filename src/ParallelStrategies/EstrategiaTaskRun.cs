using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SmartGridEDESUR.Models;

namespace SmartGridEDESUR.ParallelStrategies
{
    public class EstrategiaTaskRun : IEstrategiaParalela
    {
        public string Nombre { get { return "Task.Run"; } }
        public List<string> Resultados { get; } = new List<string>();
        public int FallasProcesadas { get { return Resultados.Count; } }
        public NivelTensionRed NivelTension { get; set; }

        private ConcurrentBag<string> _resultados = new ConcurrentBag<string>();

        public EstrategiaTaskRun(NivelTensionRed nivelTension)
        {
            NivelTension = nivelTension;
        }

        public TimeSpan ProcesarFallas(List<SensorFalla> fallas)
        {
            var reloj = Stopwatch.StartNew();
            _resultados = new ConcurrentBag<string>();
            var tareas = new List<Task>();

            foreach (var falla in fallas)
            {
                tareas.Add(Task.Run(() =>
                {
                    var resultado = ProcesarFalla(falla);
                    _resultados.Add(resultado);
                }));
            }

            Task.WaitAll(tareas.ToArray());
            Resultados.AddRange(_resultados);
            reloj.Stop();
            return reloj.Elapsed;
        }

        private string ProcesarFalla(SensorFalla falla)
        {
            int delay = 10 + (falla.Prioridad * 2);
            System.Threading.Thread.Sleep(delay);

            double cambioTension = CalcularCambioTension(falla);
            NivelTension.AjustarTension(cambioTension);

            return $"Falla {falla.Id} ({falla.Tipo}) en {falla.Ubicacion} " +
                   $"- Acción: {ObtenerAccion(falla.Tipo, falla.Prioridad)} " +
                   $"- ΔTensión: {cambioTension:0.00}V";
        }

        private double CalcularCambioTension(SensorFalla falla)
        {
            switch (falla.Tipo)
            {
                case TipoFalla.Sobrecarga:
                    return -(falla.Prioridad * 0.5);
                case TipoFalla.Cortocircuito:
                    return -(falla.Prioridad * 2.0);
                case TipoFalla.CaidaVoltaje:
                    return -(falla.Prioridad * 1.5);
                case TipoFalla.AumentoVoltaje:
                    return falla.Prioridad * 1.2;
                case TipoFalla.PerdidaFase:
                    return -(falla.Prioridad * 3.0);
                case TipoFalla.FalloEquipo:
                    return -(falla.Prioridad * 1.8);
                default:
                    return 0;
            }
        }

        private string ObtenerAccion(TipoFalla tipo, int prioridad)
        {
            if (tipo == TipoFalla.Sobrecarga)
                return prioridad > 7 ? "DESCONEXIÓN INMEDIATA" : "Monitoreo reforzado";
            else if (tipo == TipoFalla.Cortocircuito)
                return "AISLAR Y REPARAR - EQUIPO DE EMERGENCIA";
            else if (tipo == TipoFalla.CaidaVoltaje)
                return prioridad > 5 ? "AJUSTE DE TAPs TRANSFORMADORES" : "Monitorear";
            else if (tipo == TipoFalla.AumentoVoltaje)
                return "PROTECCIÓN SOBRETENSIÓN - AJUSTE REGULADORES";
            else if (tipo == TipoFalla.PerdidaFase)
                return "ENVIAR TÉCNICOS ESPECIALIZADOS";
            else if (tipo == TipoFalla.FalloEquipo)
                return "PROGRAMAR MANTENIMIENTO EQUIPO";
            else
                return "VERIFICAR CON SUPERvisor";
        }
    }
}
