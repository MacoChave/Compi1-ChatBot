﻿using System;
using System.Collections.Generic;
using ChatBot.Objeto;
using Irony.Parsing;

namespace ChatBot.Compilador
{
    class Analizador : Grammar
    {
        private static string[,] SUMA = new string[5,5];
        private static string[,] RESTA = new string[5, 5];
        private static string[,] MULTIPLICACION = new string[5, 5];
        private static string[,] DIVISION = new string[5, 5];
        private static string[,] MODULO = new string[5, 5];
        private static string[,] POTENCIA = new string[5, 5];
        /****************************************************************************
         *************************     ANALIZADOR LÉXICO     ************************
         ***********************     ANALIZADOR SINTÁCTIO     ***********************
         ****************************************************************************/
        public static ParseTreeNode AnalisisLexicoSintactico (string cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree raiz = parser.Parse(cadena);

            LlenarSuma();
            LlenarResta();
            LlenarMultiplicación();
            LlenarDivision();
            LlenarModulo();
            LlenarPotencia();

            return raiz.Root;
        }

        #region MATRIZ-ARITMETICA
        private static void LlenarPotencia()
        {
            POTENCIA[0, 0] = "";
            POTENCIA[0, 1] = "double";
            POTENCIA[0, 2] = "";
            POTENCIA[0, 3] = "double";
            POTENCIA[0, 4] = "";
            POTENCIA[1, 0] = "double";
            POTENCIA[1, 1] = "double";
            POTENCIA[1, 2] = "";
            POTENCIA[1, 3] = "double";
            POTENCIA[1, 4] = "double";
            POTENCIA[2, 0] = "";
            POTENCIA[2, 1] = "";
            POTENCIA[2, 2] = "";
            POTENCIA[2, 3] = "";
            POTENCIA[2, 4] = "";
            POTENCIA[3, 0] = "double";
            POTENCIA[3, 1] = "double";
            POTENCIA[3, 2] = "";
            POTENCIA[3, 3] = "int";
            POTENCIA[3, 4] = "int";
            POTENCIA[4, 0] = "";
            POTENCIA[4, 1] = "double";
            POTENCIA[4, 2] = "";
            POTENCIA[4, 3] = "double";
            POTENCIA[4, 4] = "double";
        }

        private static void LlenarModulo()
        {
            MODULO[0, 0] = "";
            MODULO[0, 1] = "double";
            MODULO[0, 2] = "";
            MODULO[0, 3] = "double";
            MODULO[0, 4] = "";
            MODULO[1, 0] = "double";
            MODULO[1, 1] = "double";
            MODULO[1, 2] = "";
            MODULO[1, 3] = "double";
            MODULO[1, 4] = "double";
            MODULO[2, 0] = "";
            MODULO[2, 1] = "";
            MODULO[2, 2] = "";
            MODULO[2, 3] = "";
            MODULO[2, 4] = "";
            MODULO[3, 0] = "double";
            MODULO[3, 1] = "double";
            MODULO[3, 2] = "";
            MODULO[3, 3] = "double";
            MODULO[3, 4] = "double";
            MODULO[4, 0] = "";
            MODULO[4, 1] = "double";
            MODULO[4, 2] = "";
            MODULO[4, 3] = "double";
            MODULO[4, 4] = "double";
        }

        private static void LlenarDivision()
        {
            DIVISION[0, 0] = "";
            DIVISION[0, 1] = "double";
            DIVISION[0, 2] = "";
            DIVISION[0, 3] = "double";
            DIVISION[0, 4] = "";
            DIVISION[1, 0] = "double";
            DIVISION[1, 1] = "double";
            DIVISION[1, 2] = "";
            DIVISION[1, 3] = "double";
            DIVISION[1, 4] = "double";
            DIVISION[2, 0] = "";
            DIVISION[2, 1] = "";
            DIVISION[2, 2] = "";
            DIVISION[2, 3] = "";
            DIVISION[2, 4] = "";
            DIVISION[3, 0] = "double";
            DIVISION[3, 1] = "double";
            DIVISION[3, 2] = "";
            DIVISION[3, 3] = "double";
            DIVISION[3, 4] = "double";
            DIVISION[4, 0] = "";
            DIVISION[4, 1] = "double";
            DIVISION[4, 2] = "";
            DIVISION[4, 3] = "double";
            DIVISION[4, 4] = "double";
        }

