using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1oop
{
    public class Cell : DataGridViewTextBoxCell
    {
        public string Val;
        public string Exp;

        public List<string> dependoncells = new List<string>();

        public Cell()
        {
            Val = "0";
            Exp = "0";
        }

    }
}
