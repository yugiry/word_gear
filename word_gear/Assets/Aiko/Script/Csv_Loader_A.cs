using UnityEngine;
using System.Collections.Generic;
using System.IO;

[DefaultExecutionOrder(-1)]
public class Csv_Loader_A : MonoBehaviour
{
    [System.Serializable]
    public struct CSV_Texts
    {
        public string start;           //開始時テキスト
        public string description;     //説明文
        public string problem;         //正解
        public string success;         //成功テキスト
        public string failur;          //失敗テキスト
    }

    public string file;
    //public List<string> sgame_texts = new List<string>();
    public List<CSV_Texts> csv_texts = new List<CSV_Texts>();

    /// <summary>
    /// csvファイルの読み込み用モジュール
    /// </summary>
    /// <param name = "pass">csvファイルのパス</param>
    /// <returns>csvから分割されたList<string>を返す</string></returns>
    public List<string> Csv_Input_Tmp(string pass)
    {
        List<string> str_lists = new List<string>();//値格納用リスト
        try
        {
            if (!File.Exists(pass))
            {
                Debug.LogError("ファイルが存在しない: " + pass);
            }
            //パスを指定してcsvファイルを開く
            StreamReader csv = new StreamReader(pass);

            //ファイル末尾まで実行
            while (!csv.EndOfStream)
            {
                string line = csv.ReadLine();//ファイルから1行読み込み
                string[] values = line.Split(',');//","で区切って配列に保存
                str_lists.AddRange(values);// 配列からリストに格納する
            }
            csv.Close();//ファイルを閉じる
            Debug.Log("public List<string> Csv_Input(string pass)での読み込み完了");
        }
        catch
        {
            Debug.Log("public List<string> Csv_Input(string pass)での読み込みエラー");
        }
        return str_lists;//string型リストを戻す
    }

    public List<CSV_Texts> Csv_Input(string pass)
    {
        List<CSV_Texts> str_lists = new List<CSV_Texts>();//値格納用リスト
        try
        {
            //パスを指定してcsvファイルを開く
            StreamReader csv = new StreamReader(pass);

            //ファイル末尾まで実行
            while (!csv.EndOfStream)
            {
                string line = csv.ReadLine();//ファイルから1行読み込み
                string[] values = line.Split(',');//","で区切って配列に保存
                CSV_Texts s_values = new CSV_Texts();
                if (values[0] != "ステージ")
                {
                    s_values.start = values[1].Replace(":", "\n");
                    s_values.description = values[2].Replace(":", "\n");
                    s_values.problem = values[3].Replace(":", "\n");
                    s_values.success = values[4].Replace(":", "\n");
                    s_values.failur = values[5].Replace(":", "\n");
                    str_lists.Add(s_values);
                }
            }
            csv.Close();//ファイルを閉じる
            Debug.Log("public List<string> Csv_Input(string pass)での読み込み完了");
        }
        catch
        {
            Debug.Log("public List<string> Csv_Input(string pass)での読み込みエラー");
        }
        return str_lists;//string型リストを戻す
    }

    private void Start()
    {
        //sgame_texts = Csv_Input_Tmp(file);
        csv_texts = Csv_Input(file);
    }

    private void Update()
    {

    }

    //public string file;
    //public List<string> game_texts = new List<string>();

    ///// <summary>
    ///// csvファイルの読み込み用モジュール
    ///// </summary>
    ///// <param name = "pass">csvファイルのパス</param>
    ///// <returns>csvから分割されたList<string>を返す</string></returns>
    //public List<string> Csv_Input(string pass)
    //{
    //    List<string> str_lists = new List<string>();//値格納用リスト
    //    try
    //    {
    //        //パスを指定してcsvファイルを開く
    //        StreamReader csv = new StreamReader(pass);

    //        //ファイル末尾まで実行
    //        while (!csv.EndOfStream)
    //        {
    //            string line = csv.ReadLine();//ファイルから1行読み込み
    //            string[] values = line.Split(',');//","で区切って配列に保存
    //            str_lists.AddRange(values);// 配列からリストに格納する
    //            Debug.Log(values);
    //        }
    //        csv.Close();//ファイルを閉じる
    //        Debug.Log("public List<string> Csv_Input(string pass)での読み込み完了");
    //    }
    //    catch
    //    {
    //        Debug.Log("public List<string> Csv_Input(string pass)での読み込みエラー");
    //    }
    //    return str_lists;//string型リストを戻す
//}
}
