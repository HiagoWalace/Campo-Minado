﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMinado
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class GridCell
    {
        public bool TemMina {  get; set; }
        public int MinasVizinhas { get; set; }
        public bool Descoberta { get; set; }
        public Button Botao { get; set; }

    }

    public class GameBoard
    {
        public GridCell[,] Tabuleiro { get; set; }
        public int Tamanho { get; set; }
        public int MinaCount { get; set; }

        public GameBoard ( int tamanho, int minas)
        {
            Tamanho = tamanho;
            MinaCount = minas;
            Tabuleiro = new GridCell[tamanho, tamanho];
            CriarTabuleiro();
        }

        public void CriarTabuleiro()
        {
            for (int i =0; i < Tamanho; i++)
            {
                for (int j = 0; j < Tamanho; j++)
                {
                    Tabuleiro[i, j] = new GridCell();
                }
            }

            Random random = new Random();

            // Coloca as minas aleatóriamente
            for (int i = 0; i < MinaCount; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, Tamanho);
                    y = random.Next(0, Tamanho);
                } while (Tabuleiro[x, y].TemMina);

                Tabuleiro[x, y].TemMina = true;
            }

            // Conta se a célula possuí alguma mina ao redor dela
            for (int i = 0; i < Tamanho; i++)
            {
                for (int j = 0; j < Tamanho; j++)
                {
                    if (!Tabuleiro[i, j].TemMina)
                    {
                        int minasVizinhas = 0;
                        
                        // Confere todas as 8 posições ao redor da célula
                        for (int xi = -1; xi <= 1; xi++)
                        {
                            for (int yj = -1; yj <= 1; yj++)
                            {
                                // Pula a posicção atual da cédula e verifica se está dentro do limite (limite de 1 Cédula de Distância = ( Todas as Cédulas Vizinhas) ) 
                                if (xi == 0 && yj == 0) continue;

                                int vizinhoX = i + xi;
                                int vizinhoY = j + yj;

                                if (vizinhoX >= 0 && vizinhoX < Tamanho && vizinhoY >= 0 && vizinhoY < Tamanho)
                                {
                                    if (Tabuleiro[vizinhoX, vizinhoY].TemMina )
                                    {
                                        minasVizinhas++;
                                    }
                                }
                            }
                        }
                        Tabuleiro[i, j].MinasVizinhas = minasVizinhas;
                    }
                }
            }
        }

    }
}
