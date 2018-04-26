using NetRogue.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core.Actions {
    public class PickUpAction : IActorAction {
        Player actor;
        Item item;

        public PickUpAction(Player actor, Item item) {
            this.actor = actor;
            this.item = item;
        }

        public bool Execute(World world) {
            IEnumerable<Item> items = world.Level.GetItemsAt(actor.Position);
            if (!items.Contains(item)) {
                Log.Message("What????");
                return false;
            }
            if (!actor.Inventory.IsFull) {
                actor.Inventory.Add(item);
                world.Level.RemoveItem(item);
                Log.Message($"You picked up the {item.Name}");
                return true;
            }
            Log.Message($"Your inventory is full");
            return false;
        }
    }
}
