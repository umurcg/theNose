using UnityEngine;
using System.Collections;

public class LerpThroughObjects : MonoBehaviour {

    public float speed = 1f;
    public float curvutare = 3f;
    public GameObject[] Aims;
    public float tolerance = 0.05f;
    Vector3 initialPosition;
    Vector3 aimedPosition;
    int index = -1;
    float ratio;
	// Use this for initialization

        //To Do Add curvutare

	void Start () {
        findNextAim();

	}
	
	// Update is called once per frame
	void Update () {
     

        if (Vector3.Distance(transform.position, aimedPosition) < tolerance)
        {
            transform.position = aimedPosition;
            findNextAim();
        }

        ratio +=Time.deltaTime * speed;
        transform.position = Vector3.Lerp(initialPosition, aimedPosition, ratio);
     


        //if (curvutare != 0)
        //{
        //    if (ratio < 0.5f)
        //    {
        //        transform.position = new Vector3(transform.position.x, initialPosition.y + Mathf.Sin(Mathf.Lerp(0, Mathf.PI / 2, ratio * 2)) * middleHeight, transform.position.z);

        //    }
        //    else
        //    {
        //        transform.position = new Vector3(transform.position.x, initialPosition.y + Mathf.Sin(Mathf.Lerp(0, Mathf.PI / 2, 2 - 2 * ratio)) * middleHeight, transform.position.z);
        //    }
        //}
    }


    void findNextAim()
    {
        if (index + 1 < Aims.Length)
        {
            index++;
            aimedPosition = Aims[index].transform.position;
            initialPosition = transform.position;
            ratio = 0;
        }else
        {
            this.enabled = false;
        }
    }

}
