using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : IBuild
{
    public IGame makeGame()
    {
        Rng rng = new Rng(42);
        Creature player = makePlayer();
        Game game = new Game(rng, player, this);
        enterFirstLevel0(game);
        game.ai = makeAI();
        return game;
    }
    public IAI makeAI()
    {
        return new AIBase();
    }
    public IRegion makeLevel(Rng rng, Vector3Int regionPos)
    {
        IRegion map = makeMap(rng, regionPos);
        addLevelStairs(map, map.level, rng);
        addMobsToLevel(map, rng);
        return map;
    }

    public void addLevelStairs(IRegion map, int level, Rng rng)
    {
        // Do nothing for now...
    }

    public void addMobsToLevel(IRegion map, Rng rng)
    {
        makeMobs(map, rng, 50);

    }

    public void makeMobs(IRegion region, Rng rng, int rate)
    {

        Vector3Int pos = new Vector3Int(40, 20, 0);
        addNPC(pos, region, 0);
        /*
        for (pos.y = 1; pos.y < region.dim.y - 1; pos.y++)
        {
            for (pos.x = 1; pos.x < region.dim.x - 1; pos.x++)
            {
                if (!rng.oneIn(rate)) continue;
                if (region.blocked(pos.x, pos.y)) continue;
                addNPC(pos, region, 0);
            }
        }*/
    }

    public void addNPC(Vector3Int pos, IRegion region, int level)
    {
        Vector3Int worldPos = new Vector3Int(pos.x + region.dim.x * region.regionPos.x, pos.y + region.dim.y * region.regionPos.y, region.regionPos.z);
        Creature creature = new Creature(pos, "Ant", new TermChar { c = 'a', background = ColorHex.Black, foreground = ColorHex.Red_Bright, special = "" });
        region.addEntity(creature);
        creature.position = worldPos;
    }

    public IRegion makeMap(Rng rng, Vector3Int regionPos)
    {
        Vector2Int dim = Term.StockDim();
        // Generate region
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
        IRegion map = world.curMap(game);
        Vector3Int np = world.localizePosition((Vector3Int) centorPos(map.dim));
        world.creatureSwitchRegion(game.player, np, game);
    }
}