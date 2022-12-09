using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1oop
{
    public class Cell : DataGridViewTextBoxCell
    {
        public string val;
        public string exp;

        public List<string> dependoncells = new List<string>();
        public List<string> dependfromCells = new List<string>();

        public Cell()
        {
            val = "0";
            exp = "0";
        }

        public string setName(int col, int row){
            string temp = null;
            temp += (char)(col + 65);
            temp += (char)(row + 48);
            return temp;
        }
    }
}
