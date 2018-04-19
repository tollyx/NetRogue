namespace NetRogue.Core {
    public abstract class ActorAction {
        protected Actor actor;

        protected ActorAction(Actor actor) {
            this.actor = actor;
        }

        public abstract void Execute(World world);
    }

    public class MoveAction : ActorAction {
        Direction dir;

        public MoveAction(Actor actor, Direction dir) : base(actor) {
            this.dir = dir;
        }

        public override void Execute(World world) {
            var newpos = actor.Position + dir.ToPoint();
            Actor other = world.GetActorAt(newpos);
            if (other == null) {
                if (world.Level.IsInsideBounds(newpos) && world.Level.GetTile(newpos) != Tile.Wall)
                    actor.Move(dir);
            }
            else {
                actor.Attack(other);
            }
        }
    }
}