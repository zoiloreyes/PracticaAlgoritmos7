using Nito.Collections;

public class Program
{

    // Combinacion de movimientos desde una celda
    private static int[] fila = { -1, 0, 0, 1 };
    private static int[] columna = { 0, -1, 1, 0 };
    private static int StartX = 1;
    private static int StartY = 9;
    private static int EndX = 7;
    private static int EndY = 9;

    static bool esValido(int x, int y, int N)
    {
        return (x >= 0 && x < N) && (y >= 0 && y < N);
    }

    private static void encontrarCamino(Celda celda, List<String> camino)
    {
        if (celda != null)
        {
            encontrarCamino(celda.padre, camino);
            camino.Add(celda.toString());
        }
    }

    public static List<String> encontrarCamino(int[,] matriz, int x, int y)
    {
        List<String> camino = new List<String>();

        if (matriz == null || matriz.Length == 0)
        {
            return camino;
        }

        int N = matriz.GetLength(0);

        Deque<Celda> q = new Deque<Celda>();
        Celda src = new Celda(x, y, 0, null);
        q.AddToBack(src);

        HashSet<String> visitados = new HashSet<String>();

        String llave = $"{src.x},{src.y}";
        visitados.Add(llave);

        while (q.Count != 0) {
            Celda actual = q.RemoveFromFront();
            int i = actual.x, j = actual.y;

            //Destino
            if (i == EndX && j == EndY)
            {
                encontrarCamino(actual, camino);
                return camino;
            }

            //Valor de la matriz actual
            int n = matriz[i,j];

            //Validar los cuatros posibles movimientos desde la celda
            //Repetir para cada movimiento válido
            for (int k = 0; k < fila.Length; k++)
            {
                // Obten la siguiente posicion
                x = i + fila[k];
                y = j + columna[k];

                if(esValido(x, y, N))
                {
                    Celda siguiente = new Celda(x, y, 0, actual);

                    llave = $"{siguiente.x},{siguiente.y}";

                    if(!visitados.Contains(llave))
                    {
                        q.AddToBack(siguiente);
                        visitados.Add(llave);
                    }
                }
            }
        }

        return camino;
    }

    // Iterative function to find the minimum cost to traverse
    // from the first cell to the last cell of a matrix
    public static int findMinCost(int[,] cost)
    {
        List<String> camino = new List<String>();

        // base case
        if (cost == null || cost.Length == 0)
        {
            return 0;
        }

        // `M × N` matrix
        int M = cost.GetLength(0);
        int N = cost.GetLength(1);

        int[,] T = new int[M,N];
        for (int i = 0; i < T.GetLength(0); i++)
        {
            for (int j = 0; j < T.GetLength(1); j++)
            {
                T[i, j] = int.MaxValue;
            }
        }

        List<Celda> st = new List<Celda>();
        st.Add(new Celda(StartX, StartY, 0, null));
        T[StartX, StartY] = cost[StartX, StartY];

        while (st.Count != 0)
        {
            var k = st[0];
            st.RemoveAt(0);

            int ii = k.x, jj = k.y;

            //Destino
            if (ii == EndX && jj == EndY)
            {
                encontrarCamino(k, camino);
            }

            for (int i = 0; i < fila.Length; i++)
            {
                // Obten la siguiente posicion
                var x = k.x + fila[i];
                var y = k.y + columna[i];

                if (!esValido(x, y, N)) continue;

                if (T[x, y] > T[k.x, k.y] + cost[x, y])

                {
                    T[x, y] = T[k.x, k.y] + cost[x, y];
                    st.Add(new Celda(x, y, T[x, y], k));
                }
            }

            st.Sort((a, b) =>
            {
                if(a.distancia == b.distancia)
                {
                    if(a.x != b.x)
                    {
                        return (a.x - b.x);
                    }
                    return a.y - b.y;
                }
                return a.distancia - b.distancia;
            });
        }


        Console.WriteLine("Matriz de Pesos");

        Console.WriteLine($"Camino de menor peso:");
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (camino.Exists(p => {
                    var xVal = int.Parse(p[1].ToString());
                    var yVal = int.Parse(p[4].ToString());
                    return x == xVal && yVal == y;
                }))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                if (y != 9)
                {
                    Console.Write(cost[x, y] + "-");
                }
                else
                {
                    Console.Write(cost[x, y]);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("");
        }

        Console.WriteLine(String.Join(",", camino));

        return T[EndX, EndY];
    }

    static void Main(string[] args)
    {
        //Estructura de Matriz Bidimensional para Grafo
        //Definicion de Matriz

        int[,] mapa;
        mapa = new int[10, 10];
        //Definicion de Numero Aleatorio
        Random aleatorio = new Random();
        //Iniciar ciclo de impresion de la Matriz
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                mapa[x, y] = aleatorio.Next(1, 6);
                if (y != 9)
                {
                    Console.Write(mapa[x, y] + "-");
                }
                else
                {
                    Console.Write(mapa[x, y]);
                }
            }
            Console.WriteLine("");
        }

        //Origen
        List<String> camino = encontrarCamino(mapa, 1, 9);

        var ll = findMinCost(mapa); 

        if(camino != null && camino.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine($"Camino mas corto ({camino.Count - 1} pasos):");
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if(camino.Exists(p => {
                        var xVal = int.Parse(p[1].ToString());
                        var yVal = int.Parse(p[4].ToString());
                        return x == xVal && yVal == y;
                    }))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (y != 9)
                    {
                        Console.Write(mapa[x, y] + "-");
                    }
                    else
                    {
                        Console.Write(mapa[x, y]);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("");
            }

            camino.RemoveAt(0);
            Console.WriteLine(String.Join(",",camino));
        } else
        {
            Console.WriteLine("No se encontró el destino");
        }
    }
}

public class Celda
{
    public int x, y, distancia;
    public Celda padre;

    public Celda(int x, int y, int distancia, Celda padre)
    {
        this.x = x;
        this.y = y;
        this.padre = padre;
    }

    public String toString()
    {
        return $"({x}, {y})";   
    }
}

