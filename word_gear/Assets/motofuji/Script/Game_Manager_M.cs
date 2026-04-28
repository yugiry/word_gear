using UnityEngine;
using Common;
using System;
using System.Linq;
using UnityEngine.UI;

public class Game_Manager_M : MonoBehaviour
{
    //オブジェクト
    [SerializeField] GameObject start_canvas;
    [SerializeField] GameObject game_canvas;
    [SerializeField] GameObject success_canvas;
    [SerializeField] GameObject failure_canvas;
    [SerializeField] GameObject hole;
    [SerializeField] GameObject hole_parent;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject ball_parent;
    GameObject[] hole_array;
    GameObject[] ball_array = new GameObject[10];
    RectTransform[] hole_rect_array;

    //定数
    const int hole_width = 140;
    const int hole_half_width = hole_width / 2;

    //変数
    public int time = 60;
    private int flame_time;
    public string ans_text;     //答えの文字列
    public int ans_num;         //答えの文字数の保持用
    Vector2 slide_mid;
    private int[] spawn_texts = new int[C_GrovalConst.Ball_Num];
    private bool get_correct;
    int check_inball = 0;
    private bool do_slide = false;
    private bool show_slide = false;
    private bool game_over = false;
    private bool in_game = false;

    private void Start()
    {
        start_canvas.SetActive(true);
        game_canvas.SetActive(false);
        success_canvas.SetActive(false);
        failure_canvas.SetActive(false);
    }

    private void Update()
    {
        if (!game_over && in_game)
        {
            if (time == 0)
            {
                game_over = true;
                TimeOver();
            }

            //スライダーの穴にボールが入っているか調べる
            check_inball = 0;
            for (int i = 0; i < ans_num; i++)
            {
                Slide_Status_M F_ss = hole_array[i].GetComponent<Slide_Status_M>();
                if (F_ss.In_Ball)
                {
                    check_inball++;
                }
            }
            //答えの文字数分入っていたら答えが合っているかチェックする
            if (check_inball == ans_num) CheckAnswerBall();

            if (do_slide)
            {
                MoveSlide();
            }
            if (time != 0)
            {
                if (flame_time >= 60)
                {
                    flame_time = 0;
                    time--;
                }
                flame_time++;
            }
        }
    }

    public void StageTap()
    {
        TurnCanvas(2);
        GameStart();
        flame_time = 0;
        in_game = true;
    }

    /// <summary>
    /// ステージに入ったら玉やスライダーの生成をする
    /// </summary>
    public void GameStart()
    {
        //スライダーの真ん中を求める
        RectTransform F_parent_rectpos = hole_parent.GetComponent<RectTransform>();
        slide_mid = new Vector2(-F_parent_rectpos.anchoredPosition.x, F_parent_rectpos.anchoredPosition.y);
        get_correct = false;

        SpawnHole(ans_num);
        SetUpStringOnBalls();
        SpawnBalls();
    }

    /// <summary>
    /// 玉が入る穴を生成する関数
    /// </summary>
    /// <param name="_text_num">答えの文字数</param>
    public void SpawnHole(int _text_num)
    {
        int F_start_place_x;
        int F_text_num_half = _text_num / 2;

        hole_array = new GameObject[_text_num];
        hole_rect_array = new RectTransform[_text_num];

        //出題文字数の文字数によって設置を始める位置を変える
        if (_text_num % 2 == 0)
        {
            F_start_place_x = -hole_width * F_text_num_half + hole_half_width;
        }
        else
        {
            F_start_place_x = -hole_width * F_text_num_half;
        }
        //文字分だけ設置
        for (int i = 0; i < _text_num; i++)
        {
            GameObject F_child_hole = Instantiate(hole, hole_parent.transform);
            hole_array[i] = F_child_hole;
            hole_rect_array[i] = F_child_hole.GetComponent<RectTransform>();
            Slide_Status_M F_ss = F_child_hole.GetComponent<Slide_Status_M>();

            hole_rect_array[i].anchoredPosition = new Vector2(F_start_place_x + slide_mid.x + hole_width * i, 0);
            hole_rect_array[i].localRotation = Quaternion.identity;
            hole_rect_array[i].localScale = new Vector3(1, 1, 1);

            C_GrovalLet F_gl = new C_GrovalLet();
            //生成時にどの文字が答えか保持しておく
            for (int j = 0; j < C_GrovalConst.Max_Text; j++)
            {
                if (ans_text[i] == F_gl.NumForChar[j])
                {
                    spawn_texts[i] = j;
                    F_ss.Text = j;
                }
            }
        }
    }

