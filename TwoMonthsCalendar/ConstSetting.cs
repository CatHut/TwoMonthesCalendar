using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.Design.AxImporter;

namespace TwoMonthesCalendar
{
    public static class ConstSetting
    {

        public enum RESOLUTION
        {
            R1920_1080,
            R2560_1440
        }

        public enum DAY_KIND
        {
            WEEKDAY,
            SATURDAY,
            HOLYDAY
        }

        public enum LAYOUT_DIRECTION
        {
            VIRTICAL,
            HORIZONTAL
        }

        /// <summary>
        /// 日付ラベルの先頭位置
        /// </summary>
        private static readonly System.Drawing.Size _DateLabelInitialPosition = new Size(0, 30);
        /// <summary>
        /// 日付ラベルのサイズ
        /// </summary>
        private static readonly System.Drawing.Size _DateLabelSize = new Size(165, 22);
        /// <summary>
        /// 二月目の位置
        /// </summary>
        private static readonly Dictionary<LAYOUT_DIRECTION, Size> _SecondMonthPositionDic = new Dictionary<LAYOUT_DIRECTION, Size>{
             { LAYOUT_DIRECTION.VIRTICAL, new Size(0, 700) }
            ,{ LAYOUT_DIRECTION.HORIZONTAL, new Size(1220, 0) }
        };
        /// <summary>
        /// 配置間隔
        /// </summary>
        private static readonly System.Drawing.Size _SeparateSize = new Size(167, 107);
        /// <summary>
        /// テキストボックスの先頭位置
        /// </summary>
        private static readonly System.Drawing.Size _TextBoxInitialPosition = new Size(0, 50);
        /// <summary>
        /// テキストボックスのサイズ
        /// </summary>
        private static readonly System.Drawing.Size _TextBoxSize = new Size(165, 85);
        /// <summary>
        /// 曜日表示の先頭位置
        /// </summary>
        private static readonly Size _WeekLabelInitialPosition = new Size(0, 0);
        /// <summary>
        /// 曜日表示のサイズ
        /// </summary>
        private static readonly Size _WeekLabelSize = new Size(165, 30);


        /// <summary>
        /// 前月ボタンの先頭位置
        /// </summary>
        private static readonly Size _PrevMonthButtonInitialPosition = new Size(409, 610);
        /// <summary>
        /// 前月ボタンのサイズ
        /// </summary>
        private static readonly Size _PrevMonthButtonSize = new Size(75, 45);
        /// <summary>
        /// 次月ボタンの先頭位置
        /// </summary>
        private static readonly Size _NextMonthButtonInitialPosition = new Size(684, 610);
        /// <summary>
        /// 次月ボタンのサイズ
        /// </summary>
        private static readonly Size _NextMonthButtonSize = new Size(75, 45);

        /// <summary>
        /// 今月表示の先頭位置
        /// </summary>
        private static readonly Size _ShowMonthLabelInitialPosition = new Size(484, 587);
        /// <summary>
        /// 今月表示のサイズ
        /// </summary>
        private static readonly Size _ShowMonthLabelSize = new Size(200, 30);
        /// <summary>
        /// 次月表示の先頭位置
        /// </summary>
        private static readonly Size _NextMonthLabelInitialPosition = new Size(484, 647);
        /// <summary>
        /// 次月表示のサイズ
        /// </summary>
        private static readonly Size _NextMonthLabelSize = new Size(200, 30);


        private static int _SecCount = 1;
        private static DateTime _InvalidDateTime = new DateTime(0001, 1, 1, 0, 0, 0);



        private static Dictionary<DateTime, string> _HolidayDic = null;


        /// <summary>
        /// 解像度の指定
        /// </summary>
        public static RESOLUTION Resolution { get; set; } = RESOLUTION.R2560_1440;

        /// <summary>
        /// 解像度の指定
        /// </summary>
        public static LAYOUT_DIRECTION LayoutDirection { get; set; } = LAYOUT_DIRECTION.VIRTICAL;


