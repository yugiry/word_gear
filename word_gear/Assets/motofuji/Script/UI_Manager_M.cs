using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager_M : MonoBehaviour
{
    [SerializeField] Image start_img;
    [SerializeField] Image game_img;
    [SerializeField] Image success_img;
    [SerializeField] Image failure_img;

    [SerializeField] Text start_text;
    [SerializeField] Text game_text;
    [SerializeField] Text success_text;
    [SerializeField] Text failure_text;

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

        start_text.text = csvl.csv_texts[scm.now_stage].start;
        game_text.text = csvl.csv_texts[scm.now_stage].description;
        success_text.text = csvl.csv_texts[scm.now_stage].success;
        failure_text.text = csvl.csv_texts[scm.now_stage].failur;

        start_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
        game_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
        success_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
        failure_img.sprite = wordgea_image[scm.now_stage / 3].start_img;
    }
}
