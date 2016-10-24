using UnityEngine;
using System.Collections;

//_KeySliderCompeletly
//_Dependent to: SkinnedMeshRenederer,

//This script increases blend key to 100 or decreases to 0.


public class KeySlideCompletely : MonoBehaviour, IClickAction {
	public float speed=1;
	SkinnedMeshRenderer smr;
	float key=0;
	bool increasing;
	bool decreasing;
	// Use this for initialization
	void Start () {
		smr = GetComponent<SkinnedMeshRenderer> ();
		key = smr.GetBlendShapeWeight (0);
	}
	
	// Update is called once per frame
	void Update () {
		if (increasing) {
			key += Time.deltaTime * speed;
			if (key >= 100) {
				increasing = false;
				key = 100;

			}
			smr.SetBlendShapeWeight (0, key);
		}
		else if (decreasing) {
			key -= Time.deltaTime * speed;
			if (key <= 0) {
				decreasing = false;
				key = 0;

			}
			smr.SetBlendShapeWeight (0, key);
		}

	}

	void Slide(){
		if (increasing) {
			increasing = false;
			decreasing=true;
		} else if (decreasing) {
			decreasing = false;
			increasing = true;
		}else if (key == 100) {
			decreasing = true;
		} else if (key == 0) {
			increasing = true;
		} else {
			increasing = true;
		}

	}

	public void Action(){
		Slide ();
	}
}
