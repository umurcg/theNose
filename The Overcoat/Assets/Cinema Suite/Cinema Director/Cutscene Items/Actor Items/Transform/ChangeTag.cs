using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// Attaches all objects as children of actor in hierarchy
    /// </summary>
    [CutsceneItemAttribute("Transform", "Change Tag", CutsceneItemGenre.ActorItem)]
    public class ChangeTag : CinemaActorEvent
    {
        public string Tag;

        string oldTag;
        public override void Trigger(GameObject actor)
        {
            if (Tag == "")
            {
                oldTag = actor.transform.tag;
                actor.transform.tag = "Untagged";
            }else
            {
                oldTag = actor.transform.tag;
                actor.transform.tag = Tag;
            }


            MouseTexture cis = actor.GetComponent<MouseTexture>();
            if (cis != null)
            {
                cis.checkTag();
            }

        }

        public override void Reverse(GameObject actor)
        {
            actor.transform.tag = oldTag;
        }
    }
}