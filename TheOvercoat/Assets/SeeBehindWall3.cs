﻿using UnityEngine;
using System.Collections;

//Change player material to materialk that can be seen through walls when tag object is between player an camera;
public class SeeBehindWall3 : MonoBehaviour {

    public Material SeeThrougWallsMat;
    public string[] tags;
    public LayerMask ignoreRaycast;
    bool canSeeThroughWalls;
    Camera cam;
    Material originalMat;
    Renderer rend;
    public GameObject player;


	// Use this for initialization
	void Awake () {
        cam = GetComponent<Camera>();


	}

    private void Start()
    {
        player = CharGameController.getActiveCharacter();

        for (int i = 0; i < player.transform.childCount; i++)
        {
            rend = player.transform.GetChild(i).GetComponent<Renderer>();
        }

        originalMat = rend.material;
    }

    // Update is called once per frame
    void FixedUpdate () {

        //Debug.Log(player.name);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, player.transform.position-transform.position);

        if (Physics.Raycast(ray,out hit)) { 
        
            //Debug.Log(hit.transform.tag);
            if (System.Array.IndexOf(tags,hit.transform.tag)==-1 && canSeeThroughWalls)
            {
                //Recover material
                rend.material = originalMat;
                canSeeThroughWalls = false;
                //Debug.Log("Original Mat");
            }else if(System.Array.IndexOf(tags, hit.transform.tag) != -1 && !canSeeThroughWalls)
            {
                //Assign material
                rend.material = SeeThrougWallsMat;
                randomizeMaterialColor(SeeThrougWallsMat);
                canSeeThroughWalls = true;
                //Debug.Log("You can see");
            }
        }

	
	}

    public void updateTarget()
    {
        player = CharGameController.getActiveCharacter();

    }

    void randomizeMaterialColor(Material mat)
    {
        Color random = Random.ColorHSV();
        random.a = 1;

        mat.SetColor("_OutlineColor", random);
    }
}