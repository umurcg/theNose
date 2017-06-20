using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class WindGem : MonoBehaviour {


    public  FogController fc;
    public GameObject explosionParticle;
    MeshRenderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!fc) Debug.Log("FC is null");
    }

    private void OnTriggerEnter(Collider other)
    {

        fc.gemIsCollected();

        Timing.RunCoroutine(_destroy());
    }

    IEnumerator<float> _destroy()
    {
        explosionParticle.SetActive(true);
        //yield return Timing.WaitForSeconds(1);
        rend.enabled = false;
        yield return Timing.WaitForSeconds(4f);
        Destroy(gameObject);
    }


}
