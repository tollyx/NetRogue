namespace NetRogue.Core.Entities {
    public interface IEntity {
        Point Position { get; }
        Tile Glyph { get; }
    }
}