using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

    public string m_NextScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel(m_NextScene);
        }
	
	}
}