        private static Dictionary<RESOLUTION, float> _ResolutionCoefDic = new Dictionary<RESOLUTION, float> {
            { RESOLUTION.R1920_1080, 0.75f },
            { RESOLUTION.R2560_1440, 1f }
        };

        private static float FontSizeDiff = 3f;


        private static Dictionary<RESOLUTION, Size> _FormSizeDic = new Dictionary<RESOLUTION, Size> {
            { RESOLUTION.R1920_1080, new Size(1920, 1080) },
            { RESOLUTION.R2560_1440, new Size(2560, 1440) }
        };

        /// <summary>
        /// 日付ラベルのサイズ
        /// </summary>
        public static Size DateLabelSize
        {
            get
            {
                return (_DateLabelSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        /// <summary>
        /// 日付ラベルの先頭位置
        /// </summary>
        public static Point DateLabelInitialPosition
        {
            get
            {
                return (Point)(_DateLabelInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static DAY_KIND GetDayKind(DateTime dt)
        {
            var ret = DAY_KIND.WEEKDAY;
            if(dt.DayOfWeek== DayOfWeek.Saturday) {
                ret = DAY_KIND.SATURDAY;
            }

            if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                ret = DAY_KIND.HOLYDAY;
            }

            if(HolidayDic.ContainsKey(dt))
            {
                ret = DAY_KIND.HOLYDAY;
            }

            return ret;
        }

        /// <summary>
        /// テキストボックスのサイズ
        /// </summary>
        public static Size TextBoxSize
        {
            get
            {
                return (_TextBoxSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        /// <summary>
        /// テキストボックスの先頭位置
        /// </summary>
        public static Point TextBoxInitialPosition
        {
            get
            {
                return (Point)(_TextBoxInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        /// <summary>
        /// 配置間隔
        /// </summary>
        public static Size SeparateSize
        {
            get
            {
                return (_SeparateSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        /// <summary>
        /// 二月目の位置
        /// </summary>
        public static Point SecondMonthPosition
        {
            get
            {
                return (Point)(_SecondMonthPositionDic[LayoutDirection] * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Dictionary<DateTime, string> HolidayDic
        {
            get
            {
                if (_HolidayDic == null)
                {
                    _HolidayDic = GetHoliday();
                }

                return _HolidayDic;
            }
        }

        public static bool IsHoliday(DateTime date)
        {
            return _HolidayDic.ContainsKey(date);
        }



        private static Dictionary<DateTime, string> GetHoliday()
        {
            var json = WebRequestHoliday();

            var result = ParseResult(json);

            return result;

        }

        private static string WebRequestHoliday()
        {
            //https://holidays-jp.github.io/
            var url = @"https://holidays-jp.github.io/api/v1/date.json";
            var req = new HttpClient();

            var result = req.GetStringAsync(url).Result;

            req.Dispose();

            return result;
        }

        private static Dictionary<DateTime, string> ParseResult(string json)
        {
            var ret = JsonSerializer.Deserialize<Dictionary<DateTime, string>>(json);

            return ret;
        }




        public static string GetWeekDayText(DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday:
                    return "日";
                case DayOfWeek.Monday:
                    return "月";
                case DayOfWeek.Tuesday:
                    return "火";
                case DayOfWeek.Wednesday:
                    return "水";
                case DayOfWeek.Thursday:
                    return "木";
                case DayOfWeek.Friday:
                    return "金";
                case DayOfWeek.Saturday:
                    return "土";
                default:
                    return "";

            }
        }

        /// <summary>
        /// 曜日表示の先頭位置
        /// </summary>
        public static Point WeekLabelInitialPosition
        {
            get
            {
                return (Point)(_WeekLabelInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }



        /// <summary>
        /// 曜日表示のサイズ
        /// </summary>
        public static Size WeekLabelSize
        {
            get
            {
                return (_WeekLabelSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        /// <summary>
        /// フォームのサイズ
        /// </summary>
        public static Size FormSize
        {
            get
            {
                return _FormSizeDic[Resolution];
            }
        }

        public static string SaveFolder { get; } = @"SaveData\";

        public static string SaveFileExt { get; } = ".txt";

        public static Point PrevMonthButtonInitialPosition
        {
            get
            {
                return (Point)(_PrevMonthButtonInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Size PrevMonthButtonSize
        {
            get
            {
                return (_PrevMonthButtonSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Point NextMonthButtonInitialPosition
        {
            get
            {
                return (Point)(_NextMonthButtonInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Size NextMonthButtonSize
        {
            get
            {
                return (_NextMonthButtonSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Point ShowMonthLabelInitialPosition
        {
            get
            {
                return (Point)(_ShowMonthLabelInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Size ShowMonthLabelSize
        {
            get
            {
                return (_ShowMonthLabelSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Point NextMonthLabelInitialPosition
        {
            get
            {
                return (Point)(_NextMonthLabelInitialPosition * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        public static Size NextMonthLabelSize
        {
            get
            {
                return (_NextMonthLabelSize * _ResolutionCoefDic[Resolution]).ToSize();
            }
        }

        private static Font _DateLabelFont = new Font("Yu Gothic UI",10f);
        private static Font _TextBoxDefaultFont = new Font("Yu Gothic UI", 10f);
        private static Font _WeekLabelFont = new Font("Yu Gothic UI", 12f, FontStyle.Bold);
        private static Font _ShowMonthLabelFont = new Font("Yu Gothic UI", 14f);
        private static Font _NextMonthLabelFont = new Font("Yu Gothic UI", 14f);

        public static void FontUpdate()
        {
            switch (Resolution)
            {
                case RESOLUTION.R1920_1080:
                    _DateLabelFont = new Font(_DateLabelFont.Name, _DateLabelFont.Size - FontSizeDiff);
                    _TextBoxDefaultFont = new Font(_TextBoxDefaultFont.Name, _TextBoxDefaultFont.Size - FontSizeDiff);
                    _WeekLabelFont = new Font(_WeekLabelFont.Name, _WeekLabelFont.Size - FontSizeDiff);
                    _ShowMonthLabelFont = new Font(_ShowMonthLabelFont.Name, _ShowMonthLabelFont.Size - FontSizeDiff);
                    _NextMonthLabelFont = new Font(_NextMonthLabelFont.Name, _NextMonthLabelFont.Size - FontSizeDiff);
                    break;
                case RESOLUTION.R2560_1440:
                    _DateLabelFont = new Font(_DateLabelFont.Name, _DateLabelFont.Size + FontSizeDiff);
                    _TextBoxDefaultFont = new Font(_TextBoxDefaultFont.Name, _TextBoxDefaultFont.Size + FontSizeDiff);
                    _WeekLabelFont = new Font(_WeekLabelFont.Name, _WeekLabelFont.Size + FontSizeDiff);
                    _ShowMonthLabelFont = new Font(_ShowMonthLabelFont.Name, _ShowMonthLabelFont.Size + FontSizeDiff);
                    _NextMonthLabelFont = new Font(_NextMonthLabelFont.Name, _NextMonthLabelFont.Size + FontSizeDiff);
                    break;
            }

        }

        public static Font DateLabelFont
        {
            get
            {
                return _DateLabelFont;
            }
        }

        public static Font TextBoxDefaultFont
        {
            get
            {
                return _TextBoxDefaultFont;
            }
        }

        public static Font WeekLabelFont
        {
            get
            {
                return _WeekLabelFont;

            }
        }

        public static Font ShowMonthLabelFont
        {
            get
            {
                return _ShowMonthLabelFont;
            }
        }

        public static Font NextMonthLabelFont
        {
            get
            {
                return _NextMonthLabelFont;
            }
        }

        public static void ChangeKey(ref Dictionary<DateTime, DateObject> dic, DateTime oldKey, DateTime newKey)
        {
            dic[newKey] = dic[oldKey];
            dic.Remove(oldKey);
        }

        public static DateTime InvalidDate
        {
            get
            {
                _SecCount++;
                _SecCount %= 100000; 
                return _InvalidDateTime.AddSeconds(_SecCount);
            }
        }

    }
}