using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace CampoMinado
{
    class Program
    {
        static int linhas = 10;
        static int colunas = 10;
        static int numMinas = 10;
        static bool[,] minas;
        static bool[,] revelado;
        static bool gameOver;

        static void Main(string[] args)
        {
            InicializarJogo();
            while (!gameOver)
            {
                ExibirTabuleiro();
                Console.WriteLine("\nEscolha uma célula para revelar (formato: linha coluna): ");
                string entrada = Console.ReadLine();
                string[] partes = entrada.Split(' ');

                if (partes.Length == 2 && int.TryParse(partes[0], out int linha) && int.TryParse(partes[1], out int coluna))
                {
                    RevelarCelula(linha, coluna);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Tente novamente.");
                }
            }
        }

        static void InicializarJogo()
        {
            minas = new bool[linhas, colunas];
            revelado = new bool[linhas, colunas];
            gameOver = false;
            Random rand = new Random();

            // Colocar minas aleatoriamente
            int minasColocadas = 0;
            while (minasColocadas < numMinas)
            {
                int linha = rand.Next(0, linhas);
                int coluna = rand.Next(0, colunas);

                if (!minas[linha, coluna])
                {
                    minas[linha, coluna] = true;
                    minasColocadas++;
                }
            }
        }

        static void ExibirTabuleiro()
        {
            Console.Clear();
            Console.WriteLine("Campo Minado\n");

            // Cabeçalho das colunas
            Console.Write("   ");
            for (int c = 0; c < colunas; c++) Console.Write(c + " ");
            Console.WriteLine();

            for (int i = 0; i < linhas; i++)
            {
                Console.Write(i + " | ");
                for (int j = 0; j < colunas; j++)
                {
                    if (revelado[i, j])
                    {
                        Console.Write(minas[i, j] ? "*" : ContarMinasVizinhas(i, j).ToString());
                    }
                    else
                    {
                        Console.Write("#");
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        static void RevelarCelula(int linha, int coluna)
        {
            if (linha < 0 || linha >= linhas || coluna < 0 || coluna >= colunas || revelado[linha, coluna])
                return;

            revelado[linha, coluna] = true;

            if (minas[linha, coluna])
            {
                Console.WriteLine("Você pisou em uma mina! Game Over.");
                gameOver = true;
            }
            else
            {
                int minasVizinhas = ContarMinasVizinhas(linha, coluna);
                if (minasVizinhas == 0)
                {
                    // Revela células adjacentes automaticamente
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i != 0 || j != 0)
                                RevelarCelula(linha + i, coluna + j);
                        }
                    }
                }
            }
        }

        static int ContarMinasVizinhas(int linha, int coluna)
        {
            int contagem = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int novaLinha = linha + i;
                    int novaColuna = coluna + j;

                    if (novaLinha >= 0 && novaLinha < linhas && novaColuna >= 0 && novaColuna < colunas)
                    {
                        if (minas[novaLinha, novaColuna])
                            contagem++;
                    }
                }
            }
            return contagem;
        }
    }
}

