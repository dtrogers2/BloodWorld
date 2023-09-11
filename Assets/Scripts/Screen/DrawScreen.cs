using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

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
        term.clear();
        Vector2Int t = new Vector2Int();
        Vector3Int w = new Vector3Int(0 ,0, worldPos.z);
        Vector3Int viewPortStart = new Vector3Int(worldPos.x - 16, worldPos.y - 10, worldPos.z);
        Vector3Int viewPortEnd = new Vector3Int(worldPos.x + 17, worldPos.y + 11, worldPos.z);
        TermChar termChar = outside;
        List<uint> nearby = new List<uint>();
        //Draw environment
        for (t.y = 0, w.y = viewPortStart.y; w.y < viewPortEnd.y; w.y++, t.y++)
        {
            for (t.x = 0, w.x = viewPortStart.x; w.x < viewPortEnd.x; w.x++, t.x++)
            {
                char c = outside.c;
                COLOR fg = outside.foreground;
                COLOR bg = outside.background;
                if (game.world.getCellFlags(w, game, out uint cell))
                {
                    int max = 10;
                    if (ENTITY.has(game.playerId, COMPONENT.CREATURE))
                    {
                        Creature ply = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[game.playerId];
                        max = ply.vision;
                    }
                    uint entity = cell >> Enum.GetNames(typeof(CELLFLAG)).Length;
                    if (ENTITY.has(game.playerId, COMPONENT.POSITION) && !(entity == game.playerId))
                    {
                        Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[game.playerId];
                        bool canSee = Visbility.lineTo(new Vector3Int(p.x, p.y), w, game, true) && Vector3Int.Distance(new Vector3Int(p.x, p.y), w) <= max;
                        if (canSee)
                        {
                            //Set seen flag
                            game.world.setCellFlags(viewPortStart + (Vector3Int)t, game, CELLFLAG.SEEN);
                            
                            if (ENTITY.has(entity, COMPONENT.GLYPH))
                            {
                                Glyph g = (Glyph)ComponentManager.get(COMPONENT.GLYPH).data[entity];
                                c = g.c;
                                fg = g.color;

                                if (ENTITY.has(entity, COMPONENT.CREATURE) && ENTITY.has(entity, COMPONENT.DEFENSES) && ENTITY.has(entity, COMPONENT.EGO) && entity != game.playerId)
                                {
                                    nearby.Add(entity);
                                }
                            }
                        } else // Can't see, set the glyph to non-creature on the stack
                        {
                            if (ENTITY.bitHas(cell, (uint) CELLFLAG.SEEN))
                            {
                                if (!ENTITY.has(entity, COMPONENT.CREATURE))
                                {
                                    Glyph g = (Glyph)ComponentManager.get(COMPONENT.GLYPH).data[entity];
                                    c = g.c;
                                    fg = COLOR.GrayDark;
                                } else
                                {
                                    bool hasNext = true;
                                    while (hasNext)
                                    {
                                        if (ENTITY.has(entity, COMPONENT.CELLSTACK))
                                        {
                                            CellStack cS = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[entity];
                                            entity = cS.entity;
                                            if (!ENTITY.has(entity, COMPONENT.CREATURE)) {
                                                hasNext = false;
                                                Glyph g = (Glyph)ComponentManager.get(COMPONENT.GLYPH).data[entity];
                                                c = g.c;
                                                fg = COLOR.GrayDark;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } else if (entity == game.playerId)
                    {
                        c = '@';
                        fg = COLOR.Black;
                        bg = COLOR.Gray;
                    }
                }
                term.at(t.x, t.y, c, fg, bg);
            }
        }
        renderNear(term, game, nearby);
    }

    public static void renderNear(ITerm term, IGame game, List<uint> nearby)
    {
        // Clear the nearby list
        for (int i = 11; i < term.dim.y; i++)
        {
            string empty = extend(" ", term);
            term.txt(36, i, empty, COLOR.Black, COLOR.Black);
        }
        // Need to figure out a way to stack similar creatures
        // Limit the number
        if (nearby.Count <= term.dim.y - 10)
        {
            for(int i = 0, y = 11; i < nearby.Count && y < term.dim.y; i++, y++)
            {
                int x = 36; // The x offset for drawing the nearby creatures
                Glyph g = (Glyph)ComponentManager.get(COMPONENT.GLYPH).data[nearby[i]];
                Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[nearby[i]];
                Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[nearby[i]];
                Ego e = (Ego)ComponentManager.get(COMPONENT.EGO).data[nearby[i]];
                int moodadj = EgoUtils.MoodAdj(nearby[i]);
                int dis = e.reputations[4] + moodadj;
                COLOR disCol = (dis >= 250) ? COLOR.Green : (dis > -250) ? COLOR.White : COLOR.Red;
                // TODO: change color of name based on status effects
                COLOR col = (d.hp == d.hpMax) ? COLOR.Green : ((float) d.hp / d.hpMax >= .9f) ? COLOR.GreenDark : ((float) d.hp / d.hpMax >= .7f) ? 
                    COLOR.Yellow : ((float) d.hp / d.hpMax >= .5f) ? COLOR.YellowDark : ((float) d.hp / d.hpMax >= .3f) ? COLOR.Red : ((float) d.hp / d.hpMax >= .1f) ? COLOR.RedDark : COLOR.MagentaDark;
                term.at(x++, y, g.c, COLOR.White, col);
                term.txt(++x, y, c.name, disCol, COLOR.Black);
                x += c.name.Length;
            }
        }
    }
    public static void renderHUD(ITerm term, IGame game)
    {
        if (ENTITY.has(game.playerId, COMPONENT.CREATURE))
        {
            
            Creature player = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[game.playerId];
            string hpString = "";
            string acString = "";
            if (ENTITY.has(game.playerId, COMPONENT.DEFENSES))
            {
                Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[game.playerId];
                hpString = $"{d.hp}/{d.hpMax}";
                acString = $"{d.AC}";
            }
            string name = extend(player.name, term);
            string health = extend($"Health: {hpString}", term);
            string magic = extend("Spell Slots: ", term);
            string ac = extend($"AC: {acString}", term); string str = extend("Str: ", term);
            string ev = extend("EV: ", term); string intelligence = extend("Int: ", term);
            string sh = extend("AC: ", term); string dex = extend("Dex: ", term);
            string lvl = extend("XL: ", term); string place = extend("Place: ", term);
            string noise = extend("Noise: ", term); string time = extend($"Time: {game.time}", term);
            string weapon = extend("a) ", term);
            string ranged = extend("Ranged: ", term);
            term.txt(36, 0, name, COLOR.Yellow, COLOR.Black);
            
            term.txt(36, 2, health, COLOR.YellowDark, COLOR.Black);
            term.txt(36, 3, magic, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 4, ac, COLOR.YellowDark, COLOR.Black);
            term.txt(55, 4, str, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 5, ev, COLOR.YellowDark, COLOR.Black);
            term.txt(55, 5, intelligence, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 6, sh, COLOR.YellowDark, COLOR.Black);
            term.txt(55, 6, dex, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 7, lvl, COLOR.YellowDark, COLOR.Black);
            term.txt(55, 7, place, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 8, noise, COLOR.YellowDark, COLOR.Black);
            term.txt(55, 8, time, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 9, weapon, COLOR.YellowDark, COLOR.Black);

            term.txt(36, 10, ranged, COLOR.YellowDark, COLOR.Black);
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
            term.txt(36, 1, race, COLOR.Yellow, COLOR.Black);
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
        int cursorX = 0;
        for (int i = log.archive.Count - 1, y = term.dim.y - 1; y > 21 && i >= 0; y--, i--)
        {
            for (int j = 0; j < log.archive[i].msgs.Count; j++)
            {
                string baseS = (j == 0 ? "_" : "") + log.archive[i].msgs[j].text;
                string s = extend(baseS, term);
                term.txt(cursorX, y, s, log.archive[i].msgs[j].color, COLOR.Black);
                cursorX += baseS.Length;
            }
            cursorX = 0;
        }
    }

    public static string extend(string s, ITerm term)
    {
        Vector2Int dim = term.dim;
        string mask = new string(' ', dim.x);
        return s + mask.Substring(0, dim.x - s.Length);
    }
    public static TermChar outside = new TermChar { background = COLOR.Black, c = ' ', foreground = COLOR.GrayDark};
}
