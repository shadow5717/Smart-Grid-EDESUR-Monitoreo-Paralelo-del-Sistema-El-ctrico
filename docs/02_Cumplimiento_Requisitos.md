Cumplimiento de Requisitos

Este proyecto cumple con los requisitos solicitados para la práctica de Programación Paralela:

1. Ejecución simultánea de múltiples tareas
   - Se utilizan Parallel.For, Task.Run y Thread para ejecutar procesos de fallas en paralelo.

2. Compartir datos entre tareas
   - Se emplea ConcurrentBag<SensorFalla> para almacenar las fallas detectadas de manera segura entre hilos.

3. Exploración de diferentes estrategias
   - Se implementaron tres estrategias en la carpeta ParallelStrategies:
     - EstrategiaParallelFor
     - EstrategiaTaskRun
     - EstrategiaThreads

4. Capacidad de escalar
   - El sistema puede procesar más sensores al aumentar el número de núcleos o hilos disponibles.

5. Métricas de evaluación del rendimiento
   - Se utiliza Stopwatch para medir el tiempo de ejecución de cada estrategia.
   - Los resultados se guardan en un archivo CSV (Comparativa_Tiempos.csv) y se grafican en Resultados_Graficos.png.

6. Escenario del mundo real
   - El problema de **detección de fallas en la red eléctrica** se representa mediante sensores distribuidos que generan fallos simulados.
