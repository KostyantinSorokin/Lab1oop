using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1oop
{
    internal class AddNewCR
    {
        const int b = 65;
        const int alph = 26;
        char latter = 'A';
        char firstlat = 'A';
        int nfirstlat = 1;
        string temp;

        public int addCol(DataGridView dgv, int countCol)
        {
            if (countCol < alph)
            {
                int name = b + countCol;
                latter = (char)name;
                temp += latter;
            }
            else
            {
                MessageBox.Show("Max emount of Columns");
                return 0;
            }

            DataGridViewColumn col = (DataGridViewColumn)dgv.Columns[0].Clone();
            col.HeaderCell.Value = temp;

            dgv.Columns.Add(col);
            temp = null;

            if(firstlat != 'Z')
            {
                if(latter != 'Z')
                {
                    latter++;
                }
                else
                {
                    latter = 'A';
                    firstlat++;
                }
            }
            else
            {
                firstlat = 'A';
                nfirstlat++; 
            }
            return 0;
        }
        public int addRow(DataGridView dgv, int countRow) {
            int r = countRow-1;
            if (countRow > 9)
            {
                MessageBox.Show("Max emount of Rows");
                return 0;
            }
            countRow++;
            string t = null;
            t += r;
            DataGridViewRow row = (DataGridViewRow)dgv.Rows[r].Clone();            
            dgv.Rows[r].HeaderCell.Value = (countRow-1).ToString();
            dgv.Rows.Add(row);

            for (int i = 0; i < dgv.ColumnCount; i++) {
                dgv.Rows[r ].Cells[i].Value = dgv.Rows[r+1].Cells[i].Value;
                dgv.Rows[r+1].Cells[i].Value = "";
            }

            return 0;
        }
    }
}
