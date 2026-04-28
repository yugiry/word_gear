using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class start_processing : MonoBehaviour
{
    [SerializeField] private List<Sprite> countdown_sprite = new List<Sprite>();
    [SerializeField] private Image countdown_image;
    [SerializeField] private GameObject start_button_gameobj;
    public static start_processing Instance;
    public bool Finish_Count = false;
    private bool is_count = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        countdown_image.gameObject.SetActive(false);
    }

    private void Update()
    {
        //ゲームオブジェクトのボタンのアクティブfalse
        if (fade_manager.Instance.Fade_In || fade_manager.Instance.Fade_Out)
        {
            start_button_gameobj.SetActive(false);
        }

        //カウント開始
        if(fade_manager.Instance.Finish_Fade && !is_count)
        {
           
            StartCoroutine(CountDown());
        }
    }

   

    //カウントダウンの処理関数
    IEnumerator CountDown()
    {
        is_count = true;
        Finish_Count = false;

        countdown_image.gameObject.SetActive(true);

        //画像の切り替え
        for (int i = 0; i < countdown_sprite.Count; i++)
        {
            countdown_image.sprite = countdown_sprite[i];
            //1フレーム待ってから切り替え
            yield return new WaitForSeconds(1f);
        }

        countdown_image.gameObject.SetActive(false);

        Finish_Count = true;

        //初期化
        fade_manager.Instance.InitVariable();

        is_count = false;
    }
  
}
