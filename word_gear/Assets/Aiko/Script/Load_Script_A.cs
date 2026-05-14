using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(1)]
public class Load_Script_A : MonoBehaviour
{
    public Overlapping_Needle_And_Ball_A ONAB;
    public Hold_Information_Of_Mysterious_Ball_A HIOMB;
    public Rotate_Gear_A RG;
    public Checking_Answers_A CA;
    public Create_Box_A CBX;
    public Create_Balls_A CBL;
    public Tap_Center_Ball_A TCB;
    public Failue_Game_A FG;
    public Csv_Loder_M CL;
    public Csv_Loader_A CLA;
    public BGM_Playback_A BP;
    public StageClear_Manager_M SCM;
    public Timer_A TA;
    public Continue_Synopsis_A CS;
    public VaultImage_A VI;

    [SerializeField] private GameObject needle;
    [SerializeField] private GameObject rotate_gear;
    [SerializeField] private GameObject check_answer_script;
    [SerializeField] private GameObject create_box_object;
    [SerializeField] private GameObject create_ball_object;
    [SerializeField] private GameObject center_ball;
    [SerializeField] private GameObject csv_loader;
    [SerializeField] private GameObject bgm_playback_object;
    [SerializeField] private GameObject time_bar;
    [SerializeField] private GameObject synopsis_object;
    [SerializeField] private GameObject vault_image_object;

    public Canvas Normal_Canvas;
    public Canvas Success_Canvas;
    public Canvas Failure_Canvas;

    public AudioSource Audio_Souce;
    public AudioClip[] Sound_Effect;
    public AudioClip[] BGM_Clip;

    public string Problem_Answer;
    public Text Synopsis_Text;
    public Text Deseption_Text;
    public Text Success_Text;
    public Text Failure_Text;
    //public string Answer_String;
    public GameObject[] Game_Images=new GameObject[3];
    public Sprite[] Images=new Sprite[3];

    public GameObject Clear_Over_Image;

    [SerializeField] private GameObject count_down_object;
    [SerializeField] private Sprite[] count_down_images;
    public GameObject FadeIn_FadeOut_Object;
    public bool Fadein_Time_Flag = false;

    public string Title_Scene_Name;
    public string Next_Stage_Scene_Name;

    [SerializeField] private Collider2D all_block_collider;

    [SerializeField] private GameObject stage_clear_manager;

    [SerializeField] private Image[] stage_images;


    public enum SE_Names
    {
        Click,Gear,Failure,Success,InCorrect,CountFinish,CountDown
    }
    public enum BGM_Names
    {
        Synopsis, GameStart,GameOver,GameClear
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        ONAB = needle.GetComponent<Overlapping_Needle_And_Ball_A>();
        RG=rotate_gear.GetComponent<Rotate_Gear_A>();
        CA=check_answer_script.GetComponent<Checking_Answers_A>();
        CBX = create_box_object.GetComponent<Create_Box_A>();
        CBL=create_ball_object.GetComponent<Create_Balls_A>();
        TCB=center_ball.GetComponent<Tap_Center_Ball_A>();
        FG = Failure_Canvas.GetComponent<Failue_Game_A>();
        BP=bgm_playback_object.GetComponent<BGM_Playback_A>();
        TA=time_bar.GetComponent<Timer_A>();
        CS=synopsis_object.GetComponent<Continue_Synopsis_A>();
        VI = vault_image_object.GetComponent<VaultImage_A>();

