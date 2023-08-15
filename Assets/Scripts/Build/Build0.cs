using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : IBuild
{
    public IGame makeGame()
    {
        Rng rng = new Rng(42);
        Creature player = makePlayer();
        player.maxhp = 10000;
        player.hp = 10000;
        Game game = new Game(rng, player, this);
        enterFirstLevel0(game);
        game.ai = makeAI();
        return game;
    }
    public IAI makeAI()
    {
        return new AIBase();
    }
    public IRegion makeLevel(IGame game, Vector3Int regionPos, bool addMobs = false)
    {
        IRegion map = makeMap(game.rng, regionPos);
        addMobsToRegion(map);
        return map;
    }

    public void addLevelStairs(IRegion map, Rng rng)
    {
        // Do nothing for now...
    }

    public void addMobsToRegion(IRegion map)
    {
        makeMobs(map, 50);

    }

    public void makeMobs(IRegion map, int rate)
    {

        Vector3Int pos = new Vector3Int(40 + (map.dim.x * map.regionPos.x), 20 + (map.dim.y * map.regionPos.y), 0);
        Creature creature = new Creature(pos, "Ant", new TermChar { c = 'a', background = ColorHex.Black, foreground = ColorHex.Red_Bright, special = "" });
        Vector3Int pos2 = new Vector3Int(40 + (map.dim.x * map.regionPos.x), 10 + (map.dim.y * map.regionPos.y), 0);
        Creature creature2 = new Creature(pos2, "Bat", new TermChar { c = 'b', background = ColorHex.Black, foreground = ColorHex.Black_Bright, special = "" });
        creature2.baseMoveCost = 0.5f;

        //Region must be initialized or trying to add leads to infinite loop
        addNPC(creature2, map);
        addNPC(creature, map);
    }

    public void addNPC(Creature c, IRegion map)
    {
        map.addEntity(c);
    }

    public IRegion makeMap(Rng rng, Vector3Int regionPos)
    {
        Vector2Int dim = Term.StockDim();
        IRegion map = TestMap.test(dim, regionPos, rng);
        return map;
    }

    public Creature makePlayer()
    {
        return new Creature(new Vector3Int(20, 12, 0), "Player", new TermChar { c = '@', foreground = ColorHex.White, background = ColorHex.Black, special = "" });
    }

    public Vector2Int centorPos(Vector2Int dim)
    {
        return new Vector2Int(Mathf.FloorToInt(dim.x / 2), Mathf.FloorToInt(dim.x / 2));
    }

    public void enterFirstLevel0(IGame game)
    {
        World world = game.world;
        world.addEntity(game.player, game);
        Vector3Int pos = new Vector3Int(40, 20, 0);
        Creature creature = new Creature(pos, "Ant", new TermChar { c = 'a', background = ColorHex.Black, foreground = ColorHex.Red_Bright, special = "" });
        Vector3Int pos2 = new Vector3Int(40, 10, 0);
        Creature creature2 = new Creature(pos2, "Bat", new TermChar { c = 'b', background = ColorHex.Black, foreground = ColorHex.Black_Bright, special = "" });
        creature2.baseMoveCost = 0.5f;

        world.addEntity(creature, game);
        world.addEntity(creature, game);
    }


}