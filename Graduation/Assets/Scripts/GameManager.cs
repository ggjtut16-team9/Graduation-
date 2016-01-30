using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum Step
    {
        STEP_INIT,
        STEP_QUESTION,
        STEP_ANSWER,
        STEP_DECISION,
        STEP_EXIT,
    }

    public enum Result
    {
        RESULT_NONE,
        RESULT_OK,
        RESULT_ERROR,
    }

    private Step m_mainStep { get; set; }
    private Result m_result { get; set; }
    private int m_progress { get; set; }
    private Julius_Client julius;

    private float m_timer;
    public float m_waitTime = 10.0f;
    public float m_answerTime = 2.5f;

    public AudioClip[] m_audioClips;
    public AudioSource m_audioSource;

    public UnityEngine.UI.Text m_resultText;

    public UnityEngine.UI.Text m_textQuestion;
    public UnityEngine.UI.Text m_textAnswer;
    public UnityEngine.UI.Text m_textShout;

    public UnityEngine.UI.Image m_imageQuestion;
    public UnityEngine.UI.Image m_imageAnswerBack;
    public UnityEngine.UI.Image m_imageShout;
    public UnityEngine.UI.Image m_imageAnswer;
    public UnityEngine.UI.Image m_imageAnswerDup;

    public string[] m_questions = { "みんなで行った",
                                    "全力で競い合った",
                                    "仲間ができた",
                                    "気持ちが一つになった",
                                    "気持ちが触れ合った",
                                    "心もきれいにした"
                                  };

    public string[] m_answers = { "修学旅行",
                                  "運動会",
                                  "クラブ活動",
                                  "合唱会",
                                  "文化祭",
                                  "大掃除"}; //答えの日本語

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
                    m_textAnswer.enabled = false;
                    m_textQuestion.enabled = false;
                    m_textShout.enabled = false;
                    m_imageAnswer.enabled = false;
                    m_imageAnswerDup.enabled = false;
                    m_imageAnswerBack.enabled = false;
                    m_imageQuestion.enabled = false;
                    m_imageShout.enabled = false;
                    if(loadAudio())
                    {
                        m_textQuestion.enabled = true;
                        m_imageQuestion.enabled = true;

                        m_textQuestion.text = m_questions[m_progress];

                        m_mainStep = Step.STEP_QUESTION;
                    }
                }
                break;
            case Step.STEP_QUESTION:
                {
                    if (teach())
                    {
                        m_imageAnswerBack.enabled = true;
                        m_imageAnswer.enabled = true;
                        m_textShout.enabled = true;
                        m_imageShout.enabled = true;

                        string path = string.Format("Images/answer{0:D2}", m_progress + 1);
                        m_imageAnswerDup.overrideSprite = Resources.Load(path) as Sprite;
                        m_imageAnswer.overrideSprite = Resources.Load(path) as Sprite;

                        m_mainStep = Step.STEP_ANSWER;
                    }
                }
                break;
            case Step.STEP_ANSWER:
                {
                    if(timeup(Time.deltaTime)) //10sec
                    {
                        m_mainStep = Step.STEP_INIT; //repeat once
                    }
                    CullOverlay();
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
                    if(showDecision(Time.deltaTime))
                    {
                        if (m_result == Result.RESULT_OK)
                        {
                            m_progress++;
                            if(m_progress == m_answers.Length)
                            {
                                m_mainStep = Step.STEP_EXIT;
                            }
                            else
                            {
                                m_mainStep = Step.STEP_INIT;
                            }
                        }
                        else if (m_result == Result.RESULT_ERROR)
                        { 
                            
                            m_mainStep = Step.STEP_INIT;
                            
                            
                        }
                        m_result = Result.RESULT_NONE;
                        
                    }
                }
                break;
            case Step.STEP_EXIT:
                {
                    m_resultText.text = "Clear!";
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

    void CullOverlay()
    {
        float t = m_timer;
        if(m_timer > m_answerTime)
        {
            m_timer = m_answerTime;
        }
        
        m_imageAnswer.fillAmount = 1.0f - (m_timer / m_answerTime); 
    }

    Result speech()
    {
        if(!julius.Result.Equals(""))
        {
            if(string.Compare(m_answers[m_progress], julius.Result) == 0)
            {
                return Result.RESULT_OK;
            }
            return Result.RESULT_ERROR;
        }
        return Result.RESULT_NONE;
    }

    void showMaru()
    {
        m_textAnswer.enabled = true;
        m_textAnswer.text = julius.Result;
    }

    void showBatu()
    {
        m_textAnswer.enabled = true;
        m_textAnswer.text = julius.Result;
    }

    bool showDecision(float dt)
    {
        m_timer += dt;
        if (m_timer > m_waitTime)
        {
            m_timer = 0.0f;
            m_textAnswer.enabled = false;
            return true;
        }
        return false;
    }
}