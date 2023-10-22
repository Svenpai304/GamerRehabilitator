using System.Drawing;
using System.Windows.Forms;

namespace GamerRehabilitator
{
    partial class ShockSettings
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
            this.Apply = new System.Windows.Forms.Button();
            this.keyComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.portComboBox = new System.Windows.Forms.ComboBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.collarModeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.powerSlider = new System.Windows.Forms.TrackBar();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.powerSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(355, 214);
            this.Apply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(94, 29);
            this.Apply.TabIndex = 0;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            // 
            // keyComboBox
            // 
            this.keyComboBox.FormattingEnabled = true;
            this.keyComboBox.Location = new System.Drawing.Point(131, 157);
            this.keyComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.keyComboBox.Name = "keyComboBox";
            this.keyComboBox.Size = new System.Drawing.Size(151, 24);
            this.keyComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Arduino port: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Trigger key:";
            // 
            // portComboBox
            // 
            this.portComboBox.FormattingEnabled = true;
            this.portComboBox.Location = new System.Drawing.Point(131, 128);
            this.portComboBox.Name = "portComboBox";
            this.portComboBox.Size = new System.Drawing.Size(151, 24);
            this.portComboBox.TabIndex = 5;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Enabled = false;
            this.statusTextBox.Location = new System.Drawing.Point(324, 186);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(125, 22);
            this.statusTextBox.TabIndex = 6;
            // 
            // collarModeBox
            // 
            this.collarModeBox.FormattingEnabled = true;
            this.collarModeBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.collarModeBox.Location = new System.Drawing.Point(131, 29);
            this.collarModeBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.collarModeBox.Name = "collarModeBox";
            this.collarModeBox.Size = new System.Drawing.Size(151, 24);
            this.collarModeBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Collar mode:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Collar power:";
            // 
            // powerSlider
            // 
            this.powerSlider.Location = new System.Drawing.Point(115, 62);
            this.powerSlider.Maximum = 100;
            this.powerSlider.Name = "powerSlider";
            this.powerSlider.Size = new System.Drawing.Size(334, 56);
            this.powerSlider.TabIndex = 10;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(131, 186);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(151, 22);
            this.numericUpDown1.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Trigger volume%:";
            // 
            // ShockSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 257);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.powerSlider);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.collarModeBox);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.portComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keyComboBox);
            this.Controls.Add(this.Apply);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ShockSettings";
            this.Text = "Gamer Rehabilitation";
            ((System.ComponentModel.ISupportInitialize)(this.powerSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button Apply;
        private ComboBox keyComboBox;
        private Label label1;
        private Label label2;
        private ComboBox portComboBox;
        private TextBox statusTextBox;
        private ComboBox collarModeBox;
        private Label label3;
        private Label label4;
        private TrackBar powerSlider;
        private NumericUpDown numericUpDown1;
        private Label label5;
    }
}