using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// Attaches actor as child of target in hierarchy
    /// </summary>
    [CutsceneItemAttribute("Transform", "Set Parent", CutsceneItemGenre.ActorItem)]
    public class SetParent : CinemaActorEvent
    {
        public GameObject parent;
        public override void Trigger(GameObject actor)
        {
			if (actor != null && parent != null) {                
				actor.transform.parent = parent.transform;                
			} else if (actor!=null&&parent == null) {
				actor.transform.parent = null;
			}
        }

        public override void Reverse(GameObject actor)
        {
        }
    }
}