    /// <summary>
    /// 玉に表示する文字を指定する関数
    /// </summary>
    public void SetUpStringOnBalls()
    {
        for (int i = ans_num; i < C_GrovalConst.Ball_Num; i++)
        {
            spawn_texts[i] = UnityEngine.Random.Range(0, C_GrovalConst.Max_Text);
        }

        spawn_texts = spawn_texts.OrderBy(x => Guid.NewGuid()).ToArray();
    }

    /// <summary>
    /// 玉を召喚する関数
    /// </summary>
    public void SpawnBalls()
    {
        //ディスクに開いている穴の分だけボールを生成する
        for (int i = 0; i < C_GrovalConst.Ball_Num; i++)
        {
            GameObject F_child_ball = Instantiate(ball, ball_parent.transform);
            ball_array[i] = F_child_ball;
            Ball_Status_M F_bs = F_child_ball.GetComponent<Ball_Status_M>();
            F_bs.SetPlaceAndText(i, spawn_texts[i]);
        }
    }

    /// <summary>
    /// 正誤判定の関数
    /// </summary>
    private void CheckAnswerBall()
    {
        for (int i = 0; i < ans_num; i++)
        {
            Slide_Status_M F_ss = hole_array[i].GetComponent<Slide_Status_M>();
            if (!F_ss.CheckString())
            {
                FalseAnswer();
                return;
            }
        }
        CollectAnswer();
    }

    /// <summary>
    /// 不正解
    /// </summary>
    private void FalseAnswer()
    {
        for (int i = 0; i < ans_num; i++)
        {
            Slide_Status_M F_ss = hole_array[i].GetComponent<Slide_Status_M>();
            F_ss.ReturnBalls();
        }
    }

    /// <summary>
    /// 正解
    /// </summary>
    private void CollectAnswer()
    {
        if (!get_correct)
        {
            get_correct = true;
            do_slide = true;
            game_over = true;

            Debug.Log("大正解！！！");
        }
    }

    /// <summary>
    /// 正解した後スライダーを画面右から左に動かす
    /// </summary>
    private void MoveSlide()
    {
        if (!show_slide)
        {
            if (hole_rect_array[0].anchoredPosition.x < 285f)
            {
                for (int i = 0; i < hole_rect_array.Length; i++)
                {
                    hole_rect_array[i].anchoredPosition += new Vector2(15.0f, 0f);
                }
            }
            else
            {
                show_slide = true;
            }
        }
        else
        {
            if (hole_rect_array[hole_rect_array.Length - 1].anchoredPosition.x > 150f)
            {
                for (int i = 0; i < hole_rect_array.Length; i++)
                {
                    hole_rect_array[i].anchoredPosition -= new Vector2(2.0f, 0f);
                }
            }
            else
            {
                show_slide = false;
                do_slide = false;
                //成功画面に移行
                TurnCanvas(0);
                DeleteObject();
            }
        }
    }

    /// <summary>
    /// 時間切れ
    /// </summary>
    private void TimeOver()
    {
        TurnCanvas(1);
        DeleteObject();
    }

    public void ReplayGame()
    {
        time = 60;
        flame_time = 0;
        TurnCanvas(2);
        GameStart();
    }

    private void DeleteObject()
    {
        for (int i = 0; i < hole_array.Length; i++)
        {
            Destroy(hole_array[i]);
        }
        int F_ball_num = ball_parent.gameObject.transform.childCount;
        for (int i = 0; i < F_ball_num; i++)
        {
            Destroy(ball_array[i]);
        }
    }

    /// <summary>
    /// ゲームクリアもしくはゲームオーバーになった時
    /// キャンバスを変更する
    /// </summary>
    private void TurnCanvas(int _mode)
    {
        start_canvas.SetActive(false);
        game_canvas.SetActive(false);
        success_canvas.SetActive(false);
        failure_canvas.SetActive(false);
        if (_mode == 0)
        {
            success_canvas.SetActive(true);
        }
        else if (_mode == 1)
        {
            failure_canvas.SetActive(true);
        }
        else if(_mode == 2)
        {
            game_canvas.SetActive(true);
        }
    }
}
