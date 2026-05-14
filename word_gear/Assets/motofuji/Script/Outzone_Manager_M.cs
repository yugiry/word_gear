using UnityEngine;

public class Outzone_Manager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ball(Clone)")
        {
            Ball_Status_M F_bs = collision.gameObject.GetComponent<Ball_Status_M>();
            Rigidbody2D F_rb = collision.gameObject.GetComponent<Rigidbody2D>();
            F_bs.Drop_Ans = false;
            F_rb.simulated = false;
        }
    }
}
