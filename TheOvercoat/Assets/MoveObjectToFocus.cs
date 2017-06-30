using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MoveObjectToFocus : MonoBehaviour {

    public float scaleUp = 3f;
    public float speed = 0.3f;
    public bool autoActivate = false;
	// Use this for initialization
	void Start () {
        if (autoActivate) move();
	}
	
    //IEnumerator<float> _rotateTo(Quaternion target, float speed)
    //{
    //    while (Quaternion.Angle(target, transform.rotation) > 6)
    //    {
    //        //print(Quaternion.Angle(target, transform.rotation));
    //        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    //        yield return 0;
    //    }
    //    transform.rotation = initialRot;
    //}


    public void move()
    {
        Timing.RunCoroutine(_goToMiddle(speed));
        Timing.RunCoroutine(_scaleUpTo(scaleUp, speed));
    }

    IEnumerator<float> _goToMiddle(float speed)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        float ratio = 0;
        Vector3 initialPos = transform.position;
        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(initialPos, pos, ratio);
            yield return 0;
        }
        transform.position = pos;
        yield break;
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
        yield break;
    }

}
