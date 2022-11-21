using CatHut;

namespace TwoMonthesCalendar
{
    public partial class Form1 : Form
    {

        Dictionary<DateTime, DateObject> m_DateObjectDic1st;
        Dictionary<DateTime, DateObject> m_DateObjectDic2nd;
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

            m_DateObjectDic1st = new Dictionary<DateTime, DateObject>();

            var days = DateTime.DaysInMonth(showMonth.Year, showMonth.Month);
            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(showMonth.Year, showMonth.Month, i + 1);
                m_DateObjectDic1st.Add(date, new DateObject(this, date));
            }

            m_DateObjectDic2nd = new Dictionary<DateTime, DateObject>();


            var nextMonth = m_APS.Settings.m_ShowMonth;

            days = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            for (int i = 0; i < days; i++)
            {
                var date = new DateTime(nextMonth.Year, nextMonth.Month, i + 1);
                m_DateObjectDic2nd.Add(date, new DateObject(this, date));
            }

            AdjustCalendar();

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

        }

        private void AdjustCalendar()
        {
            //å©âhÇ¶í≤êÆ

        }




    }
}