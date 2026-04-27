using System.Linq;
using UnityEngine;
[DefaultExecutionOrder(1)]
public class Create_Balls_A : MonoBehaviour
{
    [SerializeField] private Load_Script_A LS;

    [SerializeField]
    private GameObject create_spawn_object; // 生成するオブジェクト

    [SerializeField] private GameObject[] spawn_object;

    [SerializeField]
    private int itemCount = 40; // 生成するオブジェクトの数

    [SerializeField]
    private float radius = 5f; // 半径

    

    

 

    void Start()
    {
        LS = GameObject.FindGameObjectWithTag("ScriptLoader").GetComponent<Load_Script_A>();

        CreateBall();

    }

    private void CreateBall()
    {

        itemCount = LS.CA.Answers.Length;

        spawn_object = new GameObject[itemCount];

        var F_plus_start_Angle = Mathf.PI / 2.0f; // 90度

        float F_anglePerItem = 30f * Mathf.Deg2Rad; // 度→ラジアン変換
        float F_totalAngle = F_anglePerItem * (itemCount - 1);

        float F_startAngle = F_totalAngle / 2f + F_plus_start_Angle; // 中央揃え
        float F_endAngle = -F_totalAngle / 2f + F_plus_start_Angle;

        for (int i = 0; i < itemCount; i++)
        {
            float F_t = (float)i / (itemCount - 1);
            float F_angle = Mathf.Lerp(F_startAngle, F_endAngle, F_t);

            float F_x = Mathf.Cos(F_angle) * radius;
            float F_y = Mathf.Sin(F_angle) * radius;

            var F_obj = Instantiate(create_spawn_object, new Vector3(F_x, F_y), Quaternion.identity, transform);
            spawn_object[i] = F_obj;
        }

        var F_sorted_list = spawn_object.OrderBy(F_obj => F_obj.transform.GetSiblingIndex()).ToArray();

        spawn_object = F_sorted_list;

    }

}


