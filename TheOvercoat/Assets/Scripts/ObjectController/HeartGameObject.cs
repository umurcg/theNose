using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class HeartGameObject : MonoBehaviour {

    public bool obstacle;
    
    public int score;
    GirlGameController ggc;

    public float destroyTime;
    public Quaternion initialRot;

    public GameObject particle;

    //public bool debug;

	// Use this for initialization
	void Start () {
        initialRot = transform.rotation;
	}
	
    public void setGirlGameController(GirlGameController cont)
    {
        ggc = cont;
    }

	// Update is called once per frame
	void Update () {
        destroyTime -= Time.deltaTime;
   

        if (destroyTime < 0&&!obstacle)
            DestroyObject(transform.parent.gameObject);

        //if (debug)
        //{
        //    debug = false;
        //    youWin();
        //}
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Triggered " + col.transform.name);

        if (col.tag == "Respawn")
        {
            //ggc = transform.parent.parent.GetComponent<GirlGameController>();
            ggc.scoreValue += score;
            ggc.updateScore();
            if (ggc.scoreValue < 0)
            {
                youWin();
            } 
            else
            {
                //ALLAH BELAMI VERSİN
                if (obstacle)
                {
                
                    particle.SetActive(true);
                    particle.transform.SetParent(null);
                    Destroy(gameObject);
                }
                else
                {
           
                    particle.SetActive(true);
                    particle.transform.SetParent(null);
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
    
    void youWin()
    {
        GetComponent<Collider>().enabled = false;
        RotateItself ri = GetComponent<RotateItself>();
        float speed = ri.speed;
        if (ri)
            ri.enabled = false;
        GameObject player = transform.parent.GetChild(0).gameObject;
        MovementWithKeyboard2D mwk = player.GetComponent<MovementWithKeyboard2D>();
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        mwk.enabled = false;

        Timing.RunCoroutine(_rotateTo(initialRot, speed));
        Timing.RunCoroutine(_goToMiddle(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, ggc.distanceToCamera)),0.3f));
        Timing.RunCoroutine(_scaleUpTo(10f, 0.10f));
    }

    IEnumerator<float> _rotateTo(Quaternion target, float speed)
    {
        while (Quaternion.Angle(target, transform.rotation) > 6)
        {
            //print(Quaternion.Angle(target, transform.rotation));
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            yield return 0;
        }
        transform.rotation = initialRot;
    }

    IEnumerator<float> _goToMiddle(Vector3 pos, float speed)
    {
        float ratio = 0;
        Vector3 initialPos = transform.position;
        while (ratio<1)
        {
            ratio += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(initialPos, pos, ratio);
            yield return 0;
        }
        transform.position = pos;
    }

    IEnumerator<float> _scaleUpTo(float scale, float speed)
    {
        float ratio = 0;
        Vector3 initialScale = transform.localScale;
        Vector3 aimScale = initialScale * scale;
        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            transform.localScale = Vector3.Lerp(initialScale, aimScale, ratio);
            yield return 0;
        }
        transform.localScale = aimScale;

        particle.SetActive(true);
        particle.transform.SetParent(null);

        Destroy(transform.parent.gameObject);
        ggc = transform.parent.GetComponent<GirlGameController>();
        Timing.RunCoroutine(ggc._finish(0f));
    }

    //IEnumerator<float> _callFinish(float seconds)
    //{
    //    while (seconds > 0)
    //    {
    //        seconds -= Time.deltaTime;
    //        yield return 0;
    //    }
    //    ggc = transform.parent.GetComponent<GirlGameController>();
    //    ggc.finish();
    //}

}
