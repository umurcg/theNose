using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class MoveToWithoutAgent : MonoBehaviour
{

    public float speed = 3;
    IEnumerator<float> handler;
    public GameObject floor;
    CursorImageScript cis;

    public Texture2D wingsIcon;

    //Text charSubt;

   
    // Use this for initialization
    void Start()
    {
        if (floor == null)
        {
            Debug.Log("No floor object");
            enabled = false;

        }

        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();
        //charSubt = SubtitleFade.subtitles["CharacterSubtitle"];

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            stop();
            return;

        }
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits= Physics.RaycastAll(ray, 1000, ~(1 << 8));

        ////If character subtitle is active then don't raycast. Let cis do its job.
        //if (charSubt.text != "")
        //{
        //    cis.resetExternalCursor();
        //    return;
        //}

        if(hits.Length>0)
        {
            int floorHit=-1;

            for(int i=0;i<hits.Length;i++)
            {

                //Debug.Log(hits[i].transform.gameObject.name);
                if (hits[i].transform.gameObject == floor)
                {
                    floorHit = i;
                    //hitFloor = true;
                }

                if (hits[i].transform.tag == "ActiveObject")
                {
                    //Debug.Log("Active object");
                    cis.resetExternalCursor();
                    return;
                }

            }

            if (floorHit!=-1)
            {
              
                cis.externalTexture = wingsIcon;

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 aim = hits[floorHit].point;
                    aim = new Vector3(aim.x, transform.position.y, aim.z);
                    Timing.RunCoroutine(_lookAndGo(aim));
                }

                return;
                //cis.resetExternalCursor();

            }

        }






    }

    public void stop()
    {
        if (handler != null)
            Timing.KillCoroutines(handler);
    }

    public void setDestination(Vector3 pos)
    {
        Timing.RunCoroutine(_lookAndGo(pos));
    }

    public IEnumerator<float> _lookAndGo(Vector3 aim)
    {
        //Debug.Log("Look and go");
        stop();

        float dist = Vector3.Distance(aim, transform.position);
        float time = speed / dist;

        if (handler != null)
            Timing.KillCoroutines(handler);

        IEnumerator<float> localHandler = Timing.RunCoroutine(Vckrs._lookTo(gameObject, aim - transform.position, 2f));
        yield return Timing.WaitUntilDone(localHandler);
        handler = Timing.RunCoroutine(Vckrs._Tween(gameObject, aim, time));
        yield return Timing.WaitUntilDone(handler);

        //Debug.Log("Finished look and go");

        yield break;
    }

    private void OnDisable()
    {
        cis.resetExternalCursor();
    }
}
