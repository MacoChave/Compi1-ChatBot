using System;
using Irony.Parsing;

namespace ChatBot.Compilador
{
	class Gramatica : Grammar
	{
		public Gramatica() : base(caseSensitive: false)
		{
            #region ER

            /*NUMERO ENTERO*/
            RegexBasedTerminal numentero = new RegexBasedTerminal("Int", "[0-9]+");

            /*NUMERO DECIMAL*/
            RegexBasedTerminal numdecimal = new RegexBasedTerminal("double", "[0-9]+[.][0-9]+");

            /*IDENTIFICADOR*/
            IdentifierTerminal id = new IdentifierTerminal("id");

            /*STRING*/
            //CommentTerminal cadena = new CommentTerminal("String", "\"", ".", "\"");
            StringLiteral cadena = TerminalFactory.CreateCSharpString("String");
            /*STRING*/
            CommentTerminal importaciones = new CommentTerminal("String", "\"", ".[.].", "\"");

            /*CHAR*/
            StringLiteral caracter = TerminalFactory.CreateCSharpChar("Char");

            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n");
            CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");

            #endregion

            //--------------------------------------RESERVADAS------------------------------------------------

            #region Terminal

            //TIPO DATO
            var rint = ToTerm("Int");
            var rdouble = ToTerm("Double");
            var rstring = ToTerm("String");
            var rchar = ToTerm("Char");
            var rbool = ToTerm("Boolean");
            var rvoid = ToTerm("Void");

            //PALABRAS RESERVADAS
            var importar = ToTerm("Import");
            var retornar = ToTerm("Return");
            var rprint = ToTerm("Print");
            var rmain = ToTerm("Main");
            var comparar = ToTerm("CompareTo");
            var rGetUser = ToTerm("GetUser");
            var rbreak = ToTerm("Break");
            
            //OPERACIONES ARITMETICAS
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var dividir = ToTerm("/");
            var modulo = ToTerm("%");
            var potencia = ToTerm("^");

            //OPERACIONES RELACIONALES
            var igual2 = ToTerm("==");
            var diferente = ToTerm("!=");
            var menor = ToTerm("<");
            var mayor = ToTerm(">");
            var menorigual = ToTerm("<=");
            var mayorigual = ToTerm(">=");

            //OPERACIONES LOGICAS
            var rand = ToTerm("&&");
            var ror = ToTerm("||");
            var rxor = ToTerm("|&");
            var rnot = ToTerm("!");

            //OPERACIONES ESPECIALES
            var incremento = ToTerm("++");
            var decremento = ToTerm("--");
            var masigual = ToTerm("+=");
            var menosigual = ToTerm("-=");
            
            //SENTENCIAS
            var rif = ToTerm("If");
            var relse = ToTerm("Else");
            var relseif = ToTerm("Else if");
            var rswitch = ToTerm("Switch");
            var rcase = ToTerm("Case");
            var defecto = ToTerm("Default");
            var rfor = ToTerm("For");
            var rdo = ToTerm("Do");
            var rwhile = ToTerm("While");
            
            //BOOLEANOS
            var rtrue = ToTerm("true");
            var rfalse = ToTerm("false");

            //VARIOS            
            var igual1 = ToTerm("=");
            var dospuntos = ToTerm(":");
            var coma = ToTerm(",");
            var fin = ToTerm(";");
            var apar = ToTerm("(");
            var cpar = ToTerm(")");
            var alla = ToTerm("{");
            var clla = ToTerm("}");
            var acor = ToTerm("[");
            var ccor = ToTerm("]");
            
            #endregion

            #region No terminales

            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal IMPORTE = new NonTerminal("IMPORTE");
            NonTerminal IMPORTES = new NonTerminal("IMPORTES");
            NonTerminal CUERPO = new NonTerminal("CUERPO");
            NonTerminal CONTENIDOGENERAL = new NonTerminal("CONTENIDOGENERAL");
            NonTerminal ASIGNA = new NonTerminal("ASIGNA");
            NonTerminal DECLARA = new NonTerminal("DECLARA");
            NonTerminal LISTA_IDS = new NonTerminal("LISTA_IDS");
            NonTerminal TIPODATO = new NonTerminal("TIPODATO");
            NonTerminal VALOR = new NonTerminal("VALOR");
            NonTerminal EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal METODO = new NonTerminal("METODO");
            NonTerminal LISTAPARAMETROS = new NonTerminal("LISTAPARAMETROS");
            NonTerminal CUERPOMETODO = new NonTerminal("CUERPOMETODO");
            NonTerminal LLAMADAMETODO = new NonTerminal("LLAMADAMETODO");
            NonTerminal IMPRIMIR = new NonTerminal("IMPRIMIR");
            NonTerminal PARAMETROSLLAMADOS = new NonTerminal("PARAMETROSLLAMADOS");
            NonTerminal OPCIONAL = new NonTerminal("OPCIONAL");
            NonTerminal SENTENCIARETURN = new NonTerminal("SENTENCIARETURN");
            NonTerminal SENTENCIAWHILE = new NonTerminal("SENTENCIAWHILE");
            NonTerminal SENTENCIADOWHILE = new NonTerminal("SENTENCIADOWHILE");
            NonTerminal SENTENCIASWITCH = new NonTerminal("SENTENCIASWITCH");
            NonTerminal CASO = new NonTerminal("CASO");
            NonTerminal CASOS = new NonTerminal("CASOS");
            NonTerminal DEFECTO = new NonTerminal("DEFECTO");
            NonTerminal CONTENIDOSWITCH = new NonTerminal("CONTENIDOSWITCH");
            NonTerminal LISTA_ARRAY = new NonTerminal(" LISTA_ARRAY");

            NonTerminal CONDICION = new NonTerminal("CONDICION");
            NonTerminal CONDICIONPRIMA = new NonTerminal("CONDICIONPRIMA");
            NonTerminal CONDICIONAL = new NonTerminal("CONDICIONAL");
            NonTerminal LOGICOS = new NonTerminal("LOGICOS");
            NonTerminal RELACIONAL = new NonTerminal("RELACIONAL");
            NonTerminal SENTENCIAIF = new NonTerminal("SENTENCIAIF");
            NonTerminal SENTENCIAIFAUX = new NonTerminal("SENTENCIAIFAUX");
            NonTerminal SENTPRIMA = new NonTerminal("SENTPRIMA");
            NonTerminal SENTENCIAELSEIF = new NonTerminal("SENTENCIAELSEIF");
            NonTerminal SENTENCIA = new NonTerminal("SENTENCIA");
            NonTerminal SENTENCIAS = new NonTerminal("SENTENCIAS");
            NonTerminal SENTENCIAFOR = new NonTerminal("SENTENCIAFOR");
            NonTerminal ASIGNACION_CORTO = new NonTerminal("ASIGNACION_CORTO");
            NonTerminal C = new NonTerminal("C");
            NonTerminal D = new NonTerminal("D");
            
            NonTerminal OPMATEMATICA = new NonTerminal("OPMATEMATICA");
            NonTerminal OP = new NonTerminal("OP");
            NonTerminal E = new NonTerminal("E");
            NonTerminal L = new NonTerminal("L");
            NonTerminal R = new NonTerminal("R");
            NonTerminal INVOCAR = new NonTerminal("INVOCAR");
            NonTerminal LIST_ATRIBUTO = new NonTerminal("LIST_ATRIBUTO");
            NonTerminal ACCESO_VECTOR = new NonTerminal("ACCESO_VECTOR");
            NonTerminal ATRIBUTO = new NonTerminal("ATRIBUTO");

            #endregion

            #region Gramatica

            INICIO.Rule = IMPORTES + CUERPO;

            IMPORTES.Rule = IMPORTES + IMPORTE
                          | IMPORTE
                          | Empty;
            
            IMPORTE.Rule = importar + importaciones + fin;
            
            CUERPO.Rule = CUERPO + CONTENIDOGENERAL
                        | CONTENIDOGENERAL;

            CONTENIDOGENERAL.Rule = DECLARA
                                   | ASIGNA
                                   | METODO;

            DECLARA.Rule = id + dospuntos + TIPODATO + igual1 + VALOR
                         | LISTA_IDS + dospuntos + TIPODATO+ igual1 + VALOR
                         | id + dospuntos + TIPODATO + acor + E + ccor + igual1 + VALOR;

            ASIGNA.Rule = id + igual1 + C + fin
                        | id + igual1 + alla + LISTA_ARRAY + clla + fin
                        | id + acor + E + ccor + igual1 + C + fin
                        | id + acor + E + ccor + igual1 + id + acor + E + ccor + fin;

            VALOR.Rule = C + fin
                       | fin
                       | alla + LISTA_ARRAY + clla + fin;
            
            LISTA_IDS.Rule = LISTA_IDS + coma + id
                            | id;

            LISTA_ARRAY.Rule = LISTA_ARRAY + coma + C
                            | C;

            TIPODATO.Rule = rint
                          | rdouble
                          | rstring
                          | rchar
                          | rbool
                          | rvoid;
            
            METODO.Rule = id + dospuntos + TIPODATO + apar + LISTAPARAMETROS + cpar + alla + SENTENCIAS + clla
                        | rmain + dospuntos + TIPODATO + apar + LISTAPARAMETROS + cpar + alla + SENTENCIAS + clla;
            
            LISTAPARAMETROS.Rule = LISTAPARAMETROS + coma + id + dospuntos + TIPODATO
                                 | id + dospuntos + TIPODATO
                                 | Empty;
            
            SENTENCIAS.Rule = SENTENCIAS + SENTENCIA
                            | SENTENCIA;

            SENTENCIA.Rule = ASIGNA
                            | DECLARA
                            | LLAMADAMETODO + fin
                            | IMPRIMIR
                            | SENTENCIAFOR
                            | SENTENCIAIF
                            | SENTENCIARETURN
                            | SENTENCIAWHILE
                            | SENTENCIADOWHILE
                            | SENTENCIASWITCH
                            | Empty;
            
            //---------LLAMADA A METODO
            LLAMADAMETODO.Rule = id + apar + PARAMETROSLLAMADOS + cpar
                                | id + apar + cpar;
            
            PARAMETROSLLAMADOS.Rule = PARAMETROSLLAMADOS + coma + C
                                    | C;
            
            //---------PRINT
            IMPRIMIR.Rule = rprint + apar + C + cpar;
            
            //---------RETURN
            SENTENCIARETURN.Rule = C + fin
                                  | fin;
            
            //---------FOR
            //falta contenido
            SENTENCIAFOR.Rule = rfor + apar + id + dospuntos + TIPODATO + igual1 + E + fin + C + fin + OP + cpar + alla + SENTENCIAS + clla;
            
            //---------IF
            SENTENCIAIF.Rule = rif + SENTENCIAIFAUX;
            
            SENTENCIAIFAUX.Rule = apar + C + cpar + alla + SENTENCIAS + clla + SENTENCIAELSEIF;
            SENTENCIAIFAUX.ErrorRule = SyntaxError + "}";

            SENTENCIAELSEIF.Rule = relse + SENTPRIMA
                                  | Empty;
            SENTENCIAELSEIF.ErrorRule = SyntaxError + "}";

            SENTPRIMA.Rule = rif + SENTENCIAIFAUX
                            | alla + SENTENCIAS + clla;

            //---------WHILE
            SENTENCIAWHILE.Rule = rwhile + apar + C + cpar + alla + SENTENCIAS + clla;
            SENTENCIAWHILE.ErrorRule = SyntaxError + "}";

            //---------DO WHILE
            SENTENCIADOWHILE.Rule = rdo + alla + SENTENCIAS + clla + rwhile + apar + C + cpar + fin;
            SENTENCIADOWHILE.ErrorRule = SyntaxError + ";";
            
            ///--------SWITCH
            SENTENCIASWITCH.Rule = rswitch + apar + E + cpar + alla + SENTENCIAS + clla;
            SENTENCIASWITCH.ErrorRule = SyntaxError + "}";
            
            CONTENIDOSWITCH.Rule = CASOS + DEFECTO
                                  | CASOS
                                  | DEFECTO
                                  | Empty;
            
            CASOS.Rule = CASOS + CASO
                       | CASO;
            
            //---FALTA CONTENIDO
            CASO.Rule = rcase + C + dospuntos + SENTENCIAS + rbreak + fin;

            //---FALTA CONTENIDO
            DEFECTO.Rule = defecto + SENTENCIAS + dospuntos;
            
            //CONDICION
            ASIGNACION_CORTO.Rule = id + OP;

            OP.Rule = incremento | decremento;

            C.Rule = C + L + C
                   | E + R + E
                   | menos + E
                   | E;

            R.Rule = igual2
                   | diferente
                   | menor
                   | mayor
                   | menorigual
                   | mayorigual;

            L.Rule = ror
                   | rand
                   | rxor
                   | rnot;

            E.Rule = E + mas + E
                   | E + menos + E
                   | E + por + E
                   | E + dividir + E
                   | E + modulo + E
                   | E + potencia + E
                   | apar + E + cpar
                   | id
                   | numentero
                   | numdecimal
                   | cadena
                   | caracter
                   | rtrue
                   | rfalse;

            INVOCAR.Rule = id + apar + LIST_ATRIBUTO + cpar
                         | ACCESO_VECTOR;

            LIST_ATRIBUTO.Rule = LIST_ATRIBUTO + coma + ATRIBUTO
                               | ATRIBUTO
                               | Empty;

            ATRIBUTO.Rule = E;

            #endregion

            #region PREFERENCIAS
            Root = INICIO;

            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioBloque);

