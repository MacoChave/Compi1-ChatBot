namespace ChatBot.Objeto
{
    class Error
    {
        public Error()
        {
        }

        public Error(string tipo, int columna, int fila, string fuente, string comentario)
        {
            Tipo = tipo;
            Columna = columna;
            Fila = fila;
            Fuente = fuente;
            Comentario = comentario;
        }

        public string Tipo { get; set; }
        public int Columna { get; set; }
        public int Fila { get; set; }
        public string Fuente { get; set; }
        public string Comentario { get; set; }

        public override string ToString()
        {
            return $"Tipo: {Tipo} Columna: {Columna} Fila: {Fila} Fuente: {Fuente} Comentario: {Comentario}";
        }
    }
}
