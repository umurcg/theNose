using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrunkManGameController : MonoBehaviour {

    public GameObject canvas;
    public GameObject barPrefab;
    public GameObject sliderPrefab;
    public GameObject emptyUI;
    

    public Vector2 UIPosRatio;

    GameObject ui;
    GameObject kovalevHealthBar , drunkManHelathBar, slider;

    PointBarScript kovalevPS, drunkPS;

    float drunkManHealth = 100;
    float kovalevHealth = 100;

    public float UIScale;

    public Vector2 cellsize;
    public Vector2 spacing;

    Scrollbar sb;

    //// Use this for initialization
    void Start()
    {
        ui = Instantiate(emptyUI,canvas.transform) as GameObject;
        ui.transform.position = Vckrs.screenRatioToPosition(UIPosRatio.x, UIPosRatio.y);
        GridLayoutGroup glg= ui.AddComponent<GridLayoutGroup>();
        glg.cellSize = cellsize;
        glg.spacing = spacing;
 
        kovalevHealthBar = Instantiate(barPrefab/*, ui.transform*/) as GameObject;
        kovalevHealthBar.transform.parent = ui.transform;
        kovalevHealthBar.SetActive(true);
        kovalevPS = kovalevHealthBar.GetComponent<PointBarScript>();
        kovalevPS.setLimits(0, 100);
        kovalevPS.setName("Kovalev Health: ");


        drunkManHelathBar = Instantiate(barPrefab/*, ui.transform*/) as GameObject;
        drunkManHelathBar.SetActive(true);
        drunkManHelathBar.transform.parent = ui.transform;
        drunkPS = drunkManHelathBar.GetComponent<PointBarScript>();
        drunkPS.setLimits(0, 100);
        drunkPS.setName("Drunk Man Health: ");


        slider = Instantiate(sliderPrefab/*, ui.transform*/) as GameObject;
        slider.transform.parent = ui.transform;
        slider.GetComponentInChildren<Text>().text = "Shoot Velocity";
        sb = slider.GetComponent<Scrollbar>();

        ui.transform.localScale = Vector3.one * UIScale;


    }

    // Update is called once per frame
    void Update () {
	
	}


    public void damage(float damageValue)
    {
        if (enabled == false) return;

        kovalevHealth -= damageValue;
        if (kovalevHealth <= 0) lost();

        kovalevPS.setPoint(kovalevHealth);
  
    }

    public void damageEnemy(float damage)
    {
        if (enabled == false) return;

        drunkManHealth -= damage;
        if (drunkManHealth <= 0) win();

        drunkPS.setPoint(kovalevHealth);

    }

    public void lost()
    {

    }

    public void win()
    {

    }

    public void setVelocityUI(float value)
    {
        sb.value = Mathf.Clamp(value, 0, 1);
    }

}
