using System;
using System.Collections.Generic;
using SmartGridEDESUR.Models;
using SmartGridEDESUR.ParallelStrategies;
using Xunit;

namespace SmartGridEDESUR.Tests
{
    public class SmartGridTests
    {
        [Fact]
        public void ParallelFor_Funciona_Bien()
        {
            var nivelTension = new NivelTensionRed(110.0);
            var metodo = new EstrategiaParallelFor(nivelTension);
            var fallas = HacerFallas(10);

            var tiempo = metodo.ProcesarFallas(fallas);

            Assert.Equal(10, metodo.FallasProcesadas);
            Assert.True(tiempo.TotalMilliseconds > 0);
        }

        [Fact]
        public void TaskRun_Funciona_Bien()
        {
            var nivelTension = new NivelTensionRed(110.0);
            var metodo = new EstrategiaTaskRun(nivelTension);
            var fallas = HacerFallas(10);

            var tiempo = metodo.ProcesarFallas(fallas);

            Assert.Equal(10, metodo.FallasProcesadas);
            Assert.True(tiempo.TotalMilliseconds > 0);
        }

        [Fact]
        public void Threads_Funciona_Bien()
        {
            var nivelTension = new NivelTensionRed(110.0);
            var metodo = new EstrategiaThreads(nivelTension);
            var fallas = HacerFallas(10);

            var tiempo = metodo.ProcesarFallas(fallas);

            Assert.Equal(10, metodo.FallasProcesadas);
            Assert.True(tiempo.TotalMilliseconds > 0);
        }

        [Fact]
        public void Generador_Fallas_Funciona()
        {
            int numero = 25;
            var fallas = HacerFallas(numero);
            Assert.Equal(numero, fallas.Count);
        }

        private List<SensorFalla> HacerFallas(int n)
        {
            var random = new Random();
            var fallas = new List<SensorFalla>();
            for (int i = 0; i < n; i++)
            {
                fallas.Add(new SensorFalla(
                    i + 1,
                    DateTime.Now,
                    "Pueblo Nuevo, Calle Duarte #10, Los Alcarrizos",
                    110,
                    10,
                    60,
                    TipoFalla.Sobrecarga,
                    5
                ));
            }
            return fallas;
        }
    }
}
