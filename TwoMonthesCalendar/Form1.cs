using CatHut;
using System.Windows.Forms;

namespace TwoMonthesCalendar
{

    public partial class Form1 : Form
    {

        Dictionary<DateTime, DateObject> m_DateObjectDic1st;
        Dictionary<DateTime, DateObject> m_DateObjectDic2nd;
        Dictionary<DayOfWeek, Label> m_WeekLabelDic1st;
        Dictionary<DayOfWeek, Label> m_WeekLabelDic2nd;
        AppSetting m_APS;



        public Form1()
        {
            InitializeComponent();

            Initialize();

        }


        private void Initialize()
        {
            m_APS = new AppSetting();

            var showMonth = m_APS.Settings.m_ShowMonth;

            UpdateCalendar(showMonth);

        }

        private void UpdateCalendar(DateTime showMonth)
        {
            //ç°ÇÃï\é¶ÇàÍíUîjä¸
            RemoveCalendar();

            CreateCalendar(showMonth);

            AdjustCalendar();

        }

        private void CreateCalendar(DateTime showMonth)
        {
            //ì˙ïtïîï™çÏê¨
            if (m_DateObjectDic1st == null)
            {
                m_DateObjectDic1st = new Dictionary<DateTime, DateObject>();
            }

            var days = DateTime.DaysInMonth(showMonth.Year, showMonth.Month);
            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(showMonth.Year, showMonth.Month, i + 1);
                m_DateObjectDic1st.Add(date, new DateObject(this, date));
            }


            if (m_DateObjectDic2nd == null)
            {
                m_DateObjectDic2nd = new Dictionary<DateTime, DateObject>();
            }

            var nextMonth = showMonth.AddMonths(1);

            days = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(nextMonth.Year, nextMonth.Month, i + 1);
                m_DateObjectDic2nd.Add(date, new DateObject(this, date));
            }


            //ójì˙ÉâÉxÉãçÏê¨
            foreach(var temp in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (m_WeekLabelDic1st == null)
                {
                    m_WeekLabelDic1st = new Dictionary<DayOfWeek, Label>();
                }
                if (m_WeekLabelDic2nd == null)
                {
                    m_WeekLabelDic2nd = new Dictionary<DayOfWeek, Label>();
                }

                var wDay = (DayOfWeek)temp;
                m_WeekLabelDic1st.Add(wDay, new Label());
                m_WeekLabelDic2nd.Add(wDay, new Label());
                this.Controls.Add(m_WeekLabelDic1st[wDay]);
                this.Controls.Add(m_WeekLabelDic2nd[wDay]);

                m_WeekLabelDic1st[wDay].Text = ConstSetting.GetWeekDayText(wDay);
                m_WeekLabelDic2nd[wDay].Text = ConstSetting.GetWeekDayText(wDay);

            }

        }

        private void RemoveCalendar()
        {
            if (m_DateObjectDic1st != null)
            {
                var keys = m_DateObjectDic1st.Keys;
                foreach (var key in keys)
                {
                    m_DateObjectDic1st[key].RemoveFromForm(this);
                    m_DateObjectDic1st[key] = null;
                }
                m_DateObjectDic1st.Clear();
            }


            if (m_DateObjectDic2nd != null)
            {
                var keys = m_DateObjectDic2nd.Keys;
                foreach (var key in keys)
                {
                    m_DateObjectDic2nd[key].RemoveFromForm(this);
                    m_DateObjectDic2nd[key] = null;
                }
                m_DateObjectDic2nd.Clear();
            }


            if (m_WeekLabelDic1st != null)
            {
                var keys = m_WeekLabelDic1st.Keys;
                foreach (var key in keys)
                {
                    this.Controls.Remove(m_WeekLabelDic1st[key]);
                    m_WeekLabelDic1st[key].Dispose();
                    m_WeekLabelDic1st[key] = null;
                }
                m_WeekLabelDic1st.Clear();
            }


            if (m_WeekLabelDic2nd != null)
            {
                var keys = m_WeekLabelDic2nd.Keys;
                foreach (var key in keys)
                {
                    this.Controls.Remove(m_WeekLabelDic2nd[key]);
                    m_WeekLabelDic2nd[key].Dispose();
                    m_WeekLabelDic2nd[key] = null;
                }
                m_WeekLabelDic2nd.Clear();
            }


        }

        private void AdjustCalendar()
        {
            //å©âhÇ¶í≤êÆ ÉJÉåÉìÉ_Å[ïîï™
            {
                var keys = m_DateObjectDic1st.Keys;

                foreach (var key in keys)
                {
                    m_DateObjectDic1st[key].SetPosition(key, true);
                }
            }

            {
                var keys = m_DateObjectDic2nd.Keys;

                foreach (var key in keys)
                {
                    m_DateObjectDic2nd[key].SetPosition(key, false);
                }
            }

            //å©âhÇ¶í≤êÆ ójì˙ÉâÉxÉãïîï™
            {
                var keys = m_WeekLabelDic1st.Keys;
                foreach (var key in keys)
                {
                    m_WeekLabelDic1st[key].Location = new Point(
                        ConstSetting.WeekLabelInitialPosition.X + ConstSetting.SeparateSize.Width * (int)key,
                        ConstSetting.WeekLabelInitialPosition.Y
                    );
                    m_WeekLabelDic1st[key].Size = ConstSetting.WeekLabelSize;
                }
            }

            {
                var keys = m_WeekLabelDic2nd.Keys;
                foreach (var key in keys)
                {
                    m_WeekLabelDic2nd[key].Location = new Point(
                        ConstSetting.SecondMonthPosition.X + ConstSetting.WeekLabelInitialPosition.X + ConstSetting.SeparateSize.Width * (int)key,
                        ConstSetting.SecondMonthPosition.Y + ConstSetting.WeekLabelInitialPosition.Y
                    );
                    m_WeekLabelDic2nd[key].Size = ConstSetting.WeekLabelSize;
                }
            }
        }




    }
}