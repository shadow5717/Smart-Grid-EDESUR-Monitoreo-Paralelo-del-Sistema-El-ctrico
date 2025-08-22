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

        private ConcurrentBag<string> _resultados = new ConcurrentBag<string>();

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

            return "Falla " + falla.Id + " (" + falla.Tipo + ") en " + falla.Ubicacion +
                   " - Accion: " + ObtenerAccion(falla.Tipo, falla.Prioridad);
        }

        private string ObtenerAccion(TipoFalla tipo, int prioridad)
        {
            if (tipo == TipoFalla.Sobrecarga)
                return prioridad > 5 ? "DESCONEXION" : "Monitorear";
            else if (tipo == TipoFalla.Cortocircuito)
                return "AISLAR Y REPARAR";
            else if (tipo == TipoFalla.CaidaVoltaje)
                return "AJUSTAR VOLTAJE";
            else if (tipo == TipoFalla.AumentoVoltaje)
                return "PROTEGER SOBRETENSION";
            else if (tipo == TipoFalla.PerdidaFase)
                return "ENVIAR TECNICO";
            else if (tipo == TipoFalla.FalloEquipo)
                return "REPARAR EQUIPO";
            else
                return "VERIFICAR";
        }
    }
}
