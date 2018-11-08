using System.Collections.Generic;
using Irony.Parsing;

namespace ChatBot.Objeto
{
    class Ambito
    {
        private List<Variable> parametros;
        private List<Variable> variables;
        public string Nombre { get; set; }
        public string Id { get; set; }
        public string Tipo { get; set; }
        internal List<Variable> Parametros { get => parametros; set => parametros = value; }
        public ParseTreeNode Sentencias { get; set; }
        internal List<Variable> Variables { get => variables; set => variables = value; }
        public int Fila { get; set; }
        public int Columna { get; set; }

        public Ambito()
        {
        }

        public object GetVarValue(string nombre)
        {
            foreach (Variable v in Variables)
            {
                if (v.Id.Equals(nombre))
                    return v.Valor;
            }
            return null;
        }
        
        public bool CheckParametros(List<Variable> prms)
        {
            if (Parametros.Count != prms.Count)
                return false;

            for (int i = 0; i < Parametros.Count; i++)
            {
                if (!Parametros[i].Tipo.Equals(prms[i].Tipo))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"Ambito: {Nombre} Nombre: {Id} Tipo: {Tipo} Fila: {Fila} Columna: {Columna}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
