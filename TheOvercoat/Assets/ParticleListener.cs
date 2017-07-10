using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class ParticleListener : MonoBehaviour
{
    ParticleSystem ps;
    public GameObject sgcController;
    SculpturerGameController sgc;
    public float damage = 25;
    //public float particlforDamage = 5;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    //List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    //List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    void Start()
    {
        sgc = sgcController.GetComponent<SculpturerGameController>();
        ps = GetComponent<ParticleSystem>();
        GameObject player = CharGameController.getActiveCharacter();
        ps.trigger.SetCollider(0, player.transform);
    }


    
    void OnParticleTrigger()
    {

        ParticleSystem ps = GetComponent<ParticleSystem>();

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        //Debug.Log("number of enter " + numEnter + " number of exit " + numExit);

        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.remainingLifetime = 0;
            //p.startColor = new Color32(255, 0, 0, 255);
            //enter[i] = p;
            sgc.damage(damage);

        }
        //for (int i = 0; i < numExit; i++)
        //{
        //    ParticleSystem.Particle p = exit[i];
        //    p.startColor = new Color32(0, 255, 0, 255);
        //    exit[i] = p;
        //}

        // set
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        //// get the particles which matched the trigger conditions this frame
        //int numOfParticlesInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        ////int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, exit);

        //Debug.Log("Number of particle inside " + numOfParticlesInside);

        //if (numOfParticlesInside > particlforDamage)
        //{
        //    sgc.damageEnemy(damage);
        //}

        // re-assign the modified particles back into the particle system
        //ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        //sgc.damage(damage);

    }
}