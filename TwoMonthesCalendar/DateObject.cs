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
        Label m_DayLabel;
        RichTextBox m_Rtb;

        public DateObject(Form1 form, DateTime date)
        {
            m_DayLabel = new Label();
            m_Rtb = new RichTextBox();

            AddToForm(form);
        }

        public void AddToForm(Form1 form)
        {
            if (m_DayLabel != null && m_Rtb != null)
            {
                form.Controls.Add(m_DayLabel);
                form.Controls.Add(m_Rtb);
            }
        }

        public void RemoveFromForm(Form1 form)
        {
            if (form.Controls.Contains(m_DayLabel))
            {
                //this.m_DayLabel.Click -= new System.EventHandler(this.m_DayLabel_Click);
                form.Controls.Remove(m_DayLabel);
                m_DayLabel.Dispose();
            }

            if (form.Controls.Contains(m_Rtb))
            {
                //this.m_Rtb.Click -= new System.EventHandler(this.m_Rtb_Click);
                form.Controls.Remove(m_Rtb);
                m_Rtb.Dispose();
            }

        }

        public void SetPosition(DateTime date, bool is1st)
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            var weekOfMonth1stDay = cal.GetWeekOfYear(new DateTime(date.Year, date.Month, 1), dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var weekOfDay = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var weekOfMonth = weekOfDay - weekOfMonth1stDay;
            var weekDay = (int)date.DayOfWeek;


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
