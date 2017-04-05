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

    GameObject CameraObj;
    Camera Cam;

    BirdsEyeView biev;
   
    // Use this for initialization
    void Start() {

        //Add player object automatically to target list
        targetsList = targets.ToList();
        GameObject player = CharGameController.getActiveCharacter();
        if (player != null && !targetsList.Contains(player))
        {
            targetsList.Add(player);
        } else if(player==null) /* Debug.Log("Player is null");*/

        fadedObjects = new List<GameObject>();

        CameraObj = CharGameController.getCamera().gameObject;
        Cam = CameraObj.GetComponent<Camera>();

        biev = CameraObj.GetComponent<BirdsEyeView>();

    }

    // Update is called once per frame
    void Update()
    {
        //If map is open then return
        if (biev.isMapOpen()) return;

        //Debug.Log(CameraObj.name);

        //We should mask everything except building layer. If you want to add new excepttion to masking, add your layer here.
        int layerMask = 1 << 8;  // "7" here needing to be replaced by whatever layer it is you're wanting to use

        //All hitted objects should be added to this list. It will be used while recovering unhitted but faded objects
        List<GameObject> allHittedObjects = new List<GameObject>();

        //Iterate over every character
        foreach (GameObject character in targetsList)
        {
            //Debug.Log(character.name);



            //Get distance between camera and player
            float distance = Vector3.Distance(CameraObj.transform.position, character.transform.position);

      

            //Get all object of our layer in the direction of camera to character
            Ray localRay = new Ray(CameraObj.transform.position, character.transform.position - CameraObj.transform.position);
            RaycastHit[] localHits = Physics.RaycastAll(localRay, Mathf.Infinity, layerMask);

            //Iterate over every hitted object
            foreach (RaycastHit hit in localHits)
            {
                //Debug.Log(hit.transform.gameObject.layer);
                Vector3 hitPoint = hit.point;
                GameObject hittedObject = hit.transform.gameObject;


                //Check if hit point between camera and player, for that distance between obj and camera should be smaller than distance between camera and character

                if (Vector3.Distance(CameraObj.transform.position, hitPoint) < distance)
                {
                    //Debug.Log(hit.transform.name + " should be faded");

                    //While allHittedObjects will be used wether or not fadedObjects should be recovered, we are adding hittedObject that will assing to fadedObjects list.
                    if (!allHittedObjects.Contains(hittedObject)) allHittedObjects.Add(hittedObject);
                    makeObjectTransparent(hittedObject);

                }

            }

        }

                //This part is for mouse.
                //If mouse is over building no matter what make it transparent
                //For direction it uses camera face direction
                //Same layer mask is used

                Vector3 mousePosition = Cam.ScreenToWorldPoint(Input.mousePosition);
                Ray ray = new Ray(mousePosition,CameraObj.transform.forward);
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
            
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

    void makeObjectTransparent(GameObject obj)
    {
        //Chek if object is already faded. If it is then return
        if (fadedObjects == null) fadedObjects = new List<GameObject>();
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
        //Debug.Log("Adding obj " + obj.name + " to targets");
        if(!targetsList.Contains(obj))
        targetsList.Add(obj);
    }

    public void removeFromTargets(GameObject obj)
    {
        if(targetsList.Contains(obj))
        targetsList.Remove(obj);
    }

    public void registerActivePlayer()
    {
        GameObject objToRegister = CharGameController.getActiveCharacter();
        registerToTargets(objToRegister);
    }

    public void changeCamera(GameObject camObj)
    {
        CameraObj = camObj;
        Cam = camObj.GetComponent<Camera>();
    }

}
