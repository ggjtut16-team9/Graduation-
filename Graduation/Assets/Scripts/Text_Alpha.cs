using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Text_Alpha : MonoBehaviour {

    Text text;
    Color _color;
    float textbox_fillamount;    

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        _color = text.color;
	}
	
	// Update is called once per frame
	void Update () {
        textbox_fillamount = GameObject.Find("Canvas/Image").GetComponent<Image>().fillAmount;
        text.color = new Color(_color.r, _color.g, _color.b, textbox_fillamount);//吹き出しのfillamount数値に合わせてテキストのアルファ値を操作
	}
}
