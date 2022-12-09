using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1oop
{
    internal class Result
    {
        public double val;
        public Parser.Errors Code;

        public Result()
        {
            val = 0.0;
            Code = Parser.Errors.NOERR;
        }

        public Result(double v, Parser.Errors c){
            val=v;
            Code = c;
        }

        public bool except()
        {
            return (Code==Parser.Errors.NOERR);
        }

        public string GetVal()
        {
            switch (Code)
            {
                case Parser.Errors.NOERR: return val.ToString();
                case Parser.Errors.DIVBYZERO:
                    {
                        MessageBox.Show("Ділення на нуль неможливе, формула змінена на нуль");
                        return "Error";
                    }
                case Parser.Errors.NOEXP: 
                    {
                        MessageBox.Show("Немає формули");
                        return "Error"; 
                    }
                case Parser.Errors.UNBALPARENS: 
                    {
                        MessageBox.Show("Незакриті дужки");
                        return "Error"; 
                    }
                case Parser.Errors.SYNTAX: 
                    {
                        MessageBox.Show("Помилка в написанні формули або посилання на помилку");
                        return "Error"; 
                    }
            }
            return "";
        }
    }
}
