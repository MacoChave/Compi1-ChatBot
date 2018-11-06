using System;
using System.Collections.Generic;
using ChatBot.Objeto;
using Irony.Parsing;

namespace ChatBot.Compilador
{
    class Analizador : Grammar
    {
        /****************************************************************************
         *************************     ANALIZADOR LÉXICO     ************************
         ***********************     ANALIZADOR SINTÁCTIO     ***********************
         ****************************************************************************/
        public static ParseTreeNode Analizar (string cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree raiz = parser.Parse(cadena);

            return raiz.Root;
        }

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
        }

        /****************************************************************************
         ***********************     ANALIZADOR SEMÁNTICO     ***********************
         * Comprobar tipos de datos de variables y procedimientos
         ****************************************************************************/
        public static object AnalisisSemantico (ParseTreeNode root, object h1 = null)
        {
            object result = null;

            if (root.Term.Name.Equals("INICIO"))
            {
                //IMPORTES + CUERPO;
                AnalisisSemantico(root.ChildNodes[1]);
            }
            else if (root.Term.Name.Equals("IMPORTES"))
            {
                //IMPORTES.Rule = IMPORTES + IMPORTE
                //              | IMPORTE
                //              | Empty;
            }
            else if (root.Term.Name.Equals("IMPORTE"))
            {
                //IMPORTE.Rule = importar + importaciones + fin;
            }
            else if (root.Term.Name.Equals("CUERPO"))
            {
                if (root.ChildNodes.Count == 2)
                {
                    //CUERPO + CONTENIDOGENERAL
                    AnalisisSemantico(root.ChildNodes[0]);
                    AnalisisSemantico(root.ChildNodes[1]);
                }
                else
                {
                    //CONTENIDOGENERAL;
                    AnalisisSemantico(root.ChildNodes[0]);
                }
            }
            else if (root.Term.Name.Equals("CONTENIDOGENERAL"))
            {
                if (root.ChildNodes[0].Term.Name.Equals("DECLARA"))
                {
                    //DECLARA
                    List<Variable> lista = (List < Variable > )AnalisisSemantico(root.ChildNodes[0]);
                    foreach (Variable v in lista)
                        v.Ambito = "GENERAL";

                    SingletonListas sl = SingletonListas.GetInstance();
                    sl.Variables.AddRange(lista);
                }
                if (root.ChildNodes[0].Term.Name.Equals("ASIGNA"))
                {
                    //ASIGNA

                }
                if (root.ChildNodes[0].Term.Name.Equals("METODO"))
                {
                    //METODO
                    AnalisisSemantico(root.ChildNodes[0]);
                }
            }
            else if (root.Term.Name.Equals("DECLARA"))
            {
                List<Variable> list;
                string tipo = (string)AnalisisSemantico(root.ChildNodes[1]);

                if (root.ChildNodes.Count == 3)
                {
                    //LISTA_IDS + TIPODATO + VALOR
                    object valor;
                    if (root.ChildNodes[0].Token == null)
                        list = (List< Variable >)AnalisisSemantico(root.ChildNodes[0]);
                    else
                    {
                        //id + TIPODATO + VALOR
                        list = new List<Variable>();
                        Variable v = new Variable()
                        {
                            Id = root.ChildNodes[0].Token.ValueString,
                            Fila = root.ChildNodes[0].Token.Location.Line,
                            Columna = root.ChildNodes[0].Token.Location.Column
                        };
                        list.Add(v);
                    }

                    valor = AnalisisSemantico(root.ChildNodes[2]);
                    if (valor == null)
                {
                    list.Clear();
                    return list;
                }
                    foreach(Variable v in list)
                    {
                        v.Tipo = tipo;
                        v.Valor = valor;
                    }
                    if (!VerificarTipo(tipo, valor))
                    {
                        SingletonListas s = SingletonListas.GetInstance();
                        Error e = new Error()
                        {
                            Tipo = "Semántico",
                            Fuente = list[list.Count - 1].Id,
                            Columna = list[list.Count - 1].Columna,
                            Fila = list[list.Count - 1].Fila,
                            Comentario = $"No se puede convertir implícitamente el tipo {tipo} con {valor.GetType()}"
                        };
                        s.Errores.Add(e);
                        list.Clear();
                    }
                }
                else
                {
                    //id + TIPODATO + E + VALOR;
                    List<object> valor;
                    list = new List<Variable>();
                    string id = root.ChildNodes[0].Token.ValueString;
                    int tam = (int)AnalisisSemantico(root.ChildNodes[2]);

                    for (int i = 0; i < tam; i++)
                        list.Add(new Variable(tipo, i, id, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column));

                    valor = (List< object >)AnalisisSemantico(root.ChildNodes[3]);
                    if (valor.Count == 0) return list;
                    if (list.Count != valor.Count)
                    {
                        SingletonListas s = SingletonListas.GetInstance();
                        Error e = new Error()
                        {
                            Tipo = "Semántico",
                            Fuente = list[list.Count - 1].Id,
                            Columna = list[list.Count - 1].Columna,
                            Fila = list[list.Count - 1].Fila,
                            Comentario = $"Dimension incorrecta"
                        };
                        s.Errores.Add(e);
                        list.Clear();
                        return list;
                    }
                    foreach (object o in valor)
                    {
                        if (!VerificarTipo(tipo, o))
                        {
                            SingletonListas s = SingletonListas.GetInstance();
                            Error e = new Error()
                            {
                                Tipo = "Semántico",
                                Fuente = list[list.Count - 1].Id,
                                Columna = list[list.Count - 1].Columna,
                                Fila = list[list.Count - 1].Fila,
                                Comentario = $"No se puede convertir implícitamente el tipo {tipo} con {o.GetType()}"
                            };
                            s.Errores.Add(e);
                            list.Clear();
                            return list;
                        }

                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Valor = valor[i];
                        list[i].Tipo = tipo;
                    }
                }

                
                result = list;
            }
            else if (root.Term.Name.Equals("ASIGNA"))
            {
                //id + C
                //id + LISTA_ARRAY
                //id + E + C
                //id + E + id + E
            }
            else if (root.Term.Name.Equals("VALOR"))
            {
                if (root.ChildNodes[0].Term.Name.Equals("C"))
                {
                    //C
                    result = ObtenerC(root.ChildNodes[0]);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("LISTA_ARRAY"))
                {
                    //LISTA_ARRAY
                    result = AnalisisSemantico(root.ChildNodes[0]);
                }
            }
            else if (root.Term.Name.Equals("LISTA_IDS"))
            {
                if (root.ChildNodes.Count == 2)
                {
                    //LISTA_IDS + id
                    List<Variable> list = (List < Variable > )AnalisisSemantico(root.ChildNodes[0]);
                    list.Add(new Variable(root.ChildNodes[1].Token.ValueString, root.ChildNodes[1].Token.Location.Line, root.ChildNodes[1].Token.Location.Column));
                    result = list;
                }
                else
                {
                    //id;
                    List<Variable> list = new List<Variable>();
                    list.Add(new Variable(root.ChildNodes[0].Token.ValueString, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column));
                    result = list;
                }
            }
            else if (root.Term.Name.Equals("LISTA_ARRAY"))
            {
                if (root.ChildNodes.Count == 2)
                {
                    //LISTA_ARRAY + C
                    List<object> list = (List< object >)AnalisisSemantico(root.ChildNodes[0]);
                    list.Add(root.ChildNodes[1]);
                    result = list;
                }
                else
                {
                    //C;
                    List<object> list = new List<object>();
                    list.Add(ObtenerC(root.ChildNodes[0]));
                    result = list;
                }
            }
            else if (root.Term.Name.Equals("TIPODATO"))
            {
                return root.ChildNodes[0].Token.ValueString.ToLower();
            }
            else if (root.Term.Name.Equals("METODO"))
            {
                //id + TIPODATO + LISTAPARAMETROS + SENTENCIAS
                //rmain + TIPODATO + LISTAPARAMETROS + SENTENCIAS;
            }
            else if (root.Term.Name.Equals("LISTAPARAMETROS"))
            {
                //LISTAPARAMETROS + id + TIPODATO
                //id + TIPODATO
                //Empty;
            }
            else if (root.Term.Name.Equals("SENTENCIAS"))
            {
                //SENTENCIAS + SENTENCIA
                //SENTENCIA;
            }
            else if (root.Term.Name.Equals("SENTENCIA"))
            {
                //ASIGNA
                //DECLARA
                //LLAMADAMETODO
                //IMPRIMIR
                //SENTENCIAFOR
                //SENTENCIAIF
                //SENTENCIARETURN
                //SENTENCIAWHILE
                //SENTENCIADOWHILE
                //SENTENCIASWITCH
                //Empty;
            }
            else if (root.Term.Name.Equals("LLAMADAMETODO"))
            {
                //id + PARAMETROSLLAMADOS
                //id;
            }
            else if (root.Term.Name.Equals("PARAMETROSLLAMADOS"))
            {
                //PARAMETROSLLAMADOS + C
                //C;
            }
            else if (root.Term.Name.Equals("IMPRIMIR"))
            {
                //rprint + C;
            }
            else if (root.Term.Name.Equals("SENTENCIARETURN"))
            {
                //C
                //;
            }
            else if (root.Term.Name.Equals("SENTENCIAFOR"))
            {
                //rfor + id + TIPODATO + E + C + OP + SENTENCIAS
            }
            else if (root.Term.Name.Equals("SENTENCIAIF"))
            {
                //rif + SENTENCIAIFAUX;
            }
            else if (root.Term.Name.Equals("SENTENCIAIFAUX"))
            {
                //C + SENTENCIAS + SENTENCIAELSEIF;
            }
            else if (root.Term.Name.Equals("SENTENCIAELSEIF"))
            {
                //relse + SENTPRIMA
                //Empty;
            }
            else if (root.Term.Name.Equals("SENTPRIMA"))
            {
                //rif + SENTENCIAIFAUX
                //SENTENCIAS;
            }
            else if (root.Term.Name.Equals("SENTENCIAWHILE"))
            {
                //rwhile + C + SENTENCIAS;
            }
            else if (root.Term.Name.Equals("SENTENCIADOWHILE"))
            {
                //rdo + SENTENCIAS + rwhile + C;
            }
            else if (root.Term.Name.Equals("SENTENCIASWITCH"))
            {
                //rswitch + E + SENTENCIAS;
            }
            else if (root.Term.Name.Equals("CONTENIDOSWITCH"))
            {
                //CASOS + DEFECTO
                //CASOS
                //DEFECTO
                //Empty;
            }
            else if (root.Term.Name.Equals("CASOS"))
            {
                //CASOS + CASO
                //CASO;
            }
            else if (root.Term.Name.Equals("CASO"))
            {
                //rcase + EXPRESION + SENTENCIAS + rbreak;
            }
            else if (root.Term.Name.Equals("DEFECTO"))
            {
                //defecto + SENTENCIAS;
            }
            else if (root.Term.Name.Equals("ASIGNACION_CORTO"))
            {
                //id + OP;
                bool incremento = (bool)AnalisisSemantico(root.ChildNodes[1]);
                if (incremento)
                {
                    // OBTENER INCREMENTO DE VARIABLE
                }
                else
                {
                    // OBTENER DECREMENTO DE VARIABLE
                }
            }
            else if (root.Term.Name.Equals("OP"))
            {
                //incremento | decremento;
                if (root.ChildNodes[0].Token.ValueString.Equals("++"))
                    result = true;
                else
                    return false;
            }
            else if (root.Term.Name.Equals("INVOCAR"))
            {
                //id + LIST_ATRIBUTO
                //ACCESO_VECTOR;
            }
            else if (root.Term.Name.Equals("LIST_ATRIBUTO"))
            {
                //LIST_ATRIBUTO + ATRIBUTO
                //ATRIBUTO
                //Empty;
            }
            else if (root.Term.Name.Equals("ATRIBUTO"))
            {
                //E;
            }

