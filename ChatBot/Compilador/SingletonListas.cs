using System;
using System.Collections.Generic;
using ChatBot.Objeto;

namespace ChatBot.Compilador
{
    class SingletonListas
    {
        private static SingletonListas instancia;

        private SingletonListas()
        {
            Variables = new List<Variable>();
            Errores = new List<Error>();
            Ambitos = new List<Ambito>();
            Ambito a = new Ambito()
            {
                Id = "GLOBAL",
                Nombre = "GLOBAL",
                Variables = new List<Variable>()
            };
            Ambitos.Add(a);
        }

        public static SingletonListas GetInstance()
        {
            if (instancia == null)
                instancia = new SingletonListas();

            return instancia;
        }

        internal List<Variable> Variables { get; set; }
        internal List<Error> Errores { get; set; }
        internal List<Ambito> Ambitos { get; set; }
        
        internal Ambito GetAmbito(string nombre = "GLOBAL")
        {
            foreach (Ambito a in Ambitos)
            {
                if (a.Id.Equals(nombre))
                    return a;
            }
            return null;
        }

        internal object GetVarValue(string id, string ambito = "GLOBAL")
        {
            Variable v = GetVariable(id, ambito);
            return v.Valor;
        }
        
        internal void NuevoError(string tipo, string fuente, int fila, int columna, string comentario)
        {
            Error e = new Error()
            {
                Tipo = tipo,
                Fuente = fuente,
                Fila = fila,
                Columna = columna,
                Comentario = comentario
            };
            Errores.Add(e);
        }

        internal bool VerificarId(Variable v, string ambito)
        {
            Variable var = GetVariable(v.Id, ambito);
            return var == null;
        }

        internal Variable GetVariable(string id, string ambito)
        {
            Ambito a = GetAmbito(ambito);
            if (a != null)
            {
                foreach(Variable v in a.Variables)
                {
                    if (v.Id.Equals(id))
                        return v;
                }
            }
            if (!ambito.Equals("GLOBAL"))
                return GetVariable(id, "GLOBAL");

            return null;
        }

        internal void AlmacenarVariable(List<Variable> vars, string ambito)
        {
            Ambito a = GetAmbito(ambito);
            a.Variables.AddRange(vars);
        }
    }
}
