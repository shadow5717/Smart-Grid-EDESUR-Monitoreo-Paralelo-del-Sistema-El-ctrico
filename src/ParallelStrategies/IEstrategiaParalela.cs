using System;
using System.Collections.Generic;
using SmartGridEDESUR.Models;

namespace SmartGridEDESUR.ParallelStrategies
{
    public interface IEstrategiaParalela
    {
        string Nombre { get; }
        TimeSpan ProcesarFallas(List<SensorFalla> fallas);
        List<string> Resultados { get; }
        int FallasProcesadas { get; }
        NivelTensionRed NivelTension { set; }
    }
}
