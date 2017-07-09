using UnityEngine;
using System.Collections;

public class ObstacleObjectCollectableActionScript : MonoBehaviour, ICollectableObjectAction {

    //This script is special for obstacle objects that will be used as collectable objects.
    //You can add whatever u want with interfaces.

    //FUCK THE POLICE

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public  void startingToCollecting()
    {
        try
        {
            GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        }
        catch
        {
            print("no navmesh obstacle");
        }
    }
  public   void startingToUncollecting()
    {
        try
        {
            GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
        }
        catch
        {
            print("no navmesh obstacle");
        }
    }

    public void finishedToUncollecting()
    {

        StartCoroutine(DisableAndEnableKinematic());
        GameObject player=GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<ClickTriggerSingleton>().removeMe(gameObject);
        }

    }

    public IEnumerator DisableAndEnableKinematic()
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForFixedUpdate();
            GetComponent<Rigidbody>().isKinematic = true;
        }

    }


}
