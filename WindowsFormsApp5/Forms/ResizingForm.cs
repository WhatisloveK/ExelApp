using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class ResizingForm : Form
    {
        private int NumberOfRows, NumberOfColumns;
        public ResizingForm()
        {
            InitializeComponent();
            this.ShowDialog();
        }
        private void Okbutton_Click(object sender, EventArgs e)
        {
            NumberOfColumns = Int32.Parse(ColumnNumericUpDown.Value.ToString());
            NumberOfRows = Int32.Parse(RowNumericUpDown.Value.ToString());
            ResizingForm.ActiveForm.Close();
        }

        public int GetRows()
        {
            return NumberOfRows;
        }
        public int GetColumns()
        {
            return NumberOfColumns;
        }
    }
}
