using System;
using Unity.VisualScripting;
using UnityEngine;

public class Build : IBuild
{
    public IGame makeGame()
    {
        EntityManager.reset();
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
        MonData.init();
        ItemData.init();
    }
    public IAI makeAI()
    {
        return new AIBase();
    }
    public IRegion makeLevel(IGame game, Vector3Int regionPos, bool addMobs = false)
    {
        IRegion map = makeMap(game, game.rng, regionPos);
        addMobsToRegion(game, map);
        addItemsToRegion(game, map);
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

    public void addItemsToRegion(IGame game, IRegion map)
    {
        makeItems(game, map, 1);

    }

    public void makeItems(IGame game, IRegion map, int rate)
    {
        int seed2 = map.regionPos.y;
        int seed3 = map.regionPos.z;
        Rng r = new Rng(game.rng.getSeed() + map.regionPos.x + (seed2 * 100) + (seed3 * 1000));
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0;x < map.dim.x; x++)
            {
                Vector3Int posLocal = new Vector3Int(x, y, 0);
                uint cellFlags = map.getCellFlags(posLocal);
                if (ENTITY.bitHas(cellFlags, (uint)(CELLFLAG.BLOCKED | CELLFLAG.CREATURE))) { continue; }
                if (game.rng.pct(rate))
                {
                    Vector3Int posWorld = new Vector3Int(x + (map.dim.x * map.regionPos.x), y + (map.dim.y * map.regionPos.y), map.regionPos.z);
                    int roll = r.rngC(1, ItemData.entries.Length);
                    itementry entry = ItemData.entries[roll];
                    Position p = new Position { x = posWorld.x, y = posWorld.y, z = posWorld.z };
                    uint itemId = EntityManager.create();
                    object[] components = new object[entry.components.Length];
                    for (int i = 0; i < components.Length; i++)
                    {
                        components[i] = entry.components[i].CloneViaSerialization();
                    }

                    ENTITY.subscribe(itemId, components);
                    ENTITY.subscribe(itemId, p);
                    addNPC(itemId, map);
                }
            }
        }
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
                    Position p = new Position { x = posWorld.x, y = posWorld.y, z = posWorld.z };
                    uint creatureId = EntityManager.create();
                    object[] components = new object[entry.components.Length];
                    for (int i = 0; i < components.Length; i++)
                    {
                        components[i] = entry.components[i].CloneViaSerialization();//entry.components[i].Copy();
                    }
                    // ENTITY.subscribe(creatureId, entry.components); Copying the entry components is no good, need to figure out how to deep clone the entry components
                    ENTITY.subscribe(creatureId, components);
                    ENTITY.subscribe(creatureId, p);
                    ENTITY.subscribe(creatureId, new AI { });
                    HealthAdj.initHD(creatureId, game);
                    if (ENTITY.has(creatureId, COMPONENT.EGO))
                    {
                        Ego e = (Ego)ComponentManager.get(COMPONENT.EGO).data[creatureId];
                        int moodRoll = r.roll("2d6");
                        MOOD mood = (moodRoll >= 12) ? MOOD.FRIENDLY : (moodRoll >= 10) ? MOOD.DOCILE : (moodRoll > 8) ? MOOD.NEUTRAL : MOOD.AGGRESSIVE;
                        
                        e.mood = mood;
                        // Adjust kinship
                        for (int i = 0; i < Enum.GetNames(typeof(FAC)).Length; i++)
                        {
                            if (e.factions.HasFlag((FAC)(1 << i)))
                            {
                                int index = Array.IndexOf(Enum.GetValues(e.factions.GetType()), (FAC) (1 << i));
                                e.reputations[index] = (short) e.kinship;
                            }
                        }
                        /*
                        if (ENTITY.has(leaderId, COMPONENT.EGO))
                        {
                            Ego eL = (Ego)ComponentManager.get(COMPONENT.EGO).data[leaderId];
                            for (int i = 0; i < eL.reputations.Length; i++)
                            {
                                e.reputations[i] = eL.reputations[i];
                            }
                        }*/
                    }

                    addNPC(creatureId, map);
                }
            }
        }
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
        Glyph g = new Glyph { c = '@', color = COLOR.White };
        Position p = new Position { x = x, y = y, z = 0 };
        Creature c = new Creature { name = "Player", moveSpeed = 30, AP = 0f, vision = 8, exp = new int[] { 4, 0, 0, 0 } };
        Defenses d = new Defenses { HD = "1d8", hpMax = 20, hp = 20, AC = 11, regenRate = 0.7f };
        Attacks a = new Attacks { baseAtkDly = 1f, atkUsed = 0, attacks = new Attack[1] { new Attack { name = "strike", dmgDice = "1d6", atkDly = 1f } } };
        uint player = EntityManager.create();
        ENTITY.subscribe(player, new object[] { g, p, c, d, a, new Ego { factions = FAC.HUMAN } });
        game.playerId = player;
        HealthAdj.initHD(game.playerId, game);
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