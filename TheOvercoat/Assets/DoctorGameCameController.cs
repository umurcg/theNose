using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class DoctorGameCameController : GameController {

    public GameObject doctor, chair, bed, noseHead;

    characterComponents doctorCC;

	// Use this for initialization
	public override void Start () {
        base.Start();
        doctorCC = new characterComponents(doctor);
        Timing.RunCoroutine(doctorTalks());
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    IEnumerator<float> doctorTalks()
    {
        yield return 0;

        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "") yield return 0;

        handlerHolder=Timing.RunCoroutine(chair.GetComponent<WalkLookAnim>()._getUp());
        yield return Timing.WaitUntilDone(handlerHolder);

        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(doctor, Vector3.Lerp(player.transform.position, doctor.transform.position, 0.8f)));
        yield return Timing.WaitUntilDone(handlerHolder);

        sc.callSubtitleWithIndex(1);
        while (subtitle.text != "") yield return 0;

        //Wait for sit
        WalkLookAnim wlaBed = bed.GetComponent<WalkLookAnim>();
        while (!wlaBed.isSitting()) yield return 0;


        handlerHolder = Timing.RunCoroutine(Vckrs._setDestination(doctor, player.transform.position+ player.transform.forward));
        yield return Timing.WaitUntilDone(handlerHolder);

        handlerHolder = Timing.RunCoroutine(Vckrs._lookTo(doctor, player,1f));
        yield return Timing.WaitUntilDone(handlerHolder);


        sc.callSubtitleWithIndex(2);
        while (subtitle.text != "") yield return 0;

        //Activate nose merge game
        noseHead.SetActive(true);


        yield break;
    }

    public void mergedHeadAndNose()
    {
        Timing.RunCoroutine(_mergedHeadAndNose());
    }

    IEnumerator<float> _mergedHeadAndNose()
    {
        GameObject smallNose=CharGameController.getNosePos().transform.GetChild(0).gameObject;
        smallNose.SetActive(true);
        noseHead.SetActive(false);

        //Get up
        WalkLookAnim wlaBed = bed.GetComponent<WalkLookAnim>();
        wlaBed.getUp();
        while (wlaBed.isSitting()) yield return 0;

        yield return Timing.WaitForSeconds(1f);

        sc.callSubtitleWithIndex(3);
        while (subtitle.text != "") yield return 0;

        smallNose.transform.parent = null;
        Rigidbody rb = smallNose.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        smallNose.GetComponent<SphereCollider>().enabled = true;
        rb.AddForce(player.transform.forward, ForceMode.Impulse);

        yield return Timing.WaitForSeconds(2f);


        sc.callSubtitleWithIndex(4);
        while (subtitle.text != "") yield return 0;

        GetComponent<LoadScene>().Load();

        yield break;

    }
}
