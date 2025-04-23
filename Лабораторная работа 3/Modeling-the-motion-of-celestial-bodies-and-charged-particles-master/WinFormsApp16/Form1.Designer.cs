namespace WinFormsApp16
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            btnStart = new Button();
            btnPause = new Button();
            btnReset = new Button();
            numMass = new NumericUpDown();
            numCharge = new NumericUpDown();
            numField = new NumericUpDown();
            numVelocity = new NumericUpDown();
            btnApply = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMass).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numField).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numVelocity).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(27, 63);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(761, 375);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(27, 12);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 1;
            btnStart.Text = "Старт";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(108, 12);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(75, 23);
            btnPause.TabIndex = 2;
            btnPause.Text = "Пауза";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(189, 12);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 3;
            btnReset.Text = "Сброс";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // numMass
            // 
            numMass.DecimalPlaces = 2;
            numMass.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numMass.Location = new Point(270, 14);
            numMass.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numMass.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numMass.Name = "numMass";
            numMass.Size = new Size(94, 23);
            numMass.TabIndex = 4;
            numMass.Value = new decimal(new int[] { 10, 0, 0, 65536 });
            // 
            // numCharge
            // 
            numCharge.DecimalPlaces = 2;
            numCharge.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numCharge.Location = new Point(378, 14);
            numCharge.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numCharge.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numCharge.Name = "numCharge";
            numCharge.Size = new Size(87, 23);
            numCharge.TabIndex = 5;
            numCharge.Value = new decimal(new int[] { 10, 0, 0, 65536 });
            // 
            // numField
            // 
            numField.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numField.Location = new Point(476, 14);
            numField.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numField.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numField.Name = "numField";
            numField.Size = new Size(118, 23);
            numField.TabIndex = 6;
            numField.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // numVelocity
            // 
            numVelocity.DecimalPlaces = 2;
            numVelocity.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numVelocity.Location = new Point(600, 14);
            numVelocity.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numVelocity.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numVelocity.Name = "numVelocity";
            numVelocity.Size = new Size(107, 23);
            numVelocity.TabIndex = 7;
            numVelocity.Value = new decimal(new int[] { 10, 0, 0, 65536 });
            // 
            // btnApply
            // 
            btnApply.BackColor = SystemColors.ButtonShadow;
            btnApply.Location = new Point(713, 12);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 23);
            btnApply.TabIndex = 8;
            btnApply.Text = "Применить";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(270, 40);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 9;
            label1.Text = "масса частицы";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(378, 40);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 10;
            label2.Text = "заряд частицы";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(471, 40);
            label3.Name = "label3";
            label3.Size = new Size(123, 15);
            label3.TabIndex = 11;
            label3.Text = "напряженность поля";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(600, 40);
            label4.Name = "label4";
            label4.Size = new Size(118, 15);
            label4.TabIndex = 12;
            label4.Text = "начальная скорость";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnApply);
            Controls.Add(numVelocity);
            Controls.Add(numField);
            Controls.Add(numCharge);
            Controls.Add(numMass);
            Controls.Add(btnReset);
            Controls.Add(btnPause);
            Controls.Add(btnStart);
            Controls.Add(pictureBox);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMass).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numField).EndInit();
            ((System.ComponentModel.ISupportInitialize)numVelocity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox;
        private Button btnStart;
        private Button btnPause;
        private Button btnReset;
        private NumericUpDown numMass;
        private NumericUpDown numCharge;
        private NumericUpDown numField;
        private NumericUpDown numVelocity;
        private Button btnApply;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
