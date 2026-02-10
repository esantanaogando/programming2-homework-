using Tarea1Programacion2.Entities;

namespace Tarea1Programacion2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // este ejemplo son mis datos personales reales, esta fue mi forma creativa de presentarme
            // y probar la app

            Estudiante estudiante1 = new Estudiante();

            estudiante1.Nombre = "Edwin Joel Santana Ogando";
            estudiante1.Edad = 19;
            estudiante1.Carrera = "Desarrollo de Software";
            estudiante1.Matricula = 20250897;

            Console.WriteLine(estudiante1.Nombre);
            Console.WriteLine(estudiante1.Edad);
            Console.WriteLine(estudiante1.Carrera);
            Console.WriteLine(estudiante1.Matricula);

        }
    }
}
