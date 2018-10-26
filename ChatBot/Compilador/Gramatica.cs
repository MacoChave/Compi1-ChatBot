using System;
using Irony.Parsing;

namespace ChatBot.Compilador
{
	class Gramatica : Grammar
	{
		public Gramatica() : base(caseSensitive: true)
		{
			#region ER
			NumberLiteral numero = TerminalFactory.CreateCSharpNumber("numero");
			IdentifierTerminal id = new IdentifierTerminal("id");
			StringLiteral cadena = TerminalFactory.CreateCSharpString("string");
			StringLiteral caracter = TerminalFactory.CreateCSharpChar("caracter");

			CommentTerminal lineComment = new CommentTerminal("lcmt", "//", "\n", "\r\n");
			CommentTerminal blockComment = new CommentTerminal("bcmt", "/*", "*/");
			#endregion

			#region TERMINALES
			/* IMPORT */
			var tkImport = ToTerm("Import");
			var tkPutoComa = ToTerm(";");

			/* BLOQUE */
			var tkAPar = ToTerm("(");
			var tkCPar = ToTerm(")");
			var tkALlav = ToTerm("{");
			var tkCLlav = ToTerm("}");

			/* ARITMETICA */
			var tkMas = ToTerm("+");
			var tkMenos = ToTerm("-");
			var tkPor = ToTerm("*");
			var tkDiv = ToTerm("/");
			var tkMod = ToTerm("%");
			var tkPot = ToTerm("^");

			/* RELACIONALES */
			var tkEquiv = ToTerm("==");
			var tkDif = ToTerm("!=");
			var tkMenor = ToTerm("<");
			var tkMayor = ToTerm(">");
			var tkMenorIgual = ToTerm("<=");
			var tkMayorIgual = ToTerm(">=");

			/* LOGICAS */
			var tkAnd = ToTerm("&&");
			var tkOr = ToTerm("||");
			var tkXor = ToTerm("|&");
			var tkNot = ToTerm("!");

            var tkIncremento = ToTerm("++");
            var tkDecremento = ToTerm("--");

			/* VARIABLES */
			var tkPts = ToTerm(":");
            var tkPt = ToTerm(".");
			var tkIgual = ToTerm("=");
			var tkACor = ToTerm("[");
			var tkCCor = ToTerm("]");
			var tkComa = ToTerm(",");

			/* TIPOS DE DATOS */
			var tkInt = ToTerm("Int");
			var tkDouble = ToTerm("Double");
			var tkString = ToTerm("String");
			var tkChar = ToTerm("Char");
			var tkBoolean = ToTerm("Boolean");
			var tkTrue = ToTerm("True");
			var tkFalse = ToTerm("False");

			/* RESERVADAS */
			var tkRetorno = ToTerm("Return");
			var tkVoid = ToTerm("Void");
			var tkMaint = ToTerm("Main");
			var tkBrak = ToTerm("Break");

			/* SENTENCIAS */
			var tkIf = ToTerm("If");
			var tkElse = ToTerm("Else");
			var tkSwitch = ToTerm("Switch");
			var tkCase = ToTerm("Case");
			var tkDefault = ToTerm("Default");
			var tkFor = ToTerm("For");
			var tkWhile = ToTerm("While");
			var tkDo = ToTerm("Do");

			/* NATIVAS */
			var tkPrint = ToTerm("Print");
			var tkCompare = ToTerm("CompareTo");
			var tkGetUser = ToTerm("GetUser");
            #endregion

            #region NO TERMINALES
            NonTerminal INICIO = new NonTerminal("INICIO"),
                LIST_IMPORT = new NonTerminal("LIST_IMPORT"),
                IMPORT = new NonTerminal("IMPORT"),
                LIST_CUERPO = new NonTerminal("LIST_CUERPO"),
                CUERPO = new NonTerminal("CUERPO"),
                ASIGNACION_CORTO = new NonTerminal("ASIGNACION_CORTO"),
                OP = new NonTerminal("OP"),
                C = new NonTerminal("C"),
                R = new NonTerminal("R"),
                L = new NonTerminal("L"),
                E = new NonTerminal("E"),
                INVOCAR = new NonTerminal("INVOCAR"),
                LIST_ATRIBUTO = new NonTerminal("LIST_ATRIBUTO"),
                ATRIBUTO = new NonTerminal("ATRIBUTO"),
				DECLARACION = new NonTerminal("DECLARACION"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                TIPO_ASIGNACION = new NonTerminal("TIPO_ASIGNACION"),
                TIPO_DECLARACION = new NonTerminal("TIPO_DECLARACION"),
                VAR_LINEAL = new NonTerminal("VAR_LINEAL"),
				VAR_VECTOR = new NonTerminal("VAR_VECTOR"),
                VALOR_LINEAL = new NonTerminal("VALOR_LINEAL"),
                VALOR_VECTOR = new NonTerminal("VALOR_VECTOR"),
                TIPO_VALOR = new NonTerminal("TIPO_VALOR"),
                ASIGNAR_LINEAL = new NonTerminal("ASIGNAR_LINEAL"),
                ASIGNAR_VECTOR = new NonTerminal("ASIGNAR_VECTOR"),
                ACCESO_VECTOR = new NonTerminal("ACCESO_VECTOR"),
                LIST_E = new NonTerminal("LIST_E"),
                LIST_ID = new NonTerminal("LIST_ID"),
                METODO = new NonTerminal("METODO"),
                LIST_PARAMETRO = new NonTerminal("LIST_PARAMETRO"),
                PARAMETRO = new NonTerminal("PARAMETRO"),
                LIST_SENTENCIA = new NonTerminal("LIST_SENTENCIA"),
                SENTENCIA = new NonTerminal("SENTENCIA"),
                IF = new NonTerminal("IF"),
                IF_AUX = new NonTerminal("IF_AUX"),
                SENTPRIMA = new NonTerminal("SENTPRIMA"),
                ELSE = new NonTerminal("ELSE"),
                FOR = new NonTerminal("FOR"),
                WHILE = new NonTerminal("WHILE"),
                DOWHILE = new NonTerminal("DOWHILE"),
                SWITCH = new NonTerminal("SWITCH"),
                CUERPO_SWITCH = new NonTerminal("CUERPO_SWITCH"),
                LIST_CASE = new NonTerminal("LIST_CASE"),
                CASE = new NonTerminal("CASE"),
                DEFECTO = new NonTerminal("DEFECTO"),
                RETURN = new NonTerminal("RETURN"),
                LLAMADA = new NonTerminal("LLAMADA"),
                PRINT = new NonTerminal("PRINT"),
                COMPARE = new NonTerminal("COMPARE"),
                GETUSER = new NonTerminal("GETUSER"),
                TIPO_DATO = new NonTerminal("TIPO_DATO");
            #endregion

            #region GRAMATICA
            INICIO.Rule = LIST_IMPORT + LIST_CUERPO;

            LIST_IMPORT.Rule = LIST_IMPORT + IMPORT
                             | IMPORT
                             | Empty;

			IMPORT.Rule = tkImport + cadena + tkPutoComa;

            LIST_CUERPO.Rule = LIST_CUERPO + CUERPO
                             | CUERPO;

            CUERPO.Rule = DECLARACION
                        | ASIGNACION
                        | METODO;
            
            DECLARACION.Rule = LIST_ID + tkPts + TIPO_DATO + TIPO_DECLARACION;

            TIPO_DECLARACION.Rule = VAR_LINEAL
                                  | tkACor + E + tkCCor + VAR_VECTOR;

            VAR_LINEAL.Rule = VALOR_LINEAL
                            | Empty;

            VAR_VECTOR.Rule = VALOR_VECTOR
                            | Empty;

            VALOR_LINEAL.Rule = tkIgual + E;

            VALOR_VECTOR.Rule = tkIgual + TIPO_VALOR;

            TIPO_VALOR.Rule = tkALlav + LIST_E + tkCLlav
                            | E;

            ASIGNACION.Rule = id + TIPO_ASIGNACION;

            TIPO_ASIGNACION.Rule = ASIGNAR_LINEAL | ASIGNAR_VECTOR;

            ASIGNAR_LINEAL.Rule = VALOR_LINEAL;

            ASIGNAR_VECTOR.Rule = ACCESO_VECTOR + VALOR_LINEAL;

            ACCESO_VECTOR.Rule = id + tkACor + E + tkCCor;

            LIST_E.Rule = LIST_E + tkComa + E
                        | E;

            LIST_ID.Rule = MakeListRule(LIST_ID, tkComa, id);

            METODO.Rule = id + tkPts + TIPO_DATO + tkAPar + LIST_PARAMETRO + tkCPar + tkALlav + LIST_SENTENCIA + tkCLlav;

            LIST_PARAMETRO.Rule = MakeListRule(LIST_PARAMETRO, tkComa, PARAMETRO);

            PARAMETRO.Rule = id + tkPts + TIPO_DATO
                           | Empty;

            LIST_SENTENCIA.Rule = LIST_SENTENCIA + SENTENCIA
                                | SENTENCIA;

            SENTENCIA.Rule = DECLARACION + tkPutoComa
                           | ASIGNACION + tkPutoComa
                           | IF
                           | FOR
                           | WHILE
                           | DOWHILE
                           | SWITCH
                           | RETURN
                           | PRINT + tkPutoComa
                           | COMPARE
                           | GETUSER
                           | Empty;

            IF.Rule = tkIf + IF_AUX;

            IF_AUX.Rule = tkAPar + C + tkCPar + tkALlav + LIST_SENTENCIA + tkCLlav + ELSE
                        | SyntaxError + "}";

            ELSE.Rule = tkElse + SENTPRIMA
                      | Empty
                      | SyntaxError + "}";

            SENTPRIMA.Rule = tkIf + IF_AUX
                           | tkALlav + LIST_SENTENCIA + tkCLlav;

            FOR.Rule = tkFor + tkAPar + DECLARACION + tkPutoComa + E + tkPutoComa + ASIGNACION_CORTO + tkCPar + tkALlav + LIST_SENTENCIA + tkCLlav;

            WHILE.Rule = tkWhile + tkAPar + C + tkCPar + tkALlav + LIST_SENTENCIA + tkCLlav
                       | SyntaxError + "}";

            DOWHILE.Rule = tkDo + tkALlav + LIST_SENTENCIA + tkCLlav + tkWhile + tkAPar + C + tkCPar + tkPutoComa
                         | SyntaxError + "}";

            SWITCH.Rule = tkSwitch + tkAPar + E + tkCPar + tkALlav + CUERPO_SWITCH + tkCLlav
                        | SyntaxError + "}";

            CUERPO_SWITCH.Rule = LIST_CASE + DEFECTO;

            LIST_CASE.Rule = LIST_CASE + CASE
                           | CASE;

            CASE.Rule = tkCase + E + tkPts + LIST_SENTENCIA;

            DEFECTO.Rule = tkDefault + LIST_SENTENCIA
                         | Empty;

            RETURN.Rule = E + tkPutoComa
                        | tkPutoComa;

            PRINT.Rule = tkPrint + tkAPar + E + tkCPar;

            COMPARE.Rule = id + tkPt + tkCompare + tkAPar + E + tkCPar;

            GETUSER.Rule = tkGetUser + tkAPar + tkCPar;

            ASIGNACION_CORTO.Rule = id + OP;

            OP.Rule = tkIncremento | tkDecremento;

            C.Rule = C + L + C
                   | E + R + E
                   | tkNot + E
                   | E;

            R.Rule = tkEquiv
                   | tkDif
                   | tkMenor
                   | tkMayor
                   | tkMenorIgual
                   | tkMayorIgual;

            L.Rule = tkOr
                   | tkAnd
                   | tkXor
                   | tkNot;

            E.Rule = E + tkMas + E
				   | E + tkMenos + E
				   | E + tkPor + E
				   | E + tkDiv + E
				   | E + tkMod + E
				   | E + tkPot + E
				   | tkAPar + E + tkCPar
				   | id
				   | numero
				   | cadena
				   | caracter
				   | tkTrue
				   | tkFalse
                   | INVOCAR;

            INVOCAR.Rule = id + tkAPar + LIST_ATRIBUTO + tkCPar
                         | ACCESO_VECTOR;

            LIST_ATRIBUTO.Rule = LIST_ATRIBUTO + tkComa + ATRIBUTO
                               | ATRIBUTO
                               | Empty;

            ATRIBUTO.Rule = E;

			TIPO_DATO.Rule = tkInt | tkDouble | tkString | tkChar | tkBoolean;
            #endregion

            #region PREFERENCIAS
            Root = INICIO;

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(blockComment);

			MarkPunctuation(";", "(", ")", "{", "}", ":", "=", "[", "]", ",");

			RegisterOperators(1, Associativity.Left, tkMas, tkMenos);
			RegisterOperators(2, Associativity.Left, tkPor, tkDiv, tkMod);
			RegisterOperators(3, Associativity.Right, tkPot);
			RegisterOperators(6, Associativity.Left, tkOr);
			RegisterOperators(7, Associativity.Left, tkXor);
			RegisterOperators(8, Associativity.Left, tkAnd);
			RegisterOperators(9, Associativity.Left, tkNot);
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