        if (GameObject.Find("StageClearManager"))
        {
            stage_clear_manager = GameObject.Find("StageClearManager");
            CL = stage_clear_manager.GetComponent<Csv_Loder_M>();

            //if (stage_clear_manager.GetComponent<Csv_Loder_M>())
            //{
            //    Debug.Log("yomikometehairu");
            //    CLA = csv_loader.GetComponent<Csv_Loader_A>();
            //    CLA.Csv_Input(CLA.file);

            //    Synopsis_Text.text = CLA.csv_texts[SCM.now_stage].start;
            //    Deseption_Text.text = CLA.csv_texts[SCM.now_stage].description;
            //    Problem_Answer = CLA.csv_texts[SCM.now_stage].problem;
            //    Success_Text.text = CLA.csv_texts[SCM.now_stage].success;
            //    Failure_Text.text = CLA.csv_texts[SCM.now_stage].failur;
            //}
            //else
            //{
            //    Debug.Log("daishippai");
            //}

            SCM = stage_clear_manager.GetComponent<StageClear_Manager_M>();



            CL.CsvInput(CL.file);

            //if (CL.csv_texts[SCM.now_stage].start == null)
            //{
            //    Debug.Log("nakaminaiyo");
            //}
            //else
            //{
            //    Debug.Log(CL.csv_texts[SCM.now_stage].start + ":nakamiaruyo");
            //}

            Synopsis_Text.text = CL.csv_texts[SCM.now_stage].start;
            Deseption_Text.text = CL.csv_texts[SCM.now_stage].description;
            Problem_Answer = CL.csv_texts[SCM.now_stage].problem;
            Success_Text.text = CL.csv_texts[SCM.now_stage].success;
            Failure_Text.text = CL.csv_texts[SCM.now_stage].failur;

            CA.Answers = Problem_Answer;

            Game_Images[0].GetComponent<Image>().sprite = VI.Normal_Illust[SCM.now_stage];
            Game_Images[1].GetComponent<Image>().sprite = VI.Success_Illust[SCM.now_stage];
            Game_Images[2].GetComponent<Image>().sprite = VI.Failure_Illust[SCM.now_stage];


        }
        else
        {
            CLA = csv_loader.GetComponent<Csv_Loader_A>();
            CLA.Csv_Input(CLA.file);
            Debug.Log("CSV+Failured");
        }


        //SCM = GameObject.Find("StageClearManager").GetComponent<StageClear_Manager_M>();

        //for (int i = 0; i < Images.Length; i++)
        //{
        //    Game_Images[i].GetComponent<Image>().sprite = Images[i].sprite;
        //}

        Debug.Log("Start Success");

        all_block_collider.gameObject.SetActive(false);
        
        //Problem_Answer = CL.csv_texts.p;

        Normal_Canvas.gameObject.SetActive(false);
        Success_Canvas.gameObject.SetActive(false);
        Failure_Canvas.gameObject.SetActive(false);

        count_down_object.SetActive(false);
        Clear_Over_Image.gameObject.SetActive(false);

       // FadeIn_FadeOut_Object.gameObject.SetActive(false);

        Audio_Souce =GetComponent<AudioSource>();

        BP.PlayBGM(BGM_Clip[(int)BGM_Names.Synopsis]);
    }

    IEnumerator GameStartCount(float _first_time)
    {
        count_down_object.SetActive(true);
        all_block_collider.gameObject.SetActive(true);
        PlaySE(Sound_Effect[(int)SE_Names.CountDown]);

        int image_num = 0;

        count_down_object.GetComponent<Image>().sprite = count_down_images[image_num];

        while (_first_time > 0.0f)
        {
            _first_time -=1/* Time.deltaTime*/;

            yield return null;


            yield return new WaitForSeconds(1.0f);

            if (_first_time <= 0.0f)
            {
                count_down_object.transform.localScale = new Vector3(3.5f, 1.0f, 1.0f);
                //PlaySE(Sound_Effect[(int)SE_Names.CountFinish]);
            }
            else
            {
                //PlaySE(Sound_Effect[(int)SE_Names.Click]);
            }
            image_num++;
            count_down_object.GetComponent<Image>().sprite = count_down_images[image_num];
            

        }
       
        {
            BP.PlayBGM(BGM_Clip[(int)Load_Script_A.BGM_Names.GameStart]);
            all_block_collider.gameObject.SetActive(false);
            TA.CountStart();

            yield return new WaitForSeconds(1.0f);

            image_num = 0;
            count_down_object.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            count_down_object.SetActive(false);
        }



        Debug.Log("GameStartCoroutine Success");

    }

    public void GameStart()
    {
        
        StartCoroutine(Color_FadeIn());

        ResetNormalCanvas();

        CA.SetAnswer();

        CBL.CreateBall();
        CBX.CreateBox();

        

        ONAB.SetNeedle();

        //ONAB.Chosen_ball_number = ONAB.RandomBallChoose(ONAB.Chosen_ball_number,CA.Split_answers, CA.Chosen_Word);

        ONAB.RearrangeWord(CA.Split_answers, CA.Chosen_Word);

        TCB.Center_click_count = 0;

        RG.SetCenterBallColor();

        float F_game_start_count = 3.0f;

        

        StartCoroutine(GameStartCount(F_game_start_count));

        Debug.Log("GameStart Success");

    }

    public void SetoutsideTextAndImages()
    {
        for (int i = 0; i < Game_Images.Length; i++)
        {

            Game_Images[i].GetComponent<Sprite>();

        }


    }

    public void PlaySE(AudioClip _sound_effect)
    {
        Audio_Souce.PlayOneShot(_sound_effect);
    }

