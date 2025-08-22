using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SmartGridEDESUR.Models;

namespace SmartGridEDESUR.ParallelStrategies
{
    public class EstrategiaThreads : IEstrategiaParalela
    {
        public string Nombre { get { return "Threads"; } }
        public List<string> Resultados { get; } = new List<string>();
        public int FallasProcesadas { get { return Resultados.Count; } }
        public NivelTensionRed NivelTension { get; set; }

        private ConcurrentBag<string> _resultados = new ConcurrentBag<string>();
        private int _maxHilos;

        public EstrategiaThreads(NivelTensionRed nivelTension)
        {
            NivelTension = nivelTension;
            _maxHilos = Environment.ProcessorCount;
            Console.WriteLine("   Usando " + _maxHilos + " hilos");
        }

        public TimeSpan ProcesarFallas(List<SensorFalla> fallas)
        {
            var reloj = Stopwatch.StartNew();
            _resultados = new ConcurrentBag<string>();
            var hilos = new List<Thread>();
            var cola = new Queue<SensorFalla>(fallas);
            var lockObj = new object();

            for (int i = 0; i < _maxHilos; i++)
            {
                var hilo = new Thread(() =>
                {
                    while (true)
                    {
                        SensorFalla falla;
                        lock (lockObj)
                        {
                            if (cola.Count == 0) break;
                            falla = cola.Dequeue();
                        }

                        var resultado = ProcesarFalla(falla);
                        _resultados.Add(resultado);
                    }
                });

                hilos.Add(hilo);
                hilo.Start();
            }

            foreach (var hilo in hilos)
            {
                hilo.Join();
            }

            Resultados.AddRange(_resultados);
            reloj.Stop();
            return reloj.Elapsed;
        }

        private string ProcesarFalla(SensorFalla falla)
        {
            int delay = 10 + (falla.Prioridad * 2);
            Thread.Sleep(delay);

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
