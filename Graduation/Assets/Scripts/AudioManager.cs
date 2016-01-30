using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    private static AudioManager sInstance;

    public static AudioManager Instance
    {
        get
        {
            if (sInstance == null)
                sInstance = GameObject.FindObjectOfType<AudioManager>();
            return sInstance;
        }
    }

    private AudioList CurrentAudio;

    [SerializeField]
    private AudioSource m_AudioSE;
    [SerializeField]
    private AudioSource m_AudioBGM;

    public AudioClip[] AudioClipsSE;
    public AudioClip[] AudioClipsBGM;

    public Dictionary<AudioList, AudioClip> m_AudioSEClips = new Dictionary<AudioList, AudioClip>();
    public Dictionary<AudioList, AudioClip> m_AudioBGMClips = new Dictionary<AudioList, AudioClip>();

    public void Awake()
    {
        int i = 0;
        foreach (AudioClip AC in AudioClipsSE)
        {
            m_AudioSEClips.Add((AudioList)i, AC);

            i++;
        }

        //foreach (AudioClip AC in AudioClipsBGM)
        //{
        //    m_AudioBGMClips.Add((AudioList)i, AC);

        //    i++;
        //}
    }

    public void BGMPlay(AudioList name)
    {
        if (CurrentAudio == name) return;
        if (m_AudioBGM.isPlaying)
        {
            BGMStop();
        }
        CurrentAudio = name;
        m_AudioBGM.clip = m_AudioBGMClips[name];
        m_AudioBGM.Play();
    }

    public void BGMStop()
    {
        m_AudioBGM.Stop();
    }

    public void SEPlay(AudioList name)
    {
        m_AudioSE.PlayOneShot(m_AudioSEClips[name]);
    }

}
