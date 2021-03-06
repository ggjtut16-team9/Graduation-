﻿using UnityEngine;
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
    public int m_progress { get; set; }
    private Julius_Client julius;

    private float m_timer;
    public float m_waitTime = 10.0f;
    public float m_questionTime = 0.2f;
    public float m_answerTime = 2.5f;
    public float m_juliusDisableTime = 1.0f;

    public AudioClip[] m_audioClips;
    public AudioClip m_audioClipThanks;
    public AudioClip m_audioClipThanks2;
    private bool m_thanks1Played = false;
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

    public UnityEngine.UI.Image m_imageOK;
    public UnityEngine.UI.Image m_imageNG;

    public ParticleSystem m_particleOK;
    public ParticleSystem m_particleNG;

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

        m_particleOK.Stop();
        m_particleNG.Stop();

    }
	
	// Update is called once per frame
	void Update () {
	    switch(m_mainStep)
        {
            case Step.STEP_INIT:
                {
                    m_timer = 0.0f;
                    m_textAnswer.enabled = false;
                    m_textQuestion.enabled = false;
                    m_textShout.enabled = false;
                    m_imageAnswer.enabled = false;
                    m_imageAnswerDup.enabled = false;
                    m_imageAnswerBack.enabled = false;
                    m_imageQuestion.enabled = false;
                    m_imageShout.enabled = false;
                    m_imageOK.enabled = false;
                    m_imageNG.enabled = false;
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
                    if (teach(Time.deltaTime))
                    {
                        m_imageAnswerBack.enabled = true;
                        m_imageAnswerDup.enabled = true;
                        m_imageAnswer.enabled = true;
                        m_textShout.enabled = true;
                        m_imageShout.enabled = true;

                        string path = string.Format("answer{0:D2}", m_progress + 1);
                        //Resourcesのパス
                        Sprite[] allSprites = Resources.LoadAll<Sprite>("Images");
                        //ロードしたスプライトを名前で検索
                        Sprite sp = System.Array.Find<Sprite>(allSprites
                        , (sprite) => sprite.name.Equals(path));
                        m_imageAnswerDup.sprite = sp;
                        m_imageAnswer.sprite = sp;

                        //音声認識をクリア(この時点で即時認識を阻止)
                        julius.Result = "";

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
                        m_timer = 0.0f;
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
                                m_audioSource.clip = m_audioClipThanks;
                                m_audioSource.Play();
                                m_textShout.enabled = false;
                                m_imageShout.enabled = false;
                                m_imageAnswer.enabled = false;
                                m_imageAnswerDup.enabled = false;
                                m_textAnswer.enabled = true;
                                m_textQuestion.text = "ここまでプレイしてもらって";
                                m_textAnswer.text = "ありがとうございました！\nクリックで終了";

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
                    if(speechThanks())
                    {
                        Application.LoadLevel("title");
                    }
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

    bool teach(float dt)
    {
        m_timer += dt;

        if (m_timer > m_questionTime)
        {
            m_timer = m_questionTime;
        }
        m_imageQuestion.fillAmount = (m_timer / m_questionTime);
        if (m_audioSource.isPlaying)
        {
            return false;
        }
        m_timer = 0.0f;
        m_imageQuestion.fillAmount = 1.0f;
        return true;
    }

    bool timeup(float dt)
    {
        m_timer += dt;
        //1秒以内は流石に阻止
        if(m_timer < m_juliusDisableTime)
        {
            julius.Result = "";
        }
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
        m_audioSource.PlayOneShot(Resources.Load("Audios/good") as AudioClip);
        m_particleOK.Play();
        m_imageOK.enabled = true;
        m_textAnswer.enabled = true;
        m_imageAnswer.enabled = false;
        m_imageAnswerDup.enabled = false;
        m_textAnswer.text = julius.Result;
    }

    void showBatu()
    {
        m_audioSource.PlayOneShot(Resources.Load("Audios/bad") as AudioClip);
        m_particleNG.Play();
        m_imageNG.enabled = true;
        m_textAnswer.enabled = true;
        m_imageAnswer.enabled = false;
        m_imageAnswerDup.enabled = false;
        m_textAnswer.text = julius.Result;
    }

    bool showDecision(float dt)
    {
        m_timer += dt;
        if (m_timer > m_answerTime)
        {
            m_timer = 0.0f;
            m_textAnswer.enabled = false;
            m_imageOK.enabled = false;
            m_imageOK.enabled = false;
            m_particleOK.Stop();
            m_particleNG.Stop();
            return true;
        }
        return false;
    }


    bool speechThanks()
    {
        if(m_audioSource.isPlaying)
        {
            return false;
        }
        if (m_thanks1Played == false)
        {
            m_thanks1Played = true;
            m_audioSource.clip = m_audioClipThanks2;
            m_audioSource.Play();
            return false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;

    }
}