        private static void LlenarMultiplicación()
        {
            MULTIPLICACION[0, 0] = "";
            MULTIPLICACION[0, 1] = "double";
            MULTIPLICACION[0, 2] = "";
            MULTIPLICACION[0, 3] = "int";
            MULTIPLICACION[0, 4] = "";
            MULTIPLICACION[1, 0] = "double";
            MULTIPLICACION[1, 1] = "double";
            MULTIPLICACION[1, 2] = "";
            MULTIPLICACION[1, 3] = "double";
            MULTIPLICACION[1, 4] = "double";
            MULTIPLICACION[2, 0] = "";
            MULTIPLICACION[2, 1] = "";
            MULTIPLICACION[2, 2] = "";
            MULTIPLICACION[2, 3] = "";
            MULTIPLICACION[2, 4] = "";
            MULTIPLICACION[3, 0] = "int";
            MULTIPLICACION[3, 1] = "double";
            MULTIPLICACION[3, 2] = "";
            MULTIPLICACION[3, 3] = "int";
            MULTIPLICACION[3, 4] = "int";
            MULTIPLICACION[4, 0] = "";
            MULTIPLICACION[4, 1] = "double";
            MULTIPLICACION[4, 2] = "";
            MULTIPLICACION[4, 3] = "int";
            MULTIPLICACION[4, 4] = "int";
        }

        private static void LlenarResta()
        {
            RESTA[0, 0] = "";
            RESTA[0, 1] = "double";
            RESTA[0, 2] = "";
            RESTA[0, 3] = "int";
            RESTA[0, 4] = "";
            RESTA[1, 0] = "double";
            RESTA[1, 1] = "double";
            RESTA[1, 2] = "";
            RESTA[1, 3] = "double";
            RESTA[1, 4] = "double";
            RESTA[2, 0] = "";
            RESTA[2, 1] = "";
            RESTA[2, 2] = "";
            RESTA[2, 3] = "";
            RESTA[2, 4] = "";
            RESTA[3, 0] = "int";
            RESTA[3, 1] = "double";
            RESTA[3, 2] = "";
            RESTA[3, 3] = "int";
            RESTA[3, 4] = "int";
            RESTA[4, 0] = "";
            RESTA[4, 1] = "double";
            RESTA[4, 2] = "";
            RESTA[4, 3] = "int";
            RESTA[4, 4] = "int";
        }

        private static void LlenarSuma()
        {
            SUMA[0, 0] = "double";
            SUMA[0, 1] = "double";
            SUMA[0, 2] = "string";
            SUMA[0, 3] = "int";
            SUMA[0, 4] = "";
            SUMA[1, 0] = "double";
            SUMA[1, 1] = "double";
            SUMA[1, 2] = "string";
            SUMA[1, 3] = "double";
            SUMA[1, 4] = "double";
            SUMA[2, 0] = "string";
            SUMA[2, 1] = "string";
            SUMA[2, 2] = "string";
            SUMA[2, 3] = "string";
            SUMA[2, 4] = "string";
            SUMA[3, 0] = "int";
            SUMA[3, 1] = "double";
            SUMA[3, 2] = "string";
            SUMA[3, 3] = "int";
            SUMA[3, 4] = "int";
            SUMA[4, 0] = "";
            SUMA[4, 1] = "double";
            SUMA[4, 2] = "string";
            SUMA[4, 3] = "int";
            SUMA[4, 4] = "int";
        }
        
