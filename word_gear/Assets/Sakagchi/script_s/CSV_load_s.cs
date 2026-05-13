using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSV_load_s : MonoBehaviour
{
    [SerializeField] private string file;
    [SerializeField] private string file2;
    /// <summary>
    /// csvファイルの読み込み用モジュール
    /// </summary>
    /// <param name = "pass">csvファイルのパス</param>
    /// <returns>csvから分割されたList<string>を返す</string></returns>
    /// 

    public List<string> CSVInput(string _pass)
    {
        List<string> F_list = new List<string>();

        TextAsset F_csv = Resources.Load<TextAsset>(_pass);

        string[] F_lines = F_csv.text.Split('\n');

        for (int i = 1; i < F_lines.Length; i++) // 1行目スキップ
        {
            string line = F_lines[i].Trim();

            if (string.IsNullOrEmpty(line)) continue;

            string[] F_values = line.Split(',');
            F_list.AddRange(F_values);
        }


        return F_list;
    }
    
}