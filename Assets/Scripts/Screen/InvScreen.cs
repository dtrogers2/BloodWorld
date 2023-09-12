using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvScreen : OptScreen
{
    public string name { get; set; } = "invscreen";
    public uint entity;
    public Inventory inv;
    public InvScreen(IGame game, IScreenMaker m, uint entity, bool wrapX = false, bool wrapY = false) : base(game, m, wrapX, wrapY)
    {
        this.entity = entity;
        inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[entity];
        // Reset options to length of inventory
        this.options = new bool[inv.items.Count][];
        for (int i = 0; i < inv.items.Count; i++)
        {
            this.options[i] = new bool[1] {false};
        }
    }

    override public void draw(ITerm term)
    {
        term.clear();
        term.txt(0, 0, "Inventory:", COLOR.White, COLOR.Black);
        int pos = 0;
        foreach (var item in inv.items)
        {
            char c = pos2char(pos);
            Item i = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
            term.txt(0, 1 + pos, $"{c} {(options[pos][0] ? "+" : "-")} " +
                $"{(ENTITY.bitHas((uint) i.flags, (uint)ITEMFLAG.STACKABLE) ? $"{i.amt}" : "") } {i.name}" +
                $"{(i.amt > 1 && ENTITY.bitHas((uint)i.flags, (uint)ITEMFLAG.STACKABLE) ? "s": "")} " +
                $"{(ENTITY.bitHas((uint)i.flags, (uint)ITEMFLAG.CHARGES) ? $" ({i.amt} charges)" : "")}",
                (i.equipped ? COLOR.GreenDark : COLOR.White), (pos == curY) ? COLOR.GrayDark : COLOR.Black);
            pos++;
        }
    }

    override public void onKey(KeyCode keycode, IStack stack)
    {
        base.onKey(keycode, stack);
        int pos = char2pos((char)keycode);
        if ((pos >= 0 && pos < inv.items.Count) || keycode == KeyCode.Return || keycode == KeyCode.KeypadEnter)
        {
            if (keycode == KeyCode.Return || keycode == KeyCode.KeypadEnter)
            {
                pos = curY;
            }
            this.itemMenu(pos, stack);
        }
    }

    public void itemMenu(int pos, IStack stack)
    {
        stack.pop();
        stack.push(new ItemScreen(game, maker, entity, inv.items.ElementAt(pos)));
    }
}
