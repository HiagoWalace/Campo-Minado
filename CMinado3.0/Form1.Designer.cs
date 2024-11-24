using System;
using System.Drawing;
using System.Windows.Forms;

namespace CMinado
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private GameBoard gameBoard;
        private int tamanho = 10; // Tamanho do tabuleiro (Adaptavel)
        private int minas = 15; // Quantidade de minas que serão Geradas (Adaptavel)
            

        private void IniciarJogo()
        {
            gameBoard = new GameBoard(tamanho, minas);

            // Define o tamanho de cada botão de cédula
            int buttonSize = 50;
            this.ClientSize = new Size(tamanho * buttonSize, tamanho * buttonSize);

            // Cria 
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(i * buttonSize, j * buttonSize),
                        Tag = new Point(i, j) // Tag vai ser utilizada para armazenar a posição da cédula

                    };

                    // Associa o botão á célula correspondente no tabuleiro
                    gameBoard.Tabuleiro[i, j].Botao = btn;

                    // Adiciona o evento de Clique ao Botão (Tanto Clique Direito para o normal quando o Esquerdo para Marcação 
                    btn.Click += Botao_Click;
                    btn.MouseDown += Botao_MouseDown;

                    //Adiciona o Botão ao formulário
                    this.Controls.Add(btn);
                }
            }
        }

        //Desativa o Jogo   
        private void DesativarTabuleiro()
        {
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    gameBoard.Tabuleiro[i, j].Botao.Enabled = false;
                }
            }
        }

        //Evento para revelar as células com o Clique Esquedo do Mouse
        private void Botao_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Obtém a posição da célula
            Point posicao = (Point)btn.Tag;
            GridCell cell = gameBoard.Tabuleiro[posicao.X, posicao.Y];

            //Verifica se você clicou em uma célula que possuí mina e se sim termina o jogo
            if (cell.TemMina)
            {
                btn.BackColor = Color.Red;
                MessageBox.Show("Você clicou em uma mina! Fim de Jogo.");
                MostrarMinas();
                DesativarTabuleiro();
                return;
            }

            if (!cell.Descoberta)
            {
                cell.Descoberta = true;
                btn.Text = cell.MinasVizinhas > 0 ? cell.MinasVizinhas.ToString() : "";
                btn.Enabled = false;
                btn.BackColor = Color.LightGray;

                if (cell.MinasVizinhas == 0)
                {
                    RevelarVizinhas(posicao.X, posicao.Y);
                }   
            }   
        }

        // Verifica se você marcou todas as bombas, se sim define como vitória
        private void VerificarVitoria()
        {
            bool venceu = true;

            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    GridCell cell = gameBoard.Tabuleiro[i, j];

                    // Verifica se todas as minas estão marcadas e se nenhuma célula sem mina foi marcada
                    if ((cell.TemMina && cell.Botao.Text != "🚩") ||
                        (!cell.TemMina && cell.Botao.Text == "🚩"))
                    {
                        venceu = false;
                        break;
                    }
                }

                if (!venceu)
                    break;
            }

            if (venceu)
            {
                MessageBox.Show("Parabéns! Você venceu o jogo!", "Vitória");
                DesativarTabuleiro(); // Desativa o tabuleiro após a vitória
            }
        }

        // Evento para Marcar e Desmarcar com o Clique Direito do Mouse
        private void Botao_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Button btn = sender as Button;
                if (btn == null) return;

                Point posicao = (Point)btn.Tag;
                GridCell cell = gameBoard.Tabuleiro[posicao.X, posicao.Y];

                if (!cell.Descoberta)
                {
                    // Alterna entre marcado e desmarcado
                    if (btn.Text == "🚩")
                    {
                        btn.Text = "";
                        btn.BackColor = DefaultBackColor;
                    }
                    else
                    {
                        btn.Text = "🚩";
                        btn.BackColor = Color.Yellow;
                    }

                    // Verifica se o jogador venceu
                    VerificarVitoria();
                }
            }
        }

        // Função para revelar as células vizinhas
        private void RevelarVizinhas(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < tamanho && ny >= 0 && ny < tamanho)
                    {
                        GridCell cell = gameBoard.Tabuleiro[nx, ny];
                        if (!cell.Descoberta && !cell.TemMina)
                        {
                            cell.Descoberta = true;
                            cell.Botao.Text = cell.MinasVizinhas > 0 ? cell.MinasVizinhas.ToString() : "";
                            cell.Botao.Enabled = false;
                            cell.Botao.BackColor = Color.LightGray;

                            if (cell.MinasVizinhas == 0)
                            {
                                RevelarVizinhas(nx, ny); // Revela céluas Vizinhas Recursivamente 
                            }
                        }
                    }
                }
            }
        }

        //Função que revela todas as minas
        private void MostrarMinas()
        {
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    GridCell cell = gameBoard.Tabuleiro[i, j];
                    if (cell.TemMina)
                    {
                        cell.Botao.BackColor = Color.Red;
                    }
                }
            }
        }
        
    }
}

