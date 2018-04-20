namespace NetRogue.Core {
    public interface IEntity {
        Point Position { get; }
        Tile Glyph { get; }
    }
}