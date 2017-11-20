/*
 * Сделано в SharpDevelop.
 * Пользователь: bdm
 * Дата: 12.07.2013
 * Время: 8:26
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
namespace ufk
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.loadButton = new System.Windows.Forms.Button();
            this.rbKal = new System.Windows.Forms.RadioButton();
            this.rbGam = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbNev = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadButton.Location = new System.Drawing.Point(12, 118);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(460, 27);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Загрузить";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.LoadButtonClick);
            // 
            // rbKal
            // 
            this.rbKal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbKal.Location = new System.Drawing.Point(6, 13);
            this.rbKal.Name = "rbKal";
            this.rbKal.Size = new System.Drawing.Size(104, 24);
            this.rbKal.TabIndex = 1;
            this.rbKal.TabStop = true;
            this.rbKal.Text = "Калядин";
            this.rbKal.UseVisualStyleBackColor = true;
            // 
            // rbGam
            // 
            this.rbGam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbGam.Location = new System.Drawing.Point(6, 43);
            this.rbGam.Name = "rbGam";
            this.rbGam.Size = new System.Drawing.Size(104, 24);
            this.rbGam.TabIndex = 1;
            this.rbGam.TabStop = true;
            this.rbGam.Text = "Гамаюнова";
            this.rbGam.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbGam);
            this.groupBox1.Controls.Add(this.rbKal);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Платежки";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbNev);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(225, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "выясненные";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ограничение - 520 файлов";
            // 
            // cbNev
            // 
            this.cbNev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbNev.Location = new System.Drawing.Point(6, 28);
            this.cbNev.Name = "cbNev";
            this.cbNev.Size = new System.Drawing.Size(104, 24);
            this.cbNev.TabIndex = 0;
            this.cbNev.Text = "невыясненные";
            this.cbNev.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 157);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.loadButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "УФК->КУИ... (ver.1.3) 20.11.2017";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		//private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cbNev;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbGam;
		private System.Windows.Forms.RadioButton rbKal;
		private System.Windows.Forms.Button loadButton;
	}
}
