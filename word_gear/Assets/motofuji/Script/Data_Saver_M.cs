using UnityEngine;

public class Data_Saver_M : MonoBehaviour
{
    [SerializeField] private StageClear_Manager_M scm;

    public void ChengeMaxClear(int _clear_stage)
    {
        PlayerPrefs.SetInt("MaxClear", _clear_stage);
    }
}
