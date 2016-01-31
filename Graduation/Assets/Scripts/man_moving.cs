using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class man_moving : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("Canvas/OK").GetComponent<Image>().enabled)
        {
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(-5, 6));
        }
	}
}