        private static int EnumTipoDato(string tipo)
        {
            switch (tipo)
            {
                case "boolean": return 0;
                case "double": return 1;
                case "string": return 2;
                case "int": return 3;
                case "char": return 4;
                case "void": return 5;
                default: return -1;
            }
        }
        #endregion

        /****************************************************************************
         ****************     GENERAR ÁRBOL DE SINTÁXIS ABSTRACTA     ***************
         ****************************************************************************/
        public static void GenerarImagen (ParseTreeNode raiz)
        {
            string nombre = "AST-";
            nombre += System.DateTime.Now.Minute.ToString();
            nombre += ".png";
            string grafoDOT = Archivos.ControlDOT.GetDot(raiz);
            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            img.Save(nombre);
        }

        /****************************************************************************
         ***********************     ANALIZADOR SEMÁNTICO     ***********************
         * Comprobar tipos de datos de variables y procedimientos
         ****************************************************************************/
        public static object AnalisisSemantico (ParseTreeNode root)
        {
            object result = null;

            if (root.Term.Name.Equals("INICIO"))
            {
                //INICIO.Rule = IMPORTES + CUERPO;
                AnalizarImportacion(root.ChildNodes[0]);
                AnalizarCuerpo(root.ChildNodes[1]);
            }
            else if (root.Term.Name.Equals("METODO"))
            {
                //METODO.Rule = id + dospuntos + TIPODATO + apar + LISTAPARAMETROS + cpar + alla + SENTENCIAS + clla
                //            | rmain + dospuntos + TIPODATO + apar + LISTAPARAMETROS + cpar + alla + SENTENCIAS + clla;

            }
            else if (root.Term.Name.Equals("LISTAPARAMETROS"))
            {
                //LISTAPARAMETROS.Rule = LISTAPARAMETROS + coma + id + dospuntos + TIPODATO
                //                     | id + dospuntos + TIPODATO
                //                     | Empty;

            }
            else if (root.Term.Name.Equals("SENTENCIAS"))
            {
                //SENTENCIAS.Rule = SENTENCIAS + SENTENCIA
                //                | SENTENCIA;

            }
            else if (root.Term.Name.Equals("SENTENCIA"))
            {
                //SENTENCIA.Rule = ASIGNA
                //                | DECLARA
                //                | LLAMADAMETODO + fin
                //                | IMPRIMIR
                //                | SENTENCIAFOR
                //                | SENTENCIAIF
                //                | SENTENCIARETURN
                //                | SENTENCIAWHILE
                //                | SENTENCIADOWHILE
                //                | SENTENCIASWITCH
                //                | Empty;

            }
            else if (root.Term.Name.Equals("LLAMADAMETODO"))
            {
                //LLAMADAMETODO.Rule = id + apar + PARAMETROSLLAMADOS + cpar
                //                    | id + apar + cpar;

            }
            else if (root.Term.Name.Equals("PARAMETROSLLAMADOS"))
            {
                //PARAMETROSLLAMADOS.Rule = PARAMETROSLLAMADOS + coma + C
                //                        | C;

            }
            else if (root.Term.Name.Equals("IMPRIMIR"))
            {
                //IMPRIMIR.Rule = rprint + apar + C + cpar;

            }
            else if (root.Term.Name.Equals("SENTENCIARETURN"))
            {
                //SENTENCIARETURN.Rule = C + fin
                //                      | fin;

            }
            else if (root.Term.Name.Equals("SENTENCIAFOR"))
            {
                //SENTENCIAFOR.Rule = rfor + apar + id + dospuntos + TIPODATO + igual1 + E + fin + C + fin + OP + cpar + alla + SENTENCIAS + clla;

            }
            else if (root.Term.Name.Equals("SENTENCIAIF"))
            {
                //SENTENCIAIF.Rule = rif + SENTENCIAIFAUX;

            }
            else if (root.Term.Name.Equals("SENTENCIAIFAUX"))
            {
                //SENTENCIAIFAUX.Rule = apar + C + cpar + alla + SENTENCIAS + clla + SENTENCIAELSEIF;

            }
            else if (root.Term.Name.Equals("SENTENCIAELSEIF"))
            {
                //SENTENCIAELSEIF.Rule = relse + SENTPRIMA
                //                      | Empty;

            }
            else if (root.Term.Name.Equals("SENTPRIMA"))
            {
                //SENTPRIMA.Rule = rif + SENTENCIAIFAUX
                //                | alla + SENTENCIAS + clla;

            }
            else if (root.Term.Name.Equals("SENTENCIAWHILE"))
            {
                //SENTENCIAWHILE.Rule = rwhile + apar + C + cpar + alla + SENTENCIAS + clla;

            }
            else if (root.Term.Name.Equals("SENTENCIADOWHILE"))
            {
                //SENTENCIADOWHILE.Rule = rdo + alla + SENTENCIAS + clla + rwhile + apar + C + cpar + fin;

            }
            else if (root.Term.Name.Equals("SENTENCIASWITCH"))
            {
                //SENTENCIASWITCH.Rule = rswitch + apar + E + cpar + alla + SENTENCIAS + clla;

            }
            else if (root.Term.Name.Equals("CONTENIDOSWITCH"))
            {
                //CONTENIDOSWITCH.Rule = CASOS + DEFECTO
                //                      | CASOS
                //                      | DEFECTO
                //                      | Empty;

            }
            else if (root.Term.Name.Equals("CASOS"))
            {
                //CASOS.Rule = CASOS + CASO
                //           | CASO;

            }
            else if (root.Term.Name.Equals("CASO"))
            {
                //CASO.Rule = rcase + C + dospuntos + SENTENCIAS + rbreak + fin;

            }
            else if (root.Term.Name.Equals("DEFECTO"))
            {
                //DEFECTO.Rule = defecto + SENTENCIAS + dospuntos;

            }
            else if (root.Term.Name.Equals("ASIGNACION_CORTO"))
            {
                //ASIGNACION_CORTO.Rule = id + OP;

            }
            else if (root.Term.Name.Equals("INVOCAR"))
            {
                //INVOCAR.Rule = id + apar + LIST_ATRIBUTO + cpar
                //             | ACCESO_VECTOR;

            }
            else if (root.Term.Name.Equals("LIST_ATRIBUTO"))
            {
                //LIST_ATRIBUTO.Rule = LIST_ATRIBUTO + coma + ATRIBUTO
                //                   | ATRIBUTO
                //                   | Empty;

            }
            else if (root.Term.Name.Equals("ATRIBUTO"))
            {
                //ATRIBUTO.Rule = E;

            }

