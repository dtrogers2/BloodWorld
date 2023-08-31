using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawScreen
{
    public static void drawMap(ITerm term, Vector2Int startPos, IRegion map = null)
    {
        Vector3Int t = new Vector3Int();
        Vector3Int w = new Vector3Int();
        Vector2Int tdim = new Vector2Int(33, 21);
        for (t.y = 0, w.y = startPos.y; t.y < tdim.y; w.y++, t.y++)
        {
            
            for (t.x = 0, w.x = startPos.x; t.x < tdim.x; w.x++, t.x++)
            {
                
                TermChar termChar = outside;
                term.at(t.x, t.y, termChar.c, termChar.foreground, termChar.background);
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

        //Draw environment
        for (t.y = 0, w.y = viewPortStart.y; w.y < viewPortEnd.y; w.y++, t.y++)
        {
            for (t.x = 0, w.x = viewPortStart.x; w.x < viewPortEnd.x; w.x++, t.x++)
            {
                if (game.world.getCellEntity(w, game, out uint cell))
                {
                    if (ENTITY.has(cell, COMPONENT.GLYPH))
                    {
                       Glyph g = (Glyph)ComponentManager.get(COMPONENT.GLYPH).data[cell];
                       term.at(t.x, t.y, g.c, g.color, termChar.background);
                    }
                } else
                {
                    term.at(t.x, t.y, outside.c, outside.foreground, outside.background);
                }
            }
        }
    }
    public static void renderHUD(ITerm term, IGame game)
    {
        if (ENTITY.has(game.playerId, COMPONENT.CREATURE))
        {
            
            Creature player = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[game.playerId];

            string name = extend(player.name, term);
            string health = extend($"Health: {player.hp}/{player.hpMax}", term);
            string magic = extend("Magic: ", term);
            string ac = extend("AC: ", term); string str = extend("Str: ", term);
            string ev = extend("EV: ", term); string intelligence = extend("Int: ", term);
            string sh = extend("AC: ", term); string dex = extend("Dex: ", term);
            string lvl = extend("XL: ", term); string place = extend("Place: ", term);
            string noise = extend("Noise: ", term); string time = extend($"Time: {game.time}", term);
            string weapon = extend("a) ", term);
            string ranged = extend("Ranged: ", term);
            term.txt(36, 0, name, ColorHex.Yellow, ColorHex.Black);
            
            term.txt(36, 2, health, ColorHex.YellowDark, ColorHex.Black);
            term.txt(36, 3, magic, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 4, ac, ColorHex.YellowDark, ColorHex.Black);
            term.txt(55, 4, str, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 5, ev, ColorHex.YellowDark, ColorHex.Black);
            term.txt(55, 5, intelligence, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 6, sh, ColorHex.YellowDark, ColorHex.Black);
            term.txt(55, 6, dex, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 7, lvl, ColorHex.YellowDark, ColorHex.Black);
            term.txt(55, 7, place, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 8, noise, ColorHex.YellowDark, ColorHex.Black);
            term.txt(55, 8, time, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 9, weapon, ColorHex.YellowDark, ColorHex.Black);

            term.txt(36, 10, ranged, ColorHex.YellowDark, ColorHex.Black);
        }
        Vector3Int position = Vector3Int.zero;
        if (ENTITY.has(game.playerId, COMPONENT.POSITION))
        {
            Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[game.playerId];
            position.x = p.x;
            position.y = p.y;
            position.z = p.z;
            Vector3Int regionPos = game.world.regionPosition(position);
            string race = extend($"({position.x}, {position.y}, {position.z}) ({regionPos.x}, {regionPos.y}, {regionPos.z})", term);
            term.txt(36, 1, race, ColorHex.Yellow, ColorHex.Black);
        }
    }
    public static void drawMapPlayer(ITerm term, Vector3Int viewPos, IGame game)
    {
        drawMap(term, viewPos, game);
    }

    public static void renderMsgs(ITerm term, IGame game)
    {
        MsgLog log = game.log;
        if (log == null) return;
        for (int i = 0, y = 21; y < term.dim.y; y++, i++)
        {
            if (log.archive.Count > i)
            {
                string s = extend(log.archive[i], term);
                term.txt(0, y, s, ColorHex.Gray, ColorHex.Black);
            }
        }
    }

    public static string extend(string s, ITerm term)
    {
        Vector2Int dim = term.dim;
        string mask = new string(' ', dim.x);
        return s + mask.Substring(0, dim.x - s.Length);
    }
    public static TermChar outside = new TermChar { background = ColorHex.Black, c = '.', foreground = ColorHex.GrayDark};
}
