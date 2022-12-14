using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1oop
{
    internal class GiveName
    {
        public string SetName(int col, int row)
        {
            string temp = null;
            temp += (char)(col + 65);
            temp += (char)(row + 48);
            return temp;
        }
    }
}
