using NetRogue.Core.Entities;

namespace NetRogue.Core {
    public abstract class ActorAction : IActorAction {
        protected Actor actor;

        protected ActorAction(Actor actor) {
            this.actor = actor;
        }

        public abstract bool Execute(World world);
    }
}