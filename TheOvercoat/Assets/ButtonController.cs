using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//This script handles input axis as getbuttonkeyup functionality. Use it for action buttons like jumpi interation or something like that.
public class ButtonController : MonoBehaviour {

    //Signleton
    public static ButtonController bc;


    private void OnEnable()
    {
        if (bc != null)
        {
            Destroy(this);

        }
        else
        {
            bc = this;
        }
    }

    private void OnDisable()
    {
        bc = null;
    }

    public string[] axes;
    Dictionary<string, buttonBooleans> buttonBoolDict;


    struct buttonBooleans{
        public bool status;
        public bool avaible;

        public buttonBooleans(bool s, bool b)
        {
            status = s;
            avaible = b;
        }

    }

	// Use this for initialization
	void Start () {
        buttonBoolDict = new Dictionary<string, buttonBooleans>();

        foreach(string axis in axes)
        {
            buttonBooleans bb = new buttonBooleans();
            bb.status = false;
            bb.avaible = false;
            buttonBoolDict.Add(axis, bb);
        }

	}
	
	// Update is called once per frame
	void Update () {


        foreach (string ax in axes)
        {
            buttonBooleans bb = buttonBoolDict[ax];


            if (bb.avaible)
            {
                if (Input.GetAxis(ax) == 1)
                {

                    if (bb.status == false)
                    {
                        bb.status = true;
                        bb.avaible = false;
                    }
                    else
                    {
                        bb.status = false;
                    }
                }
            }
            else
            {
                if (Input.GetAxis(ax) == 0)
                {
                    bb.avaible = true;
                    
                }

             }

            buttonBoolDict[ax] = bb;

        }



    }

    public bool getKeyDown(string axis)
    {
        if (buttonBoolDict.ContainsKey(axis)) return buttonBoolDict[axis].status;

        return false;
        
    }


}
