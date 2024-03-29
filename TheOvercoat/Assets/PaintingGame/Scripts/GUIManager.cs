﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GUIManager : MonoBehaviour {

	public Slider sizeSlider;
	public TexturePainter painter;

    public void Start()
    {
        sizeSlider.value = 0.5f;
        UpdateSizeSlider();
    }

    public void SetBrushMode(int newMode){

        //Painter_BrushMode brushMode = Painter_BrushMode.PAINT;

        //switch (newMode)
        //{
        //    case 0:
        //        brushMode = Painter_BrushMode.PAINT;
        //        break;
        //    case 1:
        //        brushMode = Painter_BrushMode.BINARY;
        //        break;
        //    case 2:
        //        brushMode = Painter_BrushMode.BLACKWHOLE;
        //        break;
        //}
        

        painter.SetBrushMode ((Painter_BrushMode)newMode);
	}


	public void UpdateSizeSlider(){
		painter.SetBrushSize (sizeSlider.value);
	}
}
