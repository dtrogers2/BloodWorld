using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreen : IScreen
{
    public string name { get; set; } = "testmap";
    IRegion map;
    public MapScreen(IRegion map) { this.map = map; }


    public void draw(ITerm term)
    {
        DrawScreen.drawMap(term, new Vector2Int(), map);
    }

    public void onKey(KeyCode keycode, IStack stack)
    {
        // Do nothing
    }

    public void onKey(KeyCode keycode)
    {
        // Do nothing
    }

    public static EventManager runMapScreen(IRegion map, Sprite[] sprites)
    {
        return ScreenStack.run_SScreen(new MapScreen(map), sprites);
    }
}

public class TestMap
{
    public static IRegion test(Vector2Int dim, Vector3Int regionPos, Rng rng)
    {
        Wall wall = new Wall(new Vector3Int(), "Wall", true, true, new TermChar { background = ColorHex.Black, c = '#', foreground = ColorHex.White_Dark, special = "" });
        Vector2Int p = new Vector2Int();

        Region map = new Region(dim, regionPos, new TermChar { background = ColorHex.Black, c = '.', foreground = ColorHex.White_Dark, special = "" });
        for (p.y = 0; p.y < dim.y; p.y++)
        {
            for (p.x = 0; p.x < dim.x; p.x++)
            {
                bool edge = !(p.x > 0 && p.x < dim.x - 1 && p.y > 0 && p.y < dim.y - 1);
                bool chance = rng.oneIn(4);
                bool hasWall = (edge || chance);
                if (chance) map.tile(p.x, p.y).wall = wall;

            }
        }

        return map;
    }
}
