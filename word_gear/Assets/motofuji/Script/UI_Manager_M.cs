using UnityEngine;
using UnityEngine.UI;

public class UI_Manager_M : MonoBehaviour
{
    [SerializeField] Image start_img;
    [SerializeField] Image game_img;
    [SerializeField] Image success_img;
    [SerializeField] Image failure_img;

    [SerializeField] Text start_text;
    [SerializeField] Text game_text;
    [SerializeField] Text success_text;
    [SerializeField] Text failure_text;

    [Tooltip("クリアフラグを確認するスクリプト")]StageClear_Manager_M scm;

    private void Start()
    {
        scm = GameObject.Find("StageClearManager").GetComponent<StageClear_Manager_M>();
        
    }
}
