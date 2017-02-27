using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MovementEffects;

//Raycast to for each aim objects from owner object
//If found object is specified layer it makes it transparent
//When mouse is over on those object also make them transparent


//TODO bug when two object is in targetList


public class SeeCharacterThroughObjects : MonoBehaviour {

    //Characters that can be seen through objects
    public GameObject[] targets;
    List<GameObject> targetsList;
    List<GameObject> fadedObjects;


    // Use this for initialization
    void Start() {

        //Add player object automatically to target list
        targetsList = targets.ToList();
        GameObject player = CharGameController.getActiveCharacter();
        if (player != null && !targetsList.Contains(player))
        {
            targetsList.Add(player);
        }

        fadedObjects = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {

        //Iterate over every character
        foreach (GameObject character in targetsList)
        {
                //All hitted objects should be added to this list. It will be used while recovering unhitted but faded objects
                List<GameObject> allHittedObjects = new List<GameObject>();

                //Get distance between camera and player
                float distance=Vector3.Distance(transform.position,character.transform.position);

                //We should mask everything except building layer. If you want to add new excepttion to masking, add your layer here.
                int layerMask = 1 << 8;  // "7" here needing to be replaced by whatever layer it is you're wanting to use
                        
                //Get all object of our layer in the direction of camera to character
                Ray ray = new Ray(transform.position, character.transform.position - transform.position);
                RaycastHit[] hits = Physics.RaycastAll(ray,Mathf.Infinity,layerMask);

                //Iterate over every hitted object
                foreach (RaycastHit hit in hits)
                {
                    //Debug.Log(hit.transform.gameObject.layer);
                    Vector3 hitPoint=hit.point;
                    GameObject hittedObject = hit.transform.gameObject;
                    

                    //Check if hit point between camera and player, for that distance between obj and camera should be smaller than distance between camera and character
                
                    if (Vector3.Distance(transform.position, hitPoint) < distance)
                    {
                    //Debug.Log(hit.transform.name + " should be faded");

                    //While allHittedObjects will be used wether or not fadedObjects should be recovered, we are adding hittedObject that will assing to fadedObjects list.
                        if (!allHittedObjects.Contains(hittedObject)) allHittedObjects.Add(hittedObject);
                        makeObjectTransparent(hittedObject);
     
                    }

                }


                //This part is for mouse.
                //If mouse is over building no matter what make it transparent
                //For direction it uses camera face direction
                //Same layer mask is used

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ray = new Ray(mousePosition,Camera.main.transform.forward);
                hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
            
                //Iterate over every hitted object
                foreach (RaycastHit hit in hits)
                {
                   Vector3 hitPoint = hit.point;
                   GameObject hittedObject = hit.transform.gameObject;

                   //Debug.Log(hit.transform.name + " should be faded");
                    if(!allHittedObjects.Contains(hittedObject)) allHittedObjects.Add(hittedObject);
                    makeObjectTransparent(hittedObject);
                
                   }

            List<GameObject> objectToBeRestored = new List<GameObject>();

            //Recover objects that are not hitted in this update and faded previously which means objects that are in fadedObjects list
            foreach (GameObject fadedObj in fadedObjects)
            {

                //Check if this object isnt hit in this update method
                if (!allHittedObjects.Contains(fadedObj))
                {
                    objectToBeRestored.Add(fadedObj);
                    
                }
            }

            foreach (GameObject restoreObj in objectToBeRestored)
            {
                Timing.RunCoroutine(recoverMaterial(restoreObj));
            }


        }

    }

    void makeObjectTransparent(GameObject obj)
    {
        //Chek if object is already faded. If it is then return
        if (fadedObjects.Contains(obj)) return;

        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) return;


        if (rend.material.color.a != 1)
        {
            //Debug.Log(rend.material.color.a);
            return;
        }

        //Debug.Log("Fading " + obj.name);

        Material originalMat = rend.material;
        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._fadeObject(obj, 1f));
        fadedObjects.Add(obj);
    }

    IEnumerator<float> recoverMaterial(GameObject obj)  
    {
        // I dont know
        if (obj == null) yield break;

        //If it is restored then break which means it is not in fadedObject list anymore
        if (!fadedObjects.Contains(obj)) yield break;
                
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) yield break;

        //If material is note completely faded break enumratore
        //It will be restored in one of next updates after compeletely faded 
        if (rend.material.color.a != 0)
        {
            //Debug.Log(rend.material.color.a);
            yield break;
        }
           

        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._fadeObject(obj, 1f));
       
        fadedObjects.Remove(obj);

    }

    public void registerToTargets(GameObject obj)
    {
        Debug.Log("Adding obj " + obj.name + " to targets");
        targetsList.Add(obj);
    }

    public void removeFromTargets(GameObject obj)
    {
        if(targetsList.Contains(obj))
        targetsList.Remove(obj);
    }

}
