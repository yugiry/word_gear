using UnityEngine;

public class StageClear_Manager_M : MonoBehaviour
{
    public static StageClear_Manager_M instance;
    public bool[] ClearCheck_Flag = new bool[30];
    public int now_stage = -1;

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

    /// <summary>
    /// ステージがクリアしたらそのステージのフラグを有効化する
    /// </summary>
    public void StageClear()
    {
        ClearCheck_Flag[now_stage] = true;
    }
}
