using UnityEngine;
using UnityEngine.UI;
using Common;

public class Ball_Status_M : Text_Status_M
{
    //オブジェクト
    [SerializeField] Transform ball;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Text text;
    Transform disk_hole_parent;
    Transform disk_hole;
    Transform slide_hole_parent;
    Transform slide_hole;

    //定数

    //変数
    [SerializeField] private int disk_place;
    private int slide_place;
    public bool Drop_Ans = false;

    private void Start()
    {
        disk_hole_parent = GameObject.Find("disk_back").transform;
        slide_hole_parent = GameObject.Find("slide_hole_parent").transform;
    }

    private void Update()
    {
        if (Drop_Ans)
        {
            //ボールの位置を特定のslide_holeの位置に調整
            slide_hole = slide_hole_parent.GetChild(slide_place);
            ball.position = slide_hole.position;
            ball.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (!rb.simulated)
        {
            //ボールの位置を特定のdisk_holeの位置に調整
            disk_hole = disk_hole_parent.GetChild(disk_place);
            ball.position = disk_hole.position;
            ball.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    //ボールの場所を設定する
    public void SetPlaceAndText(int _place, int _text)
    {
        disk_place = _place;
        Text = _text;
        ChangeText();
    }

    //ボールを落とす
    public void DropBall(int _slide)
    {
        if (!Drop_Ans)
        {
            slide_place = _slide;
            rb.simulated = true;
            //Drop_Ans = true;
        }
    }

    //Textの数字によって表示させる文字を変更
    private void ChangeText()
    {
        C_GrovalLet F_grovallet = new C_GrovalLet();
        text.text = F_grovallet.NumForString[Text];
    }
}
