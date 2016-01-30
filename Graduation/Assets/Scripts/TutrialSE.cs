using UnityEngine;
using System.Collections;

public class TutrialSE : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.SEPlay(AudioList.tutrial);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
