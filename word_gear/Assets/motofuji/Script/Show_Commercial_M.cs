using UnityEngine;

public class Show_Commercial_M : MonoBehaviour
{
    public static Show_Commercial_M instance;
    public GameObject cm_canvas;
    public int play_time = 0;
    public int play_cm_time = 3;
    int show_cm_time = 120;
    int show_cm;

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

        cm_canvas.SetActive(false);
    }

    private void Update()
    {
        if(play_time == play_cm_time)
        {
            show_cm = 0;
            play_time = 0;
            cm_canvas.SetActive(true);
        }
    }

    public void PlayGame()
    {
        play_time++;
    }
}
