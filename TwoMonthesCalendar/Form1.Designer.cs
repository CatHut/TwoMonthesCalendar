namespace TwoMonthesCalendar
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIconTwoMonthesCalendar = new System.Windows.Forms.NotifyIcon(this.components);
            this.label_ShowMonth = new System.Windows.Forms.Label();
            this.label_NextMonth = new System.Windows.Forms.Label();
            this.button_PrevMonth = new System.Windows.Forms.Button();
            this.button_NextMonth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIconTwoMonthesCalendar
            // 
            this.notifyIconTwoMonthesCalendar.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconTwoMonthesCalendar.Icon")));
            this.notifyIconTwoMonthesCalendar.Text = "notifyIconTwoMonthesCalendar";
            this.notifyIconTwoMonthesCalendar.Visible = true;
            // 
            // label_ShowMonth
            // 
            this.label_ShowMonth.Location = new System.Drawing.Point(764, 275);
            this.label_ShowMonth.Name = "label_ShowMonth";
            this.label_ShowMonth.Size = new System.Drawing.Size(38, 15);
            this.label_ShowMonth.TabIndex = 0;
            this.label_ShowMonth.Text = "label1";
            this.label_ShowMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_NextMonth
            // 
            this.label_NextMonth.Location = new System.Drawing.Point(764, 331);
            this.label_NextMonth.Name = "label_NextMonth";
            this.label_NextMonth.Size = new System.Drawing.Size(38, 15);
            this.label_NextMonth.TabIndex = 1;
            this.label_NextMonth.Text = "label2";
            this.label_NextMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_PrevMonth
            // 
            this.button_PrevMonth.Location = new System.Drawing.Point(601, 301);
            this.button_PrevMonth.Name = "button_PrevMonth";
            this.button_PrevMonth.Size = new System.Drawing.Size(75, 23);
            this.button_PrevMonth.TabIndex = 2;
            this.button_PrevMonth.Text = "◀";
            this.button_PrevMonth.UseVisualStyleBackColor = true;
            this.button_PrevMonth.Click += new System.EventHandler(this.button_PrevMonth_Click);
            // 
            // button_NextMonth
            // 
            this.button_NextMonth.Location = new System.Drawing.Point(896, 301);
            this.button_NextMonth.Name = "button_NextMonth";
            this.button_NextMonth.Size = new System.Drawing.Size(75, 23);
            this.button_NextMonth.TabIndex = 3;
            this.button_NextMonth.Text = "▶";
            this.button_NextMonth.UseVisualStyleBackColor = true;
            this.button_NextMonth.Click += new System.EventHandler(this.button_NextMonth_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1993, 764);
            this.Controls.Add(this.button_NextMonth);
            this.Controls.Add(this.button_PrevMonth);
            this.Controls.Add(this.label_NextMonth);
            this.Controls.Add(this.label_ShowMonth);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private NotifyIcon notifyIconTwoMonthesCalendar;
        private Label label_ShowMonth;
        private Label label_NextMonth;
        private Button button_PrevMonth;
        private Button button_NextMonth;
    }
}