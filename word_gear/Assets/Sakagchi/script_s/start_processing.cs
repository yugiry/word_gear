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
    
    [SerializeField] private float fade_speed = 0.5f;//フェードスピード
    private float red, green, blue, alfa;

    [SerializeField] private bool fade_out = false;
    [SerializeField] private bool fade_in = false;

    [SerializeField] private Image fade_image;//パネルイメージ

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
        fade_image.gameObject.SetActive(false);
        countdown_image.gameObject.SetActive(false);
        red = fade_image.color.r;
        green = fade_image.color.g;
        blue = fade_image.color.b;
        alfa = fade_image.color.a;
    }

    private void Update()
    {
        if (fade_in)
        {
            FadeIn();
        }

        if (fade_out)
        {
            FadeOut();
        }
    }

    //ボタンが押された時の処理関数
    public void ClickStart()
    {
        fade_image.gameObject.SetActive(true);
        start_button_gameobj.SetActive(false);
        fade_in = true;
        //fade_out = true;
        
    }

    //カウントダウンの処理関数
    IEnumerator CountDown()
    {
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

    }

    //フェードインの処理関数
    void FadeIn()
    {
        //alfa値を変化
        alfa -= fade_speed * Time.deltaTime;
        alfa = Mathf.Clamp01(alfa);
        ApplyColor();
        if (alfa <= 0)
        {
            //フェードイン終了
            fade_in = false;
            fade_image.enabled = false;
            StartCoroutine(CountDown());
        }
    }

    //フェードアウトの処理関数
    void FadeOut()
    {
        //alfa値の変化
        fade_image.enabled = true;
        alfa += fade_speed * Time.deltaTime;
        alfa = Mathf.Clamp01(alfa);
        ApplyColor();
        if (alfa >= 1)
        {
            //フェードアウト終了
            fade_out = false;
            StartCoroutine(CountDown());
        }
    }

    //フェード中の画像の色の変化処理関数
    void ApplyColor()
    {
        fade_image.color = new Color(red, green, blue, alfa);
    }
}
