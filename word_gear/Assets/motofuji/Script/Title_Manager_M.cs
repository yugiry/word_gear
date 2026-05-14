using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_Manager_M : MonoBehaviour
{
    [SerializeField] private GameObject[] black_area = new GameObject[10];
    [SerializeField] private GameObject title_canvas;
    [SerializeField] private GameObject stageselect_canvas;
    [SerializeField] private Text window_text;
    [SerializeField] private Text[] button_text = new Text[10];

    [Tooltip("クリアフラグを確認するスクリプト")] private StageClear_Manager_M scm;

    private int page;

    //SE
    [SerializeField] private C_Music music_class;
    [System.Serializable]
    class C_Music
    {
        public AudioSource AS;
        public AudioClip Click_Button;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        title_canvas.SetActive(true);
        stageselect_canvas.SetActive(false);
        scm = GameObject.Find("StageClearManager").GetComponent<StageClear_Manager_M>();
    }

    public void ClickStart()
    {
        //SE
        music_class.AS.PlayOneShot(music_class.Click_Button);
        StartCoroutine(TransitionScene());
        black_area[0].SetActive(false);
        for (int i = 1; i < 10; i++)
        {
            if (scm.ClearCheck_Flag[i] == true)
            {
                black_area[i].SetActive(false);
            }
            else
            {
                black_area[i].SetActive(true);
            }
        }
    }

    //遅延させてキャンバス切り替え（タイトル）
    private IEnumerator TransitionScene()
    {
        fade_manager.Instance.Fade();
        yield return new WaitUntil(() => fade_manager.Instance.Finish_Fade_Out);
        title_canvas.SetActive(false);
        stageselect_canvas.SetActive(true);
        page = 0;
        window_text.text = $"ステージを選んでください。 {page + 1}/３";
        for (int i = 0; i < 10; i++)
        {
            button_text[i].text = $"Stage {i + 1 + page * 10}";
        }
        fade_manager.Instance.Fade_In = true;
    }

    public void ClickNext()
    {
        //SE
        music_class.AS.PlayOneShot(music_class.Click_Button);
        page++;
        if (page > 2) page = 0;
        window_text.text = $"ステージを選んでください。 {page + 1}/３";
        for (int i = 0; i < 10; i++)
        {
            button_text[i].text = $"Stage {i + 1 + page * 10}";
            if(page * 10 + i == 0)
            {
                black_area[i].SetActive(false);
            }
            else if(page * 10 + i - 1 >= 0 && scm.ClearCheck_Flag[page * 10 + i - 1] == true)
            {
                black_area[i].SetActive(false);
            }
            else
            {
                black_area[i].SetActive(true);
            }
        }
    }

    public void ClickStage(int _button_num)
    {
        //クリックしたステージの１つ前がクリア済みもしくは最初のステージならステージに移行する
        if ((_button_num + 10 * page) - 1 >= 0 && scm.ClearCheck_Flag[(_button_num + 10 * page) - 1] || _button_num + 10 * page == 0)
        {
            scm.now_stage = _button_num + 10 * page;
            //SE
            music_class.AS.PlayOneShot(music_class.Click_Button);
            StartCoroutine(PlayScene(_button_num + 10 * page));
        }
    }

   //個々のシーン再生
   private IEnumerator PlayScene(int _bn)
    {
        const int F_stage_num = 3;
        //フェード
        fade_manager.Instance.Fade();
        yield return new WaitUntil(() => fade_manager.Instance.Finish_Fade_Out);

        //ステージの切り替え処理
        switch (_bn % F_stage_num)
        {
            case 0:
                SceneManager.LoadScene("GearNeedleRotationScene");
                Debug.Log("愛甲");
                break;
            case 1:
                SceneManager.LoadScene("Word_Search");
                Debug.Log("坂口");
                break;
            case 2:
                SceneManager.LoadScene("wordgea_scene");
                Debug.Log("元藤");
                break;

        }
        fade_manager.Instance.Fade_In = true;
   }
}
