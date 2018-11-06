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
        }

        public static SingletonListas GetInstance()
        {
            if (instancia == null)
                instancia = new SingletonListas();

            return instancia;
        }

        internal List<Variable> Variables { get; set; }
        internal List<Error> Errores { get; set; }
        
        public Variable getVariable(string id)
        {
            foreach(Variable v in Variables)
            {
                if (v.Id.Equals(id)) return v;
            }
            return null;
        }

        public Variable getVariable(string id, string tipo)
        {
            foreach (Variable v in Variables)
            {
                if (v.Id.Equals(id))
                {
                    if (v.Tipo.Equals(tipo)) return v;
                }
            }
            return null;
        }

        internal object getValue(string valueString)
        {
            foreach (Variable v in Variables)
            {
                if (v.Id.Equals(valueString))
                    return v.Valor;
            }

            return null;
        }
    }
}
