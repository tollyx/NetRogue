using NetRogue.Core.Entities;

namespace NetRogue.Core {
    public class MoveAction : ActorAction {
        Direction dir;

        public MoveAction(Actor actor, Direction dir) : base(actor) {
            this.dir = dir;
        }

        public override bool Execute(World world) {
            var newpos = actor.Position + dir.ToPoint();
            if (world.GetActorAt(newpos) == null 
             && world.Level.Map.IsInsideBounds(newpos) 
             && world.Level.Map.GetTile(newpos) != Tile.Wall) 
            {
                world.Tree.Remove(actor);
                actor.Move(dir);
                world.Tree.Add(actor);
                return true;
            }
            return false;
        }
    }
}