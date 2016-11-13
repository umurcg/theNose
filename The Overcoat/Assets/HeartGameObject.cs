using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class HeartGameObject : MonoBehaviour {

    public bool obstacle;
    public float fallSpeed;
    public int score;
    GirlGameController ggc;

    public float destroyTime;

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
        destroyTime -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z)- transform.forward*Time.deltaTime*fallSpeed;

        if (destroyTime < 0&&!obstacle)
            DestroyObject(gameObject);

	}

    void OnTriggerEnter(Collider col)
    {
        print(col.tag);

        if (col.tag == "Respawn")
        {
            ggc = transform.parent.GetComponent<GirlGameController>();
            ggc.scoreValue += score;
            ggc.updateScore();
            Destroy(gameObject);
        }
    }
    
    void youWin()
    {

        RotateItself ri = GetComponent<RotateItself>();
        float speed = ri.speed;
        if (ri)
            ri.enabled = false;
        Timing.RunCoroutine(_rotateTo(Quaternion.LookRotation(Camera.main.gameObject.transform.forward,transform.up), speed));

    }

    IEnumerator<float> _rotateTo(Quaternion target, float speed)
    {
        while (Quaternion.Angle(target, transform.rotation) > 0.01)
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            yield return 0;
        }
    }


}
