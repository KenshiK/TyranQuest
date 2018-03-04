using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	public string name;

	[TextArea(3, 10)]
	public string[] sentences;

    public Dialogue()
    {

    }

    public Dialogue(string[] t)
    {
        sentences = t;
        name = "";
    }

    public Dialogue(string n, string[] t)
    {
        name = n;
        sentences = t;
    }

}
