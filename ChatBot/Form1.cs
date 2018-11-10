using System.Collections.Generic;
using System.Windows.Forms;
using ChatBot.Compilador;
using ChatBot.Objeto;
using Irony.Parsing;

namespace ChatBot
{
    public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

        private void buttonAnalizar_Click(object sender, System.EventArgs e)
        {
            SingletonListas s = SingletonListas.GetInstance();
            s.LimpiarListas();
            richTextBoxRestult.Clear();
            string texto = richTextBoxSource.Text;
            ParseTreeNode raiz = Analizador.AnalisisLexicoSintactico(texto);
            if (raiz != null)
            {
                Analizador.GenerarImagen(raiz);
                MessageBox.Show("Arbol Sintáctico Abstracto generado");
                Analizador.AnalisisSemantico(raiz);
                richTextBoxRestult.Text += "*==================================================*\n";
                richTextBoxRestult.Text += "*==================================================*\n";
                richTextBoxRestult.Text += $"* {System.DateTime.Today}\n";
                richTextBoxRestult.Text += "*==================================================*\n";

                foreach(Ambito a in s.Ambitos)
                {
                    foreach(Variable v in a.Variables)
                        richTextBoxRestult.Text += $"* {v}\n";
                }

                richTextBoxRestult.Text += "*==================================================*\n";
            }
            else
                MessageBox.Show("Arbol Sintáctico Abstracto no generado");
            if (s.Errores.Count > 0)
            {
                MessageBox.Show("Se detectaron erorres");
                richTextBoxRestult.Text += "*==================================================*\n";
                richTextBoxRestult.Text += "*==================================================*\n";
                richTextBoxRestult.Text += $"* {System.DateTime.Today}\n";
                foreach (Error err in s.Errores)
                {
                    richTextBoxRestult.Text += $"* {err}\n";
                }
                richTextBoxRestult.Text += "*==================================================*\n";
            }
        }
    }
}
