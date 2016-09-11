using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour {
    //Background of subt

    RawImage ri;
    

    Text text;

    //note format of strings
    TextAsset tAssest;

    //player for stopping player.
   public  GameObject player;
    NavMeshAgent playerNavMesh;
    CharacterControllerKeyboard cck;
    //original values
    float speed, rotationalS;


   float time = 0;

    //check for either player is sttopped or not stopped
    bool stopped = false;

    string[] textArray;
    public bool start=false;
    int index;
    Color color;
    public float colorSpeed=1f;
    string wholeText;

    float timer = 0f;
    public float typeWriterSpeed = 0.3f;
    int typeWriterIndex;
    // Use his for initialization

    void Start () {
        ri = GetComponent<RawImage>();
        text = GetComponentInChildren<Text>();
        color = new Color(ri.color.r,ri.color.g,ri.color.b,0f);
        ri.color = color;

        playerNavMesh = player.GetComponent<NavMeshAgent>();
        cck = player.GetComponent<CharacterControllerKeyboard>();
        speed = cck.speed;
        rotationalS = cck.rotateSpeed;

    }
	
    public void startDialouge(TextAsset ta)
    {
        this.tAssest = ta;
        textArray = ta.text.Split('\n');

        //for (int i = 0; i < textArray.Length; i++)
        //{
        //    Debug.Log(textArray[i]);
        //}

       
        index = 0;
        timer = 0;
        wholeText = textArray[index];
        index++;
        ScriptClickTriggerOld.isTriggersActive = false;
        start = true;

    }
    void finish()
    {
        
        text.text = "";
        timer = 0;
        index = 0;
        typeWriterIndex = 0;
        if (stopped)
            resumePlayer();

        
        start = false;

        time = 1;
     
    }



	void stopPlayer()
    {
         playerNavMesh.Stop();
        //playerNavMesh.speed = 0;
        player.GetComponent<MoveTo>().enabled = false;
        cck.speed = 0;
        cck.rotateSpeed = 0;
        stopped = true;
    }

    void resumePlayer()
    {
      // playerNavMesh.Resume();
        // playerNavMesh.speed = speed;
        player.GetComponent<MoveTo>().enabled = true;
        cck.speed = speed;
        cck.rotateSpeed = rotationalS;
        stopped = false;
    }

    
    // Update is called once per frame
	void Update () {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                time = 0;

                ScriptClickTriggerOld.isTriggersActive = true;
            }
        }

       if (!start)
        {
            if (ri.color.a > 0.001f)
            {
          

                color.a = 0;
                ri.color = Color.Lerp(ri.color, color, Time.deltaTime * colorSpeed);
                
            }
        }
        if (start)
        {

            if (!stopped)
            {
                stopPlayer();
            }

            if (ri.color.a < 0.95f)
            {
                color.a = 1;
                ri.color = Color.Lerp(ri.color, color, Time.deltaTime * colorSpeed);
            }
        

            if (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space))
            {

                if (index < textArray.Length)
                {

                  //  Debug.Log(wholeText);
                    wholeText = textArray[index];
                    
                    index++;
                    text.text = "";
                    typeWriterIndex = 0;
                    timer = 0;
                } else
                {
                   
                    finish();
                   
                }
            }
     
          
            timer += Time.deltaTime;
            if (timer > typeWriterSpeed)
            {
                timer = 0;
                if (typeWriterIndex < wholeText.Length )
                {
                    text.text = text.text + wholeText[typeWriterIndex];
                    typeWriterIndex++;
                  
                }
            }

        }
	}
}
