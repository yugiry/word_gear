using System.Collections.Generic;
using UnityEngine;

public class BGM_manager_s : MonoBehaviour
{
    public static BGM_manager_s Instance;

    public  enum SCENE_TYPE
    {
        NONE,
        TITLE,
        STAGE_SELECT,
        SITUATION,
        GAME_1,
        GAME_2,
        GAME_3,
        GAME_CLEAR,
        GAME_OVER
    }

   [SerializeField] private SCENE_TYPE scene_type;

    private AudioSource AS;

    [SerializeField] private List<AudioClip> AC = new List<AudioClip>();

    private void Awake()
    {
       if(Instance == null)
       {
            Instance = this;
            DontDestroyOnLoad(gameObject);
       }
       else
       {
            Destroy(gameObject);
       }

        AS = GetComponent<AudioSource>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    //Ç«ÇÃèÍñ Ç©éÛÇØéÊÇÈ
    public void PlayBGM(SCENE_TYPE _type)
    {
        scene_type = _type;
        ChangeBGM(AC[(int)_type]);
    }

    //BGMÇÃïœçXä÷êî
    private void ChangeBGM(AudioClip _ac)
    {
        if (AS.clip == _ac)
            return;

        AS.Stop();

        AS.clip = _ac;

        AS.loop = true;

        AS.Play();
    }
}
