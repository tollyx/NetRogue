using NetRogue.Core.Entities;

namespace NetRogue.Core.Actions {
    public class MoveAction : ActorAction {
        Direction dir;

        public MoveAction(Actor actor, Direction dir) : base(actor) {
            this.dir = dir;
        }

        public override bool Execute(World world) {
            var newpos = actor.Position + dir.ToPoint();
            if (world.Level.GetActorAt(newpos) == null 
             && world.Level.Map.IsInsideBounds(newpos) 
             && world.Level.Map.GetTile(newpos) != Tile.Wall) 
            {
                world.Level.ActorTree.Remove(actor);
                actor.Move(dir);
                world.Level.ActorTree.Add(actor);
                return true;
            }
            return false;
        }
    }
}