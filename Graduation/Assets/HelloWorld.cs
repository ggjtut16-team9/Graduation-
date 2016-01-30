using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelloWorld : MonoBehaviour {
    public UnityEngine.UI.Text text;
    public Julius_Client julius_client;

	// Use this for initialization
	void Start () {
        text.text = "Hello World!";
        julius_client = gameObject.GetComponent<Julius_Client>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!julius_client.Result.Equals(""))
        {
            string tmp = julius_client.Result;
            text.text = tmp;
        }
	}
}
