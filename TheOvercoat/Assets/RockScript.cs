using UnityEngine;
using System.Collections;

//This script deals with self destroy of rocks
//Also it holds creater, so creater can't shoot itself.

public class RockScript : MonoBehaviour {

    public GameObject creator;
    public float selfDestructionTimer = 5f;
    float timer;

    public float playerDamage = 5f;
    public float enemyDamage = 25f;

    public GameObject reciever;


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
        
        
        if (collision.transform.tag == "Player")
        {
            reciever.SendMessage("damage",playerDamage);

        } else if (collision.transform.tag == "Enemy")
        {

            reciever.SendMessage("damageEnemy", enemyDamage);
        }
    }

}
