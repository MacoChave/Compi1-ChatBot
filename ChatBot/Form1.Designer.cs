namespace ChatBot
{
	partial class Form1
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
            this.richTextBoxSource = new System.Windows.Forms.RichTextBox();
            this.buttonAnalizar = new System.Windows.Forms.Button();
            this.richTextBoxRestult = new System.Windows.Forms.RichTextBox();
            this.buttonCompilar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxSource
            // 
            this.richTextBoxSource.AcceptsTab = true;
            this.richTextBoxSource.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxSource.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxSource.Name = "richTextBoxSource";
            this.richTextBoxSource.Size = new System.Drawing.Size(370, 370);
            this.richTextBoxSource.TabIndex = 0;
            this.richTextBoxSource.Text = "";
            // 
            // buttonAnalizar
            // 
            this.buttonAnalizar.Location = new System.Drawing.Point(12, 388);
            this.buttonAnalizar.Name = "buttonAnalizar";
            this.buttonAnalizar.Size = new System.Drawing.Size(180, 23);
            this.buttonAnalizar.TabIndex = 1;
            this.buttonAnalizar.Text = "Analizar";
            this.buttonAnalizar.UseVisualStyleBackColor = true;
            this.buttonAnalizar.Click += new System.EventHandler(this.buttonAnalizar_Click);
            // 
            // richTextBoxRestult
            // 
            this.richTextBoxRestult.AcceptsTab = true;
            this.richTextBoxRestult.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxRestult.Location = new System.Drawing.Point(388, 12);
            this.richTextBoxRestult.Name = "richTextBoxRestult";
            this.richTextBoxRestult.Size = new System.Drawing.Size(370, 370);
            this.richTextBoxRestult.TabIndex = 2;
            this.richTextBoxRestult.Text = "";
            // 
            // buttonCompilar
            // 
            this.buttonCompilar.Location = new System.Drawing.Point(202, 388);
            this.buttonCompilar.Name = "buttonCompilar";
            this.buttonCompilar.Size = new System.Drawing.Size(180, 23);
            this.buttonCompilar.TabIndex = 3;
            this.buttonCompilar.Text = "Compilar";
            this.buttonCompilar.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonCompilar);
            this.Controls.Add(this.richTextBoxRestult);
            this.Controls.Add(this.buttonAnalizar);
            this.Controls.Add(this.richTextBoxSource);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBoxSource;
		private System.Windows.Forms.Button buttonAnalizar;
		private System.Windows.Forms.RichTextBox richTextBoxRestult;
		private System.Windows.Forms.Button buttonCompilar;
	}
}

