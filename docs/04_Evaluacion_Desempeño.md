 Evaluación del Desempeño

Se realizaron pruebas con 10,000 sensores simulados** procesados mediante distintas estrategias de paralelización.

// Métricas Recopiladas
- Tiempo total de ejecución (ms).
- Número de fallas detectadas.
- Escalabilidad con mayor cantidad de sensores.

## Resultados (ejemplo)
| Estrategia           | Tiempo (ms) | Fallas Detectadas |
| Parallel.For         | 120         | 3,421             |
| Task.Run             | 135         | 3,421             |
| Threads              | 180         | 3,421             |

//Observaciones
- Parallel.For fue la estrategia más rápida gracias a la optimización interna del TPL (Task Parallel Library).
- Task.Run tuvo un rendimiento cercano, demostrando su escalabilidad en escenarios de alto nivel.
- Threads fue menos eficiente por la sobrecarga de manejar manualmente los hilos.

//Evidencia
- Los resultados fueron exportados a:
  - metrics/Comparativa_Tiempos.csv
  - metrics/Resultados_Graficos.png

