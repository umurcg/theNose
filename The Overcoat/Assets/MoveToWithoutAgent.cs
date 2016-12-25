using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MoveToWithoutAgent : MonoBehaviour {

    public float speed = 3;
    IEnumerator<float> handler;
    GameObject floor;
	// Use this for initialization
	void Start () {
        floor= transform.GetChild(2).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Event e = Event.current;
  
        if (Input.anyKey)
        {

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                //print("keyboard");
                if (handler != null) 
                Timing.KillCoroutines(handler);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                //print("mouse");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))

                {
                    //print(hit.transform.name);
                    if (hit.transform.gameObject==floor)
                    {
                        Vector3 aim = hit.point;
                        aim = new Vector3(aim.x, transform.position.y, aim.z);
                        float dist = Vector3.Distance(aim, transform.position);
                        float time = speed/dist;
                        Timing.RunCoroutine(_lookAndGo(aim, time));
                        
                        
                        
                    }
                }

            }



        }
    }

    IEnumerator<float> _lookAndGo(Vector3 aim,float time)
    {
        if (handler != null) 
        Timing.KillCoroutines(handler);
        IEnumerator<float> localHandler = Timing.RunCoroutine(Vckrs._lookTo(gameObject, aim-transform.position, 2f));
        yield return Timing.WaitUntilDone(localHandler);
        handler = Timing.RunCoroutine(Vckrs._Tween(gameObject, aim, time));
    }

}
