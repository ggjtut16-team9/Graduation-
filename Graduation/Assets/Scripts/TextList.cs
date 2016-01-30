using UnityEngine;
using System.Collections;

public class TextList : MonoBehaviour {

    public string[] m_Serif;
    public string[] m_Shout;

    public string GetSerif(int i)
    {
        return m_Serif[i];
    }


    public string GetShout(int i)
    {
        return m_Shout[i];
    }
}
