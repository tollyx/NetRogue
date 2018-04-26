namespace NetRogue.Core.Entities {
    public interface IEntity {
        string Name { get; }
        Point Position { get; }
        Tile Glyph { get; }
    }
}