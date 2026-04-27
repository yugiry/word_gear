using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        gear_start_pos = transform.position;

        foreach(Image _center_ball in center_ball.GetComponentsInChildren<Image>())
        {

            if (center_ball.gameObject!=gameObject)
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
                if (Mosike_alpha.a > 0.8f)
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



        //if (JudgeStartTransform(F_end_pos, gear_pos))//右回転なら
        // {
        //     //if (JudgeStartTransform(F_end_pos, gear_pos))//右回転なら
        //     {
        //         if (Mosike_alpha.a>0.8f)
        //         {
        //             Mosike_alpha.a -= 0.0005f;
        //             Mosike_image.color = Mosike_alpha;
        //         }

        //     }
        //     //else
        //     //{
        //     //    if (mosike_alpha.a < 1.0f)
        //     //    {
        //     //        mosike_alpha.a += 0.01f;
        //     //        mosike_image.color = mosike_alpha;
        //     //    }
        //     //}

        //     gear_pos = CalculateJustBeforePos(F_end_pos, gear_pos);
        //     float F_angle = Vector2.SignedAngle(gear_start_pos, F_end_pos);

        //     transform.rotation = Quaternion.Euler(0, 0, start_angle + F_angle);
        //     Debug.Log("Gear_pos:" + gear_pos.x + "," + gear_pos.y);
        //     Debug.Log("F_end_pos:" + F_end_pos.x + "," + F_end_pos.y);
        // }
        //else//左回転なら
        //{
        //    // transform.rotation = Quaternion.Euler(0, 0, start_angle);
        //    Debug.Log("Gear_pos+Dame:" + gear_pos.x + "," + gear_pos.y);
        //    Debug.Log("F_end_pos+Dame:" + F_end_pos.x + "," + F_end_pos.y);
        //}

    }

   


    private Vector2 CalculateJustBeforePos(Vector2 _now_mouse_pos, Vector2 _now_just_before_mouse_pos)
    {
        if (_now_mouse_pos.x > 0f)
        {
            if (_now_mouse_pos.y > 0f)
            {
                _now_just_before_mouse_pos.x = _now_mouse_pos.x - 0.01f;
                _now_just_before_mouse_pos.y = _now_mouse_pos.y + 0.01f;
               
            }
            else
            {//
                _now_just_before_mouse_pos.x = _now_mouse_pos.x + 0.01f;
                _now_just_before_mouse_pos.y = System.Math.Min(_now_mouse_pos.y + 0.01f,0.0f);
            }


        }
        else
        {
            if (_now_mouse_pos.y > 0f)
            {//
                _now_just_before_mouse_pos.x = System.Math.Min(_now_mouse_pos.x - 0.01f, 0.0f);
                _now_just_before_mouse_pos.y = _now_mouse_pos.y - 0.01f;
            }
            else
            {
                _now_just_before_mouse_pos.x = _now_mouse_pos.x + 0.01f;
                _now_just_before_mouse_pos.y = System.Math.Min(_now_mouse_pos.y - 0.01f, 0.0f);
            }
        }

        return _now_just_before_mouse_pos;
    }

    private bool JudgeStartTransform(Vector2 _mouse_pos,Vector2 _just_before_mouse_pos)
    {
       //_just_before_mouse_pos= CalculateJustBeforePos(_mouse_pos, _just_before_mouse_pos);

        if (_mouse_pos.x > 0f)
        {
           
         if (_mouse_pos.y > 0f)
            {
                if (_mouse_pos.x>_just_before_mouse_pos.x)
                {
                    return true;
                }
                else
                {
                    Debug.Log("Dame1");
                    return false;
                   
                }

            }
            else
            {
                
                if (_mouse_pos.x <= _just_before_mouse_pos.x)
                {
                    return true;
                }
                else
                {
                    Debug.Log("Dame2");
                    return false;
                }
            }

        }
        else
        {
            if (_mouse_pos.y > 0f)
            {
                if (_mouse_pos.x > _just_before_mouse_pos.x)
                {
                    return true;
                }
                else
                {
                    Debug.Log("Dame3");
                    return false;
                }
            }
            else
            {
                if (_mouse_pos.x <= _just_before_mouse_pos.x)
                {
                    return true;
                }
                else
                {
                    Debug.Log("Dame4");
                    return false;
                }
            }
        }

        
    }

    private void OnMouseUp()
    {
        // mosike_alpha.a = 1.0f;
        //mosike_image.color = mosike_alpha;

        LS.StopSE();
    }



}
