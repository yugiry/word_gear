using System;
using UnityEngine;

public class Create_Box_A : MonoBehaviour
{
    [SerializeField] private Load_Script_A LS;

    public GameObject Question_Box;
    public GameObject[] Clone_Boxes;

    //[SerializeField] private GameObject checking_answer;
    //private Checking_Answers_A ca;

    private int boxes_num;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();
        
        
        boxes_num = LS.CA.Answers.Length-1;
        CreateBox();
    }

    public void CreateBox()
    {
        

            Clone_Boxes = new GameObject[LS.CA.Answers.Length];

        Vector2 F_box_size = Question_Box.transform.localScale;

        Debug.Log("f_size:"+F_box_size);

        for (int i = 0; i < LS.CA.Answers.Length; i++)
        {
            Clone_Boxes[i] = Instantiate(Question_Box, new Vector3(gameObject.transform.position.x + (F_box_size.x) * i/3, gameObject.transform.position.y, 0.0f), Quaternion.identity,this.gameObject.transform.parent);
        }
    }

    public void VanishBox()
    {
        if (Clone_Boxes[0]!=null)
        {
            Destroy(Clone_Boxes[Clone_Boxes.Length-1]);
            Array.Resize(ref Clone_Boxes, Clone_Boxes.Length-1);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
