namespace Lab1oop
{
    public partial class Form1 : Form
    {
        const int Mesures = 100;
        int rownum = 0;
        int colnum = 0;
        AddNewCR addNew = new AddNewCR();
        private Dictionary<int, Cell> dict = new Dictionary<int, Cell>();
        Cell[,] table = new Cell[Mesures, Mesures];

        public Form1()
        {
            InitializeComponent(); 
            for(int i = 0; i < Mesures; i++)
            {
                for(int j = 0; j < Mesures; j++)
                {
                    table[i,j]=new Cell();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rownum = 3;
            colnum = 3;
            
            string temp = null;
            char c = 'A';
            Cell[,] table = new Cell[Mesures, Mesures];
            for(int i = 0;i< colnum; i++)
            {
                temp += c;
                dataGridView1.Columns.Add(Name, temp);
                c++;
                temp = null;
            }
            for(int i = 0; i < rownum-1; i++)
            {
                dataGridView1.Rows.Add();
            }
            for(int i = 0;i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int rCheck = dataGridView1.Rows.Count;
            addNew.addRow(dataGridView1, rownum);
            if(rCheck<dataGridView1.Rows.Count) rownum++;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int cCheck = dataGridView1.Columns.Count;
            addNew.addCol(dataGridView1, colnum);
            if (cCheck < dataGridView1.Columns.Count) colnum++;
        }

        public void addNewVal(string expre, int r, int c)
        {
            Parser parser = new Parser();
            table[r, c].exp = expre;
            int h = 0;
            if (expre != null)
            {
                while (h < expre.Length)
                {
                    string str = null;
                    int t2, t1 = (int)expre[h];
                    str += expre[h];
                    h++;
                    if (t1 > 64 && t1 < 91)
                    {
                        str += expre[h];
                        t2 = (int)expre[h];
                        h++;
                        if(t2 - 48==r&& t1 - 65 == c)
                        {
                            dataGridView1.Rows[r].Cells[c].Value = "#CIRCLE";
                            MessageBox.Show("Error");
                            return;
                        }
                        table[t2 - 48, t1 - 65].dependoncells.Add(table[r, c].setName(c, r));
                        expre = expre.Replace(str, table[t2 - 48, t1 - 65].val);
                    }                    
                }
                if (circle(table[r, c], r, c))
                {
                    Result res = parser.Eval(expre);
                    if (res.except())
                    {
                        table[r, c].val = res.GetVal();
                        update(table[r, c], r, c);
                        dataGridView1.Rows[r].Cells[c].Value = res.GetVal();
                    }
                    else dataGridView1.Rows[r].Cells[c].Value = res.GetVal();
                }
            }
        }
        public void update(Cell cell, int r, int c)
        {
            for (int i = 0; i < cell.dependoncells.Count; i++)
            {
                int t2 = (int)cell.dependoncells[i][1] - 48, t1 = cell.dependoncells[i][0] - 65;

                Connection(table[t2, t1].val, t2, t1);
            }
        }

        public bool circle(Cell cell, int r, int c)
        {
            for (int i = 0; i < cell.dependoncells.Count; i++)
            {
                int t2 = (int)cell.dependoncells[i][1] - 48, t1 = (int)cell.dependoncells[i][0] - 65;
                for (int j = 0; j < table[t2, t1].dependoncells.Count; j++)
                {
                    if (table[t2, t1].dependoncells[j] == table[r, c].setName(r, c))
                    {
                        nameCircle(cell, r, c);
                        nameCircle(table[t2, t1], t2, t1);
                        table[t2, t1].dependoncells.RemoveAt(j);
                        MessageBox.Show("Error");
                        return false;
                    }
                }
            }
            return true;
        }

        public void nameCircle(Cell cell, int r, int c)
        {
            for (int flag = 0; flag < cell.dependoncells.Count; flag++)
            {
                int _t2 = (int)cell.dependoncells[flag][1] - 48, _t1 = (int)cell.dependoncells[flag][0] - 65;
                dataGridView1.Rows[_t2].Cells[_t1].Value = "#CIRCLE";
            }
        }

        public void Connection(string temp, int row, int column)
        {
            Parser parser = new Parser();
            int h1 = 0;
            while (h1 < temp.Length)
            {
                string str = null;
                int t2, t1 = (int)temp[h1];
                str += temp[h1];
                h1++;

                if ((t1 > 64) && (t1 < 91))
                {
                    str += temp[h1];
                    t2 = (int)temp[h1];
                    h1++;
                    temp = temp.Replace(str, table[t2 - 48, t1 - 65].val);
                }
            }
            if (circle(table[row, column], row, column))
            {
                Result Res = parser.Eval(temp);
                if (Res.except())
                {
                    table[row, column].val = Res.GetVal();
                    update(table[row, column], row, column);
                    dataGridView1.Rows[row].Cells[column].Value = Res.GetVal();
                }
                else return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count <= 2) { MessageBox.Show("Неможливло видалити"); return; }
            else
            {
                for (int i = 0; i < colnum; i++)
                {
                    addNewVal("0", rownum-1, i);
                }
                
                dataGridView1.Rows.RemoveAt(rownum-2);
                rownum--;
                dataGridView1.Rows[rownum-1].HeaderCell.Value = (rownum-1).ToString();

                for (int i = 0; i < colnum; i++)
                {
                    if (table[rownum - 1, i].exp == "0") {
                        dataGridView1.Rows[rownum - 1].Cells[i].Value = "";
                    }
                    else
                    {
                        addNewVal(table[rownum - 1, i].exp, rownum - 1, i);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count <= 2) { MessageBox.Show("Неможливло видалити"); return; }
            for (int i = 0; i < rownum-1; i++)
            {
                addNewVal("0", i, colnum-1);
            }
            dataGridView1.Columns.RemoveAt(colnum-1);
            colnum--;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string temp = (string)textBox1.Text;

                int r=dataGridView1.CurrentCell.RowIndex;
                int c=dataGridView1.CurrentCell.ColumnIndex;
                addNewVal(temp, r, c);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = table[dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex].exp;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string temp = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            int r = dataGridView1.CurrentCell.RowIndex;
            int c = dataGridView1.CurrentCell.ColumnIndex;
            if(temp!=null) addNewVal(temp, r, c);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path=sfd.FileName;
                using (StreamWriter writer = new StreamWriter(File.Create(path)))
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            writer.Write(dataGridView1.Rows[i].Cells[j].Value);

                            if (j != dataGridView1.Columns.Count - 1)
                            {
                                writer.Write("/");
                            }
                        }
                        writer.WriteLine();
                    }
                }
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fileStream = ofd.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadLine();
                    int linenum = 0;
                    while (line != null && linenum < 10)
                    {
                        string[] arLine = line.Split('/');
                        for (int i = 0; i < arLine.Length && i < 27; i++)
                        {
                            if (arLine.Length > dataGridView1.Columns.Count && dataGridView1.Columns.Count < 26)
                            {
                                int cCheck = dataGridView1.Columns.Count;
                                addNew.addCol(dataGridView1, colnum);
                                if (cCheck < dataGridView1.Columns.Count) colnum++;
                            }
                            if (arLine[i] == "")
                            {
                                dataGridView1.Rows[linenum].Cells[i].Value = "";
                                table[linenum, i].val = "0";
                                table[linenum, i].exp = "0";
                            }
                            else
                            {
                                addNewVal(arLine[i], linenum, i);
                            }
                        }
                        line = reader.ReadLine();
                        linenum++;
                        if (line != null && dataGridView1.Rows.Count < 10 && dataGridView1.Rows.Count == linenum)
                        {
                            int rCheck = dataGridView1.Rows.Count;
                            addNew.addRow(dataGridView1, rownum);
                            if (rCheck < dataGridView1.Rows.Count) rownum++;
                        }
                    }   
                }
            }
        }
    }
}