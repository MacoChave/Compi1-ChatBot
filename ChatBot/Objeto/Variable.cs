namespace ChatBot.Objeto
{
    internal class Variable
    {
        public Variable()
        {
        }

        public Variable(string id, int fila, int columna)
        {
            Id = id;
            Fila = fila;
            Columna = columna;
        }

        public Variable(string id, string tipo, int fila, int columna)
        {
            Id = id;
            Tipo = tipo;
            Fila = fila;
            Columna = columna;
        }

        public Variable(string tipo, int indice, string id, int fila, int columna)
        {
            Tipo = tipo;
            Indice = indice;
            Id = id;
            Fila = fila;
            Columna = columna;
        }

        public string Tipo { get; set; }
        public string Ambito { get; set; }
        public int Indice { get; set; }
        public string Id { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public object Valor { get; internal set; }

        public override bool Equals(object obj)
        {
            return Id.Equals(obj.ToString());
        }

        public override string ToString()
        {
            return $"Indice: {Indice} Tipo: {Tipo} Nombre: {Id} Valor: {Valor} Ambito: {Ambito}";
        }
    }
}
