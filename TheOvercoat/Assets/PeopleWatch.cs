using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class PeopleWatch : MonoBehaviour {

    public string audienceTag;
    public float watchTime;
    public int maximumAudience;

    List<GameObject> audience;
    public float probabilityPercent;

    public GameObject performer;

	// Use this for initialization
	void Start () {
        probabilityPercent = Mathf.Clamp(probabilityPercent, 0, 100);
        audience = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (audience.Count<maximumAudience && other.tag == audienceTag)
        {
            float random = Random.Range(0, 100);
            if (random < probabilityPercent)
            {
                UnityEngine.AI.NavMeshAgent nma = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                Timing.RunCoroutine(watchForSeconds(nma, watchTime, performer.transform.position));



                if (nma)
                {
                    audience.Add(other.gameObject);
                    Debug.Log("New audience!!!!" + other.name);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(audience.Contains(other.gameObject))
            audience.Remove(other.gameObject);
    }


    IEnumerator<float> watchForSeconds(UnityEngine.AI.NavMeshAgent nma, float time, Vector3 aim)
    {
        if (!nma) yield break;

        Vector3 aimOfAudience = nma.destination;

        nma.enabled = false;

        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs._lookTo(nma.gameObject, aim - nma.gameObject.transform.position, 1f));
        yield return Timing.WaitForSeconds(time);

        nma.enabled = true;
        nma.SetDestination(aimOfAudience);
        nma.Resume();

        yield break;

    }

}
