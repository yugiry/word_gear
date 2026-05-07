using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class answer_manager_s : MonoBehaviour
{
    public C_PanelInf[] panel_inf;

    [System.Serializable]
    public struct C_PanelInf
    {
        public int Length;
        public int Side;
        [SerializeField] public int Panel_Num;
        public float Scale_X;
        public float Scale_Y;
        public float Pos_X;
        public float Interval_X;//ｘ座標の間隔
        public float Pos_Y;
        public float Interval_Y;//y座標の間隔
        public float Limit_Time;//制限時間

        public Sprite Answer_Img;
    }

    [SerializeField ]private Image img;

    //パネルの位置X, Yを格納するためのクラス
    public class C_PosStock
    {
        public int X;
        public int Y;

        public C_PosStock(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
    }

    //複数のパネル配置を格納するためのクラス
    public class C_WordLayout
    {
        //パネルの位置を格納するリスト
        public List<C_PosStock> Chr_Pos_List = new List<C_PosStock>();

        //パネルの種類
        public int Panel_Type;
    }

    //private float _time;
    [SerializeField] private GameObject panel_text;
    [SerializeField] private GameObject correct_panel;
    [SerializeField] private GameObject tap_panel;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject tap;

    public static GameObject Copy_Panel;
    public int[] Panel_Box;
    private int[] correct_num;
    public static answer_manager_s Instance;
    public   int Count = 0;
    private int stage_index
    {
        get { return game_manager_s.Stage_Count - 1; }
    }

    private panel_manager_s[,] panel_grid;//パネルのグリッド

    private int[,] panel_chr_layout;//パネルの文字のレイアウト
    private C_WordLayout[] word_layout_array;//レイアウトの配列
    private bool layout_initialized = true;//レイアウトを初期化するか

    ////各_cornumのList
    public static List<int> Problem_List = new List<int>();
    public static List<int> Anser_List = new List<int>();

    private const int all_word = 71;

    //csv関連
    //CSV関連--------------------------------------
    private List<string> csv_data = new List<string>();
    private List<string> cas_data_2 = new List<string>();
    [SerializeField] private CSV_load_s CSV_LOAD;
    private const int split = 6;
    private const int answer_row  = 3;
    private const int first_falf = 15;

    //ゲームオーバー関連
    private bool game_over = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    private void OnEnable()
    {
        panel_manager_s.InitDictionary();
        LoadCSV();

        // ハズレ文字生成
        Anser_List.Clear();

        for (int i = 0; i <= all_word; i++)
        {
            if (!panel_manager_s.Problem_List.Contains(i))
            {
                Anser_List.Add(i);
            }
        }

        layout_initialized = false;

        correct_num = new int[panel_inf[stage_index].Panel_Num];
        panel_grid = new panel_manager_s[
            panel_inf[stage_index].Length,
            panel_inf[stage_index].Side
        ];

        panel_chr_layout = new int[
            panel_inf[stage_index].Length,
            panel_inf[stage_index].Side
        ];

        Panel_Box = new int[
            panel_inf[stage_index].Panel_Num
        ];
    }

    private void OnDisable()
    {
        panel_manager_s.Problem_List.Clear();

        Problem_List.Clear();


       Anser_List.Clear();

        layout_initialized = false;
        Count = 0;
    }

    //問題のロード
    void LoadCSV()
    {
        csv_data = CSV_LOAD.CSVInput("stage_inf");
        cas_data_2 = CSV_LOAD.CSVInput("stage_inf2");

        int F_problem_index = game_manager_s.Stage_Count;
        //分割
        int F_start = (F_problem_index - 1) * split;

        string F_answer;

        if (F_problem_index <= first_falf)
        {
           F_answer = csv_data[F_start + answer_row];
        }
        else
        {
            F_answer = cas_data_2[F_start + answer_row];
        }

        Debug.Log("現在ステージ = " + F_problem_index);
        Debug.Log("答え = " + F_answer);

        panel_manager_s.Problem_List.Clear();

        foreach (char F_c in F_answer)
        {

            if (panel_manager_s.Word_To_Index.ContainsKey(F_c))
            {
                panel_manager_s.Problem_List.Add(
                    panel_manager_s.Word_To_Index[F_c]
                );
            }
        }

        //イメージの切り替え
        ChangeImg( F_problem_index);
    }


    //イメージの切り替え関数
    void ChangeImg(int _index)
    {
        img.sprite = panel_inf[_index - 1].Answer_Img;
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (!layout_initialized)
        {
            InitializeLayouts();
            InitializePanels();
            PlacePanels();
            layout_initialized = true;
        }
    }

    //パネルの初期化関数
    void InitializePanels()
    { 
        
        //パネル配置処理
        for (int x = 0; x < panel_inf[stage_index].Length; x++)
            {
                for (int y = 0; y < panel_inf[stage_index].Side; y++)
                {
                    //パネルプレハブをインスタンス化
                    Copy_Panel = Instantiate(tap_panel);

                    //親オブジェクト、ローカルスケールを設定
                    Copy_Panel.transform.SetParent(parent.transform, false);
                    Copy_Panel.transform.localScale = new Vector2(panel_inf[stage_index].Scale_X, panel_inf[stage_index].Scale_Y);

                    //パネルの位置を設定
                    Copy_Panel.transform.position = new Vector2(
                        panel_inf[stage_index].Pos_X + y * panel_inf[stage_index].Interval_X,
                        panel_inf[stage_index].Pos_Y - x * panel_inf[stage_index].Interval_Y
                    );



                    panel_manager_s F_PANEL_MANAGER_S = Copy_Panel.GetComponent<panel_manager_s>();
                    F_PANEL_MANAGER_S.Word_Index = -1;
                //コンポーネントが存在する場合、_panelGridに設定
                if (F_PANEL_MANAGER_S != null)
                    {
                        panel_grid[x, y] = F_PANEL_MANAGER_S;
                    }

                    Count++;

            }
            }

        for (int x = 0; x < panel_inf[stage_index].Length; x++)
        {
            for (int y = 0; y < panel_inf[stage_index].Side; y++)
            {
                panel_grid[x, y].RefreshWord();
            }
        }

        
    }

    //レイアウトの初期化関数
    void InitializeLayouts()
    {
        int F_length = panel_inf[stage_index].Length;
        int F_side = panel_inf[stage_index].Side;

        panel_chr_layout = new int[F_length, F_side];
        word_layout_array = new C_WordLayout[3];


        for (int x = 0; x < F_length; x++)
        {
            for (int y = 0; y < F_side; y++)
            {
                panel_chr_layout[x, y] = -1;
            }
        }

        int[] F_panel_counts = new int[] {
        panel_manager_s.Problem_List.Count,
      
    };

        // PanelType1, 2, 3 のWordLayoutを初期化
        for (int i = 0; i < 3; i++)
        {
            word_layout_array[i] = new C_WordLayout
            {
                Panel_Type = i + 1
            };
        }
    }

    //設置する位置の初期化
    void SetInitialPositions(int _panel_count, int _panel_type)
    {
        List<C_PosStock> F_initial_positions = new List<C_PosStock>();

        //全ての位置をチェックしてランダムに位置を選ぶ
        for (int x = 0; x < panel_inf[stage_index].Length; x++)
        {
            for (int y = 0; y < panel_inf[stage_index].Side; y++)
            {
                if (panel_chr_layout[x, y] == -1)
                {
                    F_initial_positions.Add(new C_PosStock(x, y));
                }
            }
        }

        //初期位置のリストをシャッフル
        MixList(F_initial_positions);

        //配置に成功するまで次の初期位置を試す
        foreach (var F_start_pos in F_initial_positions)
        {
            if (CheckPlacePanel(_panel_count, _panel_type, F_start_pos))
            {
                return; //成功した場合は終了
            }
        }

        //ここまで来たら、失敗したパネルを周囲に配置する処理を追加
        for (int i = 0; i < F_initial_positions.Count && _panel_count > 0; i++)
        {
            if (panel_chr_layout[F_initial_positions[i].X, F_initial_positions[i].Y] == -1)
            {
                panel_chr_layout[F_initial_positions[i].X, F_initial_positions[i].Y] = _panel_type;
                word_layout_array[_panel_type - 1].Chr_Pos_List.Add(F_initial_positions[i]);
                _panel_count--;
            }
        }
    }

    //パネルが置けるかチェック関数
    bool CheckPlacePanel(int _panel_count, int _panel_type,  C_PosStock _start_pos)
    {
        //配置済みパネル用
        Stack<C_PosStock> F_placed_panels = new Stack<C_PosStock>();

        //開始位置にパネルを配置し、状態を更新
        panel_chr_layout[_start_pos.X, _start_pos.Y] = _panel_type;
        word_layout_array[_panel_type - 1].Chr_Pos_List.Clear();
        word_layout_array[_panel_type - 1].Chr_Pos_List.Add(_start_pos);
        F_placed_panels.Push(_start_pos);

        for (int count = 1; count < _panel_count; count++)
        {
            //現在位置の隣接可能な位置を取得
            C_PosStock F_current_pos = word_layout_array[_panel_type - 1].Chr_Pos_List[word_layout_array[_panel_type - 1].Chr_Pos_List.Count - 1];
            List<C_PosStock> F_neighbors = GetValidNeighbors(F_current_pos.X, F_current_pos.Y);

            //隣接可能な位置がない場合は配置失敗として処理を元に戻す
            if (F_neighbors.Count == 0)
            {
                //失敗した場合は配置済みのパネルを元に戻し再試行
                while (F_placed_panels.Count > 0)
                {
                    C_PosStock posToRemove = F_placed_panels.Pop();
                    panel_chr_layout[posToRemove.X, posToRemove.Y] = -1;
                    word_layout_array[_panel_type - 1].Chr_Pos_List.Remove(posToRemove);
                }
                return false;
            }

            //隣接可能な位置からランダムに1つ選択して配置
            C_PosStock F_selected_neighbor = F_neighbors[Random.Range(0, F_neighbors.Count)];
            panel_chr_layout[F_selected_neighbor.X, F_selected_neighbor.Y] = _panel_type;
            word_layout_array[_panel_type - 1].Chr_Pos_List.Add(F_selected_neighbor);
            F_placed_panels.Push(F_selected_neighbor);
        }

        return true;
    }



    List<C_PosStock> GetValidNeighbors(int _x, int _y)
    {
        List<C_PosStock> F_neighbors = new List<C_PosStock>();

        if (_x > 0 && panel_chr_layout[_x - 1, _y] == -1)
        {
            F_neighbors.Add(new C_PosStock(_x - 1, _y));
        }

        if (_x < panel_inf[stage_index].Length - 1 && panel_chr_layout[_x + 1, _y] == -1)
        {
            F_neighbors.Add(new C_PosStock(_x + 1, _y));
        }

        if (_y > 0 && panel_chr_layout[_x, _y - 1] == -1)
        {
            F_neighbors.Add(new C_PosStock(_x,_y - 1));
        }

        if (_y < panel_inf[stage_index].Side - 1 && panel_chr_layout[_x, _y + 1] == -1)
        {
            F_neighbors.Add(new C_PosStock(_x, _y + 1));
        }

        return F_neighbors;
    }

    //先頭の位置をばらばらにするList
    void MixList<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            int F_random_index = Random.Range(0, _list.Count);
            T F_temp = _list[i];
            _list[i] = _list[F_random_index];
            _list[F_random_index] = F_temp;
        }
    }

    //パネルの設置関数
    void PlacePanels()
    {
        //各パネルリストの数に応じて、初期位置を設定
        SetInitialPositions(panel_manager_s.Problem_List.Count, 1);

        //パネルグリッドの全ての位置をチェックし-1であれば0に設定
        for (int x = 0; x < panel_inf[stage_index].Length; x++)
        {
            for (int y = 0;y < panel_inf[stage_index].Side; y++)
            {
                if (panel_chr_layout[x, y] == -1)
                {
                    panel_chr_layout[x, y] = 0;
                }
            }
        }

        //各WordLayoutに対応するパネルにカタカナを割り当てる
        foreach (var F_wordLayout in word_layout_array)
        {
            AssignWord(F_wordLayout);
        }

        //不正解のパネル文字の設定
        SetWrongWord();

        //パネルの番号をpanel_manager_sのCorn_Numに設定
        for (int x = 0; x < panel_inf[stage_index].Length; x++)
        {
            for (int y = 0;y < panel_inf[stage_index].Side; y++)
            {
                panel_manager_s F_PANEL = panel_grid[x, y];
                F_PANEL.Corn_Num = panel_chr_layout[x, y];
            }
        }
    }

    //単語を割り当てる関数
    void AssignWord(C_WordLayout _wordLayout)
    {
        int F_panel_type = _wordLayout.Panel_Type;

        //指定されたWordLayoutに基づき、パネルにカタカナを割り当てる
        for (int i = 0; i < _wordLayout.Chr_Pos_List.Count; i++)
        {
            int F_x = _wordLayout.Chr_Pos_List[i].X;
            int F_y = _wordLayout.Chr_Pos_List[i].Y;

            panel_manager_s F_panel = panel_grid[F_x, F_y];

            switch (F_panel_type)
            {
                case 1:
                    if (i < panel_manager_s.Problem_List.Count)
                    { 
                        F_panel.Word_Index = panel_manager_s.Problem_List[i];
                        F_panel.RefreshWord();
                    }
                    break;
            }
        }

        //最後に、numberList0からnumberList1,2,3に含まれるすべての番号を削除
        List<int> numbersToRemove = new List<int>();
        numbersToRemove.AddRange(panel_manager_s.Problem_List);

        //削除リストに含まれる要素をnumberList0から一括で削除
        Anser_List.RemoveAll(x => numbersToRemove.Contains(x));
    }

    //不正解の文字を割り当てる関数
    void SetWrongWord()
    {
        for(int x = 0; x < panel_inf[stage_index].Length;x++)
        {
            for(int y =0;y < panel_inf[stage_index].Side;y++)
            {
                //正解じゃない場所
                if (panel_chr_layout[x,y] == 0)
                {
                    panel_manager_s F_panel = panel_grid[x, y];

                    if (Anser_List.Count > 0)
                    {
                        int F_rand = Random.Range(0, Anser_List.Count);

                        F_panel.Word_Index = Anser_List[F_rand];

                        F_panel.RefreshWord();
                    }
                }
            }
        }
    }

    //変数の初期化
    public void InitializeVariable()
    {
        //パネルの削除
        foreach(Transform F_child in parent.transform)
        {
            Destroy(F_child.gameObject);
        }

        //リストの初期化
        Anser_List.Clear();
        Problem_List.Clear();
        panel_manager_s.Problem_List.Clear();
        panel_manager_s.Word_List.Clear();
        panel_manager_s.Word_Color.Clear();
        panel_manager_s.Correct_Word.Clear();

        //カウント
        Count = 0;
        layout_initialized = false;
        game_over = false;

        game_manager_s.Instance.Game_Over = false;
        panel_manager_s.Problem_Count = 1;

        //再度読み込み
        OnEnable();
    }
}