            return result;
        }
        
        #region IMPORTACION
        private static void AnalizarImportacion(ParseTreeNode root)
        {
            if (root.Term.Name.Equals("IMPORTES"))
            {
                //IMPORTES.Rule = IMPORTES + IMPORTE
                //              | IMPORTE
                //              | Empty;

            }
            else if (root.Term.Name.Equals("IMPORTE"))
            {
                //IMPORTE.Rule = importar + importaciones + fin;

            }
        }
        #endregion

        #region CUERPO
        private static void AnalizarCuerpo(ParseTreeNode root)
        {
            if (root.Term.Name.Equals("CUERPO"))
            {
                //CUERPO.Rule = CUERPO + CONTENIDOGENERAL
                //            | CONTENIDOGENERAL;
                if (root.ChildNodes.Count == 2)
                {
                    AnalizarCuerpo(root.ChildNodes[0]);
                    AnalizarCuerpo(root.ChildNodes[1]);
                }
                else
                    AnalizarCuerpo(root.ChildNodes[0]);
            }
            else if (root.Term.Name.Equals("CONTENIDOGENERAL"))
            {
                //CONTENIDOGENERAL.Rule = DECLARA
                //                       | ASIGNA
                //                       | METODO;
                if (root.ChildNodes[0].Term.Name.Equals("DECLARA"))
                    AnalizarVariables(root.ChildNodes[0]);
                else if (root.ChildNodes[0].Term.Name.Equals("METODO"))
                    AnalizarMetodos(root.ChildNodes[0]);
            }
        }
        #endregion

