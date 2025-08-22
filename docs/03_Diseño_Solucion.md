
Solución "SmartGridEDESUR" 
├── C# SmartGridEDESUR
│   ├── Dependencias
│   ├── Models
│   │   └── C# SensorFalla.cs
│   ├── ParallelStrategies
│   │   ├── C# EstrategiaParallelFor.cs
│   │   ├── C# EstrategiaTaskRun.cs
│   │   ├── C# EstrategiaThreads.cs
│   │   └── C# IEstrategiaParalela.cs
│   └── C# Program.cs
└── C# SmartGridEDESUR.Tests
    ├── Dependencias
    └── C# SmartGridTests.cs



 Modelos
- SensorFalla.cs
  Representa un sensor de la red eléctrica con información de su estado (activo, inactivo, con falla).

Estrategias de Paralelización
- Parallel.For→ Divide el trabajo entre múltiples núcleos automáticamente.
- Task.Run → Maneja tareas asincrónicas y escalables.
- Threads → Control manual de la creación y sincronización de hilos.

 Flujo General
1. Se generan sensores con fallas simuladas.
2. Se procesa la información en paralelo con las distintas estrategias.
3. Se almacenan los resultados en una estructura compartida (ConcurrentBag).
4. Se comparan los tiempos de ejecución.
5. Se generan métricas y gráficas para análisis.
