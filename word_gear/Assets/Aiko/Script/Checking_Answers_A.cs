using UnityEngine;
[DefaultExecutionOrder(1)]
public class Checking_Answers_A : MonoBehaviour
{
    public char[] Split_answers;

    public string Answers;
    
    public bool[] Chosen_Word;
    //[SerializeField] Overlapping_Needle_And_Ball_A onab;
    //[SerializeField] Tap_Center_Ball_A tcb;
    //[SerializeField] GameObject needle;

    [SerializeField] private Load_Script_A LS;

    [SerializeField] string tag_name_center_ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Split_answers = Answers.ToCharArray();

        //onab = needle.GetComponent<Overlapping_Needle_And_Ball_A>();
        //tcb = GameObject.FindGameObjectWithTag(tag_name_center_ball).GetComponent<Tap_Center_Ball_A>();

        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        //Chosen_Word =new bool[Split_answers.Length];

        //answers = new string[onab.MysteriousBalls.Length];
    }

    public void SetAnswer()
    {
        Split_answers = Answers.ToCharArray();
        Chosen_Word = new bool[Split_answers.Length];
    }

    public void CheckTheAnswer()
    {
        for (int i = 0; i < LS.ONAB.Mysterious_Balls.Length; i++)
        {
            if (Split_answers[i].ToString() != LS.ONAB.Mysterious_Balls[i].GetComponent<Hold_Information_Of_Mysterious_Ball_A>().Ball_Letter)
            {
                Debug.Log("MatigatterunnzaMIRROR!!" + i + "番目" + Answers[i]+"!="+ LS.ONAB.Mysterious_Balls[i].GetComponent<Hold_Information_Of_Mysterious_Ball_A>().Ball_Letter);

                //LS.FG.GameOver();
                LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.InCorrect]);

                LS.TCB.ResetAllBalls();
                LS.CBX.VanishBox();
                LS.CBX.CreateBox();
                return;
            }
        }
        CorrectAnswer();
    }

    public void CorrectAnswer()
    {
        LS.CBX.VanishBox();
        LS.Normal_Canvas.gameObject.SetActive(false);
        LS.Success_Canvas.gameObject.SetActive(true);
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Success]);
        LS.BP.StopBGM();
        LS.BP.PlayBGM(LS.BGM_Clip[(int)Load_Script_A.BGM_Names.GameClear]);
        Debug.Log("toottayo!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
