using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoMonthesCalendar
{
    internal class DateObject
    {
        string m_FileName;
        public Label m_DayLabel;
        public RichTextBox m_Rtb;
        bool m_Loading;

        public DateObject(Form1 form, DateTime date)
        {
            m_FileName = ConstSetting.SaveFolder + date.ToString("yyyyMMdd") + ConstSetting.SaveFileExt;
            m_DayLabel = new Label();
            m_Rtb = new RichTextBox();

            //プロパティ初期化
            m_DayLabel.BackColor = Color.White;
            
            m_Rtb.BorderStyle = BorderStyle.None;

            //イベント設定
            this.m_Rtb.MouseDown += new System.Windows.Forms.MouseEventHandler(form.richTextBox_MouseDown);
            this.m_Rtb.MouseMove += new System.Windows.Forms.MouseEventHandler(form.richTextBox_MouseMove);
            this.m_Rtb.MouseUp += new System.Windows.Forms.MouseEventHandler(form.richTextBox_MouseUp);
            this.m_Rtb.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);

            Load();

        }

        public void ChangeDate(DateTime date)
        {
            m_FileName = ConstSetting.SaveFolder + date.ToString("yyyyMMdd") + ConstSetting.SaveFileExt;
            Load();
        }

        public void SetVisible(bool visible)
        {
            m_DayLabel.Visible = visible;
            m_Rtb.Visible = visible;
        }


        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_Loading != true)
            {
                Save();
            }
        }

        public void Save()
        {
            if (!Directory.Exists(ConstSetting.SaveFolder)) { Directory.CreateDirectory(ConstSetting.SaveFolder); }
            m_Rtb.SaveFile(m_FileName);
        }

        public void Load()
        {
            m_Loading = true;
            m_Rtb.Text = "";
            if (File.Exists(m_FileName))
            {
                m_Rtb.LoadFile(m_FileName);
            }
            m_Loading = false;
        }

        public void RemoveFromForm(Form1 form)
        {
            if (form.Controls.Contains(m_DayLabel))
            {
                form.Controls.Remove(m_DayLabel);
                m_DayLabel.Dispose();
            }

            if (form.Controls.Contains(m_Rtb))
            {
                this.m_Rtb.MouseDown -= new System.Windows.Forms.MouseEventHandler(form.richTextBox_MouseDown);
                this.m_Rtb.MouseMove -= new System.Windows.Forms.MouseEventHandler(form.richTextBox_MouseMove);
                this.m_Rtb.MouseUp -= new System.Windows.Forms.MouseEventHandler(form.richTextBox_MouseUp);
                this.m_Rtb.TextChanged -= new System.EventHandler(this.richTextBox_TextChanged);
                form.Controls.Remove(m_Rtb);
                m_Rtb.Dispose();
            }

        }

        //public boid SetUpdateEnable(bool enable)
        //{
        //    m_Rtb.U
        //}



        public void SetPosition(DateTime date, bool is1st)
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            var weekOfMonth1stDay = cal.GetWeekOfYear(new DateTime(date.Year, date.Month, 1), dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var weekOfDay = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var weekOfMonth = weekOfDay - weekOfMonth1stDay;
            var weekDay = (int)date.DayOfWeek;


            m_DayLabel.Text = date.Day.ToString();

            if (ConstSetting.HolidayDic.ContainsKey(date))
            {
                m_DayLabel.Text += " " + ConstSetting.HolidayDic[date];
            }

            if (is1st == true)
            {
                m_DayLabel.Location = new Point(
                    ConstSetting.DateLabelInitialPosition.X + ConstSetting.SeparateSize.Width * weekDay,
                    ConstSetting.DateLabelInitialPosition.Y + ConstSetting.SeparateSize.Height * weekOfMonth
                    );
                m_DayLabel.Size = ConstSetting.DateLabelSize;

                m_Rtb.Location = new Point(
                    ConstSetting.TextBoxInitialPosition.X + ConstSetting.SeparateSize.Width * weekDay,
                    ConstSetting.TextBoxInitialPosition.Y + ConstSetting.SeparateSize.Height * weekOfMonth
                    );
                m_Rtb.Size = ConstSetting.TextBoxSize;
            }
            else
            {
                m_DayLabel.Location = new Point(
                    ConstSetting.DateLabelInitialPosition.X + ConstSetting.SeparateSize.Width * weekDay,
                    ConstSetting.SecondMonthPosition.Y + ConstSetting.DateLabelInitialPosition.Y + ConstSetting.SeparateSize.Height * weekOfMonth
                    );
                m_DayLabel.Size = ConstSetting.DateLabelSize;

                m_Rtb.Location = new Point(
                    ConstSetting.TextBoxInitialPosition.X + ConstSetting.SeparateSize.Width * weekDay,
                    ConstSetting.SecondMonthPosition.Y + ConstSetting.TextBoxInitialPosition.Y + ConstSetting.SeparateSize.Height * weekOfMonth
                    );
                m_Rtb.Size = ConstSetting.TextBoxSize;
            }


        }
    }
}
