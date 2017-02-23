using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
//This is a module for generic methods, classes, enums and some other things.


//Animation type enum. Use this enum for taking actions according to animation parameter type.
public enum AnimType { Trigger, Boolean, Int,Float };


public class Vckrs : MonoBehaviour
{


    public static IEnumerator<float> _setLightIntensity(GameObject light, float speed, float intensity)
    {
        Light l = light.GetComponent<Light>();
        float initialIntesity = l.intensity;
        float ratio = 0;

        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            l.intensity = Mathf.Lerp(initialIntesity, intensity, ratio);
            yield return 0;
        }



    }

    public static IEnumerator<float> waitUntilStop(GameObject obj, float tol=0.000001f)
    {
      
        Vector3 pos = obj.transform.position;
        yield return Timing.WaitForSeconds(0.5f);
        while (Vector3.Distance(obj.transform.position, pos) > tol)
        {
            //Debug.Log((Vector3.Distance(obj.transform.position, pos)));
            pos = obj.transform.position;
            yield return 0;

        }
        //Debug.Log("break");
        yield break;
    }




    public static IEnumerator<float> _Tween(GameObject go, Vector3 aim, float speed)
    {

        Vector3 initialPosition = go.transform.position;

        float ratio = 0;



        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;


            go.transform.position = Vector3.Lerp(initialPosition, aim, ratio);
            yield return 0;

        }
        go.transform.position = aim;

    }

     //Lerp position with heigh.
     //Height depends on sin function
    public static IEnumerator<float> _TweenSinHeight(GameObject go, Vector3 aim, float speed, float maxHeight=1)
    {

        Vector3 initialPosition = go.transform.position;

        float ratio = 0;

        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;

            Vector3 newPosition = Vector3.Lerp(initialPosition, aim, ratio);
            //Debug.Log(Mathf.Sin(3.14f*ratio));
            newPosition += Vector3.up * Mathf.Sin(3.14f * ratio)*maxHeight;
            go.transform.position = newPosition;

            yield return 0;

        }
        go.transform.position = aim;
        

    }

    static public IEnumerator<float> _lookTo(GameObject go, Vector3 aim, float speed)
    {

        Vector3 localAim = go.transform.position + aim;


        localAim.y = go.transform.position.y;


        Quaternion initialRot = go.transform.rotation;
        Quaternion aimRot = Quaternion.LookRotation(localAim - go.transform.position);
        float ratio = 0;


        while (ratio < 1)
        {

            ratio += Time.deltaTime * speed;
            go.transform.rotation = Quaternion.Lerp(initialRot, aimRot, ratio);

            yield return 0;
        }
        go.transform.rotation = aimRot;

    }


    static public IEnumerator<float> _lookTo(GameObject go, GameObject aim, float speed)
    {

        Vector3 localAim = aim.transform.position;


        localAim.y = go.transform.position.y;


        Quaternion initialRot = go.transform.rotation;
        Quaternion aimRot = Quaternion.LookRotation(localAim - go.transform.position);
        float ratio = 0;


        while (ratio < 1)
        {

            ratio += Time.deltaTime * speed;
            go.transform.rotation = Quaternion.Lerp(initialRot, aimRot, ratio);

            yield return 0;
        }
        go.transform.rotation = aimRot;

    }



    //This method pace object between two position.
    //It uses nav mesh.
    static public IEnumerator<float> _pace(GameObject obj, Vector3 aim1, Vector3 aim2)
    {
        NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.Resume();
        nav.SetDestination(aim1);
        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(obj, 0.0001f));

        yield return Timing.WaitUntilDone(handler);


        Vector3 aim = aim2;
        while (true)
        {
            //print("while");
            nav.SetDestination(aim);
            handler = Timing.RunCoroutine(Vckrs.waitUntilStop(obj, 0.0001f));

            //print("waiting");
            yield return Timing.WaitUntilDone(handler);

            //print("waiting finished");

            if (aim == aim1)
            {
                aim = aim2;
            } else
            {
                aim = aim1;
            }


        }

    }


    //This method activates all scripts in an object.
    //Alsa it change its tag to ActiveObject
    static public void ActivateAnotherObject(GameObject obj)
    {
        obj.SetActive(true);
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;

            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }
        }

        obj.transform.tag = "ActiveObject";

    }

    public static void DisableAnotherObject(GameObject go)
    {
        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {

            script.enabled = false;


            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }
        }



        go.transform.tag = "Untagged";

    }

    public static void testPosition(Vector3 pos)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = pos;
    }

    public static IEnumerator<float> followObject(NavMeshAgent agent, GameObject obj)
    {
        agent.Resume();
        while (true)
        {
            agent.SetDestination(obj.transform.position);
            yield return 0;
        }
    }




    public static IEnumerator<float> _cameraSize(Camera cam, float size, float speed)
    {

        if (cam.orthographicSize > size)
        {

            while (cam.orthographicSize > size)
            {
                cam.orthographicSize -= Time.deltaTime * speed;
                yield return 0;
            }
            cam.orthographicSize = size;

        }
        else
        {

            while (cam.orthographicSize < size)
            {
                //print("increase");
                cam.orthographicSize += Time.deltaTime * speed;
                yield return 0;
            }
            cam.orthographicSize = size;
        }
    }


    public static IEnumerator<float> _fadeObject(GameObject obj, float speed, bool fullFade=false)
    {
       

        Renderer rend = obj.GetComponent<Renderer>();
        Color textureColor = rend.material.color;
        float a = textureColor.a;

        //It is for changing rendered mode at right time
        bool willBeTransparent= (a==1) ? true : false;
        StandardShaderUtils.BlendMode mode = (fullFade) ? StandardShaderUtils.BlendMode.Fade : StandardShaderUtils.BlendMode.Transparent;

        if (willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, mode);
        }


        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                rend.material.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            rend.material.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                rend.material.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            rend.material.color = textureColor;

        }



        if (!willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
        }

        yield break;
    }

    public static IEnumerator<float> _fadeInfadeOut<T>(GameObject obj, float speed) where T: MaskableGraphic
    {
        T t = obj.GetComponent<T>();

        Color textureColor = t.color;
        float a = textureColor.a;


        if (a == 0)
        {
            while (a < 1)
            {

                a += Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            t.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            t.color = textureColor;

        }
    }

    public static IEnumerator<float> _fadeInfadeOut(GameObject rawImage, float speed)
    {
        RawImage r = rawImage.GetComponent<RawImage>();

        Color textureColor = r.color;
        float a = textureColor.a;



        if (a == 0)
        {
            while (a < 1)
            {

                a += Time.deltaTime * speed;
                textureColor.a = a;
                r.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            r.color = textureColor;

        } else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                r.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            r.color = textureColor;

        }

    }

    public static IEnumerator<float> _fadeInfadeOutImage(GameObject image, float speed)
    {
        Image i = image.GetComponent<Image>();

        Color textureColor = i.color;
        float a = textureColor.a;



        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                i.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            i.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                i.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            i.color = textureColor;

        }

    }

    public static IEnumerator<float> _fadeInfadeOutText(GameObject text, float speed)
    {
        Text t = text.GetComponent<Text>();

        Color textureColor = t.color;
        float a = textureColor.a;



        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            t.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            t.color = textureColor;

        }

    }


    public static IEnumerator<float> InstantiateIn(GameObject obj, float delay, Vector3 pos)
    {
        yield return Timing.WaitForSeconds(delay);
        GameObject spawnedObj = Instantiate(obj) as GameObject;
        spawnedObj.transform.position = pos;
        yield break;

    }
    public static IEnumerator<float> InstantiateIn(GameObject obj, float delay, GameObject posObj,Vector3 offset)
    {
        yield return Timing.WaitForSeconds(delay);
        GameObject spawnedObj = Instantiate(obj) as GameObject;
        spawnedObj.transform.position = posObj.transform.position+offset;
        yield break;

    }

    //Returns nearest position to pos on navmesh.
    //If fails returns false
    public static bool findNearestPositionOnNavMesh(Vector3 pos, int areaMask, float radiusToSearch, out Vector3 foundPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, radiusToSearch, areaMask))
        {
            foundPosition = hit.position;
            return true;
        }

        foundPosition = Vector3.zero;
        return false;

    }

    void testGenerateRandomPosition()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = generateRandomPositionOnCircle(transform.position, 10f);
        }
    }

    //Return random position on a circle which is around center and have radius of indicated in paramaters
    public static Vector3 generateRandomPositionOnCircle(Vector3 center, float radius)
    {
        int angle = Random.Range(0, 360); // generates numbers in the range 0 ... 359

        float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        float z = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

        return new Vector3(x, center.y, z);

    }

    //Returns a list holding all children objects.
    public static List<Transform> getAllChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {

            children.Add(child);

            if (child.childCount > 0)
            {
                children.AddRange(getAllChildren(child));
            }

        }

        return children;

    }

    public static IEnumerator<float> showMessageForSeconds(string message, Text textComp, float seconds)
    {
        textComp.text = message;
        yield return Timing.WaitForSeconds(seconds);
        textComp.text = "";
        yield break;
    }



}


//Class for holding basic components of characters
//It is for shortcut
public class characterComponents
{
    public GameObject player;
    public Animator animator;
    public NavMeshAgent navmashagent;


    public characterComponents(GameObject p)
    {
        player = p;
        animator = p.GetComponent<Animator>();
        navmashagent = p.GetComponent<NavMeshAgent>();
    }


}
public static class StandardShaderUtils
{
    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }
}
