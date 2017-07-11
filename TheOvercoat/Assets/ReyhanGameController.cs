using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


//In this game, number of garbage area is created.
//In one of those garbage area there will be a treasure.
//User must find tat treasure with exploring gabage areas
//Garbage areas will be on random position in sphere and it must be on a  valid navmesh area 
public class ReyhanGameController : MonoBehaviour {


    public GameObject[] garbageObjects;
    public int numberOfGarbageArea = 5;
    public int numberOfGarbageInArea = 5;
    public float radiusOfWholeArea = 30f;
    public float radiusOfOneArea = 2f;
    public int navMesharea;
    public Material obajectMaterial;
    public Material dyanmicObjectMaterial;
    public GameObject canvas;
    public GameObject treasure;
    public GameObject buttonPrefab;
    

    [HideInInspector]
    public GameObject spawnedButton;
    public Transform canvas2d;
    
    SphereCollider col;

	// Use this for initialization
	void Start () {

        if (numberOfGarbageInArea > garbageObjects.Length)
        {
            Debug.Log("You cant enter number of garbage number more than garbageObjects length");
            enabled = false;
        }

        int treasureIndex = Random.Range(0, numberOfGarbageArea);

        for (int i = 0; i < numberOfGarbageArea; i++)
        {
            createGarbage(i==treasureIndex);
        }

        spawnedButton = Instantiate(buttonPrefab, canvas2d);
        spawnedButton.transform.position = Vckrs.screenRatioToPosition(0.7f, 0.3f);
        spawnedButton.transform.localScale = Vector3.one*0.7f;
        spawnedButton.GetComponent<DynamicLanguageTexts>().textID = 17;
        spawnedButton.SetActive(false);


    }

    private void OnDestroy()
    {
        Destroy(spawnedButton);
    }
    void createGarbage(bool addTreasure)
    {
        //Vector2 randomPos =Random.insideUnitCircle * radiusOfWholeArea;
        //Vector3 garbagePos = (new Vector3(randomPos.x, 0, randomPos.y))+ transform.position;
        Vector3 garbagePos= getRandomPosInCircle(transform.position, radiusOfWholeArea);
        Debug.Log("Garabage pos is " + garbagePos);
        UnityEngine.AI.NavMeshHit hit;

    

        if(UnityEngine.AI.NavMesh.SamplePosition(garbagePos,out hit, 10f,UnityEngine.AI.NavMesh.AllAreas/*NavMesh.GetAreaFromName("Street")*/))
        {

            List<GameObject> objList = garbageObjects.ToList<GameObject>();
            
            GameObject garbageArea = new GameObject();
            garbageArea.name = "GarbageObject";
            garbageArea.transform.parent = transform;
            garbageArea.transform.position = hit.position;

            for(int i=0;i<numberOfGarbageInArea;i++)
            {
                int randIndex = Random.Range(0, objList.Count - 1);
                GameObject randObj = objList[randIndex];

                //Create garbage object at random position
                Vector3 garbObjPos = getRandomPosInCircle(hit.position, radiusOfOneArea);
                GameObject spawnedObj=Instantiate(randObj);
                spawnedObj.transform.parent = garbageArea.transform;
                spawnedObj.transform.position = garbObjPos;
                spawnedObj.AddComponent<CapsuleCollider>();

                AssignMaterialToChildren assignMat= spawnedObj.AddComponent<AssignMaterialToChildren>();
                assignMat.materialToAssin = obajectMaterial;
                assignMat.assignToAllChildren();



                objList.RemoveAt(randIndex);
            }

            if (addTreasure)
            {
                //Create garbage object at random position
                Vector3 garbObjPos = getRandomPosInCircle(hit.position, radiusOfOneArea);
                treasure = Instantiate(treasure);
                treasure.transform.parent = garbageArea.transform;
                treasure.transform.position = garbObjPos;
                treasure.AddComponent<CapsuleCollider>();

                AssignMaterialToChildren assignMat = treasure.AddComponent<AssignMaterialToChildren>();
                assignMat.materialToAssin = obajectMaterial;
                assignMat.assignToAllChildren();
            }


            GarbageAreaController gac= garbageArea.AddComponent<GarbageAreaController>();
            gac.enabled = false;
            gac.canvas = canvas;
            gac.rgc = this;

            gac.closeButton = spawnedButton;

            gac.tag = "ActiveObject";
            SphereCollider sc=gac.gameObject.AddComponent<SphereCollider>();
            sc.radius = radiusOfOneArea;
            sc.isTrigger = true;

            MaterialController mc = garbageArea.AddComponent<MaterialController>();
            mc.controlChildren = true;
            mc.activeMat = dyanmicObjectMaterial;
            mc.nonActiveMat = obajectMaterial;
            

        }
        else
        {
            Debug.Log("Couldnt find pos on navmesh");
        }
    }


    [CustomEditor(typeof(ReyhanGameController))]
    public class customclassEditor : Editor
    {
        
        private ReyhanGameController script;

        public void OnSceneGUI()
        {
            script = this.target as ReyhanGameController;
            Handles.color = Color.red;
            Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                          , script.transform.forward                       // normal
                                          , script.radiusOfWholeArea);                              // radius
        }
    }

    Vector3 getRandomPosInCircle(Vector3 center,float radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        return (new Vector3(randomPos.x, 0, randomPos.y)) + center;

    }

    public void foundTreasure()
    {
        gameObject.SetActive(false);
        
    }

}
