using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class start_processing : MonoBehaviour
{
    [SerializeField] private List<Sprite> countdown_sprite = new List<Sprite>();
    [SerializeField] private Image countdown_image;
    public static start_processing Instance;
    public bool Finish_Count = false;
    private bool is_count = false;

    //音楽クラス
    [SerializeField] private C_MusicClass music_class;

    [System.Serializable]
    public class C_MusicClass
    {
        public AudioSource AS;
        public AudioClip Count_Down_SE;
    };

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
      
        //カウント開始
        if (fade_manager.Instance.Finish_Fade && !is_count && fade_manager.Instance.Start_Fade)
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

        //カウントダウンのSE
        music_class.AS.PlayOneShot(music_class.Count_Down_SE);

        //画像の切り替え
        for (int i = 0; i < countdown_sprite.Count; i++)
        {
            countdown_image.sprite = countdown_sprite[i];
            //1フレーム待ってから切り替え
            yield return new WaitForSeconds(1f);
        }

        countdown_image.gameObject.SetActive(false);

        Finish_Count = true;

        fade_manager.Instance.Start_Fade = false;
        //初期化
        fade_manager.Instance.InitVariable();

        is_count = false;
    }

    //初期化関数
    public void Initialization()
    {
        Finish_Count = false;
    }
}
