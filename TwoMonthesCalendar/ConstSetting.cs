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

        /// <summary>
        /// 日付ラベルの先頭位置
        /// </summary>
        private static readonly System.Drawing.Size _DateLabelInitialPosition = new Size(0, 30);
        /// <summary>
        /// 日付ラベルのサイズ
        /// </summary>
        private static readonly System.Drawing.Size _DateLabelSize = new Size(200, 32);
        /// <summary>
        /// 二月目の位置
        /// </summary>
        private static readonly System.Drawing.Size _SecondMonthPosition = new Size(0, 700);
        /// <summary>
        /// 配置間隔
        /// </summary>
        private static readonly System.Drawing.Size _SeparateSize = new Size(202, 107);
        /// <summary>
        /// テキストボックスの先頭位置
        /// </summary>
        private static readonly System.Drawing.Size _TextBoxInitialPosition = new Size(0, 60);
        /// <summary>
        /// テキストボックスのサイズ
        /// </summary>
        private static readonly System.Drawing.Size _TextBoxSize = new Size(200, 75);
        /// <summary>
        /// 曜日表示の先頭位置
        /// </summary>
        private static readonly Size _WeekLabelInitialPosition = new Size(0, 0);
        /// <summary>
        /// 曜日表示のサイズ
        /// </summary>
        private static readonly Size _WeekLabelSize = new Size(200, 30);
        /// <summary>
        /// 前月ボタンの先頭位置
        /// </summary>
        private static readonly Size _PrevMonthButtonInitialPosition = new Size(557, 617);
        /// <summary>
        /// 前月ボタンのサイズ
        /// </summary>
        private static readonly Size _PrevMonthButtonSize = new Size(50, 30);
        /// <summary>
        /// 次月ボタンの先頭位置
        /// </summary>
        private static readonly Size _NextMonthButtonInitialPosition = new Size(807, 617);
        /// <summary>
        /// 次月ボタンのサイズ
        /// </summary>
        private static readonly Size _NextMonthButtonSize = new Size(50, 30);

        /// <summary>
        /// 今月表示の先頭位置
        /// </summary>
        private static readonly Size _ShowMonthLabelInitialPosition = new Size(607, 587);
        /// <summary>
        /// 今月表示のサイズ
        /// </summary>
        private static readonly Size _ShowMonthLabelSize = new Size(200, 30);
        /// <summary>
        /// 次月表示の先頭位置
        /// </summary>
        private static readonly Size _NextMonthLabelInitialPosition = new Size(607, 647);
        /// <summary>
        /// 次月表示のサイズ
        /// </summary>
        private static readonly Size _NextMonthLabelSize = new Size(200, 30);



        private static Dictionary<DateTime, string> _HolidayDic = null;


        /// <summary>
        /// 解像度の指定
        /// </summary>
        public static RESOLUTION Resolution { get; set; } = RESOLUTION.R2560_1440;


        private static Dictionary<RESOLUTION, float> _ResolutionCoefDic = new Dictionary<RESOLUTION, float> {
            { RESOLUTION.R1920_1080, 0.75f },
            { RESOLUTION.R2560_1440, 1f }
        };

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
                return (Point)(_SecondMonthPosition * _ResolutionCoefDic[Resolution]).ToSize();
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
    }
}