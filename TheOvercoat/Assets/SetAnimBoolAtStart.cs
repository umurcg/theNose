using UnityEngine;
using System.Collections;

public class SetAnimBoolAtStart : MonoBehaviour
{

    public string booleanName;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool(booleanName, true);
    }

}
