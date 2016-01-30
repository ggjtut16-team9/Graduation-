using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private enum Step
    {
        STEP_INIT,
        STEP_TEACH,
        STEP_MAIN,
        STEP_DECISION,
        STEP_EXIT,
    }

    private enum Result
    {
        RESULT_NONE,
        RESULT_OK,
        RESULT_ERROR,
    }

    private Step m_mainStep;
    private Result m_result;
    private int m_progress;
    private Julius_Client julius;

    private float m_timer;
    public float m_waitTime = 10.0f;

    public AudioClip[] m_audioClips;
    public AudioSource m_audioSource;

    public UnityEngine.UI.Text resultText;

    // Use this for initialization
    void Start ()
    {
        m_mainStep = Step.STEP_INIT;
        m_result = Result.RESULT_NONE;
        m_progress = 0;
        julius = GameObject.Find("Julius_client").GetComponent<Julius_Client>();

        m_timer = 0.0f;
        

    }
	
	// Update is called once per frame
	void Update () {
	    switch(m_mainStep)
        {
            case Step.STEP_INIT:
                {
                    if(loadAudio())
                    {
                        m_mainStep = Step.STEP_TEACH;
                    }
                }
                break;
            case Step.STEP_TEACH:
                {
                    if(teach())
                    {
                        m_mainStep = Step.STEP_MAIN;
                    }
                }
                break;
            case Step.STEP_MAIN:
                {
                    if(timeup(Time.deltaTime)) //10sec
                    {
                        m_mainStep = Step.STEP_INIT; //repeat once
                    }
                    m_result = speech();
                    if (m_result == Result.RESULT_OK ||
                        m_result == Result.RESULT_ERROR )
                    {
                        m_mainStep = Step.STEP_DECISION;
                        if(m_result == Result.RESULT_OK)
                        {
                            showMaru();
                        }
                        else if(m_result == Result.RESULT_ERROR)
                        {
                            showBatu();
                        }
                    }
                }
                break;
            case Step.STEP_DECISION:
                {
                    if(showDecision())
                    {
                        if (m_result == Result.RESULT_OK)
                        {
                            m_mainStep = Step.STEP_EXIT;
                        }
                        else if (m_result == Result.RESULT_ERROR)
                        { 
                            m_progress++;
                            if(m_progress == m_audioClips.Length)
                            {
                                m_mainStep = Step.STEP_INIT;
                            }
                            
                        }
                        
                    }
                }
                break;
            case Step.STEP_EXIT:
                {
                    resultText.text = "Clear!";
                }
                break;
            default:
                {

                }
                break;
        }
	}

    bool loadAudio()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_audioClips[m_progress];
        m_audioSource.Play();
        return true;
    }

    bool teach()
    {
        if(m_audioSource.isPlaying)
        {
            return false;
        }
        return true;
    }

    bool timeup(float dt)
    {
        m_timer += dt;
        if(m_timer > m_waitTime)
        {
            m_timer = 0.0f;
            return true;
        }
        return false;
    }

    Result speech()
    {
        if(!julius.Result.Equals(""))
        {
            if(string.Compare("こんにちは", julius.Result) == 0)
            {
                return Result.RESULT_OK;
            }
            return Result.RESULT_ERROR;
        }
        return Result.RESULT_NONE;
    }

    void showMaru()
    {
        resultText.text = "ok";
    }

    void showBatu()
    {
        resultText.text = "reject";
    }

    bool showDecision()
    {
        return true;
    }
}