using System.Globalization;

namespace Lab1oop
{
    public partial class Form1 : Form
    {
        const int Mesures = 100;
        int rowNum = 0;
        int colNum = 0;
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
            rowNum = 3;
            colNum = 3;
            
            string temp = null;
            char c = 'A';
            Cell[,] table = new Cell[Mesures, Mesures];
            for(int i = 0;i< colNum; i++)
            {
                temp += c;
                dataGridView1.Columns.Add(Name, temp);
                c++;
                temp = null;
            }
            for(int i = 0; i < rowNum-1; i++)
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
            addNew.AddRow(dataGridView1, rowNum);
            if(rCheck<dataGridView1.Rows.Count) rowNum++;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int cCheck = dataGridView1.Columns.Count;
            addNew.AddCol(dataGridView1, colNum);
            if (cCheck < dataGridView1.Columns.Count) colNum++;
        }

        public void AddNewVal(string expre, int r, int c)
        {
            Parser parser = new Parser();
            table[r, c].Exp = expre;
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
                        GiveName giveName = new GiveName();
                        table[t2 - 48, t1 - 65].dependoncells.Add(giveName.SetName(c, r));
                        expre = expre.Replace(str, table[t2 - 48, t1 - 65].Val);
                    }                    
                }
                if (Circle(table[r, c], r, c))
                {
                    Result res = parser.Eval(expre);
                    if (res.except())
                    {
                        table[r, c].Val = res.GetVal();
                        dataGridView1.Rows[r].Cells[c].Value = res.GetVal();
                        Update(table[r, c], r, c);
                    }
                    else dataGridView1.Rows[r].Cells[c].Value = res.GetVal();
                }
            }
        }
        public void Update(Cell cell, int r, int c)
        {
            for (int i = 0; i < cell.dependoncells.Count; i++)
            {
                int t2 = (int)cell.dependoncells[i][1] - 48, t1 = cell.dependoncells[i][0] - 65;
                Connection(table[t2, t1].Val, t2, t1);
            }
        }

        public bool Circle(Cell cell, int r, int c)
        {
            for (int i = 0; i < cell.dependoncells.Count; i++)
            {
                int t2 = (int)cell.dependoncells[i][1] - 48, t1 = (int)cell.dependoncells[i][0] - 65;
                GiveName giveName = new GiveName();
                for (int j = 0; j < table[t2, t1].dependoncells.Count; j++)
                {
                    if (table[t2, t1].dependoncells[j] == giveName.SetName(r, c))
                    {
                        NameCircle(cell, r, c);
                        NameCircle(table[t2, t1], t2, t1);
                        table[t2, t1].dependoncells.RemoveAt(j);
                        MessageBox.Show("Error");
                        return false;
                    }
                }
            }
            return true;
        }

        public void NameCircle(Cell cell, int r, int c)
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
                    temp = temp.Replace(str, table[t2 - 48, t1 - 65].Val);
                }
            }
            if (Circle(table[row, column], row, column))
            {
                Result Res = parser.Eval(temp);
                if (Res.except())
                {
                    table[row, column].Val = Res.GetVal();
                    Update(table[row, column], row, column);
                    dataGridView1.Rows[row].Cells[column].Value = Res.GetVal();
                }
                else return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count <= 2) { MessageBox.Show("Неможливло видалити," +
                "\n повинно залишатись більше 2"); return; }
            else
            {
                bool t = false;
                for (int i = 0; i < colNum; i++)
                {
                    AddNewVal("0", rowNum-1, i);
                    if (table[rowNum - 1, i].dependoncells.Count > 0) t = true;
                }

                if (t)
                {
                    MessageBox.Show("Були посилання на видалені змінні." +
                        "\nЇх значення замінено на 0 у виразах");
                    for (int i = 0; i < colNum; i++)
                    {
                        List<string> NewList = new List<string>();
                        NewList.AddRange(table[i, colNum - 1].dependoncells);
                        string str = null;              
                        str += (char)(i+65);
                        str += (rowNum-1).ToString();
                        //MessageBox.Show(str);
                        for (int j = 0; j < NewList.Count; j++)
                        {
                            int t2 = (int)NewList[j][1] - 48,
                                t1 = (int)NewList[j][0] - 65;
                            table[t2, t1].Exp = table[t2, t1].Exp.Replace(str, "0");
                            AddNewVal(table[t2, t1].Exp, t2, t1);
                        }
                        table[rowNum - 1, i].dependoncells.Clear();
                    }
                }
                
                dataGridView1.Rows.RemoveAt(rowNum-2);
                rowNum--;
                dataGridView1.Rows[rowNum-1].HeaderCell.Value = (rowNum-1).ToString();

                for (int i = 0; i < colNum; i++)
                {
                    if (table[rowNum - 1, i].Exp == "0") {
                        dataGridView1.Rows[rowNum - 1].Cells[i].Value = "";
                    }
                    else
                    {
                        AddNewVal(table[rowNum - 1, i].Exp, rowNum - 1, i);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count <= 2) { MessageBox.Show("Неможливло видалити," +
                "\n повинно залишатись більше 2"); return; }

            bool t = false;
            for (int i = 0; i < rowNum; i++)
            {
                AddNewVal("0", i, colNum-1);
                if (table[i, colNum - 1].dependoncells.Count > 0) t = true;
            }

            if (t)
            {
                MessageBox.Show("Були посилання на видалені змінні." +
                    "\nЇх значення замінено на 0 у виразах");
                for (int i = 0; i < rowNum; i++)
                {
                    List<string> NewList = new List<string>();
                    NewList.AddRange(table[i, colNum - 1].dependoncells);
                    string str = null;
                    str += (char)(colNum +64);
                    str += (i).ToString();
                    //MessageBox.Show(str);
                    for (int j = 0; j < NewList.Count; j++)
                    {
                        //MessageBox.Show("111");
                        int t2 = (int)NewList[j][1] - 48,
                            t1 = (int)NewList[j][0] - 65;
                        
                        table[t2, t1].Exp = table[t2, t1].Exp.Replace(str, "0");
                        AddNewVal(table[t2, t1].Exp, t2, t1);
                    }
                    table[i, colNum - 1].dependoncells.Clear();
                }
            }

            dataGridView1.Columns.RemoveAt(colNum-1);
            colNum--;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string temp = (string)textBox1.Text;

                int r=dataGridView1.CurrentCell.RowIndex;
                int c=dataGridView1.CurrentCell.ColumnIndex;
                AddNewVal(temp, r, c);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = table[dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex].Exp;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string temp = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            int r = dataGridView1.CurrentCell.RowIndex;
            int c = dataGridView1.CurrentCell.ColumnIndex;
            if(temp!=null) AddNewVal(temp, r, c);
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
                                addNew.AddCol(dataGridView1, colNum);
                                if (cCheck < dataGridView1.Columns.Count) colNum++;
                            }
                            if (arLine[i] == "")
                            {
                                dataGridView1.Rows[linenum].Cells[i].Value = "";
                                table[linenum, i].Val = "0";
                                table[linenum, i].Exp = "0";
                            }
                            else
                            {
                                AddNewVal(arLine[i], linenum, i);
                            }
                        }
                        line = reader.ReadLine();
                        linenum++;
                        if (line != null && dataGridView1.Rows.Count < 10 && dataGridView1.Rows.Count == linenum)
                        {
                            int rCheck = dataGridView1.Rows.Count;
                            addNew.AddRow(dataGridView1, rowNum);
                            if (rCheck < dataGridView1.Rows.Count) rowNum++;
                        }
                    }   
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowInfo showInfo= new ShowInfo();
            showInfo.ShowHowToUse();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ShowInfo showInfo = new ShowInfo();
            showInfo.ShowAbout();
        }
    }
}