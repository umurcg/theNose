using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.UI;

//Enables and disables UI object with fading. It is similer to HideUnhideButton but more invlusive.
public class EnableDisableUI : MonoBehaviour {

    MaskableGraphic[] mgs;

    ////debug
    //public bool activateDeb = false;
    //public bool deactivateDeb = false;

    private void Awake()
    {
        Vckrs.enableAllChildren(transform);

        mgs = getAllFadebleObjectsOfChildren();
        Debug.Log(mgs.Length);

        Vckrs.disableAllChildren(transform);
    }
	
	// Update is called once per frame
	void Update () {

        //if (activateDeb)
        //{
        //    activate();
        //    activateDeb = false;
        //}

        //if (deactivateDeb)
        //{
        //    deactivate();
        //    deactivateDeb = false;
        //}
	}

    public MaskableGraphic[] getAllFadebleObjectsOfChildren()
    {
        return gameObject.GetComponentsInChildren<MaskableGraphic>();
    }

    public void activate()
    {
        Timing.RunCoroutine(_activate());
    }

    public void deactivate()
    {
        Timing.RunCoroutine(_deactivate());
    }


    public IEnumerator<float> _deactivate()
    {

        //is active or not
        //bool isActive = gameObject.activeSelf;
        //if (!isActive) yield break;

        //Set each objects alpha to 1 just in case
        foreach (MaskableGraphic mg in mgs) Vckrs.setAlpha<MaskableGraphic>(mg.gameObject, 1);

        IEnumerator<float> handler = null;

        foreach (MaskableGraphic mg in mgs) handler = Timing.RunCoroutine(Vckrs._fadeInfadeOut<MaskableGraphic>(mg.gameObject, 1f));

        //Wait for last coroutine finishes its job which means wait for fading
        yield return Timing.WaitUntilDone(handler);

        //Disable all children
        Vckrs.disableAllChildren(transform);


        yield break;

    }


    public void deactivateAndDestroy()
    {
        Timing.RunCoroutine(_deactivateAndDestroy());
    }

    public IEnumerator<float> _deactivateAndDestroy()
    {

        //is active or not
        //bool isActive = gameObject.activeSelf;
        //if (!isActive) yield break;

        //Set each objects alpha to 1 just in case
        foreach (MaskableGraphic mg in mgs) Vckrs.setAlpha<MaskableGraphic>(mg.gameObject, 1);

        IEnumerator<float> handler = null;

        foreach (MaskableGraphic mg in mgs) handler = Timing.RunCoroutine(Vckrs._fadeInfadeOut<MaskableGraphic>(mg.gameObject, 1f));

        //Wait for last coroutine finishes its job which means wait for fading
        yield return Timing.WaitUntilDone(handler);

        //Disable all children
        Vckrs.disableAllChildren(transform);

        Destroy(gameObject);

        yield break;

    }


    public IEnumerator<float> _activate()
    {
   

        //is active or not
        //bool isActive = gameObject.activeSelf;
        //if (isActive) yield break;
        Debug.Log("Activating");
        //Set each objects alpha to 1 just in case
        foreach (MaskableGraphic mg in mgs) Vckrs.setAlpha<MaskableGraphic>(mg.gameObject, 0);

        //Enabl all children
        Vckrs.enableAllChildren(transform);

        IEnumerator<float> handler = null;

        //UnityEditor.EditorApplication.isPaused = true;

        foreach (MaskableGraphic mg in mgs) handler = Timing.RunCoroutine(Vckrs._fadeInfadeOut<MaskableGraphic>(mg.gameObject, 1f));

        //Wait for last coroutine finishes its job which means wait for fading
        yield return Timing.WaitUntilDone(handler);
      
        gameObject.SetActive(true);

        yield break;

    }



}
