using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager_M : MonoBehaviour
{
    [SerializeField] Image start_img;
    [SerializeField] Image game_img;
    [SerializeField] Image success_img;
    [SerializeField] Image failure_img;
    [SerializeField] Image next_img;

    [SerializeField] Text start_text;
    [SerializeField] Text game_text;
    [SerializeField] Text success_text;
    [SerializeField] Text failure_text;

    [SerializeField] Sprite next_sprite;
    [SerializeField] Sprite title_sprite;

    [System.Serializable]
    public struct Images_
    {
        public Sprite start_img;
        public Sprite success_img;
        public Sprite failure_img;
    }

    public List<Images_> wordgea_image = new List<Images_>();

    [Tooltip("クリアフラグを確認するスクリプト")]StageClear_Manager_M scm;
    [Tooltip("")] Csv_Loder_M csvl;

    private void Start()
    {
        GameObject F_scm_obj = GameObject.Find("StageClearManager");

        scm = F_scm_obj.GetComponent<StageClear_Manager_M>();
        csvl = F_scm_obj.GetComponent<Csv_Loder_M>();
        //scm = StageClear_Manager_M.instance;

        //表示するテキストボックスにステージにあった文を入れる
        start_text.text = csvl.csv_texts[scm.now_stage].start;
        game_text.text = csvl.csv_texts[scm.now_stage].description;
        success_text.text = csvl.csv_texts[scm.now_stage].success;
        failure_text.text = csvl.csv_texts[scm.now_stage].failur;
        //表示する画像をステージにあった画像に変更する
        start_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
        game_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
        success_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
        failure_img.sprite = wordgea_image[scm.now_stage / 3].start_img;

        //最終ステージの場合のみnext_imgの画像をタイトルボタンの画像にする
        if(scm.now_stage != 29)
        {
            next_img.sprite = next_sprite;
        }
        else
        {
            next_img.sprite = title_sprite;
        }
    }
}
