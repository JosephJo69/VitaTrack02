namespace WF_Proyecto002
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
            Btn_SeleccionarFoto = new Button();
            Btn_Enviar = new Button();
            pictureBox1 = new PictureBox();
            TBRespuesta = new TextBox();
            BTNReset = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // Btn_SeleccionarFoto
            // 
            Btn_SeleccionarFoto.Location = new Point(54, 257);
            Btn_SeleccionarFoto.Name = "Btn_SeleccionarFoto";
            Btn_SeleccionarFoto.Size = new Size(112, 23);
            Btn_SeleccionarFoto.TabIndex = 0;
            Btn_SeleccionarFoto.Text = "Seleccionar Foto";
            Btn_SeleccionarFoto.UseVisualStyleBackColor = true;
            Btn_SeleccionarFoto.Click += Btn_SeleccionarFoto_Click;
            // 
            // Btn_Enviar
            // 
            Btn_Enviar.Location = new Point(72, 286);
            Btn_Enviar.Name = "Btn_Enviar";
            Btn_Enviar.Size = new Size(75, 23);
            Btn_Enviar.TabIndex = 1;
            Btn_Enviar.Text = "Enviar";
            Btn_Enviar.UseVisualStyleBackColor = true;
            Btn_Enviar.Click += Btn_Enviar_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(46, 133);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(127, 105);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // TBRespuesta
            // 
            TBRespuesta.Location = new Point(221, 133);
            TBRespuesta.Multiline = true;
            TBRespuesta.Name = "TBRespuesta";
            TBRespuesta.ReadOnly = true;
            TBRespuesta.Size = new Size(547, 105);
            TBRespuesta.TabIndex = 4;
            // 
            // BTNReset
            // 
            BTNReset.Location = new Point(70, 315);
            BTNReset.Name = "BTNReset";
            BTNReset.Size = new Size(82, 23);
            BTNReset.TabIndex = 5;
            BTNReset.Text = "Reiniciar DB";
            BTNReset.UseVisualStyleBackColor = true;
            BTNReset.Click += BTNReset_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Gill Sans MT", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(287, 32);
            label1.Name = "label1";
            label1.Size = new Size(253, 67);
            label1.TabIndex = 6;
            label1.Text = "VitaTrack";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGreen;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(BTNReset);
            Controls.Add(TBRespuesta);
            Controls.Add(pictureBox1);
            Controls.Add(Btn_Enviar);
            Controls.Add(Btn_SeleccionarFoto);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Btn_SeleccionarFoto;
        private Button Btn_Enviar;
        private PictureBox pictureBox1;
        private TextBox TBRespuesta;
        private Button BTNReset;
        private Label label1;
    }
}