            return result;
        }
        
        private static object NegarE(ParseTreeNode root)
        {
            // -E
            object result = ObtenerE(root);
            if (result is int)
                return (int)result * -1;
            if (result is bool)
                return !(bool)result;
            if (result is double)
                return (double)result * -1;

            SingletonListas s = SingletonListas.GetInstance();
            Error e = new Error()
            {
                Tipo = "Semántico",
                Columna = root.ChildNodes[0].Token.Location.Column,
                Fila = root.ChildNodes[0].Token.Location.Line,
                Fuente = "-",
                Comentario = $"{result.ToString()} tiene tipo incompatible, no se puede negar."
            };
            return null;
        }

        private static object ObtenerC(ParseTreeNode root)
        {
            object result;

            if (root.ChildNodes.Count == 3)
            {
                if (root.ChildNodes[1].Term.Name.Equals("C"))
                {
                    //C + L + C
                    object valor1 = ObtenerC(root.ChildNodes[0]);
                    object valor2 = ObtenerC(root.ChildNodes[2]);
                    result = OperarLogico(valor1, valor2, root.ChildNodes[1]);
                }
                else
                {
                    //E + R + E
                    object valor1 = ObtenerE(root.ChildNodes[0]);
                    object valor2 = ObtenerE(root.ChildNodes[2]);
                    result = OperarLogico(valor1, valor2, root.ChildNodes[1]);
                }
            }
            if (root.ChildNodes.Count == 2)
            {
                //menos + E
                result = NegarE(root.ChildNodes[1]);
            }
            else
            {
                //E;
                try
                {
                    result = ObtenerE(root.ChildNodes[0]);
                }
                catch (Exception ex)
                {
                    result = null;
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Columna = 0,
                        Fila = 0,
                        Fuente = ex.Source,
                        Tipo = "Semántico",
                        Comentario = ex.Message
                    };
                    s.Errores.Add(e);
                }
            }
            return result;
        }

        private static object ObtenerE(ParseTreeNode root)
        {
            if (root.ChildNodes.Count == 3)
            {
                object valor1 = ObtenerE(root.ChildNodes[0]);
                object valor2 = ObtenerE(root.ChildNodes[2]);
                int columna = root.ChildNodes[1].Token.Location.Column;
                int fila = root.ChildNodes[1].Token.Location.Line;
                if (valor1 == null || valor2 == null)
                    return null;
                if (root.ChildNodes[1].Token.ValueString.Equals("+"))
                    return OperadorSuma(valor1, valor2, columna, fila);
                if (root.ChildNodes[1].Token.ValueString.Equals("-"))
                    return OperadorResta(valor1, valor2, columna, fila);
                if (root.ChildNodes[1].Token.ValueString.Equals("*"))
                    return OperadorPor(valor1, valor2, columna, fila);
                if (root.ChildNodes[1].Token.ValueString.Equals("/"))
                    return OperadorDiv(valor1, valor2, columna, fila);
                if (root.ChildNodes[1].Token.ValueString.Equals("%"))
                    return OperadorMod(valor1, valor2, columna, fila);
                if (root.ChildNodes[1].Token.ValueString.Equals("^"))
                    return OperadorPot(valor1, valor2, columna, fila);
            }
            else
            {
                //E
                if (root.ChildNodes[0].Token == null)
                    return ObtenerE(root.ChildNodes[0]);
                //rtrue
                if (root.ChildNodes[0].Token.ValueString.ToLower().Equals("true"))
                    return true;
                //rfalse
                if (root.ChildNodes[0].Token.ValueString.ToLower().Equals("false"))
                    return false;
                if (root.ChildNodes[0].Term.Name.Equals("id"))
                {
                    //id
                    SingletonListas temp = SingletonListas.GetInstance();
                    return temp.getValue(root.ChildNodes[0].Token.ValueString);
                }
                //numentero
                if (root.ChildNodes[0].Term.Name.ToLower().Equals("int"))
                    return int.Parse(root.ChildNodes[0].Token.ValueString);
                //numdecimal
                if (root.ChildNodes[0].Term.Name.ToLower().Equals("double"))
                    return double.Parse(root.ChildNodes[0].Token.ValueString);
                //cadena
                if (root.ChildNodes[0].Term.Name.ToLower().Equals("string"))
                    return root.ChildNodes[0].Token.ValueString;
                //caracter
                if (root.ChildNodes[0].Term.Name.ToLower().Equals("char"))
                    return char.Parse(root.ChildNodes[0].Token.ValueString);
            }

            return null;
        }

        private static object OperadorSuma(object valor1, object valor2, int columna, int fila)
        {
            if (valor1 is string || valor2 is string)
                return valor1.ToString() + valor2.ToString();
            if (valor1 is int || valor2 is int)
            {
                if (valor1 is double)
                    return (double)valor1 + (int)valor2;
                if (valor2 is double)
                    return (int)valor1 + (double)valor2;
                if (valor1 is char)
                    return (char)valor1 + (int)valor2;
                if (valor2 is char)
                    return (int)valor1 + (char)valor2;
                if (valor1 is bool)
                    return getValueBool((bool)valor1) + (int)valor2;
                if (valor2 is bool)
                    return (int)valor1 + getValueBool((bool)valor2);
                return (int)valor1 + (int)valor2;
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return getValueBool((bool)valor1) + (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 + getValueBool((bool)valor2);
                if (valor1 is char)
                    return (char)valor1 + (double)valor2;
                if (valor2 is char)
                    return (double)valor1 + (char)valor2;
                return (double)valor1 + (double)valor2;
            }
            if (valor1 is char || valor2 is char)
            {
                if (valor1 is bool || valor2 is bool)
                {
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Tipo = "Semántico",
                        Columna = columna,
                        Fila = fila,
                        Fuente = "+",
                        Comentario = "El operador + no opera bool y char"
                    };
                    s.Errores.Add(e);
                    return null;
                }
                return (char)valor1 + (char)valor2;
            }
            if (valor1 is bool || valor2 is bool)
                return getValueBool((bool)valor1) + getValueBool((bool)valor2);

            return null;
        }

        private static object OperadorResta(object valor1, object valor2, int columna, int fila)
        {
            if (valor1 is string || valor2 is string)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "-",
                    Comentario = "El operador - no opera string"
                };
                s.Errores.Add(e);
                return null;
            }
            if (valor1 is int || valor2 is int)
            {
                if (valor1 is double)
                    return (double)valor1 - (int)valor2;
                if (valor2 is double)
                    return (int)valor1 - (double)valor2;
                if (valor1 is bool)
                    return getValueBool((bool)valor1) - (int)valor2;
                if (valor2 is bool)
                    return (int)valor1 - getValueBool((bool)valor2);
                if (valor1 is char)
                    return (char)valor1 - (int)valor2;
                if (valor2 is char)
                    return (int)valor1 - (char)valor2;
                return (int)valor1 - (int)valor2;
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return getValueBool((bool)valor1) - (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 - getValueBool((bool)valor2);
                if (valor1 is char)
                    return (char)valor1 - (double)valor2;
                if (valor2 is char)
                    return (double)valor1 - (char)valor2;
                return (double)valor1 - (double)valor2;
            }
            if (valor1 is char || valor2 is char)
            {
                if (valor1 is bool || valor2 is bool)
                {
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Tipo = "Semántico",
                        Columna = columna,
                        Fila = fila,
                        Fuente = "-",
                        Comentario = "El operador - no opera bool y char"
                    };
                    s.Errores.Add(e);
                    return null;
                }

                return (int)((char)valor1 - (char)valor2);
            }
            if (valor1 is bool || valor2 is bool)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "-",
                    Comentario = "El operador - no opera bool"
                };
                s.Errores.Add(e);
            }

            return null;
        }

        private static object OperadorPor(object valor1, object valor2, int columna, int fila)
        {
            if (valor1 is string || valor2 is string)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "*",
                    Comentario = "El operador * no opera string"
                };
                s.Errores.Add(e);
                return null;
            }
            if (valor1 is int || valor2 is int)
            {
                if (valor1 is double)
                    return (double)valor1 * (int)valor2;
                if (valor2 is double)
                    return (int)valor1 * (double)valor2;

                if (valor1 is bool)
                    return getValueBool((bool)valor1) * (int)valor2;
                if (valor2 is bool)
                    return (int)valor1 * getValueBool((bool)valor2);


                if (valor1 is char)
                    return (char)valor1 * (int)valor2;
                if (valor2 is char)
                    return (int)valor1 * (char)valor2;

                return (int)valor1 * (int)valor2;
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return getValueBool((bool)valor1) * (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 * getValueBool((bool)valor2);

                if (valor1 is char)
                    return (char)valor1 * (double)valor2;
                if (valor2 is char)
                    return (double)valor1 * (char)valor2;

                return (double)valor1 * (double)valor2;
            }
            if (valor1 is char || valor2 is char)
            {
                if (valor1 is bool || valor2 is bool)
                {
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Tipo = "Semántico",
                        Columna = columna,
                        Fila = fila,
                        Fuente = "*",
                        Comentario = "El operador * no opera bool y char"
                    };
                    s.Errores.Add(e);
                    return null;
                }
                return (int)((char)valor1 * (char)valor2);
            }
            if (valor1 is bool || valor2 is bool)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "*",
                    Comentario = "El operador * no opera bool"
                };
                s.Errores.Add(e);
            }
            return null;
        }

        private static object OperadorDiv(object valor1, object valor2, int columna, int fila)
        {
            if (valor1 is string || valor2 is string)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "/",
                    Comentario = "El operador / no opera string"
                };
                s.Errores.Add(e);
                return null;
            }
            if (valor1 is int || valor2 is int)
            {
                if (valor1 is double)
                    return (double)valor1 / (int)valor2;
                if (valor2 is double)
                    return (int)valor1 / (double)valor2;

                if (valor1 is bool)
                    return (double)(getValueBool((bool)valor1) / (int)valor2);
                if (valor2 is bool)
                    return (double)((int)valor1 / getValueBool((bool)valor2));


                if (valor1 is char)
                    return (double)((char)valor1 / (int)valor2);
                if (valor2 is char)
                    return (double)((int)valor1 / (char)valor2);

                return (double)((int)valor1 / (int)valor2);
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return getValueBool((bool)valor1) / (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 / getValueBool((bool)valor2);

                if (valor1 is char)
                    return (char)valor1 / (double)valor2;
                if (valor2 is char)
                    return (double)valor1 / (char)valor2;

                return (double)valor1 / (double)valor2;
            }
            if (valor1 is char || valor2 is char)
            {
                if (valor1 is bool || valor2 is bool)
                {
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Tipo = "Semántico",
                        Columna = columna,
                        Fila = fila,
                        Fuente = "/",
                        Comentario = "El operador / no opera bool y char"
                    };
                    s.Errores.Add(e);
                    return null;
                }
                return (double)((char)valor1 / (char)valor2);
            }
            if (valor1 is bool || valor2 is bool)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "/",
                    Comentario = "El operador / no opera bool"
                };
                s.Errores.Add(e);
            }
            return null;
        }

        private static object OperadorMod(object valor1, object valor2, int columna, int fila)
        {
            if (valor1 is string || valor2 is string)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "%",
                    Comentario = "El operador % no opera string"
                };
                s.Errores.Add(e);
                return null;
            }
            if (valor1 is int || valor2 is int)
            {
                if (valor1 is double)
                    return (double)valor1 % (int)valor2;
                if (valor2 is double)
                    return (int)valor1 % (double)valor2;

                if (valor1 is bool)
                    return (double)(getValueBool((bool)valor1) % (int)valor2);
                if (valor2 is bool)
                    return (double)((int)valor1 % getValueBool((bool)valor2));


                if (valor1 is char)
                    return (double)((char)valor1 % (int)valor2);
                if (valor2 is char)
                    return (double)((int)valor1 % (char)valor2);

                return (double)((int)valor1 % (int)valor2);
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return getValueBool((bool)valor1) % (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 % getValueBool((bool)valor2);

                if (valor1 is char)
                    return (char)valor1 % (double)valor2;
                if (valor2 is char)
                    return (double)valor1 % (char)valor2;

                return (double)valor1 % (double)valor2;
            }
            if (valor1 is char || valor2 is char)
            {
                if (valor1 is bool || valor2 is bool)
                {
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Tipo = "Semántico",
                        Columna = columna,
                        Fila = fila,
                        Fuente = "%",
                        Comentario = "El operador % no opera bool y char"
                    };
                    s.Errores.Add(e);
                    return null;
                }
                return (double)((char)valor1 % (char)valor2);
            }
            if (valor1 is bool || valor2 is bool)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "%",
                    Comentario = "El operador % no opera bool"
                };
                s.Errores.Add(e);
            }
            return null;
        }

        private static object OperadorPot(object valor1, object valor2, int columna, int fila)
        {
            if (valor1 is string || valor2 is string)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "^",
                    Comentario = "El operador ^ no opera string"
                };
                s.Errores.Add(e);
                return null;
            }
            if (valor1 is int || valor2 is int)
            {
                if (valor1 is double)
                    return Math.Pow((double)valor1, (int)valor2);
                if (valor2 is double)
                    return Math.Pow((int)valor1, (double)valor2);

                if (valor1 is char)
                    return Math.Pow((char)valor1, (int)valor2);
                if (valor2 is char)
                    return Math.Pow((int)valor1, (char)valor2);

                if (valor1 is bool)
                    return Math.Pow(getValueBool((bool)valor1), (int)valor2);
                if (valor2 is bool)
                    return Math.Pow((int)valor1, getValueBool((bool)valor2));

                return Math.Pow((int)valor1, (int)valor2);

              

              
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return Math.Pow(getValueBool((bool)valor1), (double)valor2);
                if (valor2 is bool)
                    return Math.Pow((double)valor1, getValueBool((bool)valor2));

                if (valor1 is char)
                    return Math.Pow((char)valor1, (double)valor2);
                if (valor2 is char)
                    return Math.Pow((double)valor1, (char)valor2);

                return Math.Pow((double)valor1, (double)valor2);
            }
            if (valor1 is char || valor2 is char)
            {
                if (valor1 is bool || valor2 is bool)
                {
                    SingletonListas s = SingletonListas.GetInstance();
                    Error e = new Error()
                    {
                        Tipo = "Semántico",
                        Columna = columna,
                        Fila = fila,
                        Fuente = "^",
                        Comentario = "El operador ^ no opera bool y char"
                    };
                    s.Errores.Add(e);
                    return null;
                }
                return Math.Pow((char)valor1, (char)valor2);
            }
            if (valor1 is bool || valor2 is bool)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = columna,
                    Fila = fila,
                    Fuente = "^",
                    Comentario = "El operador ^ no opera bool"
                };
                s.Errores.Add(e);
            }
            return null;
        }

        private static object OperarLogico(object valor1, object valor2, ParseTreeNode node)
        {
            if (!(valor1 is bool) && !(valor2 is bool))
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
                return null;
            }
            switch (node.Token.ValueString)
            {
                case "||":
                    return (bool)valor1 || (bool)valor2;
                case "&&":
                    return (bool)valor1 && (bool)valor2;
                case "^":
                    return (bool)valor1 ^ (bool)valor2;
                case "!":
                    return !(bool)valor1;
            }
            return null;
        }

        private static object OperarRelacional(object valor1, object valor2, ParseTreeNode node)
        {
            switch (node.Token.ValueString)
            {
                case "==":
                    if (valor1 is int && valor2 is int)
                        return ((int)valor1 == (int)valor2) ? 1 : 0;
                    if (valor1 is double && valor2 is double)
                        return ((double)valor1 == (double)valor2) ? 1 : 0;
                    if (valor1 is char && valor2 is char)
                        return ((char)valor1 == (char)valor2) ? 1 : 0;
                    if (valor1 is bool && valor2 is bool)
                        return ((bool)valor1 == (bool)valor2) ? 1 : 0;
                    break;
                case "!=":
                    if (valor1 is int && valor2 is int)
                        return ((int)valor1 != (int)valor2) ? 1 : 0;
                    if (valor1 is double && valor2 is double)
                        return ((double)valor1 != (double)valor2) ? 1 : 0;
                    if (valor1 is char && valor2 is char)
                        return ((char)valor1 != (char)valor2) ? 1 : 0;
                    if (valor1 is bool && valor2 is bool)
                        return ((bool)valor1 != (bool)valor2) ? 1 : 0;
                    break;
                case "<":
                    if (valor1 is int && valor2 is int)
                        return ((int)valor1 < (int)valor2) ? 1 : 0;
                    if (valor1 is double && valor2 is double)
                        return ((double)valor1 < (double)valor2) ? 1 : 0;
                    if (valor1 is char && valor2 is char)
                        return ((char)valor1 < (char)valor2) ? 1 : 0;
                    if (valor1 is bool && valor2 is bool)
                        return ((int)valor1 < (int)valor2) ? 1 : 0;
                    break;
                case "<=":
                    if (valor1 is int && valor2 is int)
                        return ((int)valor1 <= (int)valor2) ? 1 : 0;
                    if (valor1 is double && valor2 is double)
                        return ((double)valor1 <= (double)valor2) ? 1 : 0;
                    if (valor1 is char && valor2 is char)
                        return ((char)valor1 <= (char)valor2) ? 1 : 0;
                    if (valor1 is bool && valor2 is bool)
                        return ((int)valor1 <= (int)valor2) ? 1 : 0;
                    break;
                case ">":
                    if (valor1 is int && valor2 is int)
                        return ((int)valor1 > (int)valor2) ? 1 : 0;
                    if (valor1 is double && valor2 is double)
                        return ((double)valor1 > (double)valor2) ? 1 : 0;
                    if (valor1 is char && valor2 is char)
                        return ((char)valor1 > (char)valor2) ? 1 : 0;
                    if (valor1 is bool && valor2 is bool)
                        return ((int)valor1 > (int)valor2) ? 1 : 0;
                    break;
                case ">=":
                    if (valor1 is int && valor2 is int)
                        return ((int)valor1 >= (int)valor2) ? 1 : 0;
                    if (valor1 is double && valor2 is double)
                        return ((double)valor1 >= (double)valor2) ? 1 : 0;
                    if (valor1 is char && valor2 is char)
                        return ((char)valor1 >= (char)valor2) ? 1 : 0;
                    if (valor1 is bool && valor2 is bool)
                        return ((int)valor1 >= (int)valor2) ? 1 : 0;
                    break;
            }
            if (valor1 is string || valor2 is string)
            {
                SingletonListas s = SingletonListas.GetInstance();
                Error e = new Error()
                {
                    Tipo = "Semántico",
                    Columna = node.Token.Location.Column,
                    Fila = node.Token.Location.Line,
                    Fuente = node.Token.ValueString,
                    Comentario = $"El operador {node.Token.ValueString} no admite el tipo {valor1.GetType()} con {valor2.GetType()}"
                };
                s.Errores.Add(e);
                return null;
            }
            return null;
        }

        private static bool VerificarTipo(string tipo, object valor)
        {
            switch (tipo)
            {
                case "int":
                    if (!(valor is string))
                    {
                        if (valor is int)
                            return true;

                    }  return false;
                case "double":
                    if (!(valor is string))
                    {

                        if (valor is double)
                            return true;
                    }
                    return false; 
                case "string":
                    return valor is string;
                case "char":
                    return (valor is char) || (valor is int);
                case "boolean":
                    return (valor is bool) || (valor is int);
            }
            return false;
        }

        private static int getValueBool(bool valor)
        {
            return (valor) ? 1 : 0;
        }

    }
}
