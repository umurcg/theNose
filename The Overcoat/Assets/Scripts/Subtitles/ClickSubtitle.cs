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
		gameObject.GetComponent<SubtitleController>().startSubtitle();
		if (ifDesroyItself)
			Destroy(this);

		}
		}
