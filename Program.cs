using System;

namespace InventarioBiblioteca
{
    class Program
    {
        // Constantes para definir las dimensiones del arreglo bidimensional
        const int CANTIDAD_LIBROS = 5;     // Filas (0 a 4)
        const int CANTIDAD_SUCURSALES = 3; // Columnas (0 a 2)

        // Arreglo de nombres para facilitar la interacción con el usuario
        static string[] libros = {
            "Cien Años de Soledad",
            "Don Quijote de la Mancha",
            "El Principito",
            "1984",
            "Fahrenheit 451"
        };

        static string[] sucursales = {
            "Sucursal Norte (0)",
            "Sucursal Centro (1)",
            "Sucursal Sur (2)"
        };

        // Arreglo bidimensional [Filas, Columnas] -> [Libros, Sucursales]
        static int[,] stock = new int[CANTIDAD_LIBROS, CANTIDAD_SUCURSALES];

        static void Main(string[] args)
        {
            InicializarInventario();

            int opcion;
            do
            {
                MostrarMenu();
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("\n[ERROR] Ingrese un número válido.");
                    Pausar();
                    continue;
                }

                Console.Clear();
                switch (opcion)
                {
                    case 1:
                        MostrarInventarioCompleto(); // O(F * C)
                        break;
                    case 2:
                        ConsultarDisponibilidad();  // O(1)
                        break;
                    case 3:
                        ActualizarDisponibilidad(); // O(1)
                        break;
                    case 4:
                        CalcularTotalLibro();       // O(C)
                        break;
                    case 5:
                        CalcularTotalSucursal();    // O(F)
                        break;
                    case 6:
                        MostrarBajoInventario();    // O(F * C)
                        break;
                    case 7:
                        SucursalMayorDisponibilidad(); // O(C)
                        break;
                    case 0:
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }

                if (opcion != 0) Pausar();

            } while (opcion != 0);
        }

