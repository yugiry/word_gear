using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class panel_manager_s : MonoBehaviour
{

    private enum WORD_TYPE
    {
        HIRAGANA,
        KATAKANA,
        ALPHABET
    }

    [SerializeField] private GameObject word_gameobject = null;
    [SerializeField] private Text word_display;
    [SerializeField] private Image panel_image;
    [SerializeField] private Sprite correct_sprite;
    [SerializeField] private Sprite incorrect_sprite;

    private WORD_TYPE word_type;
    [HideInInspector] public int Word_Index;
    public static List<int> Word_List = new List<int>();
    public static List<int> Problem_List = new List<int>();
    public static List<int> Anser_List = new List<int>();
    public static List<Text>  Word_Color = new List<Text>();
    public static List<panel_manager_s> Correct_Word = new List<panel_manager_s>();
    [HideInInspector] public int Number;
    [HideInInspector] public int[] Panel_Num;
    [HideInInspector] public int Corn_Num;
    private int word_num;
    public static int Problem_Count = 1;
    public bool Touch = true;
    public static bool Initialized = false;

    //Dictionary-----------------------------------
    private static Dictionary<int, string> hiragana;
    private static Dictionary<int, string> katakana;
    private static Dictionary<int, string> alphabet;
    public static Dictionary<char, int> Word_To_Index = new Dictionary<char, int>();


    //ゲームクリア関連
    public static bool Game_Clear = false;

    //指定された値に対応するリストを取得するメソッド
    public static List<int> GetProblemList(int F_value)
    {
        switch (F_value)
        {
            case 1:
                return Problem_List;
            default:
                return new List<int>();
        }
    }

  

    private void Awake()
    {
        InitDictionary();
        word_type = WORD_TYPE.KATAKANA;
        
    }


    private void Start()
    {

        Number = answer_manager_s.Instance.Count;
        Panel_Num = answer_manager_s.Instance.Panel_Box;
        Debug.Log(name + " => " + word_display.name);
    }

    private void OnEnable()
    {
        

        //if (game_data_s.Instance.Answer_List.Count > 0)
        //{
        //    //numberList0からランダムに値を取得して設定
        //    int F_randomIndex = Random.Range(0, game_data_s.Instance.Answer_List.Count);
        //    this.Word_Index = game_data_s.Instance.Answer_List[F_randomIndex];
        //    game_data_s.Instance.Answer_List.RemoveAt(F_randomIndex);

        //    SetWord();
        //}
        
    }

    private void OnDisable()
    {

        Word_List.Clear();
        Word_Color.Clear();
        Correct_Word.Clear();

       Problem_Count = 1;

        word_num = 0;

        if (this.Touch)
        {
            this.Touch = false;
        }
    }

    //Dictionaryの設定関数
    static void SettingDictionary()
    {
        hiragana = new Dictionary<int, string>()
        {
            {0,"あ"},{1,"い"},{2,"う"},{3,"え"},{4,"お"},
            {5,"か"},{6,"き"},{7,"く"},{8,"け"},{9,"こ"},
            {10,"さ"},{11,"し"},{12,"す"},{13,"せ"},{14,"そ"},
            {15,"た"},{16,"ち"},{17,"つ"},{18,"て"},{19,"と"},
            {20,"な"},{21,"に"},{22,"ぬ"},{23,"ね"},{24,"の"},
            {25,"は"},{26,"ひ"},{27,"ふ"},{28,"へ"},{29,"ほ"},
            {30,"ま"},{31,"み"},{32,"む"},{33,"め"},{34,"も"},
            {35,"や"},{36,"ゆ"},{37,"よ"},
            {38,"ら"},{39,"り"},{40,"る"},{41,"れ"},{42,"ろ"},
            {43,"わ"},{44,"を"},{45,"ん"},

            {46,"が"},{47,"ぎ"},{48,"ぐ"},{49,"げ"},{50,"ご"},
            {51,"ざ"},{52,"じ"},{53,"ず"},{54,"ぜ"},{55,"ぞ"},
            {56,"だ"},{57,"ぢ"},{58,"づ"},{59,"で"},{60,"ど"},
            {61,"ば"},{62,"び"},{63,"ぶ"},{64,"べ"},{65,"ぼ"},
            {66,"ぱ"},{67,"ぴ"},{68,"ぷ"},{69,"ぺ"},{70,"ぽ"},
            {71,"ー" },
            {72,"ぁ"},{73,"ぃ"},{74,"ぅ"},{75,"ぇ"},{76,"ぉ"},
            {77,"ゃ"},{78,"ゅ"},{79,"ょ"},
            {80,"っ"},
            {81,"ゎ"}
        };

        katakana = new Dictionary<int, string>()
        {
            {0,"ア"},{1,"イ"},{2,"ウ"},{3,"エ"},{4,"オ"},
            {5,"カ"},{6,"キ"},{7,"ク"},{8,"ケ"},{9,"コ"},
            {10,"サ"},{11,"シ"},{12,"ス"},{13,"セ"},{14,"ソ"},
            {15,"タ"},{16,"チ"},{17,"ツ"},{18,"テ"},{19,"ト"},
            {20,"ナ"},{21,"ニ"},{22,"ヌ"},{23,"ネ"},{24,"ノ"},
            {25,"ハ"},{26,"ヒ"},{27,"フ"},{28,"ヘ"},{29,"ホ"},
            {30,"マ"},{31,"ミ"},{32,"ム"},{33,"メ"},{34,"モ"},
            {35,"ヤ"},{36,"ユ"},{37,"ヨ"},
            {38,"ラ"},{39,"リ"},{40,"ル"},{41,"レ"},{42,"ロ"},
            {43,"ワ"},{44,"ヲ"},{45,"ン"},

            {46,"ガ"},{47,"ギ"},{48,"グ"},{49,"ゲ"},{50,"ゴ"},
            {51,"ザ"},{52,"ジ"},{53,"ズ"},{54,"ゼ"},{55,"ゾ"},
            {56,"ダ"},{57,"ヂ"},{58,"ヅ"},{59,"デ"},{60,"ド"},
            {61,"バ"},{62,"ビ"},{63,"ブ"},{64,"ベ"},{65,"ボ"},
            {66,"パ"},{67,"ピ"},{68,"プ"},{69,"ペ"},{70,"ポ"},
            {71,"ー" },
            {72,"ァ"},{73,"ィ"},{74,"ゥ"},{75,"ェ"},{76,"ォ"},
            {77,"ャ"},{78,"ュ"},{79,"ョ"},
            {80,"ッ"},
            {81,"ヮ"},
        };

        alphabet = new Dictionary<int, string>()
        {
            {0,"a"},{1,"b"},{2,"c"},{3,"d"},{4,"e"},
            {5,"f"},{6,"g"},{7,"h"},{8,"i"},{9,"j"},
            {10,"k"},{11,"l"},{12,"m"},{13,"n"},{14,"o"},
            {15,"p"},{16,"q"},{17,"r"},{18,"s"},{19,"t"},
            {20,"u"},{21,"v"},{22,"w"},{23,"x"},{24,"y"},
            {25,"z"},

        };

        Word_To_Index.Clear();

        foreach (var pair in katakana)
        {
            char c = pair.Value[0];
            Word_To_Index[c] = pair.Key;
        }
    
    }



    //文字の設定関数
    public void SetWord()
    {
        string text = "";

        switch (word_type)
        {
            case WORD_TYPE.HIRAGANA:
                if (hiragana.TryGetValue(Word_Index, out text))
                    word_display.text = text;
                break;

            case WORD_TYPE.KATAKANA:
                if (katakana.TryGetValue(Word_Index, out text))
                    word_display.text = text;
                break;

            case WORD_TYPE.ALPHABET:
                if (alphabet.TryGetValue(Word_Index, out text))
                    word_display.text = text;
                break;
        }

    }


    // Update is called once per frame
    void Update()
    {
      
    }
       

    public void Paneltouch()
    {
        //正解済みにパネルを押すとリセット
        if (!this.Touch)
        {
            ResetAnswer();
            return;
        }

        if(game_manager_s.Instance.Game_Over)
        {
            return;
        }

        Word_List.Add(this.Word_Index);

        Word_Color.Add(this.word_display);

        int F_index = Word_List.Count - 1;

        //あっていたら文字の色を変更
        if (Word_List[F_index] == Problem_List[F_index])
        {
            panel_image.sprite = correct_sprite;
            Correct_Word.Add(this);

            this.Touch = false;
            word_num++;
        }
        else
        {
            //間違った場合、リストをクリアして色をリセット
            ResetAnswer();
            return;
        }

        //全てのパネルが正しい順番で押された場合の処理
        if (Word_List.Count >= Problem_List.Count)
        {
            StartCoroutine(ProblemCount());
            Game_Clear = true;
        }


    }

    //csvのロード関数
    public void LoadAnswerWord(string _answer_text)
    {
        Problem_List.Clear();

        foreach(char F_c in _answer_text)
        {
            if(Word_To_Index.ContainsKey(F_c))
            {
                int F_index = Word_To_Index[F_c];
                Problem_List.Add(F_index);
            }
        }
    }

    //不正解のパネルをタップしたとき
    public void ResetAnswer()
    {
        foreach(var F_panel in Correct_Word)
        {
            F_panel.Touch  = true;
            F_panel.panel_image.sprite = F_panel.incorrect_sprite;
        }

        Word_Color.Clear();
        Word_List.Clear();
        Correct_Word.Clear();

        word_num = 0;


    }

    private IEnumerator ProblemCount()
    {
    

        //_correctSE = true;

        //正解のパネルの_cantouchをfalseに設定
        foreach (var panel in Correct_Word)
        {
            panel.Touch = false;
        }

        Word_List.Clear();
        Word_Color.Clear();
        word_num = 0;

        yield return new WaitForSeconds(3.0f);

  

        if (Problem_Count == 3)
        {
            //正解のパネルの_cantouchをfalseに設定
            foreach (var panel in Correct_Word)
            {
                panel.Touch = true;
            }
        }

        Problem_Count++;

        //_problemSE = true;
    }

    //表示更新関数
    public void RefreshWord()
    {
        SetWord();
    }

    //Dictionaryの初期化
    public static void InitDictionary()
    {
        if (Initialized)
            return;

        SettingDictionary();

        Initialized = true;
    }

}
