using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(15)]
public class Tap_Center_Ball_A : MonoBehaviour
{
    
    [SerializeField] private Load_Script_A LS;

    

    private int center_click_count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        LS.ONAB.Chosen_ball_number = LS.ONAB.RandomBallChoose(LS.ONAB.Chosen_ball_number, LS.CA.Split_answers, LS.CA.Chosen_Word);
        

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
        LS.PlaySE(LS.Sound_Effect[0]);

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

                if (center_click_count == LS.ONAB.MysteriousBalls.Length - 1)
                {
                    LS.CA.CheckTheAnswer();
                    return;
                }
                else
                {
                    center_click_count++;
                    LS.CB.VanishBox();
                    LS.ONAB.Chosen_ball_number = LS.ONAB.RandomBallChoose(LS.ONAB.Chosen_ball_number,/*onab.MysteriousBalls*/LS.CA.Split_answers,/*onab.Ball_flg*/LS.CA.Chosen_Word);

                    LS.ONAB.Overlapped_Needle_Ball_Flag = false;
                }

               

                



            }
            
        }
        else
        {
            Debug.Log("Damejanaika?");
        }
    }

    public void ResetAllBalls()
    {
        for (int i = 0; i < LS.ONAB.MysteriousBalls.Length; i++)
        {
            LS.HIOMB = LS.ONAB.MysteriousBalls[i].GetComponent<Hold_Information_Of_Mysterious_Ball_A>();
            LS.HIOMB.Img.sprite = LS.HIOMB.Ball_Image[0];
            LS.HIOMB.Pressed_Flag = false;
            LS.HIOMB.Ball_text.gameObject.SetActive(false);
            LS.CA.Chosen_Word[i] = false;

            center_click_count = 0;

        }

        LS.ONAB.Chosen_ball_number = LS.ONAB.RandomBallChoose(LS.ONAB.Chosen_ball_number, LS.CA.Split_answers, LS.CA.Chosen_Word);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
