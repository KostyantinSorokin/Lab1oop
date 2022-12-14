using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab1oop
{
    internal class Parser
    {
        const int alph = 26;
        enum Types { NONE, DELIMITR, VARIABLE, NUMBER};
        public enum Errors { NOERR, SYNTAX, UNBALPARENS, NOEXP, DIVBYZERO };
        public Errors TokErrors;
        private string exp;
        private int expIdx;
        private string token;

        private Types tokType;
        private double[] vars= new double[alph];

        public Parser()
        {
            for(int i=0; i < alph; i++)
            {
                vars[i]=0.0;
            }
        }

        bool IsDelim(char c)
        {
            if("+-*/^()|%".IndexOf(c) != -1)
            {
                return true;
            }
            return false;
        }

        void getToken()
        {
            tokType = Types.NONE;

            token = "";
            if (expIdx == exp.Length) return;
            while (expIdx < exp.Length && char.IsWhiteSpace(exp[expIdx])) expIdx++;
            if (expIdx == exp.Length) return;
            if (IsDelim(exp[expIdx]))
            {
                token+=exp[expIdx];
                expIdx++;
                tokType = Types.DELIMITR;
            }
            else if (Char.IsLetter(exp[expIdx]))
            {
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }
                tokType = Types.VARIABLE;
            }
            else if (Char.IsDigit(exp[expIdx])){
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }
                tokType = Types.NUMBER;
            }
        }

        void atom(out double result)
        {
            switch (tokType)
            {
                case Types.NUMBER:
                    
                    try
                    {
                        result = double.Parse(token);
                    }
                    catch (FormatException)
                    {
                        result = 0.0;
                        TokErrors = Errors.SYNTAX;
                    }
                    getToken();
                    return;

                case Types.VARIABLE:

                    result = FindVar(token);
                    getToken();
                    return;
                
                default:
                    result = 0.0;
                    TokErrors = Errors.SYNTAX;
                    break;
            }
        }

        double FindVar(string valName)
        {
            if (valName.Contains("max{"))
            {
                string t1 = null;
                for (int i = 4; i < valName.Length - 1; i++)
                {
                    t1 += valName[i];
                }
                string[] ar1 = t1.Split(',');
                double[] ar2 = new double[ar1.Length];
                ar2[0] = Convert.ToDouble(ar1[0]);
                double m = ar2[0];
                for (int i = 1; i < ar2.Length; i++)
                {
                    ar2[i] = Convert.ToDouble(ar1[i]);
                    if (ar2[i] > m)
                    {
                        m = ar2[i];
                    }
                }
                return m;
            }
            if (valName.Contains("min{"))
            {
                string t1 = null;
                for (int i = 4; i < valName.Length-1; i++)
                {
                    t1 += valName[i];
                }
                string[] ar1 = t1.Split(',');
                double[] ar2 = new double[ar1.Length];
                ar2[0] = Convert.ToDouble(ar1[0]);
                double m = ar2[0];
                for (int i = 1; i < ar2.Length; i++)
                {
                    ar2[i] = Convert.ToDouble(ar1[i]);
                    if (ar2[i] < m)
                    {
                        m = ar2[i];
                    }
                }
                return m;
            }
            TokErrors = Errors.SYNTAX;
            return 0;
        }
        void EvalExp1(out double result)
        {
            int varIdx;
            Types TtokType;
            string tempToken;

            if(tokType == Types.VARIABLE)
            {
                tempToken = String.Copy(token);
                TtokType = tokType;
                varIdx = Char.ToUpper(token[0])-'A';
                getToken();
                if(token != "=")
                {
                    PutBack();
                    token= String.Copy(tempToken);
                    tokType = TtokType;
                }
                else
                {
                    getToken();
                    EvalExp2(out result);
                    vars[varIdx] = result;
                    return;
                }
            }
            EvalExp2(out result);
        }

        void PutBack()
        {
            for (int i = 0; i < token.Length; i++) expIdx--;
        }

        void EvalExp2(out double result)
        {
            string op;
            double partResult;

            EvalExp3(out result);
            while((op=token)== "+" || op == "-")
            {
                getToken();
                EvalExp3(out partResult);
                switch (op) {
                    case "-":
                        result = result - partResult;
                        break;
                    case "+":
                        result = result + partResult;
                        break;
                }
            }
        }

        void EvalExp3(out double result)
        {
            string op;
            double partResult;

            EvalExp4(out result);
            while((op = token)=="*" || op== "/" || op == "%"|| op == "|")
            {
                getToken();
                EvalExp4(out partResult);
                switch (op)
                {
                    case "*":
                        result=result * partResult;
                        break;
                    case "/":
                        if (partResult == 0.0)
                        {
                            TokErrors = Errors.DIVBYZERO;
                            result = partResult;
                        }else
                            result=result / partResult;
                        break;
                    case "%":
                        if (partResult == 0.0)
                        {
                            TokErrors = Errors.DIVBYZERO;
                            result = partResult;
                        }
                        else
                            result = (int)result % (int)partResult;
                        break;
                    case "|":
                        if (partResult == 0.0)
                        {
                            TokErrors = Errors.DIVBYZERO;
                        }
                        else 
                            result=(double)((int)result / (int)partResult);
                        break;
                }
                
            }

        }
        void EvalExp4(out double result)
        {
            double partResult, ex;
            int t;

            EvalExp5(out result);
            if (token == "^")
            {
                getToken();
                EvalExp4(out partResult);
                ex = result;
                if (partResult == 0.0)
                {
                    result = 1.0;
                    return;
                }
                for (t = (int)partResult - 1; t > 0; t--)
                {
                    result = result * (double)ex;
                }
            }
        }

        void EvalExp5(out double result)
        {
            if(token == "(")
            {
                getToken();
                EvalExp2(out result);
                if (token != ")")
                    TokErrors = Errors.UNBALPARENS;
                getToken();
            }
            else
                atom(out result);
        }

        public Result Eval(string expstr)
        {
            double result;
            exp = expstr;
            expIdx = 0;
            TokErrors = Errors.NOERR;

            getToken();
            if(token == "") {
                TokErrors = Errors.NOEXP;
                return new Result(0.0, TokErrors);
            }
            EvalExp1(out result);
            if (token != "")
                TokErrors = Errors.SYNTAX;
            return new Result(result, TokErrors);
        }
    }
}
