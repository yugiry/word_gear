//using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Failue_Game_A : MonoBehaviour
{
    private Load_Script_A LS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LS= GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        LS.ImageAppearAndChange(2);

        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Failure]);

        LS.CBL.VanishAllBalls();
        LS.Normal_Canvas.gameObject.SetActive(false);
        LS.Failure_Canvas.gameObject.SetActive(true);

        LS.BP.StopBGM();
        LS.BP.PlayBGM(LS.BGM_Clip[(int)Load_Script_A.BGM_Names.GameOver]);
    }

    //public IEnumerator WaitFadeOut()
    //{
    //    StartCoroutine(LS.Color_FadeOut());

    //    IEnumerator enumerator = LS.Color_FadeOut();
    //    // 終わるまで待つ
    //    yield return enumerator;

       
    //}

    public void RetryButtonClick()
    {
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Click]);
        Show_Commercial_M.instance.PlayGame();

        StartCoroutine(LS.WaitFadeOut(2));
    }

    public void RetryGame()
    {
        
        //LS.Normal_Canvas.gameObject.SetActive(true);

        //StartCoroutine(WaitFadeOut());

        //LS.CBX.CreateBox();

        //LS.TCB.ResetAllBalls();

        //LS.Failure_Canvas.gameObject.SetActive(false);

        LS.TA.CountReset();

        LS.GameStart();

        //LS.TCB.ResetAllBalls();

        LS.BP.StopBGM();
        LS.BP.PlayBGM(LS.BGM_Clip[(int)Load_Script_A.BGM_Names.GameStart]);

        //カウントダウンのリセット
        LS.ResetNormalCanvas();
    }

    public void TitleButtonClick()
    {
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Click]);
        LS.BP.StopBGM();
        StartCoroutine(LS.WaitFadeOut(3));
    }

    public void TitleBack()
    {
       

        //タイトルシーンに戻る処理
        SceneManager.LoadScene(LS.Title_Scene_Name);
    }

}
