using UnityEngine;

public class Data_Saver_M : MonoBehaviour
{
    [SerializeField] private StageClear_Manager_M scm;

    //クリアした最大のステージ数をスマホ内部に保存する
    public void ChengeMaxClear(int _clear_stage)
    {
        PlayerPrefs.SetInt("MaxClear", _clear_stage);
    }

    //内部に保存した最大クリアステージ数を削除する
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.G))
        {
            PlayerPrefs.DeleteKey("MaxClear");
        }
    }
}
