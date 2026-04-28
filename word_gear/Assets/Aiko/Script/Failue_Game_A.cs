//using System.Diagnostics;
using UnityEngine;


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
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Failure]);

        LS.CBL.VanishAllBalls();
        LS.Normal_Canvas.gameObject.SetActive(false);
        LS.Failure_Canvas.gameObject.SetActive(true);

        LS.BP.StopBGM();
        LS.BP.PlayBGM(LS.BGM_Clip[(int)Load_Script_A.BGM_Names.GameOver]);
    }

    public void RetryGame()
    {
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Click]);
        //LS.Normal_Canvas.gameObject.SetActive(true);



        //LS.CBX.CreateBox();

        //LS.TCB.ResetAllBalls();

        //LS.Failure_Canvas.gameObject.SetActive(false);

        LS.GameStart();

        //LS.TCB.ResetAllBalls();

        LS.BP.StopBGM();
        LS.BP.PlayBGM(LS.BGM_Clip[(int)Load_Script_A.BGM_Names.GameStart]);

        //カウントダウンのリセット
        LS.ResetNormalCanvas();
    }

    public void TitleBack()
    {
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Click]);
        LS.BP.StopBGM();

        //タイトルシーンに戻る処理

    }

}
