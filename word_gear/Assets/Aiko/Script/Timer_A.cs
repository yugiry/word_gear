using UnityEngine;
using UnityEngine.UI;
using TMPro;
[DefaultExecutionOrder(1)]
public class Timer_A : MonoBehaviour
{
   public float Limit = 30.5f;  // 制限時間
    public float Time_Limit;
    float now = 0f; // 経過時間
    public Text time;    //現在の時間
    public Slider Timer_Gauge;   //残り時間ゲージ
    public bool Count_Start_Flag = false;

    private Load_Script_A LS;

    void Start()
    {
        Timer_Gauge.value = 1.0f;  //制限時間ゲージ
        LS= GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();
        Time_Limit = Limit;
    }

    public void CountStart()
    {
        Count_Start_Flag = true;
        Time_Limit = Limit;
        now = 0;
        
        Timer_Gauge.value = 1.0f;
    }

    public void CountReset()
    {
        Count_Start_Flag = false;
        Time_Limit = Limit;
        now = 0;
        
        Timer_Gauge.value = 1.0f;
    }

    void Update()
    {
        if (!Count_Start_Flag) 
        {
            return;
        }

        // 時間制御
        now += Time.deltaTime; // タイマー
        
        float t = now / Limit; // スライダーの値ー正規化
        Timer_Gauge.value = Mathf.Lerp(1f, 0f, t);
        Time_Limit = Limit - now; // 残り時間
        Time_Limit = Mathf.Max(Time_Limit, 0f);
        //string timeLog = Time_Limit.ToString("F0");
        //time.text = timeLog + "秒";
        //time.color = (Time_Limit > 10.5f) ? Color.green : Color.red; // 文字の色（1.5秒以上は緑、未満は赤）

        if (Time_Limit <=0.0f)
        {
            LS.FG.GameOver();
        }

    }
}