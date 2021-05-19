
namespace pos
{
    partial class Form1
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
            this.panelBar = new System.Windows.Forms.Panel();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnSaleReg = new System.Windows.Forms.Button();
            this.saleRegister = new pos.SaleRegister();
            this.btnItems = new System.Windows.Forms.Button();
            this.Items = new pos.Items();
            this.panelBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBar
            // 
            this.panelBar.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelBar.Controls.Add(this.btnHome);
            this.panelBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBar.Location = new System.Drawing.Point(0, 0);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(1384, 45);
            this.panelBar.TabIndex = 0;
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(23, 12);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(84, 26);
            this.btnHome.TabIndex = 0;
            this.btnHome.Text = "HOME";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnSaleReg
            // 
            this.btnSaleReg.Location = new System.Drawing.Point(173, 131);
            this.btnSaleReg.Name = "btnSaleReg";
            this.btnSaleReg.Size = new System.Drawing.Size(240, 180);
            this.btnSaleReg.TabIndex = 2;
            this.btnSaleReg.Text = "SALES REGISTER";
            this.btnSaleReg.UseVisualStyleBackColor = true;
            this.btnSaleReg.Click += new System.EventHandler(this.btnSaleReg_Click);
            // 
            // saleRegister
            // 
            this.saleRegister.AllowDrop = true;
            this.saleRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.saleRegister.Location = new System.Drawing.Point(0, 44);
            this.saleRegister.Margin = new System.Windows.Forms.Padding(4);
            this.saleRegister.MaximumSize = new System.Drawing.Size(1384, 721);
            this.saleRegister.MinimumSize = new System.Drawing.Size(1384, 721);
            this.saleRegister.Name = "saleRegister";
            this.saleRegister.Size = new System.Drawing.Size(1384, 721);
            this.saleRegister.TabIndex = 1;
            this.saleRegister.Load += new System.EventHandler(this.saleRegister_Load);
            // 
            // btnItems
            // 
            this.btnItems.Location = new System.Drawing.Point(458, 131);
            this.btnItems.Name = "btnItems";
            this.btnItems.Size = new System.Drawing.Size(240, 180);
            this.btnItems.TabIndex = 3;
            this.btnItems.Text = "ITEM LIST";
            this.btnItems.UseVisualStyleBackColor = true;
            this.btnItems.Click += new System.EventHandler(this.btnItems_Click);
            // 
            // Items
            // 
            this.Items.BackColor = System.Drawing.Color.Yellow;
            this.Items.Location = new System.Drawing.Point(0, 44);
            this.Items.MaximumSize = new System.Drawing.Size(1384, 721);
            this.Items.MinimumSize = new System.Drawing.Size(1384, 721);
            this.Items.Name = "Items";
            this.Items.Size = new System.Drawing.Size(1384, 721);
            this.Items.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1384, 761);
            this.Controls.Add(this.btnItems);
            this.Controls.Add(this.btnSaleReg);
            this.Controls.Add(this.panelBar);
            this.Controls.Add(this.saleRegister);
            this.Controls.Add(this.Items);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(1400, 800);
            this.MinimumSize = new System.Drawing.Size(1400, 800);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBar;
        private SaleRegister saleRegister;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnSaleReg;
        private System.Windows.Forms.Button btnItems;
        private Items Items;
    }
}

