using UnityEngine;

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
        Split_answers = Answers.ToCharArray();

        //onab = needle.GetComponent<Overlapping_Needle_And_Ball_A>();
        //tcb = GameObject.FindGameObjectWithTag(tag_name_center_ball).GetComponent<Tap_Center_Ball_A>();

        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        Chosen_Word =new bool[Split_answers.Length];

        //answers = new string[onab.MysteriousBalls.Length];
    }

    public void CheckTheAnswer()
    {
        for (int i = 0; i < LS.ONAB.MysteriousBalls.Length; i++)
        {
            if (Split_answers[i].ToString() != LS.ONAB.MysteriousBalls[i].GetComponent<Hold_Information_Of_Mysterious_Ball_A>().Ball_Letter)
            {
                Debug.Log("MatigatterunnzaMIRROR!!" + i + "番目" + Answers[i]+"!="+ LS.ONAB.MysteriousBalls[i].GetComponent<Hold_Information_Of_Mysterious_Ball_A>().Ball_Letter);

                LS.TCB.ResetAllBalls();
                LS.CB.VanishBox();
                LS.CB.CreateBox();
                return;
            }
        }
        LS.CB.VanishBox();
        LS.Normal_Canvas.gameObject.SetActive(false);
        LS.Success_Canvas.gameObject.SetActive(true);
        Debug.Log("toottayo!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
