using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using TwoMonthesCalendar;

namespace CatHut {

    /// <summary>
    /// アプリケーションの設定だけを羅列するクラス
    /// </summary>
    public class SettingContents {

        //ここに設定項目を羅列
        public DateTime m_ShowMonth;
        public Point m_Location;
        public ConstSetting.RESOLUTION m_Resolution;


        //コンストラクタ（初期値指定を実装）
        public SettingContents() {
            this.m_ShowMonth = DateTime.Today;
            this.m_Location = new Point(0, 0);
            this.m_Resolution = ConstSetting.RESOLUTION.R2560_1440; 
        }


    }

    class AppSetting {
    
        private String SaveFile = "AppSetting.xml";

        public SettingContents Settings { set; get; }


        //コンストラクタ
        public AppSetting() {
            Initialize();
        }

        //終了時の呼び出し
        public void Exit() {
            SaveData();
        }

        //XMLファイルをSampleClassオブジェクトに復元する
        private void Initialize() {

            LoadData();

        }

        //XMLファイルをSampleClassオブジェクトに復元する
        private void LoadData()
        {

            //XmlSerializerオブジェクトを作成
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SettingContents));


            //ファイルがあれば読み込み、無ければインスタンスを自分で作る。
            try
            {
                //読み込むファイルを開く
                var sr = new System.IO.StreamReader(SaveFile, new System.Text.UTF8Encoding(false));

                //XMLファイルから読み込み、逆シリアル化する
                this.Settings = (SettingContents)serializer.Deserialize(sr);

                //ファイルを閉じる
                sr.Close();
            }
            catch
            {
                Debug.WriteLine("xmlファイル読み込み失敗");
                Debug.WriteLine("デフォルト値設定");
                Settings = new SettingContents();
            }
        }

        //XMLファイルを書き出し
        public void SaveData() {

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SettingContents));


            try {
                //書き込むファイルを開く（UTF-8 BOM無し）
                var sw = new System.IO.StreamWriter(SaveFile, false, new System.Text.UTF8Encoding(false));

                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, Settings);

                //ファイルを閉じる
                sw.Close();
            }
            catch {
                Debug.WriteLine("書き込み先" + SaveFile + "に書き込めません。" + Environment.NewLine
                    + "開いている場合は閉じてください");
            }
        }



    
    }
}
