using NetRogue.Core.Entities;

namespace NetRogue.Core {
    public class AttackAction : ActorAction {
        private Actor target;

        public AttackAction(Actor actor, Actor target) : base(actor) {
            this.target = target;
        }

        public override bool Execute(World world) {
            if (Point.DistanceSquared(actor.Position, target.Position) <= 1) {
                actor.Attack(target);
                return true;
            }
            return false;
        }
    }
}