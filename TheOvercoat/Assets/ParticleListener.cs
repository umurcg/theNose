using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ParticleListener : MonoBehaviour
{
    ParticleSystem ps;
    public GameObject sgcController;
    SculpturerGameController sgc;
    public int damage = 25;
    public float particlforDamage = 5;

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
        ps.trigger.SetCollider(0, player.GetComponent<Collider>());
    }

    void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame
        int numOfParticlesInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        //int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, exit);

        //Debug.Log("Number of particle inside " + numOfParticlesInside);

        if (numOfParticlesInside> particlforDamage)
        {
            sgc.damageEnemy(damage);
        }

        // re-assign the modified particles back into the particle system
        //ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

    }
}