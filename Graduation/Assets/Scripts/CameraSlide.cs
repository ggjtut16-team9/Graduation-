using UnityEngine;
using System.Collections;

public class CameraSlide : MonoBehaviour {

    public Vector3 m_SlidePower;
    public Camera camera1;
    public Camera camera2;

    private Vector3 defposition;

    float t = 0;

    // Use this for initialization
    void Start () {

        defposition = camera1.transform.position;
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        t += Time.deltaTime / 2;
        if (camera1.enabled == true)
        {
            camera1.transform.position  =Vector3.Lerp(defposition, defposition + m_SlidePower, t);
            if (t > 1)
            {
                t = 0;
                camera1.enabled = false;
                camera1.transform.position = defposition;
                camera2.enabled = true;
            }
        }
        else if (camera2 == true)
        {
            if(t > 3)
            {
                t = 0;
                camera1.enabled = true;
                camera2.enabled = false;
            }
        }

        
	}
}
