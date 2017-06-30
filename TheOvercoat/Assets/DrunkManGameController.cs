using UnityEngine;
using System.Collections;

public class DrunkManGameController : MonoBehaviour {

    float drunkManHealth = 100;
    float kovalevHealth = 100;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void playerDamage(float damage)
    {
        kovalevHealth -= damage;
        if (kovalevHealth <= 0) lost();

    }

    public void damageEnemy(float damage)
    {
        drunkManHealth -= damage;
        if (drunkManHealth <= 0) win();

    }

    public void lost()
    {

    }

    public void win()
    {

    }
}
