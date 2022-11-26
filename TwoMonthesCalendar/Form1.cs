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
        Point m_TempMousePoint;
        AppSetting m_APS;



        public Form1()
        {
            InitializeComponent();

            notifyIconTwoMonthesCalendar.ContextMenuStrip = ContextMenu();

            Initialize();

        }

        private ContextMenuStrip ContextMenu()
        {
            // アイコンを右クリックしたときのメニューを返却
            var menu = new ContextMenuStrip();
            menu.Items.Add("終了", null, (s, e) => {
                Application.Exit();
            });

            menu.Items.Add("解像度 1920_1080向け", null, (s, e) => {
                ConstSetting.Resolution = ConstSetting.RESOLUTION.R1920_1080;
                AdjustCalendar();
                m_APS.SaveData();
            });

            menu.Items.Add("解像度 2560_1440向け", null, (s, e) => {
                ConstSetting.Resolution = ConstSetting.RESOLUTION.R2560_1440;
                AdjustCalendar();
                m_APS.SaveData();
            });

            return menu;
        }

        private void Initialize()
        {
            m_APS = new AppSetting();

            ConstSetting.Resolution = m_APS.Settings.m_Resolution;
            var showMonth = m_APS.Settings.m_ShowMonth;

            UpdateCalendar(showMonth);

        }

        private void UpdateCalendar(DateTime showMonth)
        {
            this.SuspendLayout();

            //今の表示を一旦破棄
            RemoveCalendar();

            CreateCalendar(showMonth);

            AdjustCalendar();

            this.ResumeLayout();
        }

        public void richTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Middle) == MouseButtons.Middle)
            {
                //位置を記憶する
                m_TempMousePoint = new Point(e.X, e.Y);
            }
        }

        public void richTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Middle) == MouseButtons.Middle)
            {
                this.Left += e.X - m_TempMousePoint.X;
                this.Top += e.Y - m_TempMousePoint.Y;
            }
        }

        public void richTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Middle) == MouseButtons.Middle)
            {
                SaveSetting();
            }
        }

        private void SaveSetting()
        {
            m_APS.Settings.m_Location = this.Location;
            m_APS.Settings.m_Resolution = ConstSetting.Resolution;
            m_APS.SaveData();
        }

        private void CreateCalendar(DateTime showMonth)
        {
            //日付部分作成
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


            //曜日ラベル作成
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

                m_WeekLabelDic1st[wDay].TextAlign = ContentAlignment.MiddleCenter;
                m_WeekLabelDic2nd[wDay].TextAlign = ContentAlignment.MiddleCenter;

                m_WeekLabelDic1st[wDay].ForeColor = Color.White;
                m_WeekLabelDic2nd[wDay].ForeColor = Color.White;
            }

            //年月表示更新
            label_ShowMonth.Text = showMonth.ToString("yyyy年MM月");
            label_NextMonth.Text = nextMonth.ToString("yyyy年MM月");


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
            //見栄え調整 カレンダー部分
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

            //見栄え調整 曜日ラベル部分
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
                    m_WeekLabelDic2nd[key].Size = ConstSetting.WeekLabelSize;                }
            }

            //ボタンとラベル
            label_NextMonth.Location = ConstSetting.NextMonthLabelInitialPosition;
            label_NextMonth.Size = ConstSetting.NextMonthLabelSize;
            label_NextMonth.TextAlign = ContentAlignment.MiddleCenter;
            label_NextMonth.ForeColor = Color.White;

            label_ShowMonth.Location = ConstSetting.ShowMonthLabelInitialPosition;
            label_ShowMonth.Size = ConstSetting.ShowMonthLabelSize;
            label_ShowMonth.TextAlign = ContentAlignment.MiddleCenter;
            label_ShowMonth.ForeColor = Color.White;

            button_NextMonth.Location = ConstSetting.NextMonthButtonInitialPosition;
            button_NextMonth.Size = ConstSetting.NextMonthButtonSize;

            button_PrevMonth.Location = ConstSetting.PrevMonthButtonInitialPosition;
            button_PrevMonth.Size = ConstSetting.PrevMonthButtonSize;


        }

        private void Save()
        {

            if (m_DateObjectDic1st != null)
            {
                var keys = m_DateObjectDic1st.Keys;
                foreach (var key in keys)
                {
                    m_DateObjectDic1st[key].Save();
                }
            }


            if (m_DateObjectDic2nd != null)
            {
                var keys = m_DateObjectDic2nd.Keys;
                foreach (var key in keys)
                {
                    m_DateObjectDic2nd[key].Save();
                }
                m_DateObjectDic2nd.Clear();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;//フォームの枠を非表示にする
            this.TransparencyKey = this.BackColor;


            this.Size = ConstSetting.FormSize;
            this.Location = m_APS.Settings.m_Location;
        }

        private void button_NextMonth_Click(object sender, EventArgs e)
        {
            m_APS.Settings.m_ShowMonth = m_APS.Settings.m_ShowMonth.AddMonths(1);
            m_APS.SaveData();

            var showMonth = m_APS.Settings.m_ShowMonth;
            UpdateCalendar(showMonth);
        }

        private void button_PrevMonth_Click(object sender, EventArgs e)
        {
            m_APS.Settings.m_ShowMonth = m_APS.Settings.m_ShowMonth.AddMonths(-1);
            m_APS.SaveData();

            var showMonth = m_APS.Settings.m_ShowMonth;
            UpdateCalendar(showMonth);

        }
    }
}