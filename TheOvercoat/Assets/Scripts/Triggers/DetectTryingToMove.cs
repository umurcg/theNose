using UnityEngine;
using System.Collections;

public class DetectTryingToMove : MonoBehaviour  {


    //This script detects movement trial of player.
    //If it detects then calls trying method from ITryingMove interface.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.Space))
        {

            ITryingTomove ittm = GetComponent<ITryingTomove>();
            ittm.trying();

        }

        if (Input.GetMouseButtonDown(0))
        {



            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
  
                if (hit.transform.CompareTag("Floor"))
                {

                    ITryingTomove ittm = GetComponent<ITryingTomove>();
                    ittm.trying();

                }
                //       Instantiate(prefab, hit.transform.position, Quaternion.identity);
            }

        }




    }






}
