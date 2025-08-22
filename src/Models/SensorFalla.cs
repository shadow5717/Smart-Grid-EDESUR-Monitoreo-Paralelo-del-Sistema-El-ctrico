using System;

namespace SmartGridEDESUR.Models
{
    public class SensorFalla
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Ubicacion { get; set; }
        public double Voltaje { get; set; }
        public double Corriente { get; set; }
        public double Frecuencia { get; set; }
        public TipoFalla Tipo { get; set; }
        public int Prioridad { get; set; }
        public string CodigoSector { get; set; }

        public SensorFalla(int id, DateTime time, string lugar,
                          double vol, double corr, double freq,
                          TipoFalla tipoFalla, int prio)
        {
            Id = id;
            Timestamp = time;
            Ubicacion = lugar;
            Voltaje = vol;
            Corriente = corr;
            Frecuencia = freq;
            Tipo = tipoFalla;
            Prioridad = prio;
            CodigoSector = ObtenerCodigoSector(lugar);
        }

        private string ObtenerCodigoSector(string ubicacion)
        {
            if (ubicacion.Contains("Pueblo Nuevo")) return "LA-PN";
            if (ubicacion.Contains("Los Alcarrizos Centro")) return "LA-CENTRO";
            if (ubicacion.Contains("La Guáyiga")) return "LA-GUAY";
            if (ubicacion.Contains("Ensanche Luperón")) return "LA-LUP";
            if (ubicacion.Contains("Villa Carmen")) return "LA-VC";
            if (ubicacion.Contains("Mata Mamón")) return "LA-MM";
            if (ubicacion.Contains("Bajos de Haina")) return "LA-HAINA";
            if (ubicacion.Contains("El Café")) return "LA-CAFE";
            if (ubicacion.Contains("La Ciénaga")) return "LA-CIEN";
            if (ubicacion.Contains("Las Caobas")) return "LA-CAOB";
            if (ubicacion.Contains("Palmarejo")) return "LA-PALM";
            if (ubicacion.Contains("Villa Liberación")) return "LA-VL";
            if (ubicacion.Contains("Los Pérez")) return "LA-PEREZ";
            if (ubicacion.Contains("La Isabela")) return "LA-ISAB";

            return "LA-OTRO";
        }
    }

    public enum TipoFalla
    {
        Sobrecarga,
        Cortocircuito,
        CaidaVoltaje,
        AumentoVoltaje,
        PerdidaFase,
        FalloEquipo
    }
}
