using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using System.Linq;

[DefaultExecutionOrder(2)]
public class Overlapping_Needle_And_Ball_A : MonoBehaviour
{
    [SerializeField] private Load_Script_A LS;
    //private Hold_Information_Of_Mysterious_Ball_A hiomb;
   // private CheckingAnswers ca;

    private Vector2 needle_pos;
    //[SerializeField] private Transform pivotPoint;
    private float needle_start_angle;

    private Collider2D needle_collider;

    private string MbLetter;
    public GameObject CenterBall;

    public GameObject[] MysteriousBalls;
    public bool Overlapped_Needle_Ball_Flag;
    public GameObject Overlapping_Ball;
    private Collider2D overlapping_ball_collider;
    [SerializeField] private Vector2 overlapping_ball_center_collider;

    [SerializeField] private GameObject[] overlapping_balls;

    public bool[] Ball_flg;
    public int Chosen_ball_number;

    //外部
    float previous_z;
    private int rotate_gear_num;
    //[SerializeField] private RectTransform image_size;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        MysteriousBalls = GameObject.FindGameObjectsWithTag("MysteriousBall");

        var sortedList = MysteriousBalls.OrderBy(obj => obj.transform.GetSiblingIndex()).ToArray();

        MysteriousBalls = sortedList;

        Ball_flg =new bool[MysteriousBalls.Length];

       
        needle_collider=GetComponent<Collider2D>();

        Overlapping_Ball = FindClosestBall();

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RandomBallChoose(int _chosen_ball, /*GameObject*//*string*/char[] _mysteryous_balls,bool[] _ball_flag)
    {
        //_chosen_ball= Random.Range(0, _mysteryous_balls.Length);
       
        for (int i = 0; i < _mysteryous_balls.Length; i++)
        {
          
            if (!_ball_flag[i])
            {
               
                break;
            }
           
            if (i==_mysteryous_balls.Length-1)
            return Random.Range(0, _mysteryous_balls.Length);
        }
        
        do
        {
            

            _chosen_ball = Random.Range(0, _mysteryous_balls.Length);
        } while (_ball_flag[_chosen_ball]);
        
        MbLetter = _mysteryous_balls[_chosen_ball].ToString()/*.GetComponent<HoldInformationOfMysteriousBall>().Ball_Letter*/;
        CenterBall.GetComponentInChildren<Text>().text = MbLetter;
       
        _ball_flag[_chosen_ball] = true;
        return _chosen_ball;
    }

    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "MysteriousBall")
        {
            Vector2 F_center = needle_collider.bounds.center;

            //for (int i = 0; i < MysteriousBalls.Length; i++)
            //{
            //    overlapping_balls_collider[i] = MysteriousBalls[i].GetComponent<Collider2D>();
            //    overlapping_balls_center_collider[i] = overlapping_balls_collider[i].bounds.center;
            //}

            bool F_overlap_flag = NeedlePosOverlappingBallPos(F_center, overlapping_ball_center_collider);

            Debug.Log(F_overlap_flag);

            LS.HIOMB = other.GetComponent<Hold_Information_Of_Mysterious_Ball_A>();

            if (!LS.HIOMB.Pressed_Flag&&F_overlap_flag)
            {
                Overlapped_Needle_Ball_Flag = true;
                //other.gameObject;
            }
            else
            {
                Overlapped_Needle_Ball_Flag = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "MysteriousBall")
        {
            LS.HIOMB = other.GetComponent<Hold_Information_Of_Mysterious_Ball_A>();
            Overlapped_Needle_Ball_Flag = false;
            if (!LS.HIOMB.Pressed_Flag)
            {
                //Overlapped_Needle_Ball_Flag = false;
                //Overlapping_Ball = null;
            }

            //MbLetter = other.gameObject.GetComponent<HoldInformationOfMysteriousBall>().BallLetter;
        }
    }

    private void OnMouseDown()
    {
        needle_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
       // needle_pos.y -= image_size.rect.height/2;
        needle_start_angle = transform.eulerAngles.z;
        previous_z = needle_start_angle;
        rotate_gear_num = 0;
    }

    private void OnMouseDrag()
    {
        Vector3 F_end_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        float F_angle = Vector2.SignedAngle(needle_pos, F_end_pos);
        float F_newz = needle_start_angle + F_angle;

        //回転方向判定
        float F_delta = Mathf.DeltaAngle(previous_z, F_newz);

        if (Mathf.Abs(F_delta) > 0.05f) // ノイズ防止
            rotate_gear_num = LS.SoundEffectGearRepearting(rotate_gear_num, LS.Sound_Effect[(int)Load_Script_A.SE_Names.Gear]);

        transform.rotation = Quaternion.Euler(0, 0, needle_start_angle + F_angle);

        Overlapping_Ball = FindClosestBall();

        

        // 次フレーム用に保存
        previous_z = F_newz;

    }

    private void OnMouseUp()
    {
       
        LS.StopSE();
    }

    GameObject FindClosestBall()
    {
        GameObject[] F_balls= GameObject.FindGameObjectsWithTag("MysteriousBall");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        //Vector3 position = transform.position;
        Vector3 F_center = needle_collider.bounds.center;
        foreach (GameObject go in F_balls)
        {
            Vector2 diff = go.GetComponent<Collider2D>().bounds.center - F_center;
            float curDistance = diff.sqrMagnitude; // 距離の二乗を使用
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
       
        overlapping_ball_center_collider = closest.GetComponent<Collider2D>().bounds.center;
        return closest;
    }

    private bool NeedlePosOverlappingBallPos(Vector2 _needle_center,Vector2 _ball_pos)
    {
        float _distance =Vector2.Distance(_needle_center, _ball_pos);

        Debug.Log("Distance:"+_needle_center+","+_ball_pos+","+_distance);

        if (_distance>-0.1&&_distance<0.1)
        {
            Debug.Log("Yes");
            return true;
        }
        else
        {
            Debug.Log("No");
            return false;
        }


            
    }


}
