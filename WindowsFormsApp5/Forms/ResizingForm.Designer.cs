namespace WindowsFormsApp5
{
    partial class ResizingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ColumnNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RowNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Okbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ColumnNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RowNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ColumnNumericUpDown
            // 
            this.ColumnNumericUpDown.Location = new System.Drawing.Point(122, 68);
            this.ColumnNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.ColumnNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ColumnNumericUpDown.Name = "ColumnNumericUpDown";
            this.ColumnNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.ColumnNumericUpDown.TabIndex = 0;
            this.ColumnNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RowNumericUpDown
            // 
            this.RowNumericUpDown.Location = new System.Drawing.Point(122, 132);
            this.RowNumericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.RowNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RowNumericUpDown.Name = "RowNumericUpDown";
            this.RowNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.RowNumericUpDown.TabIndex = 1;
            this.RowNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of columns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Number of rows";
            // 
            // Okbutton
            // 
            this.Okbutton.Location = new System.Drawing.Point(142, 192);
            this.Okbutton.Name = "Okbutton";
            this.Okbutton.Size = new System.Drawing.Size(75, 23);
            this.Okbutton.TabIndex = 4;
            this.Okbutton.Text = "OK";
            this.Okbutton.UseVisualStyleBackColor = true;
            this.Okbutton.Click += new System.EventHandler(this.Okbutton_Click);
            // 
            // ResizingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 260);
            this.Controls.Add(this.Okbutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RowNumericUpDown);
            this.Controls.Add(this.ColumnNumericUpDown);
            this.Name = "ResizingForm";
            this.Text = "ResizingForm";
            ((System.ComponentModel.ISupportInitialize)(this.ColumnNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RowNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown ColumnNumericUpDown;
        private System.Windows.Forms.NumericUpDown RowNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Okbutton;
    }
}