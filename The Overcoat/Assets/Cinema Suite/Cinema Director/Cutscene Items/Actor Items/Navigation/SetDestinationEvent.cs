using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// An event for setting a navigation destination.
    /// Only executes in runtime. Not reversable.
    /// </summary>
    [CutsceneItemAttribute("Navigation", "Set Destination", CutsceneItemGenre.ActorItem)]
    public class SetDestinationEvent : CinemaActorEvent

    {
        // The destination target
        public Vector3 target;
		public GameObject AimObject;
        public Vector3 offset;

        public float forward;
        public float rigth;

        /// <summary>
        /// Trigger this event and set a new destination.
        /// </summary>
        /// <param name="actor">The actor with a NavMeshAgent to set a new destination for.</param>
        public override void Trigger(GameObject actor)
        {
            NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
				if (AimObject != null) {
                    

                    agent.SetDestination (AimObject.transform.position+offset+AimObject.transform.forward*forward+ AimObject.transform.right*rigth);
				} else {
					agent.SetDestination (target);
				}
            }
        }
    }
}