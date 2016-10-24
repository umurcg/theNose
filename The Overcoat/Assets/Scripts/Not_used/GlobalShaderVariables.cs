using UnityEngine;
using System.Collections;



    [RequireComponent(typeof(Camera))]
public class GlobalShaderVariables : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    [SerializeField]
    private Texture2D noiseOffsetTexture;

    private void Awake()
    {
        //Shader.SetGlobalTexture("_NoiseOffsets", this.noiseOffsetTexture);
    }

    private void OnPreRender()
    {
        Shader.SetGlobalVector("_CamPos", this.transform.position);
        Shader.SetGlobalVector("_CamRight", this.transform.right);
        Shader.SetGlobalVector("_CamUp", this.transform.up);
        Shader.SetGlobalVector("_CamForward", this.transform.forward);

        //Shader.SetGlobalFloat("_AspectRatio", (float)Screen.width / (float)Screen.height);
        //Shader.SetGlobalFloat("_FieldOfView", Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f) * 2f);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
