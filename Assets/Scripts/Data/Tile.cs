
using Assets.Scripts.Data;
public class Tile  {
    public TileType Type {get;set;}
    public Entity Entity { get; set; }

    public Tile() {
        this.Type = TileType.Unknown;
    }
}
