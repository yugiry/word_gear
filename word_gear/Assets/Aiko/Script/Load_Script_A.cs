using UnityEngine;

public class Load_Script_A : MonoBehaviour
{
    public Overlapping_Needle_And_Ball_A ONAB;
    public Hold_Information_Of_Mysterious_Ball_A HIOMB;
    public Rotate_Gear_A RG;
    public Checking_Answers_A CA;
    public Create_Box_A CB;
    public Tap_Center_Ball_A TCB;
    public Csv_Loader_A CL;

    [SerializeField] private GameObject needle;
    [SerializeField] private GameObject rotate_gear;
    [SerializeField] private GameObject check_answer_script;
    [SerializeField] private GameObject create_box_object;
    [SerializeField] private GameObject center_ball;
    [SerializeField] private GameObject csv_loader;

    public Canvas Normal_Canvas;
    public Canvas Success_Canvas;
    public Canvas Failure_Canvas;

    public AudioSource Audio_Souce;
    public AudioClip[] Sound_Effect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ONAB = needle.GetComponent<Overlapping_Needle_And_Ball_A>();
        RG=rotate_gear.GetComponent<Rotate_Gear_A>();
        CA=check_answer_script.GetComponent<Checking_Answers_A>();
        CB=create_box_object.GetComponent<Create_Box_A>();
        TCB=center_ball.GetComponent<Tap_Center_Ball_A>();

        CL=csv_loader.GetComponent<Csv_Loader_A>();
        Debug.Log(CL.file);
        CL.Csv_Input(CL.file);

        Normal_Canvas.gameObject.SetActive(false);
        Success_Canvas.gameObject.SetActive(false);
        Failure_Canvas.gameObject.SetActive(false);

        Audio_Souce=GetComponent<AudioSource>();
    }

    public void PlaySE(AudioClip _sound_effect)
    {
        Audio_Souce.PlayOneShot(_sound_effect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