        #region VARIABLES
        private static object AnalizarVariables(ParseTreeNode root, string H_AMBITO = "GLOBAL")
        {
            object result = null;

            if (root.Term.Name.Equals("DECLARA"))
            {
                DeclaracionVariable(root, H_AMBITO);
            }
            else if (root.Term.Name.Equals("ASIGNA"))
            {

            }
            return result;
        }

        private static void DeclaracionVariable(ParseTreeNode root, string H_AMBITO)
        {
            //DECLARA.Rule = id + TIPODATO + VALOR
            //             | LISTA_IDS + TIPODATO + VALOR
            //             | id + TIPODATO + E + VALOR;
            SingletonListas s = SingletonListas.GetInstance();

            if (root.ChildNodes.Count == 3)
            {
                List<Variable> vars;
                string tipoDato = ObtenerTipoDato(root.ChildNodes[1]);
                string tipoDatoValor = tipoDato;
                if (root.ChildNodes[2].Token != null)
                    tipoDatoValor = ObtenerTipoDatoValor(root.ChildNodes[2], H_AMBITO);

                if (root.ChildNodes[0].Token == null)
                {
                    vars = ObtenerListaId(root.ChildNodes[0], H_AMBITO);
                    foreach (Variable v in vars)
                        v.Tipo = tipoDato;
                }
                else
                {
                    vars = new List<Variable>()
                        {
                            new Variable(root.ChildNodes[0].Token.ValueString, tipoDato, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column)
                        };
                }

                int fila = vars[vars.Count - 1].Fila;
                int columna = vars[vars.Count - 1].Columna;
                string id = vars[vars.Count - 1].Id;

                if (tipoDato.Equals(tipoDatoValor))
                    s.AlmacenarVariable(vars, H_AMBITO);
                else
                    s.NuevoError(H_AMBITO, id, fila, columna, "Tipo de datos no coinciden");
            }
            else
            {

            }
        }

        private static List<Variable> ObtenerListaId(ParseTreeNode root, string H_AMBITO)
        {
            //LISTA_IDS.Rule = LISTA_IDS + id
            //                | id;
            SingletonListas s = SingletonListas.GetInstance();
            if (root.ChildNodes.Count == 2)
            {
                List<Variable> vars = ObtenerListaId(root.ChildNodes[0], H_AMBITO);
                Variable v = new Variable()
                {
                    Indice = 0,
                    Ambito = H_AMBITO,
                    Columna = root.ChildNodes[1].Token.Location.Column,
                    Fila = root.ChildNodes[1].Token.Location.Line,
                    Id = root.ChildNodes[1].Token.ValueString
                };
                if (s.VerificarId(v, H_AMBITO))
                    vars.Add(v);
                else
                    s.NuevoError(v.Ambito, v.Id, v.Fila, v.Columna, $"Ya se encuentra declarada la variable {v.Id}");

                return vars;
            }
            else
            {
                List<Variable> vars = new List<Variable>();
                Variable v = new Variable()
                {
                    Indice = 0,
                    Ambito = H_AMBITO,
                    Columna = root.ChildNodes[0].Token.Location.Column,
                    Fila = root.ChildNodes[0].Token.Location.Line,
                    Id = root.ChildNodes[0].Token.ValueString
                };
                if (s.VerificarId(v, H_AMBITO))
                    vars.Add(v);
                else
                    s.NuevoError(v.Ambito, v.Id, v.Fila, v.Columna, $"Ya se encuentra declarada la variable {v.Id}");

                return vars;
            }
        }

