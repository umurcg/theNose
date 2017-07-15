using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointBarScript : MonoBehaviour {

    float maxPoint = 100;
    float minPoint = 0;

    public float point=0;

    public Text percentage;
    public Text barName;
    public Image fill;

	// Use this for initialization
	void Start () {

        updateUI();
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setPoint(float point)
    {
        this.point = point;
        updateUI();
    }

    public void setLimits(float max, float min)
    {
        maxPoint = max;
        minPoint = min;

        updateUI();

    }

    public void setName(string n)
    {
        barName.text = n;
    }

    public void updateUI()
    {
        float percent = (Mathf.Clamp(point, minPoint, maxPoint) / maxPoint);
        //Debug.Log("Percent is " + percent);

        percentage.text = "%" +(int)(percent *100);
        fill.fillAmount = percent;
    }

    public void disableBar()
    {
        gameObject.SetActive(false);
        setName("");
    }

    public void enableBar(string nameOfBar)
    {
        gameObject.SetActive(true);
        setName(nameOfBar);
    }

}
