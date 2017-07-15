using UnityEngine;
using System.Collections;
//using UnityEditor;


public class SetTagLayerRecursevly : MonoBehaviour {

    public string Tag;
    public int layer;


    public static void setTag(GameObject obj, string tagString)
    {
        obj.transform.tag = tagString;

        int count = obj.transform.childCount;

        //Break recursive
        if (count == 0) return;

        for (int i= 0; i < count; i++)
        {
            setTag(obj.transform.GetChild(i).gameObject, tagString);
        }
    }



    public static void setLayer(GameObject obj, int lay)
    {
        obj.layer = lay;

        int count = obj.transform.childCount;

        //Break recursive
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            setLayer(obj.transform.GetChild(i).gameObject, lay);
        }
    }

    public void setTag()
    {
        setTag(gameObject, Tag);
    }

    public void setLayer()
    {
        setLayer(gameObject, layer);
    }


}



#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(SetTagLayerRecursevly), true)]
public class SetTagLayerRecursevlyEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SetTagLayerRecursevly script = (SetTagLayerRecursevly)target;
        if (GUILayout.Button("Set Tags "))
        {
            script.setTag();

        }

        if (GUILayout.Button("Set Layer "))
        {
            script.setLayer();

        }
    }
}

#endif