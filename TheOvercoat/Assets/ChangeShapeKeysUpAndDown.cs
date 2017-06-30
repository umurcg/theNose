using UnityEngine;
using System.Collections;

public class ChangeShapeKeysUpAndDown : MonoBehaviour {

    SkinnedMeshRenderer smr;
    float blend = 0;
    public float speed = 200;

    bool increase = true;

    // Use this for initialization
    void Start()
    {
        smr = GetComponent<SkinnedMeshRenderer>();
        smr.SetBlendShapeWeight(0, blend);
    }

    // Update is called once per frame
    void Update () {
        if (increase)
        {
            blend += Time.deltaTime * speed;
            if (blend > 100)
            {
                increase = false;

            }
        }else
        {
            blend -= Time.deltaTime * speed;
            if (blend <= 0)
            {
                increase = true;
            }
        }
        smr.SetBlendShapeWeight(0, blend);
    }


    //void changeBlendKey()
    //{
    //    if (blend < 100)
    //    {
    //        blend += Input.GetAxis("Mouse X") * Time.deltaTime * speed;
    //    }
    //    else
    //    {
    //        blend = 100;
    //    }

    //    if (blend > 0)
    //    {
    //        blend -= Input.GetAxis("Mouse Y") * Time.deltaTime * speed;
    //    }
    //    else
    //    {
    //        blend = 0;
    //    }
      
    //}
}