        // Carga de datos iniciales
        static void InicializarInventario()
        {
            int[,] datosIniciales = {
                { 5, 2, 8 }, // Libro 0
                { 1, 4, 3 }, // Libro 1
                { 10, 0, 2 },// Libro 2
                { 3, 3, 1 }, // Libro 3
                { 7, 6, 4 }  // Libro 4
            };

            for (int f = 0; f < CANTIDAD_LIBROS; f++)
            {
                for (int c = 0; c < CANTIDAD_SUCURSALES; c++)
                {
                    stock[f, c] = datosIniciales[f, c];
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("====== INVENTARIO POR SUCURSAL ======");
            Console.WriteLine("1. Mostrar inventario completo");
            Console.WriteLine("2. Consultar disponibilidad");
            Console.WriteLine("3. Actualizar disponibilidad");
            Console.WriteLine("4. Total disponible de un libro");
            Console.WriteLine("5. Total de inventario por sucursal");
            Console.WriteLine("6. Mostrar libros con bajo inventario");
            Console.WriteLine("7. Sucursal con mayor disponibilidad");
            Console.WriteLine("0. Salir");
            Console.Write("\nSeleccione una opción: ");
        }

        // 1. Mostrar la matriz completa -> O(F * C)
        static void MostrarInventarioCompleto()
        {
            Console.WriteLine("=== INVENTARIO COMPLETO DE LIBROS ===");
            Console.WriteLine("{0,-28} | {1,-10} | {2,-10} | {3,-10}", "Libro / Sucursal", "Norte (0)", "Centro (1)", "Sur (2)");
            Console.WriteLine(new string('-', 65));

            for (int f = 0; f < CANTIDAD_LIBROS; f++)
            {
                Console.Write("{0,-28} | ", $"[{f}] {libros[f]}");
                for (int c = 0; c < CANTIDAD_SUCURSALES; c++)
                {
                    Console.Write("{0,-10} | ", stock[f, c]);
                }
                Console.WriteLine();
            }
        }

        // 2. Consultar disponibilidad -> O(1)
        static void ConsultarDisponibilidad()
        {
            Console.WriteLine("=== CONSULTAR DISPONIBILIDAD ===");
            int libro = LeerIndice("Ingrese el número del libro (0 a 4): ", 0, CANTIDAD_LIBROS - 1);
            int sucursal = LeerIndice("Ingrese el número de la sucursal (0 a 2): ", 0, CANTIDAD_SUCURSALES - 1);

            int cantidad = stock[libro, sucursal];
            Console.WriteLine($"\nDisponibilidad de '{libros[libro]}' en {sucursales[sucursal]}: {cantidad} unidades.");
        }

        // 3. Actualizar disponibilidad -> O(1)
        static void ActualizarDisponibilidad()
        {
            Console.WriteLine("=== ACTUALIZAR DISPONIBILIDAD ===");
            int libro = LeerIndice("Ingrese el número del libro (0 a 4): ", 0, CANTIDAD_LIBROS - 1);
            int sucursal = LeerIndice("Ingrese el número de la sucursal (0 a 2): ", 0, CANTIDAD_SUCURSALES - 1);

            Console.Write("Ingrese la nueva cantidad disponible: ");
            if (int.TryParse(Console.ReadLine(), out int nuevaCantidad) && nuevaCantidad >= 0)
            {
                stock[libro, sucursal] = nuevaCantidad;
                Console.WriteLine("\n¡Inventario actualizado con éxito!");
            }
            else
            {
                Console.WriteLine("\n[ERROR] Cantidad no válida.");
            }
        }

        // 4. Calcular el total de un libro -> O(C)
        static void CalcularTotalLibro()
        {
            Console.WriteLine("=== TOTAL DISPONIBLE DE UN LIBRO ===");
            int libro = LeerIndice("Ingrese el número del libro (0 a 4): ", 0, CANTIDAD_LIBROS - 1);

            int acumulador = 0;
            for (int c = 0; c < CANTIDAD_SUCURSALES; c++)
            {
                acumulador += stock[libro, c];
            }

            Console.WriteLine($"\nEl libro '{libros[libro]}' tiene un total de {acumulador} unidades en la biblioteca.");
        }

        // 5. Calcular el inventario de una sucursal -> O(F)
        static void CalcularTotalSucursal()
        {
            Console.WriteLine("=== TOTAL DE INVENTARIO POR SUCURSAL ===");
            int sucursal = LeerIndice("Ingrese el número de la sucursal (0 a 2): ", 0, CANTIDAD_SUCURSALES - 1);

            int acumulador = 0;
            for (int f = 0; f < CANTIDAD_LIBROS; f++)
            {
                acumulador += stock[f, sucursal];
            }

            Console.WriteLine($"\nLa {sucursales[sucursal]} tiene un total acumulado de {acumulador} libros en stock.");
        }

        // 6. Identificar libros con bajo inventario (<= 3) -> O(F * C)
        static void MostrarBajoInventario()
        {
            Console.WriteLine("=== LIBROS CON BAJO INVENTARIO (<= 3 UNIDADES) ===");
            bool encontro = false;

            for (int f = 0; f < CANTIDAD_LIBROS; f++)
            {
                for (int c = 0; c < CANTIDAD_SUCURSALES; c++)
                {
                    if (stock[f, c] <= 3)
                    {
                        Console.WriteLine($"- '{libros[f]}' en {sucursales[c]}: {stock[f, c]} unidades. Posición: stock[{f},{c}]");
                        encontro = true;
                    }
                }
            }

            if (!encontro)
            {
                Console.WriteLine("No se encontraron elementos con bajo inventario.");
            }
        }

        // 7. Determinar la sucursal con mayor disponibilidad -> O(C)
        static void SucursalMayorDisponibilidad()
        {
            Console.WriteLine("=== SUCURSAL CON MAYOR DISPONIBILIDAD ===");
            int libro = LeerIndice("Ingrese el número del libro (0 a 4): ", 0, CANTIDAD_LIBROS - 1);

            int maxStock = stock[libro, 0];
            int sucursalMayor = 0;

            for (int c = 1; c < CANTIDAD_SUCURSALES; c++)
            {
                if (stock[libro, c] > maxStock)
                {
                    maxStock = stock[libro, c];
                    sucursalMayor = c;
                }
            }

            Console.WriteLine($"\nEl libro '{libros[libro]}' se encuentra en mayor cantidad en {sucursales[sucursalMayor]} con {maxStock} unidades.");
        }

        // Métodos auxiliares de lectura y pausa
        static int LeerIndice(string mensaje, int min, int max)
        {
            int valor;
            do
            {
                Console.Write(mensaje);
            } while (!int.TryParse(Console.ReadLine(), out valor) || valor < min || valor > max);
            return valor;
        }

        static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}