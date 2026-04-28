using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(0)]
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
    public Csv_Loader_A CL;
    public BGM_Playback_A BP;

    [SerializeField] private GameObject needle;
    [SerializeField] private GameObject rotate_gear;
    [SerializeField] private GameObject check_answer_script;
    [SerializeField] private GameObject create_box_object;
    [SerializeField] private GameObject create_ball_object;
    [SerializeField] private GameObject center_ball;
    [SerializeField] private GameObject csv_loader;
    [SerializeField] private GameObject bgm_playback_object;

    public Canvas Normal_Canvas;
    public Canvas Success_Canvas;
    public Canvas Failure_Canvas;

    public AudioSource Audio_Souce;
    public AudioClip[] Sound_Effect;
    public AudioClip[] BGM_Clip;

    public Text Synopsis_Text;
    public Text Deseption_Text;
    public Text Success_Text;
    public Text Failure_Text;
    //public string Answer_String;
    public GameObject[] Game_Images=new GameObject[3];

    public enum SE_Names
    {
        Click,Gear,Failure,Success,InCorrect
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
        CL=csv_loader.GetComponent<Csv_Loader_A>();
        


        CL.Csv_Input(CL.file);

        Normal_Canvas.gameObject.SetActive(false);
        Success_Canvas.gameObject.SetActive(false);
        Failure_Canvas.gameObject.SetActive(false);

        Audio_Souce=GetComponent<AudioSource>();

        BP.PlayBGM(BGM_Clip[(int)BGM_Names.Synopsis]);
    }

    public void GameStart()
    {
        ResetNormalCanvas();

        CA.SetAnswer();

        CBL.CreateBall();
        CBX.CreateBox();

        

        ONAB.SetNeedle();

        ONAB.Chosen_ball_number = ONAB.RandomBallChoose(ONAB.Chosen_ball_number,CA.Split_answers, CA.Chosen_Word);

        TCB.Center_click_count = 0;

        RG.SetCenterBallColor();


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

        //カウントダウンのリセット

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
