using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.U2D;
using UnityEngine.UI;
using System.Numerics;

public class game_manager_s : MonoBehaviour
{
    public static int Stage_Count = 1;//ステージのカウント
    public static game_manager_s Instance;
    private const float wait_time = 0.5f;//切り替わる時間変数
    [SerializeField] private GameObject main_game_scene;//ゲームメインシーンゲームオブジェクト
    private const int first_half = 15;

    //時間関連
    public C_TimeRelated Time_Related_Class;

    [System.Serializable]
    public class C_TimeRelated
    {
        public Slider Time_Slider;//制限時間バー
        public float[] Time_Limit;//制限時間
        [HideInInspector] public float Now_Time;//残り時間
    }

    //ゲームUIの情報クラス
    [System.Serializable]
    public class C_GameSceneUI
    {
        public Sprite[] Sprite;//背景の画像配列
        public Image Image;//背景
        public Text Dialogue_Text;//セリフテキスト
        public bool Insert_An_Ad = false;//広告フラグ
        public GameObject Scene;//シーンのゲームオブジェクト
    }

    //ゲームオーバー関連---------------------------------------
    public C_GameOver Game_Over_Class;
    public bool Game_Over = false;

    [System.Serializable]
    public class C_GameOver : C_GameSceneUI
    {
    
    }

    private bool go_running = false;//ゲームオーバー起動中か
    private const int failure_row = 5;//失敗セリフのCSVの列

    //ゲームクリア関連-----------------------------------------
    public C_GameClear Game_Clear_Class;

    [System.Serializable]
    public class C_GameClear : C_GameSceneUI
    {

    }

    private bool gc_running = false;
    private const int success_row = 4;

    //状況説明シーン関連---------------------------------------
    public C_SituationScene Situation_Scene_Class;

    [System.Serializable]
    public class C_SituationScene : C_GameSceneUI
    {
       //public  UnityEngine.Vector2 Situation_Text_Pos;
       //public Transform Dialogue_Recttransform;
    }
    private bool situation_running = false;
    private const int situation_row = 1;


    //問題関連------------------------------------------------
    public C_ProblemClass Problem_Class;

    [System.Serializable]
    public class C_ProblemClass
    {
        public CSV_load_s CSV_LOAD;//csvのスクリプト
        public GameObject Dialogue_Gameobject;
        public Text Dialogue;
    }


    private const int split = 6;
    private const int problem_row = 2;
    private List<string> csv_data = new List<string>();
    private List<string> csv_data_2 = new List<string>();


    private void Awake()
    {
       if(Instance == null)
       {
            Instance = this;
       }

        if (Problem_Class.Dialogue == null &&
         Problem_Class.Dialogue_Gameobject != null)
        {
            Problem_Class.Dialogue =
                Problem_Class.Dialogue_Gameobject.GetComponent<Text>();
        }
    }


    private void OnEnable()
    {
        LoadProblemText();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Situation_Scene_Class.Scene.SetActive(true);
        main_game_scene.SetActive(false);
        Game_Over_Class.Scene.SetActive(false);
        Game_Clear_Class.Scene.SetActive(false);
        Time_Related_Class.Time_Slider.value = 1f;
        //Situation_Scene_Class.Dialogue_Recttransform.anchoredPosition = Situation_Scene_Class.Situation_Text_Pos;//状況説明のテキストの代入
    }

    // Update is called once per frame
    void Update()
   {
        if (start_processing.Instance.Finish_Count)
        {

            TimeControl();
        }


        if (Game_Over && !go_running)
        {
            StartCoroutine(GameOver());
        }

        if(panel_manager_s.Game_Clear  && !gc_running)
        {
            StartCoroutine(GameClear());
        }
    }

    //キャンバスの切り替え
    public void ChangeCanvas()
    {
        Situation_Scene_Class.Scene.SetActive(false);
        main_game_scene.SetActive(true);
    }

    //時間の制御関数
    void TimeControl()
    {
        if (Game_Over || panel_manager_s.Game_Clear)
            return;

        Time_Related_Class.Now_Time += Time.deltaTime;
        float F_t = Time_Related_Class.Now_Time / Time_Related_Class.Time_Limit[Stage_Count - 1];//スライダーの正規化
        Time_Related_Class.Time_Slider.value = Mathf.Lerp(1f, 0, F_t);
        float F_remaeining_time = Time_Related_Class.Time_Limit[Stage_Count - 1] - Time_Related_Class.Now_Time;//残り時間
        F_remaeining_time = Mathf.Max(F_remaeining_time, 0f);

        if(Time_Related_Class.Now_Time >= Time_Related_Class.Time_Limit[Stage_Count - 1])
        {
            //ゲームオーバー
            Game_Over = true;
        }
    }

