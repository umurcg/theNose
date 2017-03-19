using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MovementEffects;


//This script provides player several objects
//When player is activate owner, it store random objects in a static variable.
//After that player can uncollect them.

public class CollectableObjSupplier : MonoBehaviour, IClickAction {


    public static CollectableObject activeScript;

    public List<GameObject> collectedObjs;
    public float radiusForUncollect;
    public GameObject[] prefabs;
    public LayerMask rayCastMask;
    public GameObject UIText;
    public GameObject _3DPanel;
    public GameObject UI3Dicon;

    public string countMessage;
    public string additionalTutorialString;

    public enum animationType { Boolean, Trigger};
    public animationType AnimationType = animationType.Boolean;
    public string collectAnimation;
    public float animationTime = 5f;

    //Cursor textures for uncollecting
    public Texture2D outsideOfRadius;
    public Texture2D insideOfRadius;

    //Max number of collectabl objects
    public int maxNumber;
    CursorImageScript cis;


	// Use this for initialization
	void Start () {
        collectedObjs = new List<GameObject>();
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
	}
	
	// Update is called once per frame
	void Update () {

        //If don'tt have item in inventory retun
        if (collectedObjs.Count == 0) {
            if (activeScript == this) activeScript = null;
            return;
        }

        //Mouse right click will be uncollect obj in that position.
        //For uncollecting mouse will be near of player with given raidus
        //For that we will raycast
        Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~rayCastMask))
        {
            //Dest between hit point and player
            GameObject player = CharGameController.getActiveCharacter();
            float distance = Vector3.Distance(new Vector3(player.transform.position.x,hit.point.y, player.transform.position.z), hit.point);
            //Debug.Log("HitName: "+hit.transform.name  + "distance"+distance);
            

            if (distance > radiusForUncollect)
            {
                //Debug.Log("You can't uncollect");

                //Set cursor
                if (cis) cis.externalTexture = outsideOfRadius;
                
            }else
            {
                //Debug.Log("You can collect");

                if (cis) cis.externalTexture = insideOfRadius;

                if (Input.GetButtonUp("Uncollect"))
                {
                    uncollect(hit.point);
                }

            }

       
            

        }

     

	}

    IEnumerator<float> supplyWithAnimation()
    {
        Animator playerAnim = CharGameController.getActiveCharacter().GetComponent<Animator>();
        PlayerComponentController pcc= CharGameController.getActiveCharacter().GetComponent<PlayerComponentController>();

        if (collectAnimation != "")
        {
            pcc.StopToWalk();

            if (AnimationType == animationType.Trigger)
            {
                playerAnim.SetTrigger(collectAnimation);
            }else
            {
                playerAnim.SetBool(collectAnimation, true);
            }


            yield return Timing.WaitForSeconds(animationTime);
        }

        supply();

        if (collectAnimation != "")
        {

            pcc.ContinueToWalk();

            if (AnimationType == animationType.Trigger)
            {
                playerAnim.SetTrigger(collectAnimation);
            }
            else
            {
                playerAnim.SetBool(collectAnimation, false);
            }

        }

        yield break;

    }

    void supply()
    {
        if (prefabs.Length == 0) return;

        while (collectedObjs.Count < maxNumber)
        {
            collectedObjs.Add(prefabs[Random.Range(0, prefabs.Length)]);
        }


        refreshMessage();

    }

    void uncollect(Vector3 pos)
    {
        Debug.Log("Uncollect");  
        if (collectedObjs.Count == 0) return;

        GameObject objectToSpawn = collectedObjs[Random.Range(0, collectedObjs.Count)];
        GameObject spawnedObject = Instantiate(objectToSpawn) as GameObject;
        spawnedObject.transform.position = pos;

        collectedObjs.Remove(objectToSpawn);

        //Check for cursor
        if (collectedObjs.Count == 0 && cis) cis.externalTexture = null;

        refreshMessage();
    }

    public void clearInventory()
    {
        collectedObjs.Clear();
        if(cis) cis.externalTexture = null;

        refreshMessage();
    }

    public void Action()
    {
        Timing.RunCoroutine(supplyWithAnimation());
        
    }

    void refreshMessage()
    {
        if(UI3Dicon!=null && _3DPanel != null)
        {
            if (collectedObjs.Count == 0 && _3DPanel.transform.childCount>0)
            {
                
                Destroy(_3DPanel.transform.GetChild(0).gameObject);
            }else if (_3DPanel.transform.childCount == 0)
            {
                GameObject spawnedObj=Instantiate(UI3Dicon) as GameObject;
                spawnedObj.transform.parent = _3DPanel.transform;
                spawnedObj.transform.localPosition = Vector2.zero;
                spawnedObj.transform.localScale = Vector3.one * 5;
            }
        }

        Text text = UIText.GetComponent<Text>();
        if (text)
        {
            if (collectedObjs.Count == 0)
            {
                text.text = "";
            }else
            {
                text.text = countMessage + " " + collectedObjs.Count +"/n"+additionalTutorialString;
            }
        }
         
    }
}
