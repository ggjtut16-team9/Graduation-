using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

    public string m_NextScene;

	// Use this for initialization
	void Start () {
        AudioManager.Instance.BGMPlay(AudioList.friend);
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.BGMStop();
            Application.LoadLevel(m_NextScene);
        }
	
	}
}
