using UnityEngine;
using System.Collections;

public class Show_Commercial_M : MonoBehaviour
{
    public static Show_Commercial_M instance;
    public GameObject cm_canvas;
    public int play_time = 0;
    public int play_cm_time = 3;
    int show_cm_time = 120;
    int show_cm;
    bool now_cm = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        cm_canvas.SetActive(false);
    }

    private void Update()
    {
        if (now_cm == true)
        {
            show_cm++;
        }
        //カウントが一定の値に達したらcmを表示する
        if (play_time == play_cm_time && !now_cm)
        {
            show_cm = 0;
            cm_canvas.SetActive(true);
            now_cm = true;
        }
        //時間経過でステージの画面に切り替わる
        if(show_cm == show_cm_time)
        {
            StartCoroutine(CMFade());
        }
    }

    private IEnumerator CMFade()
    {
        fade_manager.Instance.Fade();
        yield return new WaitUntil(() => fade_manager.Instance.Finish_Fade_Out);
        cm_canvas.SetActive(false);
        show_cm = play_time = 0;
        now_cm = false;
        fade_manager.Instance.Fade_In = true;
    }

    //ステージに入ったらカウントを進める
    public void PlayGame()
    {
        play_time++;
    }
}
