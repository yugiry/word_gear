using UnityEngine;
[DefaultExecutionOrder(-1)]
public class BGM_Playback_A : MonoBehaviour
{
    public AudioSource BGM_Source;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGM_Source = GetComponent<AudioSource>();
        BGM_Source.loop = true;
    }

    public void PlayBGM(AudioClip _bgm_clip)
    {
        BGM_Source.clip = _bgm_clip;
        BGM_Source.Play();
    }

    public void StopBGM()
    {
        BGM_Source.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
