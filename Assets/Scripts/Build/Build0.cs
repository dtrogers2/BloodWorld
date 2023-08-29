using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : IBuild
{
    public Wall defaultWall { get; set; }

    public PositionComponent POSITIONS { get; set; } = new PositionComponent();
    public GlyphComponent GLYPHS { get; set; } = new GlyphComponent();
    public uint wallId { get; set; }
    public uint floorId { get; set; }

    public uint emptyId { get; set; }
    public IGame makeGame()
    {

        initComponents();
        makeEnv();
        Rng rng = new Rng(42);
        Creature player = makePlayer();
        defaultWall = new Wall(new Vector3Int(), "Wall", true, true, new TermChar { background = ColorHex.Black, c = '#', foreground = ColorHex.Gray });
        player.maxhp = 10000;
        player.hp = 10000;
        uint playerId = makePlayer2();
        Game game = new Game(rng, player, playerId, this);
        enterFirstLevel0(game);
        game.ai = makeAI();
        return game;
    }

    public void initComponents()
    {
        //positions = new PositionComponent();
        //glyphs = new GlyphComponent();
    }

    public void makeEnv()
    {
        Glyph wallGlyph = new Glyph { c = '#', color = ColorHex.White };
        Glyph floorGlyph = new Glyph { c = '.', color = ColorHex.White };
        Glyph emptyGlyph = new Glyph { c = ' ', color = ColorHex.Black };
        emptyId = EntityManager.create();
        ENTITY.subscribe(emptyId, emptyGlyph, GLYPHS);
        wallId = EntityManager.create();
        ENTITY.subscribe(wallId, wallGlyph, GLYPHS);
        floorId = EntityManager.create();
        ENTITY.subscribe(floorId, floorGlyph, GLYPHS);
    }
    public IAI makeAI()
    {
        return new AIBase();
    }
    public IRegion makeLevel(IGame game, Vector3Int regionPos, bool addMobs = false)
    {
        IRegion map = makeMap(game, game.rng, regionPos);
        //addMobsToRegion(game, map);
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
        for(int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
                Vector3Int posLocal = new Vector3Int(x, y, 0);
                if (map.blocked(posLocal)) { continue; }
                if (game.rng.pct(rate))
                {
                    Vector3Int posWorld = new Vector3Int(x + (map.dim.x * map.regionPos.x), y + (map.dim.y * map.regionPos.y), 0);
                    int roll = game.rng.rngC(1, MonData.entries.Length);
                    monsterentry entry = MonData.entries[roll];
                    Creature c = new Creature(posWorld, MONTYPE.GetName(typeof(MONTYPE), entry.mid), new TermChar { c = entry.basechar, background = ColorHex.Black, foreground = entry.color}); ;
                    c.baseAtkCost = entry.baseAtkSpeed;
                    c.baseMoveCost = entry.baseMovSpeed;
                    c.hp = entry.avg_hp;
                    c.maxhp = entry.avg_hp;
                    addNPC(c, map);
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

    public void addNPC(Creature c, IRegion map)
    {
        map.addEntity(c);
    }

    public IRegion makeMap(IGame game, Rng rng, Vector3Int regionPos)
    {
        Vector2Int dim = Term.StockDim();
        IRegion map = MapGen.test(game, regionPos, rng, defaultWall);
        return map;
    }

    public Creature makePlayer()
    {
        return new Creature(new Vector3Int(0, 0, 0), "Player", new TermChar { c = '@', foreground = ColorHex.White, background = ColorHex.Black});
    }

    public uint makePlayer2()
    {
        Glyph g = new Glyph { c = '@', color = ColorHex.White };
        Position p = new Position { x = 0, y = 0, z = 0 };
        uint player = EntityManager.create();
        ENTITY.subscribe(player, new object[2] { g, p}, new ComponentInf[2] { GLYPHS, POSITIONS });
        return player;
    }

    public Vector2Int centorPos(Vector2Int dim)
    {
        return new Vector2Int(Mathf.FloorToInt(dim.x / 2), Mathf.FloorToInt(dim.x / 2));
    }

    public void enterFirstLevel0(IGame game)
    {
        World world = game.world;
        world.addEntity(game.player, game);
        world.addEntity(game.playerId, game);
        //Vector3Int pos = new Vector3Int(40, 20, 0);
        //Creature creature = new Creature(pos, "Ant", new TermChar { c = 'a', background = ColorHex.Black, foreground = ColorHex.Red });
        //Vector3Int pos2 = new Vector3Int(40, 10, 0);
        //Creature creature2 = new Creature(pos2, "Bat", new TermChar { c = 'b', background = ColorHex.Black, foreground = ColorHex.Black });
        //creature2.baseMoveCost = 0.5f;

        //world.addEntity(creature, game);
        //world.addEntity(creature, game);
    }


}