using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(10)]
public class Continue_Synopsis_A : MonoBehaviour
{
    [SerializeField] private Load_Script_A LS;

    
    [SerializeField] private GameObject synopsis_text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();
        //normal_canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextClick()
    {
        LS.PlaySE(LS.Sound_Effect[0]);
        synopsis_text.gameObject.SetActive(false );
        LS.Normal_Canvas.gameObject.SetActive(true);
        LS.BP.StopBGM();
        LS.BP.PlayBGM(LS.BGM_Clip[0]);
    }

}
