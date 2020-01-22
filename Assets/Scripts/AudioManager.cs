using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;
    public static AudioManager instance;
    int soundID = 0;
    public float[] volume;
    bool muteSFX_On;
    bool muteMusic_On;
    int muteID_SFX;
    int muteID_Music;

    int randomStart = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    void Start()
    {
        
        Play("Theme");
        muteID_SFX = PlayerPrefs.GetInt("MuteOrNot_SFX", 0);
        if (muteID_SFX == 1)
        {
            for (int i = 1; i <= 8; i++)
            {

                volume[i] = sounds[i].source.volume;

                sounds[i].source.volume = 0;
            }
            muteSFX_On = true;
            
        }
        muteID_Music = PlayerPrefs.GetInt("MuteOrNot_Music", 0);
        if (muteID_Music == 1)
        {
            for (int i = 9; i < 12; i++)
            {

                volume[i] = sounds[i].source.volume;

                sounds[i].source.volume = 0;
            }
            muteMusic_On = true;
        }
    }
    // Update is called once per frame
   public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
    public void PlayTheme2()
    {

        randomStart = UnityEngine.Random.Range(0, 3);


    }
    public void MuteSFX()
    {
        if (muteSFX_On == false)
        {
            for (int i = 1; i <= 8; i++)
            {

                volume[i] = sounds[i].source.volume;

                sounds[i].source.volume = 0;
            }
            muteSFX_On = true;
            muteID_SFX = 1;
            PlayerPrefs.SetInt("MuteOrNot_SFX", muteID_SFX);
        }
        else if (muteSFX_On == true)
        {
          
            for (int i = 1; i <= 8; i++)
            {
                sounds[i].source.volume = volume[i];
                soundID++;
            }
            muteSFX_On = false;
            muteID_SFX = 0;
            PlayerPrefs.SetInt("MuteOrNot_SFX", muteID_SFX);
        }

    }
    public void MuteMusic()
    {
        if (muteMusic_On == false)
        {
            for (int i = 9; i < 12; i++)
            {

                volume[i] = sounds[i].source.volume;

                sounds[i].source.volume = 0;
            }
            muteMusic_On = true;
            muteID_Music = 1;
            PlayerPrefs.SetInt("MuteOrNot_Music", muteID_Music);
        }
        else if (muteMusic_On == true)
        {

            for (int i = 9; i < 12; i++)
            {
                sounds[i].source.volume = volume[i];
                soundID++;
            }
            muteMusic_On = false;
            muteID_Music = 0;
            PlayerPrefs.SetInt("MuteOrNot_Music", muteID_Music);
        }
    }
}
