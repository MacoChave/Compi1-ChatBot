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

        internal void AddVariableGlobal(List<Variable> variables)
        {
            Ambito a = GetAmbito();
            if (a != null)
                a.Variables.AddRange(variables);
        }
     
        internal Ambito GetAmbito(string nombre = "GLOBAL", List<Variable> prms = null)
        {
            foreach (Ambito a in Ambitos)
            {
                if (a.Nombre.Equals(nombre))
                {
                    if (prms == null)
                        return a;
                    if (a.CheckParametros(prms))
                        return a;
                }
            }
            return null;
        }
        
        internal object GetVarValue(string nombre, string ambito = "GLOBAL")
        {
            Ambito a = GetAmbito(ambito);
            if (a != null)
                return a.GetVarValue(nombre);

            return null;
        }
    }
}
