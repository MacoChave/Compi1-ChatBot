using System;
using Irony.Parsing;

namespace ChatBot.Archivos
{
    class ControlDOT
    {
        private static int contador;
        private static string grafo;

        public static string GetDot (ParseTreeNode raiz)
        {
            grafo = "digraph G {" +
                    "node [shape = \"box\"];" +
                    $"nodo0 [label = \"{Escapar(raiz.ToString())}\"];\r\n";
            contador = 1;
            RecorrerAST("node0", raiz);
            grafo += "}";

            return grafo;
        }

        private static void RecorrerAST(string v, ParseTreeNode raiz)
        {
            foreach (ParseTreeNode item in raiz.ChildNodes)
            {
                string hijo = "nodo" + contador.ToString();
                grafo += hijo + "[label = \"" + Escapar(item.ToString()) + "\"];";
                grafo += $"{v} -> {hijo};\r\n";
                contador++;

                RecorrerAST(hijo, item);
            }
        }

        private static object Escapar(string v)
        {
            v = v.Replace("\\", "\\\\");
            v = v.Replace("\"", "\\\"");

            return v;
        }
    }
}
