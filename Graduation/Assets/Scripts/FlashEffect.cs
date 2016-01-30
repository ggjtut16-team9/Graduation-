using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Color c = Color.black;
        c.a = Mathf.Sin(Time.frameCount * (0.08f)) * 0.5f;
        GetComponent<Text>().color = c;
    }
}
