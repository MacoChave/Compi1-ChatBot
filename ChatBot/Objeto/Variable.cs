namespace ChatBot.Objeto
{
    class Variable
    {
        public string Ambito { get; set; }
        public int Indice { get; set; }
        public string Id { get; set; }
        public string Tipo { get; set; }
        public object Valor { get; internal set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        

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

        public override string ToString()
        {
            return $"Indice: {Indice} Nombre: {Id} Tipo: {Tipo} Valor: {Valor} Fila: {Fila + 1} Columna: {Columna}";
        }

        public override bool Equals(object obj)
        {
            return Id.Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
