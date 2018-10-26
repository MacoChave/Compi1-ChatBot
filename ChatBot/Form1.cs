using System.Windows.Forms;
using ChatBot.Compilador;
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
            string texto = richTextBoxSource.Text;
            ParseTreeNode raiz = Analizador.Analizar(texto);
            if (raiz != null)
            {
                Analizador.GenerarImagen(raiz);
                MessageBox.Show("Arbol Sintáctico Abstracto generado");
            }
        }
    }
}
