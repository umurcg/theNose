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
    public Quaternion initialRot;

    //public bool debug;

	// Use this for initialization
	void Start () {
        initialRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        destroyTime -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z)- transform.forward*Time.deltaTime*fallSpeed;

        if (destroyTime < 0&&!obstacle)
            DestroyObject(gameObject);

        //if (debug)
        //{
        //    debug = false;
        //    youWin();
        //}
	}

    void OnTriggerEnter(Collider col)
    {
        print(col.tag);

        if (col.tag == "Respawn")
        {
            ggc = transform.parent.GetComponent<GirlGameController>();
            ggc.scoreValue += score;
            ggc.updateScore();
            if (ggc.scoreValue < 0)
            {
                youWin();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    void youWin()
    {

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
        Timing.RunCoroutine(_goToMiddle(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)),0.3f));
        Timing.RunCoroutine(_scaleUpTo(10f, 0.10f));
    }

    IEnumerator<float> _rotateTo(Quaternion target, float speed)
    {
        while (Quaternion.Angle(target, transform.rotation) > 6)
        {
            print(Quaternion.Angle(target, transform.rotation));
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
        Destroy(gameObject);
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
