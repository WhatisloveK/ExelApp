using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {

      
        private Dictionary<string, List<string>> dependentVariables = new Dictionary<string, List<string>>();
        private CurrentCell currentCell = new CurrentCell(0, 0, false);
        private Class26BasedSys f;
        private MyHashTable myTable;        
        private int row = 100, colom = 100;
        private Dictionary<string, List<string>> dependencisByVariable = new Dictionary<string, List<string>>();

        class CurrentCell
        {
            public int Colom, Row;
            public bool flag;

            public CurrentCell(int c, int r, bool f)
            {
                Colom = c;
                Row = r;
                flag = f;

            }
        }  

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView(row, colom);
            myTable = MyHashTable.GetInstance();
        }

        public void InitializeDataGridView(int rows, int columns)
        {
            dataGridView1.ColumnCount = columns;
            dataGridView1.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            f = new Class26BasedSys();
            for (int i = 0; i < columns; ++i)
            {
                dataGridView1.Columns[i].Name = f.ToSys(i);
            }

            dataGridView1.RowCount = rows;
            dataGridView1.RowHeadersVisible = true;

            DataGridViewCellStyle rowHeaderStyle = new DataGridViewCellStyle();
            rowHeaderStyle.BackColor = Color.Beige;
            rowHeaderStyle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = rowHeaderStyle;

            //f = new Class26BasedSys();
            for (int i = 0; i < rows; ++i)
            {
                dataGridView1.Rows[i].HeaderCell.Value = (i).ToString();
            }
            dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dataGridView1.RowHeadersWidth = 55;

        }

        #region DynamicInput
        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void tb_TextChanged(object sender, EventArgs e)

        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox1.Text = textBox.Text;
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;

            tb.TextChanged += new EventHandler(tb_TextChanged);
        }

        #endregion

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizingForm resizingForm = new ResizingForm();
            //resizingForm.ShowDialog();
            int rows = resizingForm.GetRows();
            int columns = resizingForm.GetColumns();
            if (rows != 0 && columns != 0)
            {
                if (dataGridView1.ColumnCount <= columns && dataGridView1.RowCount <= rows)
                {
                    InitializeDataGridView(rows, columns);
                }
                else
                {
                    var result = MessageBox.Show("Possible lose of data. Continue action?", "Deleting rows, columns ALERT", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {

                        int starterRowCount = dataGridView1.ColumnCount - columns;
                        int starterColumnCount = dataGridView1.RowCount - rows;
                        //Deleting rows and columns from memory
                        for (int i = columns ; i < dataGridView1.ColumnCount; i++)
                        {
                            for (int j = 0; j < dataGridView1.RowCount; j++)
                                myTable.DeleteHash(f.ToSys(i) + "." + j);
                        }
                        for (int i = rows ; i < dataGridView1.RowCount; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                                myTable.DeleteHash(f.ToSys(j) + "." + i);
                        }

                        InitializeDataGridView(rows, columns);

                        for (int c = 0; c < columns; c++)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                currentCell.Colom = c;
                                currentCell.Row = r;
                                CellCalculation(f.ToSys(c) + "." + r, myTable.getFormula(f.ToSys(c) + "." + r));
                            }
                        }



                    }
                }

            }
        }

        #region Saving and Loading data
        private void SaveFile()
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "ExelFile|*.exl";
            saveFileDialog1.Title = "Save a Excel File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                StreamWriter sw = new StreamWriter(fs);
                int colom = dataGridView1.ColumnCount;
                int row = dataGridView1.RowCount;
                sw.WriteLine(colom);
                sw.WriteLine(row);
                for (int c = 0; c < colom; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        sw.WriteLine(myTable.getFormula(f.ToSys(c) + "." + r));
                    }
                }
                sw.Close();
                fs.Close();
            }
        }
        private void ReadFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "ExelFile|*.exl";
            openFileDialog1.Title = "Select an Excel File";
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

            int colom = dataGridView1.ColumnCount;
            int row = dataGridView1.RowCount;

            for (int c = 1; c <= colom; c++)
            {
                for (int r = 1; r <= row; r++)
                {
                    myTable.DeleteHash(f.ToSys(c) + "." + r);
                }
            }

            dataGridView1.ColumnCount = 0;
            dataGridView1.RowCount = 1;


            Int32.TryParse(sr.ReadLine(), out colom);
            Int32.TryParse(sr.ReadLine(), out row);

            
            InitializeDataGridView(row, colom);



            for (int c = 0; c < colom; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    string formula = sr.ReadLine();
                    if (formula != "")
                    {
                        myTable.AddFormula(f.ToSys(c) + "." + r, formula);
                        currentCell.Colom = c;
                        currentCell.Row = r;
                        CellCalculation(f.ToSys(c) + "." + r, myTable.getFormula(f.ToSys(c) + "." + r));
                    }
                }
            }
           

            sr.Close();

        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadFile();
        }
        #endregion


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string tb = textBox1.Text;
            if (tb.Length == 0 || tb[tb.Length - 1] == '+' || tb[tb.Length - 1] == '-' || tb[tb.Length - 1] == '='
               || tb[tb.Length - 1] == '>' || tb[tb.Length - 1] == '<' || tb[tb.Length - 1] == '/' || tb[tb.Length - 1] == '*')
            {
                textBox1.Text += f.ToSys(e.ColumnIndex) + "." + e.RowIndex;
            }
            else
            {
                Match matchRes, match = Regex.Match(tb, @"[A-Z]+.[0-9]+");
                matchRes = match;
                while (match.Success)
                {
                    matchRes = match;
                    match = match.NextMatch();
                }

                if (tb.IndexOf(matchRes.Value) == tb.Length - matchRes.Value.Length)
                {
                    tb = tb.Replace(matchRes.Value, f.ToSys(e.ColumnIndex) + "." + e.RowIndex);
                    textBox1.Text = tb;
                }
            }

           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            formulaLabel.Text = "   "+ f.ToSys(e.ColumnIndex) + "." + e.RowIndex 
                +":  "+ myTable.getFormula(f.ToSys(e.ColumnIndex) + "." + e.RowIndex);
        }



        #region Recalculations
        private void Recalculate(string cell)
        {

            string formula = myTable.getFormula(cell);
            MatchCollection matchRes = Regex.Matches(formula, @"[A-Z]+.[0-9]+");
            if (matchRes.Count != 0)
            {
                foreach (Match match in matchRes)
                {
                    Recalculate(match.Value);
                }
            }

            string colNum = cell.Split('.')[0];
            string rowNum = cell.Split('.')[1];

            myTable.AddValue(cell, Calculator.Evaluate(myTable.getFormula(cell), ref myTable).ToString());
            dataGridView1.Rows[Int32.Parse(rowNum)].Cells[f.FromSys(colNum)].Value = myTable.values[cell];

        }
        private void finalRecalculation(string cell)
        {
            if (dependencisByVariable.ContainsKey(cell))
            {
                if (dependencisByVariable[cell].Contains(cell))
                {                   
                    foreach (string val in dependencisByVariable[cell])
                    {
                        int colomN = f.FromSys(val.Split('.')[0]);
                        int rowN = Int32.Parse(val.Split('.')[1]);
                        myTable.AddValue(val, "IncorrectData");
                        dataGridView1.Rows[rowN].Cells[colomN].Value = "Incorrect Formula";                     
                    }                  
                }
                else
                {
                    foreach (string val in dependencisByVariable[cell])
                    {
                        Recalculate(val);
                    }
                }
            }
        }
        #endregion


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if  (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                string formula = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                myTable.AddFormula(f.ToSys(e.ColumnIndex) + "." + e.RowIndex, formula);
                CurrentCell tmp = currentCell;
                currentCell.Colom = e.ColumnIndex;
                currentCell.Row = e.RowIndex;
                CellCalculation(f.ToSys(e.ColumnIndex) + "." + e.RowIndex, formula);
                currentCell = tmp;
                //string value = Calculator.Evaluate(formula, ref myTable).ToString();
                
                //myTable.AddValue(f.ToSys(e.ColumnIndex) + "." + e.RowIndex, value);
                //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;

                //CalculateDependentVariables(f.ToSys(e.ColumnIndex) + "." + e.RowIndex, formula);
               // dependencisByVariable = CalculateDependencisByVariable();
                //finalRecalculation(f.ToSys(e.ColumnIndex) + "." + e.RowIndex);

            }
        }



        #region Dependencis and recursion
        private Dictionary<string, List<string>> CalculateDependencisByVariable()
        {
            Dictionary<string, List<string>> DependencisByVariable = new Dictionary<string, List<string>>();
            foreach (var pair in dependentVariables)
            {
                foreach (string val in pair.Value)
                {
                    if (DependencisByVariable.ContainsKey(val) && !DependencisByVariable[val].Contains(pair.Key))
                    {
                        DependencisByVariable[val].Add(pair.Key);
                    }
                    else if (!DependencisByVariable.ContainsKey(val))
                    {
                        List<string> values = new List<string>();
                        values.Add(pair.Key);
                        DependencisByVariable.Add(val, values);
                    }
                }
            }
            string cell;
            foreach (var valpair in DependencisByVariable)
            {

                for (int i = 0; i < valpair.Value.Count; i++)
                {
                    cell = valpair.Value[i];
                    if (DependencisByVariable.ContainsKey(cell))
                    {
                        foreach (string val in DependencisByVariable[cell])
                        {
                            if (!valpair.Value.Contains(val))
                            {
                                DependencisByVariable[valpair.Key].Add(val);
                            }
                        }
                    }
                }
            }
            return DependencisByVariable;
        }


        private void CalculateDependentVariables(string cell,string formula)
        {
            MatchCollection matchRes = Regex.Matches(formula, @"[A-Z]+.[0-9]+");


            if (dependentVariables.ContainsKey(cell))
                dependentVariables[cell].Clear();

            foreach (Match match in matchRes)
            {

                if (dependentVariables.ContainsKey(cell))
                {
                    if ((f.FromSys(match.Value.Split('.')[0]) + 1) > dataGridView1.ColumnCount || Int32.Parse(match.Value.Split('.')[1]) > dataGridView1.RowCount)
                    {
                        dependentVariables[cell].Add(cell);
                    }
                    else
                    {
                        dependentVariables[cell].Add(match.Value);
                    }
                }
                else
                {
                    List<string> values = new List<string>();
                    if ((f.FromSys(match.Value.Split('.')[0]) + 1) > dataGridView1.ColumnCount || Int32.Parse(match.Value.Split('.')[1]) > dataGridView1.RowCount)
                    {
                        values.Add(cell);
                        dependentVariables.Add(cell, values);
                    }
                    else
                    {

                        values.Add(match.Value);
                        dependentVariables.Add(cell, values);
                    }

                }
            }
        }
       


        private string Recursion()
        {
            foreach (var pair in dependencisByVariable)
            {
                foreach (string val in pair.Value)
                {
                    if (val == pair.Key)
                    {
                        return val;
                    }
                }
            }
            return "";
        }

        #endregion


        private void CellCalculation(string cell,string formula)
        {
            try
            {
                Precalculation(formula);
                CalculateDependentVariables(cell, formula);
                dependencisByVariable = CalculateDependencisByVariable();
                if (formula != "")
                {
                    string recurtionCell = Recursion();

                    if (recurtionCell != "")
                    {

                        if (!dependencisByVariable[recurtionCell].Contains(cell))
                        {
                            string value = Calculator.Evaluate(formula, ref myTable).ToString();
                            dataGridView1.Rows[currentCell.Row].Cells[currentCell.Colom].Value = value;
                            myTable.AddValue(cell, value);

                        }

                        if (dependencisByVariable[recurtionCell].Contains(cell))
                        {
                            if (dependencisByVariable.ContainsKey(cell))
                            {
                                foreach (string val in dependencisByVariable[recurtionCell])
                                {
                                    if (!dependencisByVariable[cell].Contains(val))
                                    {
                                        dependencisByVariable[cell].Add(val);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                       
                            string value = Calculator.Evaluate(formula, ref myTable).ToString();
                            dataGridView1.Rows[currentCell.Row].Cells[currentCell.Colom].Value = value;
                            myTable.AddValue(cell, value);
                        
                    }
                }
            }
            catch(ArgumentException)
            {
                Debug.WriteLine("Exception has been handled succesfully");

                if (!dependencisByVariable.ContainsKey(cell))// Getting incorrect formula
                {
                    List<string> v = new List<string>();
                    v.Add(cell);
                    dependencisByVariable.Add(cell, v);
                }
                else
                {
                    dependencisByVariable[cell].Add(cell);
                }
            }
            finalRecalculation(cell);
        }

        private void Precalculation(string expr)
        {
            
                int lpar = 0;
                int rpar = 0;
                foreach (char ch in expr)
                {
                    if (ch == '(') lpar++;
                    if (ch == ')') rpar++;
                }
                if (lpar != rpar)
                    throw new ArgumentException("Invalid input");

                expr = "#" + expr + "#";

                List<string> templates = new List<string>();
                string template1 = @"[\+\-\*\/^]( )*([\+\-\*\/^])+";
                string template2 = @"([0-9]|[A-Z]|\))( )+(\(|[a-zA-Z]|[0-9])";
                string template3 = @"==+|>>+|<<+|=>|=<";
                string template4 = @"#( )*[\+\*\/^]|[\+\-\*\//^]( )*#";
                string template5 = @"(([0-9]|[A-Z])\()|(\)([0-9]|[A-Z]))";
                string template6 = @"[a-z]+|\)\(";
                string template7 = @"[A-Z]+;[1-9]+[A-Z]+;[1-9]+";

                Regex regex1 = new Regex(template1);
                Regex regex2 = new Regex(template2);
                Regex regex3 = new Regex(template3);
                Regex regex4 = new Regex(template4);
                Regex regex5 = new Regex(template5);
                Regex regex6 = new Regex(template6);
                Regex regex7 = new Regex(template7);


                MatchCollection matches1 = regex1.Matches(expr);
                MatchCollection matches2 = regex2.Matches(expr);
                MatchCollection matches3 = regex3.Matches(expr);
                MatchCollection matches4 = regex4.Matches(expr);
                MatchCollection matches5 = regex5.Matches(expr);
                MatchCollection matches6 = regex6.Matches(expr);
                MatchCollection matches7 = regex7.Matches(expr);

                if (matches1.Count > 0) throw new ArgumentException("Invalid input");
                if (matches2.Count > 0) throw new ArgumentException("Invalid input");
                if (matches3.Count > 0) throw new ArgumentException("Invalid input");
                if (matches4.Count > 0) throw new ArgumentException("Invalid input");
                if (matches5.Count > 0) throw new ArgumentException("Invalid input");
                if (matches6.Count > 0) throw new ArgumentException("Invalid input");
                if (matches7.Count > 0) throw new ArgumentException("Invalid input");
            
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        { 
            //changeValue(e.RowIndex, e.ColumnIndex);

            if (currentCell.flag == false)
            {
                currentCell.Colom = e.ColumnIndex;
                currentCell.Row = e.RowIndex;               
                currentCell.flag = true;                
            }
            else
            {
                string formula = textBox1.Text;
                
                    
                    string cell = f.ToSys(currentCell.Colom) + "." + currentCell.Row;
                    myTable.AddFormula(cell, formula);

                    CellCalculation(cell, formula);
               
                
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = myTable.getFormula(f.ToSys(e.ColumnIndex) + "." + e.RowIndex);
                currentCell.Colom = e.ColumnIndex;
                currentCell.Row = e.RowIndex;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel=true;
            }
        }

        #region Test getters
        public Dictionary<string, List<string>> TestCalculatingDependentVariables(string cell, string formula)
        {
            currentCell.Colom = f.FromSys(cell.Split('.')[0]);
            currentCell.Row = Int32.Parse(cell.Split('.')[1]);
            CalculateDependentVariables(cell, formula);
            return dependentVariables;
        }

        public Dictionary<string, List<string>> TestCalculatingDependencisByVariables()
        {
            dependencisByVariable = CalculateDependencisByVariable();
            return dependencisByVariable;
        }

       

        public string TestRecursion()
        {
            return Recursion();
        }

        #endregion
    }
}
