using Irony.Parsing;

namespace ChatBot.Compilador
{
    class Analizador : Grammar
    {
        public static ParseTreeNode Analizar (string cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree raiz = parser.Parse(cadena);

            return raiz.Root;
        }

        public static void GenerarImagen (ParseTreeNode raiz)
        {
            string nombre = "AST-";
            nombre += System.DateTime.Now.Second.ToString();
            nombre += ".png";
            string grafoDOT = Archivos.ControlDOT.GetDot(raiz);
            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            img.Save(nombre);
        }
    }
}
