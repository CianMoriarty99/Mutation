using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        foreach(Sound s in sounds)
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
        Play("Music1");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
            return;
        s.source.Play();
    }



    public IEnumerator StartFade(string sound, float duration)
    {

        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            yield return null;
        }

        float currentTime = 0;
        float start = s.source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