			MarkPunctuation(";", "(", ")", "{", "}", ":", "=", "[", "]", ",");

            this.RegisterOperators(1, Associativity.Left, mas, menos);
            this.RegisterOperators(2, Associativity.Left, por, dividir, modulo);
            this.RegisterOperators(3, Associativity.Right, potencia);
            this.RegisterOperators(5, igual2, diferente, menor, mayor, menorigual, mayorigual);
            this.RegisterOperators(6, Associativity.Left, ror);
            this.RegisterOperators(7, Associativity.Left, rxor);
            this.RegisterOperators(8, Associativity.Left, rand);
            this.RegisterOperators(9, Associativity.Left, diferente);
            this.RegisterOperators(10, apar, cpar);
            #endregion
        }

		public override void ReportParseError(ParsingContext context)
		{
			base.ReportParseError(context);
			string error = context.CurrentToken.ValueString;
			int fila;
			int columna;
			string descripcion = "";

			if (error.Contains("Invalid character"))
			{
				fila = context.Source.Location.Line;
				columna = context.Source.Location.Column;

				string delimStr = ":";
				char[] delim = delimStr.ToCharArray();
				string[] division = error.Split(delim, 2);
				descripcion = "Caracter Invalido " + division[0];
			}
			else
			{
				fila = context.Source.Location.Line;
				columna = context.Source.Location.Column;
				descripcion = "Se esperaba: " + context.GetExpectedTermSet().ToString();
			}

            Console.WriteLine($"FILA:{fila} COLUMNA:{columna} DESCRIPCION:{descripcion}");
		}
	}
}
