using UnityEngine;
using System.Collections;
using MovementEffects;

//This script deals with self destroy of rocks
//Also it holds creater, so creater can't shoot itself.

public class RockScript : MonoBehaviour {

    public GameObject creator;
    public float selfDestructionTimer = 5f;
    float timer;

    public float playerDamage = 5f;
    public float enemyDamage = 25f;

    public GameObject reciever;
    public GameObject particle;


    // Use this for initialization
    void Start () {
        timer = selfDestructionTimer;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0)
        {
            Destroy(gameObject);
            return;
        }else
        {
            timer -= Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (enabled == false) return;

        //Debug.Log("hit object " + collision.gameObject.name+" create name is "+creator.name);

        if (creator!=null && collision.gameObject == creator)
        {
            //Debug.Log("I hit creater");
            return;
        }

        //Debug.Log("Didnt return");

        if (collision.transform.tag == "Player")
        {
            reciever.SendMessage("damage", playerDamage);

            //Impact force
            GameObject player = collision.gameObject;
            Timing.RunCoroutine(Vckrs.addImpactForceCC(player, Vckrs.eliminiteY(player.transform.position) - Vckrs.eliminiteY(transform.position)*-1));
        
            explode();

        } else if (collision.transform.tag == "Enemy")
        {

            reciever.SendMessage("damageEnemy", enemyDamage);
            explode();
        } else if (collision.transform.tag == "Sculpture")
        {
            collision.gameObject.SendMessage("Explode");
            explode();
        } else if (collision.transform.tag == "Floor")
        {
            explode();
        }

        //Destroy after it hits something
  
    }

    public void explode()
    {
        //Debug.Log("explod");
        if (particle != null)
        {
            GameObject spawnedParticle = Instantiate(particle) as GameObject;
            spawnedParticle.transform.position = transform.position;
            spawnedParticle.SetActive(true);
        }
        Destroy(gameObject);

    }

}
