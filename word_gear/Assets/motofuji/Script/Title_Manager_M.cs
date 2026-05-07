using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_Manager_M : MonoBehaviour
{
    [SerializeField] private GameObject title_canvas;
    [SerializeField] private GameObject stageselect_canvas;
    [SerializeField] private Text window_text;
    [SerializeField] private Text[] button_text = new Text[10];

    [Tooltip("クリアフラグを確認するスクリプト")] private StageClear_Manager_M scm;

    private int page;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        title_canvas.SetActive(true);
        stageselect_canvas.SetActive(false);
        scm = GameObject.Find("StageClearManager").GetComponent<StageClear_Manager_M>();
    }

    public void ClickStart()
    {
        title_canvas.SetActive(false);
        stageselect_canvas.SetActive(true);
        page = 0;
        window_text.text = $"ステージを選んでください。 {page + 1}/３";
        for (int i = 0; i < 10; i++)
        {
            button_text[i].text = $"Stage {i + 1 + page * 10}";
        }
    }

    public void ClickNext()
    {
        page++;
        if (page > 2) page = 0;
        window_text.text = $"ステージを選んでください。 {page + 1}/３";
        for (int i = 0; i < 10; i++)
        {
            button_text[i].text = $"Stage {i + 1 + page * 10}";
        }
    }

    public void ClickStage(int _button_num)
    {
        //クリックしたステージの１つ前がクリア済みもしくは最初のステージならステージに移行する
        //if (_button_num - 1 >= 0 && scm.ClearCheck_Flag[_button_num - 1] || _button_num == 0)
        {
            scm.now_stage = _button_num;
            switch(_button_num % 3)
            {
                case 0:
                    SceneManager.LoadScene("GearNeedleRotationScene");
                    Debug.Log("愛甲");
                    break;
                case 2:
                    SceneManager.LoadScene("wordgea_scene");
                    Debug.Log("元藤");
                    break;
                case 1:
                    SceneManager.LoadScene("Word_Search");
                    Debug.Log("坂口");
                    break;
            }
        }
    }
}
