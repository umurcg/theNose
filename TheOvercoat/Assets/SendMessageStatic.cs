using UnityEngine;
using System.Collections;

public class SendMessageStatic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void save()
    {
        GlobalController.Instance.SaveData();
    }

    public void load()
    {
        GlobalController.Instance.LoadData();
    }

}
