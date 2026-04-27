using UnityEngine;

public class Slide_Status_M : Text_Status_M
{
    public bool In_Ball;
    [SerializeField] private Ball_Status_M ball_status;

    //入っている文字が自身と同じか判定
    public bool CheckString()
    {
        if(Text == ball_status.Text)
        {
            return true;
        }

        return false;
    }

    public void ReturnBalls()
    {
        ball_status.Drop_Ans = false;
        In_Ball = false;
        ball_status = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "ball(Clone)")
        {
            if (In_Ball)
            {
                //ボールを取り出す
                ball_status.Drop_Ans = false;
            }
            Ball_Status_M F_bs = collision.gameObject.GetComponent<Ball_Status_M>();
            collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            ball_status = F_bs;
            F_bs.Drop_Ans = true;
            In_Ball = true;
            Debug.Log("ボールが入った");
        }
    }
}