    //ゲームオーバー関数
    private IEnumerator GameOver()
    {
        go_running = true;
        yield return new WaitForSeconds(wait_time);
        Game_Over = false;
        main_game_scene.SetActive(false);
        Game_Over_Class.Scene.SetActive(true);
        go_running = false;
    }

    //ゲームクリア関数
   private IEnumerator GameClear()
    {
        gc_running = true;
        yield return new WaitForSeconds(wait_time);
        panel_manager_s.Game_Clear = false;
        main_game_scene.SetActive(false);
        Game_Clear_Class.Scene.SetActive(true);
        gc_running = false;
    }

    //問題csvのロード
    void LoadProblemText()
    {
        Debug.Log("Problem_Class = " + Problem_Class);
        Debug.Log("CSV_LOAD = " + Problem_Class.CSV_LOAD);
        Debug.Log("Dialogue = " + Problem_Class.Dialogue);

        csv_data = Problem_Class.CSV_LOAD.CSVInput("stage_inf");
        csv_data_2 = Problem_Class.CSV_LOAD.CSVInput("stage_inf");

        //分割
        int F_start = (Stage_Count - 1) * split;

        string F_problem;

        string F_situation;

        string F_gameover_text;

        string F_gameclear_text;

        if(Stage_Count < first_half)
        {
            F_problem = csv_data[F_start + problem_row];

            F_situation = csv_data[F_start + situation_row];

            F_gameover_text = csv_data[F_start + failure_row];

            F_gameclear_text = csv_data[F_start + success_row];
        }
        else
        {
            F_problem = csv_data_2[F_start + problem_row];

            F_situation = csv_data_2[F_start + situation_row];

            F_gameover_text = csv_data_2[F_start + failure_row];

            F_gameclear_text = csv_data_2[F_start + success_row];
        }

            Problem_Class.Dialogue.text = F_problem.Replace("\\n", "\n");


        ChangeUI(F_gameover_text,F_gameclear_text,F_situation);
    }

    //ステージによってテキストと背景を切り替え
    void ChangeUI(string _go_text,string _gc_text,string _sit_text)
    {
        //ゲームオーバー
        Game_Over_Class.Image.sprite = Game_Over_Class.Sprite[Stage_Count -1];
        Game_Over_Class.Dialogue_Text.text = _go_text.Replace("\\n", "\n");
        //ゲームクリア
        Game_Clear_Class.Image.sprite = Game_Clear_Class.Sprite[Stage_Count - 1];
        Game_Clear_Class.Dialogue_Text.text = _gc_text.Replace("\\n", "\n");
        //状況説明
        Situation_Scene_Class.Image.sprite = Situation_Scene_Class.Sprite[Stage_Count - 1];
        Situation_Scene_Class.Dialogue_Text.text = _sit_text.Replace("\\n", "\n");
    }

    //初期化関数
    void InitializeVariableGO()
    {
        //UIの切り替え
        Situation_Scene_Class.Scene.SetActive(true);
        main_game_scene.SetActive(false);
        Game_Over_Class.Scene.SetActive(false);
        Game_Clear_Class.Scene.SetActive(false);

        //時間
        Time_Related_Class.Now_Time = 0f;
        Time_Related_Class.Time_Slider.value = 1f;

        //状態
        Game_Over = false;
        panel_manager_s.Game_Clear = false;

        //問題文   
        LoadProblemText();
    }

    //ゲームオーバー時にリトライボタンを押したとき
    public void GameOverReTryButton()
    {
        if (Game_Over_Class.Insert_An_Ad)
        {
            StopAllCoroutines();
            go_running = false;
            answer_manager_s.Instance.InitializeVariable();
            InitializeVariableGO();
            Game_Over_Class.Insert_An_Ad = false;
        }
    }

    //ゲームオーバー時にタイトルボタンを押したとき
    public void GameOverReturnTitle()
    {
        Debug.Log("タイトルへ");
    }

    //ゲームクリア時の次の問題ボタンンを押した時
    public void GameClearNextButton()
    {
        Debug.Log("次");
    }
}
