namespace test
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.GetSelector = new System.Windows.Forms.Button();
            this.selector_x = new System.Windows.Forms.TextBox();
            this.selector_y = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selector_got = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.selector_to_find = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.find_selector = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.found_region = new System.Windows.Forms.TextBox();
            this.get_tree = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // GetSelector
            // 
            this.GetSelector.Location = new System.Drawing.Point(62, 86);
            this.GetSelector.Margin = new System.Windows.Forms.Padding(4);
            this.GetSelector.Name = "GetSelector";
            this.GetSelector.Size = new System.Drawing.Size(310, 34);
            this.GetSelector.TabIndex = 0;
            this.GetSelector.Text = "Get Selector";
            this.GetSelector.UseVisualStyleBackColor = true;
            this.GetSelector.Click += new System.EventHandler(this.GetSelector_Click);
            // 
            // selector_x
            // 
            this.selector_x.Location = new System.Drawing.Point(84, 28);
            this.selector_x.Margin = new System.Windows.Forms.Padding(4);
            this.selector_x.Name = "selector_x";
            this.selector_x.Size = new System.Drawing.Size(104, 28);
            this.selector_x.TabIndex = 1;
            this.selector_x.Text = "100";
            // 
            // selector_y
            // 
            this.selector_y.Location = new System.Drawing.Point(266, 28);
            this.selector_y.Margin = new System.Windows.Forms.Padding(4);
            this.selector_y.Name = "selector_y";
            this.selector_y.Size = new System.Drawing.Size(104, 28);
            this.selector_y.TabIndex = 1;
            this.selector_y.Text = "100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y";
            // 
            // selector_got
            // 
            this.selector_got.Location = new System.Drawing.Point(62, 150);
            this.selector_got.Margin = new System.Windows.Forms.Padding(4);
            this.selector_got.Multiline = true;
            this.selector_got.Name = "selector_got";
            this.selector_got.ReadOnly = true;
            this.selector_got.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.selector_got.Size = new System.Drawing.Size(542, 505);
            this.selector_got.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox1.Location = new System.Drawing.Point(632, 2);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(3, 675);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // selector_to_find
            // 
            this.selector_to_find.Location = new System.Drawing.Point(675, 64);
            this.selector_to_find.Margin = new System.Windows.Forms.Padding(4);
            this.selector_to_find.Multiline = true;
            this.selector_to_find.Name = "selector_to_find";
            this.selector_to_find.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.selector_to_find.Size = new System.Drawing.Size(542, 439);
            this.selector_to_find.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(672, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Selector";
            // 
            // find_selector
            // 
            this.find_selector.Location = new System.Drawing.Point(675, 532);
            this.find_selector.Margin = new System.Windows.Forms.Padding(4);
            this.find_selector.Name = "find_selector";
            this.find_selector.Size = new System.Drawing.Size(544, 34);
            this.find_selector.TabIndex = 0;
            this.find_selector.Text = "Find Selector";
            this.find_selector.UseVisualStyleBackColor = true;
            this.find_selector.Click += new System.EventHandler(this.FindSelector_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(672, 616);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Region";
            // 
            // found_region
            // 
            this.found_region.Location = new System.Drawing.Point(762, 612);
            this.found_region.Margin = new System.Windows.Forms.Padding(4);
            this.found_region.Name = "found_region";
            this.found_region.Size = new System.Drawing.Size(454, 28);
            this.found_region.TabIndex = 7;
            // 
            // get_tree
            // 
            this.get_tree.Location = new System.Drawing.Point(1297, 429);
            this.get_tree.Margin = new System.Windows.Forms.Padding(4);
            this.get_tree.Name = "get_tree";
            this.get_tree.Size = new System.Drawing.Size(304, 34);
            this.get_tree.TabIndex = 0;
            this.get_tree.Text = "Get Tree";
            this.get_tree.UseVisualStyleBackColor = true;
            this.get_tree.Click += new System.EventHandler(this.GetTree_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox2.Location = new System.Drawing.Point(1262, 2);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(3, 675);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1628, 675);
            this.Controls.Add(this.found_region);
            this.Controls.Add(this.selector_to_find);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.selector_got);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selector_y);
            this.Controls.Add(this.selector_x);
            this.Controls.Add(this.get_tree);
            this.Controls.Add(this.find_selector);
            this.Controls.Add(this.GetSelector);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetSelector;
        private System.Windows.Forms.TextBox selector_x;
        private System.Windows.Forms.TextBox selector_y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox selector_got;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox selector_to_find;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button find_selector;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox found_region;
        private System.Windows.Forms.Button get_tree;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

