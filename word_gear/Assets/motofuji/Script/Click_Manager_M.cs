using Common;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Click_Manager_M : MonoBehaviour
{
    //オブジェクト
    [SerializeField] private RectTransform disk_transform;
    [SerializeField] private GameObject slide_parent;
    [SerializeField] private GameObject ball_parent;
    private RectTransform[] slide_child;

    //定数
    private const float round = 360f;
    private const int interval_radian = 30;
    private const int half_interval_radian = interval_radian / 2;

    //変数
    private Vector2 object_pos;
    private Vector2 prev_mouse;             //ドラッグしている時のマウスの座標
    private Vector2 mouse;                  //クリックした時のマウスの座標
    private Vector2 mouse_pos;
    private float before_delta;
    private Vector2 click_pos;
    private Vector2[] start_pos;            //クリックされた時のスライダーの位置の保持用
    private int slider_index = 0;           //スライダーが何個あるかの保持用
    private bool return_rotation = false;
    private bool on_click = false;
    private float disk_rotation;
    //private bool click_rotation = false;
    public float angle_delta;
    [SerializeField] private float total_angle;
    [SerializeField] private float angle;
    [SerializeField] private List<Vector2> vecList = new List<Vector2>();


    //SE
    [SerializeField] private C_Music music_class;
    [System.Serializable]
    class C_Music
    {
        public AudioSource AS;
        public AudioClip Release_Disk;//ディスク回転時のSE
        public AudioClip Click_Button;//ボタンが押されたとき
    }
    private bool check_se_disk = false;
    private void Update()
    {
        disk_rotation = disk_transform.localEulerAngles.z;
        if (disk_rotation < 0) disk_rotation += round;

        //角度が5度以下になるまで回転し続ける
        if (return_rotation)
        {
            if (disk_rotation > 0f)
            {
                disk_transform.transform.Rotate(0f, 0f, round * Time.deltaTime);
            }
            if (disk_rotation < 5f)
            {
                return_rotation = false;
                disk_transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }

    #region ダイアル
    //ディスクがクリックされた時
    public void ClickMouseOnDisk()
    {
        if (!start_processing.Instance.Finish_Count)
            return;

        if (!return_rotation)
        {
            object_pos = Camera.main.WorldToScreenPoint(disk_transform.position);

            prev_mouse = mouse = Input.mousePosition;

            click_pos = (Vector2)Input.mousePosition - object_pos;
            before_delta = angle_delta = Mathf.Atan2(click_pos.y, click_pos.x) * Mathf.Rad2Deg;
            if (angle_delta < 0)
            {
                angle_delta += round;
                before_delta += round;
            }

            on_click = true;

            total_angle = 0;

            vecList.Clear();

            Debug.Log("ディスクをクリック");
        }
    }

    //ディスクがドラッグされた時
    public void DragMouseOnDisk()
    {
        if (!start_processing.Instance.Finish_Count)
            return;

        if (!return_rotation && on_click)
        {
            //現在の入力位置(スクリーン座標)
            mouse_pos = (Vector2)Input.mousePosition - object_pos;

            //開始位置と現在位置の角度差を計算(アークタンジェントでベクトル→角度変換)
            float F_angle_current = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            if (F_angle_current < 0) F_angle_current += round;
            if (5f > total_angle && total_angle >= 0f)
            {
                Debug.Log($"ｺﾝﾆﾁﾊ{before_delta}、{F_angle_current}、{before_delta - F_angle_current}ｺﾝﾆﾁﾊ");
                if (before_delta - F_angle_current < 0)
                {
                    before_delta = F_angle_current;
                }
                else if (before_delta > 300f && F_angle_current > 0 && (before_delta - 360f) - F_angle_current < 0)
                {
                    before_delta = F_angle_current;
                }
                else
                {
                    total_angle = F_angle_current - before_delta;
                }
            }
            else if (total_angle < 30f)
            {
                //Debug.Log($"ｺﾝﾆﾁﾊ{before_delta}、{F_angle_current}、{before_delta - F_angle_current}ｺﾝﾆﾁﾊ");
                if (before_delta - F_angle_current < 0)
                {
                    before_delta = F_angle_current + 30f;
                    if (before_delta > 360f) before_delta -= 360f;
                }
                else if (before_delta > 0f && F_angle_current > 300f && before_delta - (F_angle_current - 360f) < 0)
                {
                    before_delta = F_angle_current + 30f;
                    if (before_delta > 360f) before_delta -= 360f;
                }
                else
                {
                    Debug.Log($"{before_delta}、{F_angle_current}");
                    //total_angle = F_angle_current - before_delta;
                }
            }
            else
            {
                total_angle = F_angle_current - before_delta;
            }

            //total_angle -= Mathf.Abs(angle);
            if (total_angle < 0) total_angle += round;

            //計算された角度差をもとにオブジェクトを回転させる
            disk_transform.rotation = Quaternion.Euler(0, 0, total_angle);

            //SE
            if (!check_se_disk)
            {
                music_class.AS.PlayOneShot(music_class.Release_Disk);
                check_se_disk = true;

            }
            Debug.Log("ディスクをドラッグ中");
        }
    }

    //ディスクが離された時
    public void RevertDisk()
    {
        if (!start_processing.Instance.Finish_Count)
            return;

        if (!return_rotation)
        {
            //現在のディスクの角度
            float F_disk_radian = disk_transform.localEulerAngles.z;

            //玉が置けない箇所は見ない
            if (315 > F_disk_radian && F_disk_radian > 15)
            {
                for (int i = 0; i <= round; i += interval_radian)
                {
                    if (i - half_interval_radian < F_disk_radian && F_disk_radian < i + half_interval_radian)
                    {
                        disk_transform.localRotation = Quaternion.Euler(0, 0, i);

                        //排出口の下にスライダーの穴がある時
                        for (int j = 0; j < slider_index; j++)
                        {
                            if (slide_child[j].anchoredPosition.x == 0)
                            {
                                //今排出口にある玉の情報を呼び出す
                                GameObject F_ball = ball_parent.transform.GetChild(i / interval_radian - 1).gameObject;
                                Ball_Status_M F_bs = F_ball.GetComponent<Ball_Status_M>();

                                F_bs.DropBall(j);

                                Debug.Log("玉排出");
                            }
                        }
                    }
                }
            }

            //0度までゆっくり戻す
            if (F_disk_radian > 0) return_rotation = true;

            on_click = false;

            check_se_disk = false;
            Debug.Log("ディスクを離した");
        }
    }
    #endregion

    #region スライダー
    //スライダーがクリックされた時
    public void ClickMouseOnSlide()
    {
        if (!start_processing.Instance.Finish_Count)
            return;

        //オブジェクトのスクリーン上の位置を取得(ワールド→スクリーン変換)
        object_pos = Camera.main.WorldToScreenPoint(slide_parent.transform.position);
        //クリック時の入力位置(スクリーン座標)
        click_pos = (Vector2)Input.mousePosition - object_pos;

        //スライダーの子オブジェクト達を取得
        slider_index = 0;
        slide_child = new RectTransform[slide_parent.transform.childCount];
        start_pos = new Vector2[slide_parent.transform.childCount];
        foreach (RectTransform child in slide_parent.transform)
        {
            //"SlideChild"tagのオブジェクトとその座標を記憶
            if (child.gameObject.tag == "SlideChild")
            {
                slide_child[slider_index] = gameObject.GetComponent<RectTransform>();
                slide_child[slider_index] = child;
                start_pos[slider_index] = child.anchoredPosition;
                slider_index++;
            }
        }

        Debug.Log("スライダーをクリック");
    }

    //スライダーがドラッグされた時
    public void DragMouseOnSlide()
    {
        if (!start_processing.Instance.Finish_Count)
            return;

        //現在の入力位置(スクリーン座標)
        mouse_pos = (Vector2)Input.mousePosition - object_pos;
        Vector2 F_move_vec = mouse_pos - click_pos;

        //記憶した子オブジェクト達の移動処理
        for (int i = 0; i < slider_index; i++)
        {
            slide_child[i].anchoredPosition = new Vector2(start_pos[i].x + F_move_vec.x, start_pos[i].y);
        }

        Debug.Log("スライダーをドラッグ中");
    }

    //スライダーが離された時
    public void RevertSlide()
    {
        if (!start_processing.Instance.Finish_Count)
            return;

        int F_near_child = 0;
        Vector2 F_move_pos;

        //X軸の絶対値が一番小さいスライダーオブジェクトを調べる
        for (int i = 1; i < slider_index; i++)
        {
            if (Mathf.Abs(slide_child[F_near_child].anchoredPosition.x) > Mathf.Abs(slide_child[i].anchoredPosition.x))
            {
                F_near_child = i;
            }
        }
        //一番小さいスライダーオブジェクトのX値の符号を反転させて記憶
        F_move_pos = new Vector2(-slide_child[F_near_child].anchoredPosition.x, 0);
        //すべてのスライダーオブジェクトに適応させる
        for (int i = 0; i < slider_index; i++)
        {
            slide_child[i].anchoredPosition += F_move_pos;
        }

        Debug.Log("スライダーを離した");
    }
    #endregion

    #region シーン移行

    public void ClickNextButton()
    {
        //SE
        music_class.AS.PlayOneShot(music_class.Click_Button);
        StartCoroutine(PlayNextStage());
    }

    //遅延関数
    private IEnumerator PlayNextStage()
    {
        fade_manager.Instance.Fade();
        yield return new WaitUntil(() => fade_manager.Instance.Finish_Fade_Out);
        StageClear_Manager_M F_scm = GameObject.Find("StageClearManager").GetComponent<StageClear_Manager_M>();
        F_scm.StageClear();
        if(F_scm.now_stage >= 29)
        {
            SceneManager.LoadScene("titlescene");
        }
        else
        {
            Show_Commercial_M.instance.PlayGame();
            SceneManager.LoadScene("GearNeedleRotationScene");
        }
            fade_manager.Instance.Fade_In = true;
    }

    public void ClickTitleButton()
    {
        //SE
        music_class.AS.PlayOneShot(music_class.Click_Button);
        StartCoroutine(PlayReturnTitle());       
    }

    //遅延関数
    private IEnumerator PlayReturnTitle()
    {
        fade_manager.Instance.Fade();
        yield return new WaitUntil(() => fade_manager.Instance.Finish_Fade_Out);
        StageClear_Manager_M F_scm = GameObject.Find("StageClearManager").GetComponent<StageClear_Manager_M>();
        F_scm.now_stage = -1;
        Show_Commercial_M.instance.PlayGame();
        SceneManager.LoadScene("titlescene");
        fade_manager.Instance.Fade_In = true;
    }

    #endregion
}
