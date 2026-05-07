using UnityEngine;

public class StageClear_Manager_M : MonoBehaviour
{
    public static StageClear_Manager_M instance;
    public bool[] ClearCheck_Flag = new bool[30];
    public int now_stage = -1;
    public Data_Saver_M ds;

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

    private void Start()
    {
        int F_max_clear = PlayerPrefs.GetInt("MaxClear", -1);
        for(int i = 0; i <= F_max_clear; i++)
        {
            ClearCheck_Flag[i] = true;
        }
    }

    /// <summary>
    /// ステージがクリアしたらそのステージのフラグを有効化する
    /// </summary>
    public void StageClear()
    {
        ClearCheck_Flag[now_stage] = true;
        ds.ChengeMaxClear(now_stage);
        now_stage++;
    }
}
