using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-1)]
public class Tap_Center_Ball_A : MonoBehaviour
{
    
    [SerializeField] private Load_Script_A LS;

    

    public int Center_click_count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        //LS.ONAB.Chosen_ball_number = LS.ONAB.RandomBallChoose(LS.ONAB.Chosen_ball_number, LS.CA.Split_answers, LS.CA.Chosen_Word);
        

        // center_click_count = -1;
    }

    //private void OnMouseDown()
    //{
    //    if (onab.Overlapped_Needle_Ball_Flag)
    //    {
    //        onab.Overlapping_Ball.GetComponent<HoldInformationOfMysteriousBall>().BallLetter = gameObject.GetComponentInChildren<Text>().text;
    //        onab.Overlapping_Ball.GetComponentInChildren<Text>().gameObject.SetActive(true);
    //        Debug.Log("Tottannjanaika?");
    //    }
    //    else
    //    {
    //        Debug.Log("Damejanaika?");
    //    }

    //    Debug.Log("Tottannjanaika?");

    //}

    public void ClickCenterBall()
    {
        LS.PlaySE(LS.Sound_Effect[(int)Load_Script_A.SE_Names.Click]);

        if (LS.ONAB.Overlapped_Needle_Ball_Flag&& LS.ONAB.Overlapping_Ball!=null)
        {
            LS.HIOMB = LS.ONAB.Overlapping_Ball.GetComponent<Hold_Information_Of_Mysterious_Ball_A>();
            if (!LS.HIOMB.Pressed_Flag)
            {
                LS.HIOMB.Ball_text.text = gameObject.GetComponentInChildren<Text>().text;
                LS.HIOMB.Ball_Letter = LS.HIOMB.Ball_text.text;
                // Debug.Log("Tottannjanaika?");
                LS.HIOMB.Img.sprite = LS.ONAB.Overlapping_Ball.GetComponent<Hold_Information_Of_Mysterious_Ball_A>().Ball_Image[1];
                // Debug.Log("Tottannjanaika2");
                LS.HIOMB.Ball_text.gameObject.SetActive(true);
                LS.HIOMB.Pressed_Flag = true;
                Debug.Log("Tottannjanaika?");

                LS.RG.Mosike_alpha.a = 1.0f;

                LS.RG.Mosike_image.color = LS.RG.Mosike_alpha;

                if (Center_click_count == LS.ONAB.Mysterious_Balls.Length - 1)
                {
                    LS.CA.CheckTheAnswer();
                    return;
                }
                else
                {
                    Center_click_count++;
                    LS.CBX.VanishBox();
                    
                    /*LS.ONAB.Chosen_ball_number =*/ LS.ONAB.ChosenWord(LS.ONAB.Words_Num,Center_click_count, LS.CA.Split_answers); //LS.ONAB.RandomBallChoose(LS.ONAB.Chosen_ball_number,/*onab.MysteriousBalls*/LS.CA.Split_answers,/*onab.Ball_flg*/LS.CA.Chosen_Word);
                    //LS.ONAB.Words_Num[LS.ONAB.Chosen_ball_number]++;
                    LS.ONAB.Overlapped_Needle_Ball_Flag = false;
                }

               

                



            }
            
        }
        else if (LS.HIOMB.Pressed_Flag && LS.ONAB.Overlapping_Ball)
        {
            LS.RG.Mosike_alpha.a = 1.0f;

            LS.RG.Mosike_image.color = LS.RG.Mosike_alpha;

            LS.HIOMB.Ball_text.text = gameObject.GetComponentInChildren<Text>().text;
            LS.HIOMB.Ball_Letter = LS.HIOMB.Ball_text.text;

            LS.ONAB.SwapWord(LS.ONAB.Words_Num, Center_click_count, LS.CA.Split_answers);

            

            Debug.Log("aiya-?");
        }
        else
        {
            Debug.Log("Damejanaika?");
        }
    }

    public void ResetAllBalls()
    {
        Debug.Log("ball_num" + LS.ONAB.Mysterious_Balls.Length);

        for (int i = 0; i < LS.ONAB.Mysterious_Balls.Length; i++)
        {
            Debug.Log("koko");
            LS.HIOMB = LS.ONAB.Mysterious_Balls[i].GetComponent<Hold_Information_Of_Mysterious_Ball_A>();

            if (LS.HIOMB==null)
            {
                Debug.Log("LS.HIOMB=NULL");
            }

            //if (LS.HIOMB.Img.sprite==null)
            //{
            //    Debug.Log("LS.HIOMB.IMG>SPrite=NULL");
            //}

            //if (LS.HIOMB.Img.sprite != LS.HIOMB.Ball_Image[0])
            //{
            //    
            //}
            LS.HIOMB.Img.sprite = LS.HIOMB.Ball_Image[0];

            LS.HIOMB.Pressed_Flag = false;
            LS.HIOMB.Ball_text.gameObject.SetActive(false);
            LS.CA.Chosen_Word[i] = false;

            Center_click_count = 0;

        }
        LS.ONAB.RearrangeWord(LS.CA.Split_answers, LS.CA.Chosen_Word);
        //LS.ONAB.Chosen_ball_number++ /*= LS.ONAB.RandomBallChoose(LS.ONAB.Chosen_ball_number, LS.CA.Split_answers, LS.CA.Chosen_Word)*/;

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
