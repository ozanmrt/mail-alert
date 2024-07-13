namespace MailUyarı
{
    partial class Gelismis_Grid
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
            this.advancedDataGridView1 = new ADGV.AdvancedDataGridView();
            this.konu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gonderen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tarih = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // advancedDataGridView1
            // 
            this.advancedDataGridView1.AutoGenerateContextFilters = true;
            this.advancedDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.advancedDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.konu,
            this.gonderen,
            this.tarih});
            this.advancedDataGridView1.DateWithTime = false;
            this.advancedDataGridView1.Location = new System.Drawing.Point(26, 44);
            this.advancedDataGridView1.Name = "advancedDataGridView1";
            this.advancedDataGridView1.Size = new System.Drawing.Size(409, 264);
            this.advancedDataGridView1.TabIndex = 1;
            this.advancedDataGridView1.TimeFilter = false;
            // 
            // konu
            // 
            this.konu.HeaderText = "Konu";
            this.konu.MinimumWidth = 22;
            this.konu.Name = "konu";
            this.konu.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // gonderen
            // 
            this.gonderen.HeaderText = "Gönderen";
            this.gonderen.MinimumWidth = 22;
            this.gonderen.Name = "gonderen";
            this.gonderen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tarih
            // 
            this.tarih.HeaderText = "Tarih";
            this.tarih.MinimumWidth = 22;
            this.tarih.Name = "tarih";
            this.tarih.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(570, 96);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "durum";
            this.Column1.HeaderText = "Column1";
            this.Column1.Items.AddRange(new object[] {
            "sadsadasd",
            "asdsadsadas",
            "dsadsad",
            "asdasdas"});
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Gelismis_Grid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 605);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.advancedDataGridView1);
            this.Name = "Gelismis_Grid";
            this.Text = "Gelismis_Grid";
            this.Load += new System.EventHandler(this.Gelismis_Grid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ADGV.AdvancedDataGridView advancedDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn konu;
        private System.Windows.Forms.DataGridViewTextBoxColumn gonderen;
        private System.Windows.Forms.DataGridViewTextBoxColumn tarih;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column1;
    }
}