        private static void ObtenerListaArray(ParseTreeNode root)
        {
            //LISTA_ARRAY.Rule = LISTA_ARRAY + coma + C
            //                | C;

        }

        private static void AsignacionVariable(ParseTreeNode root, string H_AMBITO = "GLOBAL")
        {
            //ASIGNA.Rule = id + igual1 + C + fin
            //            | id + igual1 + alla + LISTA_ARRAY + clla + fin
            //            | id + acor + E + ccor + igual1 + C + fin
            //            | id + acor + E + ccor + igual1 + id + acor + E + ccor + fin;

        }

        private static string ObtenerTipoDato(ParseTreeNode root)
        {
                //TIPODATO.Rule = rint
                //              | rdouble
                //              | rstring
                //              | rchar
                //              | rbool
                //              | rvoid;
                return root.ChildNodes[0].Token.ValueString.ToLower();
        }

        private static string ObtenerTipoDatoValor(ParseTreeNode root, string H_AMBITO)
        {
            //VALOR.Rule = C
            //           |
            //           | LISTA_ARRAY;
            if (root.ChildNodes[0].Term.Name.Equals("C"))
                return AnalizarOperacion(root.ChildNodes[0], H_AMBITO);
            else if (root.ChildNodes[0].Term.Name.Equals("LISTA_ARRAY"))
                return "";
            return "";
        }
        #endregion

        #region OPERACIONES
        private static string AnalizarOperacion(ParseTreeNode root, string H_AMBITO)
        {
            string result = null;

            if (root.Term.Name.Equals("OP"))
            {
                //OP.Rule = incremento | decremento;
            }
            else if (root.Term.Name.Equals("C"))
            {
                //C.Rule = C + L + C
                //       | E + R + E
                //       | menos + E
                //       | E;
                result = ObtenerC(root, H_AMBITO);
            }
            return result;
        }

        private static string ObtenerC(ParseTreeNode root, string H_AMBITO)
        {
            string result;

            if (root.ChildNodes.Count == 3)
            {
                if (root.ChildNodes[1].Term.Name.Equals("C"))
                {
                    //C + L + C
                    string valor1 = ObtenerC(root.ChildNodes[0], H_AMBITO);
                    string valor2 = ObtenerC(root.ChildNodes[2], H_AMBITO);
                    if (EnumTipoDato(valor1) == -1 || EnumTipoDato(valor2) == -1)
                        return "";
                    result = OperarLogico(valor1, valor2, root.ChildNodes[1]);
                }
                else
                {
                    //E + R + E
                    string valor1 = ObtenerE(root.ChildNodes[0], H_AMBITO);
                    string valor2 = ObtenerE(root.ChildNodes[2], H_AMBITO);
                    if (EnumTipoDato(valor1) == -1 || EnumTipoDato(valor2) == -1)
                        return "";
                    result = OperarLogico(valor1, valor2, root.ChildNodes[1]);
                }
            }
            if (root.ChildNodes.Count == 2)
            {
                //menos + E
                string valor = ObtenerE(root.ChildNodes[1], H_AMBITO);
                if (EnumTipoDato(valor) == -1)
                    return "";
                result = NegarE(root.ChildNodes[0], valor, H_AMBITO);
            }
            else
            {
                //E;
                result = ObtenerE(root.ChildNodes[0], H_AMBITO);
            }
            return result;
        }

