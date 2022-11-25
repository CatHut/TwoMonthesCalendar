using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoMonthesCalendar
{
    public static class ConstSetting
    {
        private static readonly System.Drawing.Size _DateLabelInitialPosition = new Size(30, 30);
        private static readonly System.Drawing.Size _DateLabelSize = new Size(30, 30);
        private static readonly System.Drawing.Size _SecondMonthPosition = new Size(30, 960);
        private static readonly System.Drawing.Size _SeparateSize = new Size(330, 160);
        private static readonly System.Drawing.Size _TextBoxInitialPosition = new Size(30, 30);
        private static readonly System.Drawing.Size _TextBoxSize = new Size(300, 100);

        public enum RESOLUTION
        {
            R1920_1080,
            R2560_1440
        }

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

        /// <summary>
        /// 解像度の指定
        /// </summary>
        public static RESOLUTION Resolution { get; set; } = RESOLUTION.R2560_1440;
    }
}