using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.EventSystems.EventTrigger;

public class Build : IBuild
{
    public IGame makeGame()
    {

        initComponents();
        makeEnv();
        Rng rng = new Rng(42);
        Game game = new Game(rng, this);
        makePlayer(game);
        //enterFirstLevel0(game);
        game.ai = makeAI();
        return game;
    }

    public void initComponents()
    {
        ComponentManager.init();
    }

    public void makeEnv()
    {
        Env.init();
        //Glyph wallGlyph = new Glyph { c = '#', color = ColorHex.White };
        //Glyph floorGlyph = new Glyph { c = '.', color = ColorHex.White };
        //Glyph emptyGlyph = new Glyph { c = ' ', color = ColorHex.Black };
        //emptyId = EntityManager.create();
        //ENTITY.subscribe(emptyId, emptyGlyph, COMPONENT.GLYPH);
        //wallId = EntityManager.create();
        //ENTITY.subscribe(wallId, wallGlyph, COMPONENT.GLYPH);
        //floorId = EntityManager.create();
        //ENTITY.subscribe(floorId, floorGlyph, COMPONENT.GLYPH);
    }
    public IAI makeAI()
    {
        return new AIBase();
    }
    public IRegion makeLevel(IGame game, Vector3Int regionPos, bool addMobs = false)
    {
        IRegion map = makeMap(game, game.rng, regionPos);
        addMobsToRegion(game, map);
        return map;
    }

    public void addLevelStairs(IRegion map, Rng rng)
    {
        // Do nothing for now...
    }

    public void addMobsToRegion(IGame game, IRegion map)
    {
        makeMobs(game, map, 1);

    }

    public void makeMobs(IGame game, IRegion map, int rate)
    {
        int seed2 = map.regionPos.y;
        int seed3 = map.regionPos.z;
        Rng r = new Rng(game.rng.getSeed() + map.regionPos.x + (seed2 * 100) + (seed3 * 1000));
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
                Vector3Int posLocal = new Vector3Int(x, y, 0);
                uint cellFlags = map.getCellFlags(posLocal);
                if ( ENTITY.bitHas(cellFlags, (uint)(CELLFLAG.BLOCKED | CELLFLAG.CREATURE))) { continue; }
                if (game.rng.pct(rate))
                {
                    Vector3Int posWorld = new Vector3Int(x + (map.dim.x * map.regionPos.x), y + (map.dim.y * map.regionPos.y), map.regionPos.z);
                    int roll = r.rngC(1, MonData.entries.Length);
                    monsterentry entry = MonData.entries[roll];

                    //Glyph g = new Glyph { c = entry.basechar, color = entry.color };
                    Position p = new Position { x = posWorld.x, y = posWorld.y, z = posWorld.z };
                    //Creature c = new Creature { name = MONTYPE.GetName(typeof(MONTYPE), entry.mid), hpMax = entry.avg_hp, hp = entry.avg_hp, moveRate = entry.moveRate, attackRate = entry.attackRate, actionPoints = 0f };
                    uint creatureId = EntityManager.create();
                    ENTITY.subscribe(creatureId, entry.data, entry.components);
                    ENTITY.subscribe(creatureId, p, COMPONENT.POSITION);

                    addNPC(creatureId, map);
                }
            }
        }
        //Vector3Int pos = new Vector3Int(40 + (map.dim.x * map.regionPos.x), 20 + (map.dim.y * map.regionPos.y), 0);
        //Creature creature = new Creature(pos, "Ant", new TermChar { c = 'a', background = ColorHex.Black, foreground = ColorHex.Red });
        //Vector3Int pos2 = new Vector3Int(40 + (map.dim.x * map.regionPos.x), 10 + (map.dim.y * map.regionPos.y), 0);
        //Creature creature2 = new Creature(pos2, "Bat", new TermChar { c = 'b', background = ColorHex.Black, foreground = ColorHex.Black });
        //creature2.baseMoveCost = 0.5f;

        //Region must be initialized or trying to add leads to infinite loop
        //addNPC(creature2, map);
        //addNPC(creature, map);
    }

    public void addNPC(uint c, IRegion map)
    {
        map.addEntity(c);
    }

    public IRegion makeMap(IGame game, Rng rng, Vector3Int regionPos)
    {
        Vector2Int dim = Term.StockDim();
        IRegion map = MapGen.test(game, regionPos);
        return map;
    }

    public void makePlayer(IGame game)
    {
        Rng rng = new Rng(game.rng.getSeed());
        bool foundPosition = false;
        int x = 0;
        int y = 0;
        while (!foundPosition)
        {
            x = rng.rngC(Term.StockDim().x, Term.StockDim().x * 2);
            y = rng.rngC(Term.StockDim().x, Term.StockDim().y * 2);
            int xLocal = x % Term.StockDim().x;
            int yLocal = y % Term.StockDim().y;
            if (game.world.getRegion(new Vector3Int(x, y), game, out IRegion r)) {
                uint cellFlags = r.getCellFlags(new Vector3Int(xLocal, yLocal));
                if (ENTITY.bitHas(cellFlags, (uint)(CELLFLAG.BLOCKED | CELLFLAG.CREATURE))) { continue; }
                foundPosition = true;
            }

        }
        Glyph g = new Glyph { c = '@', color = ColorHex.White };
        Position p = new Position { x = x, y = y, z = 0 };
        Creature c = new Creature { name = "player", hpMax = 100, hp = 100, moveRate = 1f, attackRate = 1f, actionPoints = 0f };
        uint player = EntityManager.create();
        ENTITY.subscribe(player, new object[3] { g, p, c}, new COMPONENT[3] { COMPONENT.GLYPH, COMPONENT.POSITION, COMPONENT.CREATURE });
        game.playerId = player;
        game.world.addEntity(game.playerId, game);
    }

    public Vector2Int centorPos(Vector2Int dim)
    {
        return new Vector2Int(Mathf.FloorToInt(dim.x / 2), Mathf.FloorToInt(dim.x / 2));
    }

    public void enterFirstLevel0(IGame game)
    {
        World world = game.world;
        world.addEntity(game.playerId, game);
    }


}