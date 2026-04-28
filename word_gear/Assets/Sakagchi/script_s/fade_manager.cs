using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class fade_manager : MonoBehaviour
{
    public float fade_speed;//フェードスピード
    private float red, green, blue, alfa;

    public bool Fade_Out = false;
    public bool Fade_In = false;
    public bool Finish_Fade = false;

    [SerializeField] private Image fade_image;//パネルイメージ

    public static fade_manager Instance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       if(Instance == null)
        {
            Instance = this;
        }
       
    }

    void Start()
    {
        fade_image.gameObject.SetActive(false);
        red = fade_image.color.r;
        green = fade_image.color.g;
        blue = fade_image.color.b;
        alfa = fade_image.color.a;
    }

    // Update is called once per frame
    void Update()
    {

        if (Fade_In)
        {
            FadeIn();
        }

        if (Fade_Out)
        {
            FadeOut();
        }
    }

    //フェードの処理関数
    public void Fade()
    {
       
        fade_image.gameObject.SetActive(true);
        fade_image.enabled = true;

        //fade_image.transform.SetAsLastSibling(); // 最前面へ

        Fade_In = false;
        Fade_Out = true;

    }

    //フェードインの処理関数
    public void FadeIn()
    {
        //alfa値を変化
        alfa -=  Time.deltaTime / fade_speed;
        alfa = Mathf.Clamp01(alfa);
        ApplyColor();
        if (alfa <= 0)
        {
            //フェードイン終了
            Fade_In = false;
            fade_image.enabled = false;
            Finish_Fade = true;
        }
    }

    //フェードアウトの処理関数
    public void FadeOut()
    {
        //alfa値の変化
        fade_image.enabled = true;
        alfa += Time.deltaTime / fade_speed;
        alfa = Mathf.Clamp01(alfa);
        ApplyColor();
        if (alfa >= 1)
        {
            //フェードアウト終了
            Fade_Out = false;
            game_manager_s.Instance.ChangeCanvas();
            Fade_In = true;
        }
    }

    //フェード中の画像の色の変化処理関数
    void ApplyColor()
    {
        fade_image.color = new Color(red, green, blue, alfa);
    }

    //変数の初期化
    public void InitVariable()
    {
        Fade_Out = false;
        Fade_In = false;
        Finish_Fade = false ;
    }
}
