using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class BotShopping : MonoBehaviour
{

    public string audienceTag;
    public float shopTime;
    public int maximumAudience=1;
    public string animationName;
    public GameObject lookFocus;

    List<GameObject> audience;
    public float probabilityPercent;

    //public GameObject performer;

    // Use this for initialization
    void Start()
    {
        probabilityPercent = Mathf.Clamp(probabilityPercent, 0, 100);
        audience = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (audience.Count < maximumAudience && other.tag == audienceTag)
        {
            float random = Random.Range(0, 100);
            if (random < probabilityPercent)
            {
                UnityEngine.AI.NavMeshAgent nma = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                Timing.RunCoroutine(watchForSeconds(nma, shopTime));



                if (nma)
                {
                    audience.Add(other.gameObject);
                    //Debug.Log("New audience!!!!" + other.name);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (audience.Contains(other.gameObject))
            audience.Remove(other.gameObject);
    }


    IEnumerator<float> watchForSeconds(UnityEngine.AI.NavMeshAgent nma, float time)
    {
        if (!nma) yield break;

        Vector3 aimOfAudience = nma.destination;
        MoveRandomlyOnNavMesh moron = nma.GetComponent<MoveRandomlyOnNavMesh>();
        if (moron) moron.enabled = false;

        nma.SetDestination(transform.position);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs.waitUntilStop(nma.gameObject)));

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(Vckrs._lookTo(nma.gameObject, lookFocus, 1f)));


        Animator anim = nma.GetComponent<Animator>();
        if (anim) anim.SetBool(animationName, true);
        
        yield return Timing.WaitForSeconds(time);

        if (anim) anim.SetBool(animationName, false);

        nma.enabled = true;
        nma.SetDestination(aimOfAudience);
        nma.Resume();

        yield break;

    }

}
