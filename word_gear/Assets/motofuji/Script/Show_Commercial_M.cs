using UnityEngine;

public class Show_Commercial_M : MonoBehaviour
{
    public static Show_Commercial_M instance;
    public GameObject cm_canvas;
    public int play_time = 0;
    public int play_cm_time = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
