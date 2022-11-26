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
        private static readonly System.Drawing.Size _DateLabelSize = new Size(300, 30);
        /// <summary>
        /// 二月目の位置
        /// </summary>
        private static readonly System.Drawing.Size _SecondMonthPosition = new Size(0, 780);
        /// <summary>
        /// 配置間隔
        /// </summary>
        private static readonly System.Drawing.Size _SeparateSize = new Size(302, 132);
        /// <summary>
        /// テキストボックスの先頭位置
        /// </summary>
        private static readonly System.Drawing.Size _TextBoxInitialPosition = new Size(0, 60);
        /// <summary>
        /// テキストボックスのサイズ
        /// </summary>
        private static readonly System.Drawing.Size _TextBoxSize = new Size(300, 100);
        /// <summary>
        /// 曜日表示の先頭位置
        /// </summary>
        private static readonly Size _WeekLabelInitialPosition = new Size(0, 0);
        /// <summary>
        /// 曜日表示のサイズ
        /// </summary>
        private static readonly Size _WeekLabelSize = new Size(300, 30);

        private static Dictionary<DateTime, string> _HolidayDic = null;


        /// <summary>
        /// 解像度の指定
        /// </summary>
        public static RESOLUTION Resolution { get; set; } = RESOLUTION.R2560_1440;


        private static Dictionary<RESOLUTION, float> _ResolutionCoefDic = new Dictionary<RESOLUTION, float> {
            { RESOLUTION.R1920_1080, 0.75f },
            { RESOLUTION.R2560_1440, 1f }
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

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var result = req.GetStringAsync(url).Result;

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
    }
}