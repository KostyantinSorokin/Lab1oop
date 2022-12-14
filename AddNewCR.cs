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
        string? temp;

        public int AddCol(DataGridView dgv, int countCol)
        {
            if (countCol < alph)
            {
                int name = b + countCol;
                latter = (char)name;
                temp += latter;
            }
            else
            {
                MessageBox.Show("Неможна збільшити кількість рядків," +
                    "\nдосягнуто максимуму їх кількості");
                return 0;
            }

            DataGridViewColumn col = (DataGridViewColumn)dgv.Columns[0].Clone();
            col.HeaderCell.Value = temp;

            dgv.Columns.Add(col);
            temp = null;
            if(latter != 'Z')
            {
                latter++;
            }
            return 0;
        }
        public int AddRow(DataGridView dgv, int countRow) {
            int r = countRow-1;
            if (countRow > 9)
            {
                MessageBox.Show("Неможна збільшити кількість рядків," +
                    "\nдосягнуто максимуму їх кількості.");
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
