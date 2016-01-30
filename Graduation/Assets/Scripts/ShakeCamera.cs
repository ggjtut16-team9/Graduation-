using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour {

    Quaternion qq = Quaternion.Euler(0, 5, 0);
    Quaternion q = Quaternion.identity;

    float t = 0;

	// Use this for initialization
	void Start () {
        q = transform.localRotation;
    }
	
	// Update is called once per frame
	void Update () {

        t += Time.deltaTime * 0.2f;

        transform.localRotation = Quaternion.Lerp(q,qq,t);
        if( t > 1)
        {
            t = 0;
            q = transform.localRotation;
            qq.eulerAngles *= -1;
        }
        transform.Rotate(Vector3.up,(Mathf.Sin(Time.frameCount *(0.001f)) * 0.1f));

    }
}