        private static string ObtenerE(ParseTreeNode root, string H_AMBITO)
        {
            if (root.ChildNodes.Count == 3)
            {
                //E.Rule = E + mas + E
                //       | E + menos + E
                //       | E + por + E
                //       | E + dividir + E
                //       | E + modulo + E
                //       | E + potencia + E
                string valor1 = ObtenerE(root.ChildNodes[0], H_AMBITO);
                string valor2 = ObtenerE(root.ChildNodes[2], H_AMBITO);
                int a = EnumTipoDato(valor1);
                int b = EnumTipoDato(valor2);
                int columna = root.ChildNodes[1].Token.Location.Column;
                int fila = root.ChildNodes[1].Token.Location.Line;

                if (a == -1 || b == -1)
                    return "";

                if (root.ChildNodes[1].Token.ValueString.Equals("+"))
                    return SUMA[a,b];
                if (root.ChildNodes[1].Token.ValueString.Equals("-"))
                    return RESTA[a,b];
                if (root.ChildNodes[1].Token.ValueString.Equals("*"))
                    return MULTIPLICACION[a, b];
                if (root.ChildNodes[1].Token.ValueString.Equals("/"))
                    return DIVISION[a, b];
                if (root.ChildNodes[1].Token.ValueString.Equals("%"))
                    return MODULO[a, b];
                if (root.ChildNodes[1].Token.ValueString.Equals("^"))
                    return POTENCIA[a, b];
            }
            else
            {
                //E
                if (root.ChildNodes[0].Token == null)
                    return ObtenerE(root.ChildNodes[0], H_AMBITO);
                //rtrue
                if (root.ChildNodes[0].Token.ValueString.ToLower().Equals("true"))
                    return "boolean";
                //rfalse
                if (root.ChildNodes[0].Token.ValueString.ToLower().Equals("false"))
                    return "boolean";
                if (root.ChildNodes[0].Term.Name.Equals("id"))
                {
                    //id
                    SingletonListas s = SingletonListas.GetInstance();
                    Variable v = s.GetVariable(root.ChildNodes[0].Token.ValueString, H_AMBITO);
                    if (v != null)
                        return v.Tipo;
                    else
                        s.NuevoError("Semántico", root.ChildNodes[0].Token.ValueString, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, $"La variable {root.ChildNodes[0].Token.ValueString} no se encuentra declarada");
                }
                //numentero
                //numdecimal
                //cadena
                //caracter
                return root.ChildNodes[0].Term.Name.ToLower();
            }

            return "";
        }
        
        private static string OperarLogico(string valor1, string valor2, ParseTreeNode node)
        {
            int a = EnumTipoDato(valor1);
            int b = EnumTipoDato(valor2);

            if (a != EnumTipoDato("boolean") && b != EnumTipoDato("boolean"))
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = node.Token.Location.Column,
                    Fila = node.Token.Location.Line,
                    Fuente = node.Token.ValueString,
                    Comentario = $"El operador {node.Token.ValueString} solo admite bool"
                };
                s.Errores.Add(e);
                return "";
            }

            return "boolean";
        }

        private static string OperarRelacional(string valor1, string valor2, ParseTreeNode node, string H_AMBITO)
        {
            if (valor1.Equals(valor2))
                return "boolean";
            else
            {
                SingletonListas s = SingletonListas.GetInstance();
                s.NuevoError(H_AMBITO, node.Token.ValueString, node.Token.Location.Line, node.Token.Location.Column, $"El operador {node.Token.ValueString} no admite comparación {valor1} y {valor2}");
                return "";
            }
        }
                
        private static string NegarE(ParseTreeNode root, string valor, string H_AMBITO)
        {
            // -E
            int tipo = EnumTipoDato(valor);
            if (tipo == EnumTipoDato("int") || tipo == EnumTipoDato("double") || tipo == EnumTipoDato("boolean"))
                return valor;
            SingletonListas s = SingletonListas.GetInstance();
            s.NuevoError(H_AMBITO, root.Token.ValueString, root.Token.Location.Line, root.Token.Location.Column, $"No se puede negar {valor}");
            return "";
        }
        #endregion

        #region METODOS
        private static void AnalizarMetodos(ParseTreeNode root)
        {

        }
        #endregion

        #region SENTENCIAS
        private static void AnalizarSentencias(ParseTreeNode root)
        {

        }
        #endregion
    }
}
