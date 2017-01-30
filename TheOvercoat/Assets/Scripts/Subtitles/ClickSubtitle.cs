using UnityEngine;
using System.Collections;

public class ClickSubtitle : MonoBehaviour, IClickAction {
	public bool ifDesroyItself = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Action(){     
		if (gameObject.GetComponent<SubtitleController>() != null)
		{
		gameObject.GetComponent<SubtitleController>().startSubtitle();
		if (ifDesroyItself)
			Destroy(this);

		}
		}
}