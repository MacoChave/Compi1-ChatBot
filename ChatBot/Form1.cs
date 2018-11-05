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
            SingletonListas s = SingletonListas.getInstance();
            s.Variables.Clear();
            s.Errores.Clear();
            richTextBoxRestult.Clear();
            string texto = richTextBoxSource.Text;
            ParseTreeNode raiz = Analizador.Analizar(texto);
            if (raiz != null)
            {
                Analizador.GenerarImagen(raiz);
                MessageBox.Show("Arbol Sintáctico Abstracto generado");
                Analizador.AnalisisSemantico(raiz);
                richTextBoxRestult.Text += "*==================================================*\n";
                richTextBoxRestult.Text += "*==================================================*\n";
                richTextBoxRestult.Text += $"* {System.DateTime.Today}\n";
                foreach (Variable v in s.Variables)
                {
                    richTextBoxRestult.Text += $"* {v}\n";
                }
                richTextBoxRestult.Text += "*==================================================*\n";
            }
            if (s.Errores.Count > 0)
            {
                MessageBox.Show("Arbol Sintáctico Abstracto no generado");
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
