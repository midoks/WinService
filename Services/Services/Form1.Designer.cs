namespace Services
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_start = new System.Windows.Forms.Button();
            this.restart = new System.Windows.Forms.Button();
            this.project_add = new System.Windows.Forms.Button();
            this.project_del = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_choose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_PN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listProjectBox = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(76, 30);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(57, 30);
            this.button_start.TabIndex = 1;
            this.button_start.Text = "start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // restart
            // 
            this.restart.Location = new System.Drawing.Point(137, 30);
            this.restart.Name = "restart";
            this.restart.Size = new System.Drawing.Size(59, 30);
            this.restart.TabIndex = 2;
            this.restart.Text = "restart";
            this.restart.UseVisualStyleBackColor = true;
            // 
            // project_add
            // 
            this.project_add.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.project_add.Location = new System.Drawing.Point(6, 206);
            this.project_add.Name = "project_add";
            this.project_add.Size = new System.Drawing.Size(30, 25);
            this.project_add.TabIndex = 3;
            this.project_add.Text = " +";
            this.project_add.UseVisualStyleBackColor = true;
            this.project_add.Click += new System.EventHandler(this.project_add_Click);
            // 
            // project_del
            // 
            this.project_del.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.project_del.Location = new System.Drawing.Point(44, 206);
            this.project_del.Name = "project_del";
            this.project_del.Size = new System.Drawing.Size(30, 25);
            this.project_del.TabIndex = 4;
            this.project_del.Text = " -";
            this.project_del.UseVisualStyleBackColor = true;
            this.project_del.Click += new System.EventHandler(this.project_del_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_choose);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_PN);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_start);
            this.groupBox1.Controls.Add(this.restart);
            this.groupBox1.Location = new System.Drawing.Point(163, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 152);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controller";
            // 
            // button_choose
            // 
            this.button_choose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_choose.Location = new System.Drawing.Point(76, 66);
            this.button_choose.Name = "button_choose";
            this.button_choose.Size = new System.Drawing.Size(118, 24);
            this.button_choose.TabIndex = 8;
            this.button_choose.Text = "choose dir";
            this.button_choose.UseVisualStyleBackColor = true;
            this.button_choose.Click += new System.EventHandler(this.button_choose_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "ProjectName";
            // 
            // textBox_PN
            // 
            this.textBox_PN.Location = new System.Drawing.Point(9, 35);
            this.textBox_PN.Name = "textBox_PN";
            this.textBox_PN.Size = new System.Drawing.Size(63, 21);
            this.textBox_PN.TabIndex = 6;
            this.textBox_PN.TextChanged += new System.EventHandler(this.textBox_PN_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(9, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "scrpit dir:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listProjectBox);
            this.groupBox2.Controls.Add(this.project_add);
            this.groupBox2.Controls.Add(this.project_del);
            this.groupBox2.Location = new System.Drawing.Point(11, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(134, 243);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "listProjects";
            // 
            // listProjectBox
            // 
            this.listProjectBox.FormattingEnabled = true;
            this.listProjectBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.listProjectBox.IntegralHeight = false;
            this.listProjectBox.ItemHeight = 12;
            this.listProjectBox.Location = new System.Drawing.Point(6, 19);
            this.listProjectBox.Name = "listProjectBox";
            this.listProjectBox.Size = new System.Drawing.Size(120, 184);
            this.listProjectBox.TabIndex = 5;
            this.listProjectBox.SelectedIndexChanged += new System.EventHandler(this.listProjectBox_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(163, 175);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 85);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stauts";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(379, 272);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Services";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button restart;
        private System.Windows.Forms.Button project_add;
        private System.Windows.Forms.Button project_del;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listProjectBox;
        private System.Windows.Forms.TextBox textBox_PN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_choose;

    }
}

