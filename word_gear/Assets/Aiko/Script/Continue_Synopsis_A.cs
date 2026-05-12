using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
[DefaultExecutionOrder(10)]
public class Continue_Synopsis_A : MonoBehaviour
{
    [SerializeField] private Load_Script_A LS;

    private bool once_click_text_flag = false;
    
    [SerializeField] private GameObject synopsis_text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();
        //normal_canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextbuttonClick()
    {
        if (!once_click_text_flag)
        {
            LS.PlaySE(LS.Sound_Effect[0]);
            once_click_text_flag = true;
        StartCoroutine(LS.WaitFadeOut(0));
        }
        
    }

    public void TextClick()
    {
       

        //StartCoroutine( LS.Color_FadeOut());

        //StartCoroutine(WaitFade());

        synopsis_text.gameObject.SetActive(false);
        LS.Normal_Canvas.gameObject.SetActive(true);
        LS.BP.StopBGM();
        // LS.BP.PlayBGM(LS.BGM_Clip[(int)Load_Script_A.BGM_Names.GameStart]);

        //LS.FadeIn_FadeOut_Object.gameObject.SetActive(true);
        once_click_text_flag = false;
        LS.GameStart();



    }

    //IEnumerator WaitFade()
    //{
    //    IEnumerator enumerator = LS.Color_FadeOut();
    //    // TestCorが終わるまで待つ
    //    yield return enumerator;

       
    //}

    public void NextButtonClick()
    {
        LS.PlaySE(LS.Sound_Effect[0]);
        LS.SCM.StageClear();
        StartCoroutine(LS.WaitFadeOut(1));
    }

    public void NextStage()
    {
        Show_Commercial_M.instance.PlayGame();

        //LS.SCM.now_stage += 1;
        SceneManager.LoadScene(LS.Next_Stage_Scene_Name);

    }

    public void TitleButtonClick()
    {
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Click]);
        LS.BP.StopBGM();
        StartCoroutine(LS.WaitFadeOut(4));
    }

    public void TitleBack()
    {
       // LS.PlaySE(LS.Sound_Effect[0]);
        //LS.BP.StopBGM();
        //タイトルシーンに戻す。
        SceneManager.LoadScene(LS.Title_Scene_Name);
    }

}