    public void StopSE()
    {
        Audio_Souce.Stop();
    }

    public int SoundEffectGearRepearting(int _rotate_gear_num, AudioClip _sound_effect)
    {



        if (_rotate_gear_num == 0)
        {
            PlaySE(_sound_effect);
            _rotate_gear_num = 1;
        }
        else if (_rotate_gear_num > 20)
        {

            _rotate_gear_num = 0;
        }
        else
        {
            _rotate_gear_num++;
        }

        Debug.Log("gear" + _rotate_gear_num);
        return _rotate_gear_num;
    }

    public void ResetNormalCanvas()
    {
        needle.transform.rotation = Quaternion.identity;
        rotate_gear.transform.rotation= Quaternion.identity;

        Failure_Canvas.gameObject.SetActive(false);
        Success_Canvas.gameObject.SetActive(false);
        Normal_Canvas.gameObject.SetActive(true);
        Clear_Over_Image.gameObject.SetActive(false);


    }

    public void ImageAppearAndChange(int _image_num)
    {
        Debug.Log("%d:" + _image_num);

        Clear_Over_Image.SetActive(true);
        Clear_Over_Image.GetComponent<Image>().sprite = Images[_image_num];

        Debug.Log("ImagrAppearAndChange Success");

    }

    public IEnumerator Color_FadeIn()
    {
        // 画面をフェードインさせるコールチン
        // 前提：画面を覆うPanelにアタッチしている

        // 色を変えるゲームオブジェクトからImageコンポーネントを取得
        Image fade = FadeIn_FadeOut_Object.GetComponent<Image>();

        // フェード元の色を設定（黒）★変更可
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (255.0f / 255.0f));

        // フェードインにかかる時間（秒）★変更可
        const float fade_time = 0.5f;

        // ループ回数（0はエラー）★変更可
        const int loop_count = 10;

        // ウェイト時間算出
        float wait_time = fade_time / loop_count;

        // 色の間隔を算出
        float alpha_interval = 255.0f / loop_count;

        // 色を徐々に変えるループ
        for (float alpha = 255.0f; alpha >= 0.0f; alpha -= alpha_interval)
        {
            // 待ち時間
            yield return new WaitForSeconds(wait_time);

            // Alpha値を少しずつ下げる
            Color new_color = fade.color;
            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }


        Debug.Log("FadeIn Success");
    }

    public IEnumerator Color_FadeOut()
    {
        // 画面をフェードインさせるコールチン
        // 前提：画面を覆うPanelにアタッチしている

        // 色を変えるゲームオブジェクトからImageコンポーネントを取得
        Image fade = FadeIn_FadeOut_Object.GetComponent<Image>();

        // フェード後の色を設定（黒）★変更可
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f));

        // フェードインにかかる時間（秒）★変更可
        const float fade_time = 0.5f;

        // ループ回数（0はエラー）★変更可
        const int loop_count = 10;

        // ウェイト時間算出
        float wait_time = fade_time / loop_count;

        // 色の間隔を算出
        float alpha_interval = 255.0f / loop_count;

        // 色を徐々に変えるループ
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alpha_interval)
        {
            // 待ち時間
            yield return new WaitForSeconds(wait_time);

            // Alpha値を少しずつ上げる
            Color new_color = fade.color;
            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }


        Debug.Log("FadeOut Success");


    }

   

    public IEnumerator WaitFadeOut(int _start_function)
    {
        //StartCoroutine(Color_FadeOut());

        Debug.Log("start");

        IEnumerator enumerator = Color_FadeOut();
        // 終わるまで待つ
        yield return enumerator;

        Debug.Log("end");

        switch (_start_function)
        {
            case 0:
                all_block_collider.gameObject.SetActive(true);
                CS.TextClick();
                
                break;
                case 1:
                CS.NextStage();
                break;
                case 2:
                FG.RetryGame();
                break;
            case 3:
                
                FG.TitleBack();
                break;
            case 4:
                CS.TitleBack();
                break;

        }

    }

    public IEnumerator WaitFadeIn(int _start_function)
    {
        StartCoroutine(Color_FadeOut());

        IEnumerator enumerator = Color_FadeIn();
        // 終わるまで待つ
        yield return enumerator;

        switch (_start_function)
        {
            case 0:
                all_block_collider.gameObject.SetActive(true);
                CS.TextClick();
                
                break;
            case 1:
                CS.NextStage();
                break;
            case 2:
                FG.RetryGame();
                break;
            case 3:
                FG.TitleBack();
                break;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
