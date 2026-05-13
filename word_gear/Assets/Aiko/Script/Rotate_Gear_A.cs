using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(1)]
public class Rotate_Gear_A : MonoBehaviour
{
    // public GameObject Gear;//回転させる歯車

    [SerializeField] private Load_Script_A LS;

    private Vector2 gear_pos;
    private Vector2 gear_start_pos;
    private float start_angle;
    
    [SerializeField] private GameObject center_ball;
    public Image Mosike_image;
    public Color Mosike_alpha;

    

    //外部
    float previous_z;
    int rotate_gear_num;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        //gear_pos = transform.position;
        //gear_start_pos = transform.position;

        //foreach(Image _center_ball in center_ball.GetComponentsInChildren<Image>())
        //{

        //    if (center_ball.gameObject!=gameObject)
        //    {
        //        Mosike_image = _center_ball.GetComponentInChildren<Image>();
        //        Mosike_alpha = Mosike_image.color;
               
        //    }
           
        //}

        //rotate_gear_num = 0;
        
    }

    public void SetCenterBallColor()
    {
        gear_start_pos = transform.position;

        foreach (Image _center_ball in center_ball.GetComponentsInChildren<Image>())
        {

            if (center_ball.gameObject != gameObject)
            {
                Mosike_image = _center_ball.GetComponentInChildren<Image>();
                Mosike_alpha = Mosike_image.color;

            }

        }

        rotate_gear_num = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnMouseDown()
    {
        gear_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gear_start_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        start_angle = transform.eulerAngles.z;

        previous_z = start_angle;

        rotate_gear_num = 0;
    }

    private void OnMouseDrag()
    {
        Vector2 F_end_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //bool F_judge_transform= JudgeStartTransform(F_end_pos, gear_pos);


        // 回転量計算
        float F_angle = Vector2.SignedAngle(gear_start_pos, F_end_pos);
        float F_newz = start_angle + F_angle;

        //回転方向判定
        float F_delta = Mathf.DeltaAngle(previous_z, F_newz);

        if (Mathf.Abs(F_delta) > 0.05f) // ノイズ防止
        {
            if (F_delta > 0)
            {
                Debug.Log("反時計回り");

                LS.StopSE();
            }
            else
            {
                Debug.Log("時計回り");

                // 例：時計回り時の処理（今のモザイク減少をこっちに）
                if (Mosike_alpha.a > 0.1f)
                {
                    Mosike_alpha.a -= 0.0005f;
                    Mosike_image.color = Mosike_alpha;
                }
                // 回転適用
                transform.rotation = Quaternion.Euler(0, 0, F_newz);

                //StartCoroutine("SoundEffectGearRepearting");
               rotate_gear_num= LS.SoundEffectGearRepearting(rotate_gear_num, LS.Sound_Effect[(int)Load_Script_A.SE_Names.Gear]);
            }
        }
       

            // 次フレーム用に保存
            previous_z = F_newz;

    }

   


    

    private void OnMouseUp()
    {
        // mosike_alpha.a = 1.0f;
        //mosike_image.color = mosike_alpha;

        LS.StopSE();
    }



}
