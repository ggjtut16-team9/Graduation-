using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShoutSpriteMoving : MonoBehaviour {
    Text shout;
    [SerializeField]
    int fontsize_max = 125;
    [SerializeField]
    int fontsize_min = 75;

    int scaler = 1;//boolより計算に直結させるためint 
	// Use this for initialization
	void Start () {
        shout = GameObject.Find("Shout").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (shout.fontSize == fontsize_max) scaler = -1;
        if (shout.fontSize == fontsize_min) scaler = 1;

        shout.fontSize += scaler;
    }
}
