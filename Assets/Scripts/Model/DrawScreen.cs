using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawScreen
{
    public static void drawMap(ITerm term, Vector2Int startPos, IRegion map = null)
    {
        Vector2Int t = new Vector2Int();
        Vector2Int w = new Vector2Int();
        Vector2Int tdim = new Vector2Int(33, 21);
        for (t.y = 0, w.y = startPos.y; t.y < tdim.y; w.y++, t.y++)
        {
            
            for (t.x = 0, w.x = startPos.x; t.x < tdim.x; w.x++, t.x++)
            {
                
                TermChar termChar = outside;
                if (map != null)
                {
                    termChar = (map.legal(w.x, w.y)) ? map.tile(w.x, w.y).glyph() : outside;
                }
                term.at(t.x, t.y, termChar.c, termChar.foreground, termChar.background, termChar.special);
            }
        }
    }

    public static void drawMap(ITerm term, Vector3Int worldPos, IGame game)
    {
        Vector2Int t = new Vector2Int();
        Vector3Int w = new Vector3Int(0 ,0, worldPos.z);

        Vector3Int viewPortStart = new Vector3Int(worldPos.x - 16, worldPos.y - 10, worldPos.z);
        Vector3Int viewPortEnd = new Vector3Int(worldPos.x + 17, worldPos.y + 11, worldPos.z);
        TermChar termChar = outside;
        for (t.y = 0, w.y = viewPortStart.y; w.y < viewPortEnd.y; w.y++, t.y++)
        {
            for (t.x = 0, w.x = viewPortStart.x; w.x < viewPortEnd.x; w.x++, t.x++)
            {
                if (game.world.getTile(w, game, out Tile tile))
                {
                    termChar = tile.glyph();
                    term.at(t.x, t.y, termChar.c, termChar.foreground, termChar.background, termChar.special);
                } else
                {
                    term.at(t.x, t.y, outside.c, outside.foreground, outside.background, outside.special);
                }
            }
        }
    }
    public static void renderHUD(ITerm term, IGame game)
    {
        Creature player = game.player;
        string name = extend(player.name, term);
        string race = extend($"({player.position.x}, {player.position.y}, {player.position.z})", term);
        string health = extend("Health: ", term);
        string magic = extend("Magic: ", term);
        string ac = extend("AC: ", term); string str = extend("Str: ", term);
        string ev = extend("EV: ", term); string intelligence = extend("Int: ", term);
        string sh = extend("AC: ", term); string dex = extend("Dex: ", term);
        string lvl = extend("XL: ", term); string place = extend("Place: ", term);
        string noise = extend("Noise: ", term); string time = extend($"Time: {game.time}", term);
        string weapon = extend("a) ", term);
        string ranged = extend("Ranged: ", term);
        term.txt(36, 0, name, ColorHex.Yellow_Bright, ColorHex.Black, "");
        term.txt(36, 1, race, ColorHex.Yellow_Bright, ColorHex.Black, "");
        term.txt(36, 2, health, ColorHex.Yellow_Dark, ColorHex.Black, "");
        term.txt(36, 3, magic, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 4, ac, ColorHex.Yellow_Dark, ColorHex.Black, "");
        term.txt(55, 4, str, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 5, ev, ColorHex.Yellow_Dark, ColorHex.Black, "");
        term.txt(55, 5, intelligence, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 6, sh, ColorHex.Yellow_Dark, ColorHex.Black, "");
        term.txt(55, 6, dex, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 7, lvl, ColorHex.Yellow_Dark, ColorHex.Black, "");
        term.txt(55, 7, place, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 8, noise, ColorHex.Yellow_Dark, ColorHex.Black, "");
        term.txt(55, 8, time, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 9, weapon, ColorHex.Yellow_Dark, ColorHex.Black, "");

        term.txt(36, 10, ranged, ColorHex.Yellow_Dark, ColorHex.Black, "");

    }
    public static void drawMapPlayer(ITerm term, Vector3Int playerPos, IGame game)
    {
        if (playerPos == null) { playerPos = new Vector3Int(); }
        drawMap(term, playerPos, game);
    }

    public static void renderMsgs(ITerm term, IGame game)
    {
        MsgLog log = game.log;
        if (log == null) return;
        int startY = term.dim.y - 5;
        int maxLines = term.dim.y - startY;
        for (int i = 0; i < maxLines; i++)
        {
            if (log.archive.Count > i)
            {
                string s = extend(log.archive[i], term);
                term.txt(0, 22, s, ColorHex.White_Dark, ColorHex.Black, "");
            }
        }
    }

    public static string extend(string s, ITerm term)
    {
        Vector2Int dim = term.dim;
        string mask = new string(' ', dim.x);
        return s + mask.Substring(0, dim.x - s.Length);
    }
    public static TermChar outside = new TermChar { background = ColorHex.Black, c = '!', foreground = ColorHex.Black, special = "" };
}
