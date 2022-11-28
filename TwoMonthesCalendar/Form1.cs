using CatHut;
using System.Drawing;
using System.Runtime.InteropServices;
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
            this.WindowState = FormWindowState.Minimized;
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

                if(ConstSetting.Resolution != ConstSetting.RESOLUTION.R1920_1080)
                {
                    ConstSetting.Resolution = ConstSetting.RESOLUTION.R1920_1080;
                    ConstSetting.FontUpdate();
                    AdjustCalendar();
                    m_APS.SaveData();
                }
            });

            menu.Items.Add("解像度 2560_1440向け", null, (s, e) => {
                if (ConstSetting.Resolution != ConstSetting.RESOLUTION.R2560_1440)
                {
                    ConstSetting.Resolution = ConstSetting.RESOLUTION.R2560_1440;
                    ConstSetting.FontUpdate();
                    AdjustCalendar();
                    m_APS.SaveData();
                }

            });

            return menu;
        }

        private void Initialize()
        {
            m_APS = new AppSetting();

            ConstSetting.Resolution = m_APS.Settings.m_Resolution;
            var showMonth = m_APS.Settings.m_ShowMonth;

            this.DoubleBuffered = true;

            if(ConstSetting.Resolution == ConstSetting.RESOLUTION.R1920_1080)
            {
                ConstSetting.FontUpdate();
            }

            CreateCalendar();
            AddControls();
            UpdateCalendar(showMonth);



        }

        private void UpdateCalendar(DateTime showMonth)
        {
            BeginControlUpdate(this);

            //今の表示を一旦破棄
            //RemoveCalendar();

            RefreshCalendar(showMonth);

            AdjustCalendar();


            EndControlUpdate(this);


            this.Refresh();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0x000B;

        /// <summary>
        /// コントロールの再描画を停止させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void BeginControlUpdate(Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// コントロールの再描画を再開させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EndControlUpdate(Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            control.Invalidate();
        }


        private void AddControls()
        {
            var controlList = new List<Control>();

            controlList.AddRange(m_WeekLabelDic1st.Values.ToArray());
            controlList.AddRange(m_WeekLabelDic2nd.Values.ToArray());

            {
                var keys = m_DateObjectDic1st.Keys;

                foreach(var key in keys)
                {
                    controlList.Add(m_DateObjectDic1st[key].m_DayLabel);
                    controlList.Add(m_DateObjectDic1st[key].m_Rtb);
                }
            }

            {
                var keys = m_DateObjectDic2nd.Keys;

                foreach (var key in keys)
                {
                    controlList.Add(m_DateObjectDic2nd[key].m_DayLabel);
                    controlList.Add(m_DateObjectDic2nd[key].m_Rtb);
                }
            }

            this.Controls.AddRange(controlList.ToArray());

            controlList.Clear();

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


        private void CreateCalendar()
        {
            //日付部分作成
            if (m_DateObjectDic1st == null)
            {
                m_DateObjectDic1st = new Dictionary<DateTime, DateObject>();

                var date = new DateTime(1900, 1, 1);
                for (int i = 0; i < 31; i++)
                {
                    date = date.AddDays(1);
                    m_DateObjectDic1st.Add(date, new DateObject(this, date));
                }
            }

            if (m_DateObjectDic2nd == null)
            {
                m_DateObjectDic2nd = new Dictionary<DateTime, DateObject>();
                var date = new DateTime(1900, 1, 1);
                for (int i = 0; i < 31; i++)
                {
                    date = date.AddDays(1);
                    m_DateObjectDic2nd.Add(date, new DateObject(this, date));
                }
            }


            //曜日ラベル作成
            foreach (var temp in Enum.GetValues(typeof(DayOfWeek)))
            {
                var wDay = (DayOfWeek)temp;

                if (m_WeekLabelDic1st == null)
                {
                    m_WeekLabelDic1st = new Dictionary<DayOfWeek, Label>();
                }

                if (m_WeekLabelDic2nd == null)
                {
                    m_WeekLabelDic2nd = new Dictionary<DayOfWeek, Label>();
                }

                m_WeekLabelDic1st.Add(wDay, new Label());
                m_WeekLabelDic2nd.Add(wDay, new Label());

                m_WeekLabelDic1st[wDay].Text = ConstSetting.GetWeekDayText(wDay);
                m_WeekLabelDic2nd[wDay].Text = ConstSetting.GetWeekDayText(wDay);

                m_WeekLabelDic1st[wDay].TextAlign = ContentAlignment.MiddleCenter;
                m_WeekLabelDic2nd[wDay].TextAlign = ContentAlignment.MiddleCenter;

                m_WeekLabelDic1st[wDay].ForeColor = Color.White;
                m_WeekLabelDic2nd[wDay].ForeColor = Color.White;
            }


            //今月次月表示
            label_NextMonth.TextAlign = ContentAlignment.MiddleCenter;
            label_NextMonth.ForeColor = Color.White;
            label_NextMonth.BackColor = Color.DarkGray;
            label_ShowMonth.TextAlign = ContentAlignment.MiddleCenter;
            label_ShowMonth.ForeColor = Color.White;
            label_ShowMonth.BackColor = Color.DarkGray;



        }

        private void RefreshCalendar(DateTime showMonth)
        {
            //日付部分設定
            var color = Color.WhiteSmoke;   //背景色
            var nextMonth = showMonth.AddMonths(1);
            {
                var days = DateTime.DaysInMonth(showMonth.Year, showMonth.Month);

                var keys = m_DateObjectDic1st.Keys.ToList();
                int i = 0;

                foreach (var key in keys)
                {
                    if (i >= days) { break; }

                    var date = new DateTime(showMonth.Year, showMonth.Month, i + 1);
                    ConstSetting.ChangeKey(ref m_DateObjectDic1st, key, date);
                    m_DateObjectDic1st[date].ChangeDate(date);
                    m_DateObjectDic1st[date].SetDateColor(date);
                    m_DateObjectDic1st[date].SetBackColor(color);


                    i++;
                }
            }

            {
                var days = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);

                var keys = m_DateObjectDic2nd.Keys.ToList();
                int i = 0;
                foreach (var key in keys)
                {
                    if (i >= days) { break; }

                    var date = new DateTime(nextMonth.Year, nextMonth.Month, i + 1);
                    ConstSetting.ChangeKey(ref m_DateObjectDic2nd, key, date);
                    m_DateObjectDic2nd[date].ChangeDate(date);
                    m_DateObjectDic2nd[date].SetDateColor(date);
                    m_DateObjectDic2nd[date].SetBackColor(color);

                    i++;
                }
            }


            //曜日ラベル設定
            foreach (var temp in Enum.GetValues(typeof(DayOfWeek)))
            {
                var wDay = (DayOfWeek)temp;

                m_WeekLabelDic1st[wDay].Text = ConstSetting.GetWeekDayText(wDay);
                m_WeekLabelDic2nd[wDay].Text = ConstSetting.GetWeekDayText(wDay);

                m_WeekLabelDic1st[wDay].TextAlign = ContentAlignment.MiddleCenter;
                m_WeekLabelDic2nd[wDay].TextAlign = ContentAlignment.MiddleCenter;

                m_WeekLabelDic1st[wDay].ForeColor = Color.White;
                m_WeekLabelDic2nd[wDay].ForeColor = Color.White;

                m_WeekLabelDic1st[wDay].BackColor = Color.Gray;
                m_WeekLabelDic2nd[wDay].BackColor = Color.Gray;


                if (wDay == DayOfWeek.Saturday)
                {
                    m_WeekLabelDic1st[wDay].BackColor = Color.Blue;
                    m_WeekLabelDic2nd[wDay].BackColor = Color.Blue;

                    m_WeekLabelDic1st[wDay].ForeColor = Color.White;
                    m_WeekLabelDic2nd[wDay].ForeColor = Color.White;
                }

                if (wDay == DayOfWeek.Sunday)
                {
                    m_WeekLabelDic1st[wDay].BackColor = Color.Red;
                    m_WeekLabelDic2nd[wDay].BackColor = Color.Red;

                    m_WeekLabelDic1st[wDay].ForeColor = Color.White;
                    m_WeekLabelDic2nd[wDay].ForeColor = Color.White;

                }


            }


            //年月表示更新
            label_ShowMonth.Text = showMonth.ToString("yyyy年 MM月");
            label_NextMonth.Text = nextMonth.ToString("yyyy年 MM月");


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
            //位置調整
            {
                var keys = m_DateObjectDic1st.Keys;

                foreach (var key in keys)
                {
                    var month = new DateTime(key.Year, key.Month, 1);
                    if (m_APS.Settings.m_ShowMonth == month) {
                        m_DateObjectDic1st[key].SetPosition(key, true);
                        m_DateObjectDic1st[key].SetDateFont();
                        m_DateObjectDic1st[key].SetTextBoxFont();

                        if(key.Date == DateTime.Today)
                        {
                            m_DateObjectDic1st[key].SetBackColor(Color.Cornsilk);
                        }

                    }
                }
            }

            {
                var keys = m_DateObjectDic2nd.Keys;
                var nextMonth = m_APS.Settings.m_ShowMonth.AddMonths(1);

                foreach (var key in keys)
                {
                    var month = new DateTime(key.Year, key.Month, 1);
                    if (nextMonth == month)
                    {
                        m_DateObjectDic2nd[key].SetPosition(key, false);
                        m_DateObjectDic2nd[key].SetDateFont();
                        m_DateObjectDic2nd[key].SetTextBoxFont();

                        if (key.Date == DateTime.Today)
                        {
                            m_DateObjectDic2nd[key].SetBackColor(Color.Cornsilk);
                        }
                    }
                }
            }

            //表示/非表示更新
            {
                var keys = m_DateObjectDic1st.Keys.ToList();

                foreach (var key in keys)
                {
                    var month = new DateTime(key.Year, key.Month, 1);
                    if (m_APS.Settings.m_ShowMonth != month)
                    {
                        m_DateObjectDic1st[key].SetVisible(false);
                        var date = ConstSetting.InvalidDate;
                        ConstSetting.ChangeKey(ref m_DateObjectDic1st, key, date);

                    }
                    else
                    {
                        m_DateObjectDic1st[key].SetVisible(true);
                    }
                }
            }

            {
                var keys = m_DateObjectDic2nd.Keys.ToList();
                var nextMonth = m_APS.Settings.m_ShowMonth.AddMonths(1);

                foreach (var key in keys)
                {
                    var month = new DateTime(key.Year, key.Month, 1);
                    var i = 1;
                    if (nextMonth != month)
                    {
                        m_DateObjectDic2nd[key].SetVisible(false);
                        var date = ConstSetting.InvalidDate;
                        ConstSetting.ChangeKey(ref m_DateObjectDic2nd, key, date);
                    }
                    else
                    {
                        m_DateObjectDic2nd[key].SetVisible(true);
                    }
                }
            }

            //今日をハイライト




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
                    m_WeekLabelDic1st[key].Font = ConstSetting.WeekLabelFont;
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
                    m_WeekLabelDic2nd[key].Font = ConstSetting.WeekLabelFont;
                }
            }

            //ボタンとラベル
            label_NextMonth.Location = ConstSetting.NextMonthLabelInitialPosition;
            label_NextMonth.Size = ConstSetting.NextMonthLabelSize;
            label_NextMonth.Font = ConstSetting.NextMonthLabelFont;

            label_ShowMonth.Location = ConstSetting.ShowMonthLabelInitialPosition;
            label_ShowMonth.Size = ConstSetting.ShowMonthLabelSize;
            label_ShowMonth.Font = ConstSetting.ShowMonthLabelFont;

            button_NextMonth.Location = ConstSetting.NextMonthButtonInitialPosition;
            button_NextMonth.Size = ConstSetting.NextMonthButtonSize;

            button_PrevMonth.Location = ConstSetting.PrevMonthButtonInitialPosition;
            button_PrevMonth.Size = ConstSetting.PrevMonthButtonSize;

            this.Size = ConstSetting.FormSize;



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

        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;//フォームの枠を非表示にする
            this.TransparencyKey = this.BackColor;

            this.WindowState = FormWindowState.Normal;
            this.Size = ConstSetting.FormSize;
            this.Location = m_APS.Settings.m_Location;
        }
    }
}