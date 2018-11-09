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
            img.Save(nombre);
        }

        /****************************************************************************
         ***********************     ANALIZADOR SEMÁNTICO     ***********************
         * Comprobar tipos de datos de variables y procedimientos
         ****************************************************************************/
        public static object AnalisisSemantico (ParseTreeNode root, string H_AMBITO = "GLOBLAL", string H_TIPO_PROC = "void", bool H_FLUJO_CONTROL = false)
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
                    SingletonListas s = SingletonListas.GetInstance();
                    List<Variable> lista = (List < Variable > )AnalisisSemantico(root.ChildNodes[0]);
                    foreach (Variable v in lista)
                    {
                        v.Ambito = "GENERAL";
                        if (s.GetVarValue(v.Id) != null)
                        {
                            Error e = new Error()
                            {
                                Tipo = "Semántico",
                                Fuente = v.Id,
                                Columna = v.Columna,
                                Fila = v.Fila,
                                Comentario = $"{v.Id} ya se encuentra declarada"
                            };
                            s.Errores.Add(e);
                            lista.Remove(v);
                        }
                    }

                    s.AddVariableGlobal(lista);
                }
                if (root.ChildNodes[0].Term.Name.Equals("ASIGNA"))
                {
                    //ASIGNA

                }
                if (root.ChildNodes[0].Term.Name.Equals("METODO"))
                {
                    //METODO
                    Ambito ambito = (Ambito)AnalisisSemantico(root.ChildNodes[0]);
                    SingletonListas s = SingletonListas.GetInstance();
                    s.Ambitos.Add(ambito);
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
                            Columna = root.ChildNodes[0].Token.Location.Column,
                            Tipo = tipo,
                            Valor = DefaultValue(tipo)
                        };
                        list.Add(v);
                    }
                    
                    if (root.ChildNodes[2].ChildNodes.Count > 0)
                    {
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
                }
                else
                {
                    //id + TIPODATO + E + VALOR;
                    List<object> valor;
                    list = new List<Variable>();
                    string id = root.ChildNodes[0].Token.ValueString;
                    int tam = (int)AnalisisSemantico(root.ChildNodes[2]);

                    for (int i = 0; i < tam; i++)
                    {
                        Variable v = new Variable()
                        {
                            Indice = i,
                            Id = id,
                            Tipo = tipo,
                            Valor = DefaultValue(tipo),
                            Fila = root.ChildNodes[0].Token.Location.Line,
                            Columna = root.ChildNodes[0].Token.Location.Column,
                        };
                        list.Add(v);
                    }

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
                    result = ObtenerC(root.ChildNodes[0], H_AMBITO);
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
                    list.Add(ObtenerC(root.ChildNodes[0], H_AMBITO));
                    result = list;
                }
            }
            else if (root.Term.Name.Equals("TIPODATO"))
            {
                return root.ChildNodes[0].Token.ValueString.ToLower();
            }
            else if (root.Term.Name.Equals("METODO"))
            {
                //rmain + TIPODATO + LISTAPARAMETROS + SENTENCIAS;
                //id + TIPODATO + LISTAPARAMETROS + SENTENCIAS
                Ambito ambito = new Ambito()
                {
                    Fila = root.ChildNodes[0].Token.Location.Line,
                    Columna = root.ChildNodes[0].Token.Location.Column,
                    Id = root.ChildNodes[0].Token.ValueString.ToLower(),
                    Nombre = "METODO",
                    Sentencias = root.ChildNodes[3]
                };
                string tipo = AnalisisSemantico(root.ChildNodes[1]).ToString();
                List<Variable> parametros = (List < Variable > )AnalisisSemantico(root.ChildNodes[2]);
                List<Variable> variables = (List < Variable > )AnalisisSemantico(root.ChildNodes[3], H_AMBITO, tipo);

                ambito.Tipo = tipo;
                ambito.Parametros = parametros;
                ambito.Variables = variables;

                result = ambito;
            }
            else if (root.Term.Name.Equals("LISTAPARAMETROS"))
            {
                if (root.ChildNodes.Count == 3)
                {
                    //LISTAPARAMETROS + id + TIPODATO
                    List<Variable> parametros = (List < Variable > )AnalisisSemantico(root.ChildNodes[0]);
                    Variable v = new Variable()
                    {
                        Fila = root.ChildNodes[1].Token.Location.Line,
                        Columna = root.ChildNodes[1].Token.Location.Column,
                        Id = root.ChildNodes[1].Token.ValueString,
                        Indice = 0,
                        Ambito = H_AMBITO
                    };
                    string tipo = AnalisisSemantico(root.ChildNodes[2]).ToString();
                    v.Tipo = tipo;
                    v.Valor = DefaultValue(tipo);
                    parametros.Add(v);

                    result = parametros;
                }
                else if (root.ChildNodes.Count == 2)
                {
                    //id + TIPODATO
                    List<Variable> parametros = new List<Variable>();
                    Variable v = new Variable()
                    {
                        Fila = root.ChildNodes[0].Token.Location.Line,
                        Columna = root.ChildNodes[0].Token.Location.Column,
                        Id = root.ChildNodes[0].Token.ValueString,
                        Indice = 0,
                        Ambito = H_AMBITO
                    };
                    string tipo = AnalisisSemantico(root.ChildNodes[1]).ToString();
                    v.Tipo = tipo;
                    v.Valor = DefaultValue(tipo);
                    parametros.Add(v);

                    result = parametros;
                }
                else //Empty;
                    result = new List<Variable>();
            }
            else if (root.Term.Name.Equals("SENTENCIAS"))
            {
                if (root.ChildNodes.Count == 2)
                {
                    //SENTENCIAS + SENTENCIA
                    List<Variable> vars;
                    object res;
                    res = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC);
                    if (res != null)
                        vars = (List<Variable>)res;
                    else
                        vars = new List<Variable>();
                    res = AnalisisSemantico(root.ChildNodes[1], H_AMBITO, H_TIPO_PROC);
                    if (res != null)
                        vars.AddRange((List < Variable > )res);
                    result = vars;
                }
                else
                {
                    //SENTENCIA;
                    List<Variable> vars;
                    object res;
                    res = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC);
                    if (res != null)
                        vars = (List<Variable>)res;
                    else
                        vars = new List<Variable>();
                    result = vars;
                }
            }
            else if (root.Term.Name.Equals("SENTENCIA"))
            {
                if (root.ChildNodes[0].Term.Name.Equals("ASIGNA"))
                {
                    //ASIGNA
                }
                else if (root.ChildNodes[0].Term.Name.Equals("DECLARA"))
                {
                    //DECLARA
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("LLAMADAMETODO"))
                {
                    //LLAMADAMETODO
                }
                else if (root.ChildNodes[0].Term.Name.Equals("IMPRIMIR"))
                {
                    //IMPRIMIR
                }
                else if (root.ChildNodes[0].Term.Name.Equals("SENTENCIAFOR"))
                {
                    //SENTENCIAFOR
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC, true);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("SENTENCIAIF"))
                {
                    //SENTENCIAIF
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("SENTENCIARETURN"))
                {
                    //SENTENCIARETURN
                    AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("SENTENCIAWHILE"))
                {
                    //SENTENCIAWHILE
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC, true);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("SENTENCIADOWHILE"))
                {
                    //SENTENCIADOWHILE
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC, true);
                }
                else if (root.ChildNodes[0].Term.Name.Equals("SENTENCIASWITCH"))
                {
                    //SENTENCIASWITCH
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC, true);
                }
                else
                {
                    return new List<Variable>();
                }
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
                result = AnalisisSemantico(root.ChildNodes[6], H_AMBITO, H_TIPO_PROC, true);
            }
            else if (root.Term.Name.Equals("SENTENCIAIF"))
            {
                //rif + SENTENCIAIFAUX;
                result = AnalisisSemantico(root.ChildNodes[1], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
            }
            else if (root.Term.Name.Equals("SENTENCIAIFAUX"))
            {
                //C + SENTENCIAS + SENTENCIAELSEIF;
                List<Variable> vars = (List < Variable > )AnalisisSemantico(root.ChildNodes[1], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
                vars.AddRange((List < Variable > )AnalisisSemantico(root.ChildNodes[2], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL));

                result = vars;
            }
            else if (root.Term.Name.Equals("SENTENCIAELSEIF"))
            {
                //relse + SENTPRIMA
                //Empty;
                if (root.ChildNodes.Count == 2)
                    result = AnalisisSemantico(root.ChildNodes[1], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
                else
                    result = new List<Variable>();
            }
            else if (root.Term.Name.Equals("SENTPRIMA"))
            {
                //rif + SENTENCIAIFAUX
                //SENTENCIAS;
                if (root.ChildNodes.Count == 2)
                    result = AnalisisSemantico(root.ChildNodes[1], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
                else
                    result = AnalisisSemantico(root.ChildNodes[0], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
            }
            else if (root.Term.Name.Equals("SENTENCIAWHILE"))
            {
                //rwhile + C + SENTENCIAS;
                result = AnalisisSemantico(root.ChildNodes[2], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
            }
            else if (root.Term.Name.Equals("SENTENCIADOWHILE"))
            {
                //rdo + SENTENCIAS + rwhile + C;
                result = AnalisisSemantico(root.ChildNodes[1], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
            }
            else if (root.Term.Name.Equals("SENTENCIASWITCH"))
            {
                //rswitch + E + SENTENCIAS;
                result = AnalisisSemantico(root.ChildNodes[2], H_AMBITO, H_TIPO_PROC, H_FLUJO_CONTROL);
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

        private static object NegarE(ParseTreeNode root, string H_AMBITO)
        {
            // -E
            object result = ObtenerE(root, H_AMBITO);
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

        private static object ObtenerC(ParseTreeNode root, string H_AMBITO)
        {
            object result;

            if (root.ChildNodes.Count == 3)
            {
                if (root.ChildNodes[1].Term.Name.Equals("C"))
                {
                    //C + L + C
                    object valor1 = ObtenerC(root.ChildNodes[0], H_AMBITO);
                    object valor2 = ObtenerC(root.ChildNodes[2], H_AMBITO);
                    result = OperarLogico(valor1, valor2, root.ChildNodes[1]);
                }
                else
                {
                    //E + R + E
                    object valor1 = ObtenerE(root.ChildNodes[0], H_AMBITO);
                    object valor2 = ObtenerE(root.ChildNodes[2], H_AMBITO);
                    result = OperarLogico(valor1, valor2, root.ChildNodes[1]);
                }
            }
            if (root.ChildNodes.Count == 2)
            {
                //menos + E
                result = NegarE(root.ChildNodes[1], H_AMBITO);
            }
            else
            {
                //E;
                try
                {
                    result = ObtenerE(root.ChildNodes[0], H_AMBITO);
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

        private static object ObtenerE(ParseTreeNode root, string H_AMBITO)
        {
            if (root.ChildNodes.Count == 3)
            {
                object valor1 = ObtenerE(root.ChildNodes[0], H_AMBITO);
                object valor2 = ObtenerE(root.ChildNodes[2], H_AMBITO);
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
                    return ObtenerE(root.ChildNodes[0], H_AMBITO);
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
                    return temp.GetVarValue(root.ChildNodes[0].Token.ValueString, H_AMBITO);
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
                    return BoolToInt((bool)valor1) + (int)valor2;
                if (valor2 is bool)
                    return (int)valor1 + BoolToInt((bool)valor2);
                return (int)valor1 + (int)valor2;
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return BoolToInt((bool)valor1) + (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 + BoolToInt((bool)valor2);
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
                return BoolToInt((bool)valor1) + BoolToInt((bool)valor2);

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
                    return BoolToInt((bool)valor1) - (int)valor2;
                if (valor2 is bool)
                    return (int)valor1 - BoolToInt((bool)valor2);
                if (valor1 is char)
                    return (char)valor1 - (int)valor2;
                if (valor2 is char)
                    return (int)valor1 - (char)valor2;
                return (int)valor1 - (int)valor2;
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return BoolToInt((bool)valor1) - (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 - BoolToInt((bool)valor2);
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
                    return BoolToInt((bool)valor1) * (int)valor2;
                if (valor2 is bool)
                    return (int)valor1 * BoolToInt((bool)valor2);


                if (valor1 is char)
                    return (char)valor1 * (int)valor2;
                if (valor2 is char)
                    return (int)valor1 * (char)valor2;

                return (int)valor1 * (int)valor2;
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return BoolToInt((bool)valor1) * (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 * BoolToInt((bool)valor2);

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
                    return (double)(BoolToInt((bool)valor1) / (int)valor2);
                if (valor2 is bool)
                    return (double)((int)valor1 / BoolToInt((bool)valor2));


                if (valor1 is char)
                    return (double)((char)valor1 / (int)valor2);
                if (valor2 is char)
                    return (double)((int)valor1 / (char)valor2);

                return (double)((int)valor1 / (int)valor2);
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return BoolToInt((bool)valor1) / (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 / BoolToInt((bool)valor2);

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
                    return (double)(BoolToInt((bool)valor1) % (int)valor2);
                if (valor2 is bool)
                    return (double)((int)valor1 % BoolToInt((bool)valor2));


                if (valor1 is char)
                    return (double)((char)valor1 % (int)valor2);
                if (valor2 is char)
                    return (double)((int)valor1 % (char)valor2);

                return (double)((int)valor1 % (int)valor2);
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return BoolToInt((bool)valor1) % (double)valor2;
                if (valor2 is bool)
                    return (double)valor1 % BoolToInt((bool)valor2);

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
                    return Math.Pow(BoolToInt((bool)valor1), (int)valor2);
                if (valor2 is bool)
                    return Math.Pow((int)valor1, BoolToInt((bool)valor2));

                return Math.Pow((int)valor1, (int)valor2);

              

              
            }
            if (valor1 is double || valor2 is double)
            {
                if (valor1 is bool)
                    return Math.Pow(BoolToInt((bool)valor1), (double)valor2);
                if (valor2 is bool)
                    return Math.Pow((double)valor1, BoolToInt((bool)valor2));

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
        
        private static object DefaultValue(string tipo)
        {

            if (tipo.Equals("int"))
                return 0;
            if (tipo.Equals("double"))
                return 0.0;
            if (tipo.Equals("string"))
                return "";
            if (tipo.Equals("char"))
                return '\0';
            if (tipo.Equals("boolean"))
                return false;
            return 0;
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

        private static int BoolToInt(bool valor)
        {
            return (valor) ? 1 : 0;
        }

    }
}
