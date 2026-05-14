using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
[DefaultExecutionOrder(-1)]
public class Hold_Information_Of_Mysterious_Ball_A : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string Ball_Letter;
    public int Input_Ball_Num = -1;
    public Text Ball_text;
    public Sprite[] Ball_Image;
    public Image Img;
    public bool Pressed_Flag;
    void Start()
    {
        Img= gameObject.GetComponent < Image >();
        Img.sprite = Ball_Image[0];
        Ball_text = gameObject.GetComponentInChildren<Text>();
        Ball_text.text = Ball_Letter;
        Ball_text.gameObject.SetActive(false);
        Pressed_Flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
