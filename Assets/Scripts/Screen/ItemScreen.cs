using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class ItemScreen : OptScreen
{
    public string name = "ItemScreen";
    public uint me;
    public uint item;
    public Item it;
    public ItemScreen(IGame game, IScreenMaker m, uint me, uint item, bool[][] options, bool wrapX = false, bool wrapY = false) : base(game, m, options, wrapX, wrapY)
    {
        it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        this.me = me;
        this.item = item;
        // Reset options to length of inventory
        this.options = new bool[1][];
        this.options[0] = new bool[4] { false, false, false, false };
    }

    override public void draw(ITerm term)
    {
        term.clear();
        term.txt(0, 0, $"{it.name}", COLOR.White, COLOR.Black);
        int y = 1;
        term.txt(0, ++y, $"{it.description}", COLOR.White, COLOR.Black);
        y++;
        int x = 0;
        string drop = "(d)rop";
        string thr = "(t)hrow";
        string unequip = "(u)nequip";
        string wear = "(w)ear";
        term.txt(x, y, drop, COLOR.White, (curX == 0) ? COLOR.GrayDark : COLOR.Black);
        x += drop.Length + 1;
        term.txt(x, y, thr, COLOR.White, (curX == 1) ? COLOR.GrayDark : COLOR.Black);
        x += thr.Length + 1;
        term.txt(x, y, unequip, COLOR.White, (curX == 2) ? COLOR.GrayDark : COLOR.Black);
        x += unequip.Length + 1;
        term.txt(x, y, wear, COLOR.White, (curX == 3) ? COLOR.GrayDark : COLOR.Black);
        x += wear.Length + 1;
    }

    override public void onKey(KeyCode keycode, IStack stack)
    {
        base.onKey(keycode, stack);

        switch(keycode)
        {
            case KeyCode.D:
                {
                    dropItem(stack);
                    break;
                }
        }
    }

    public bool dropItem(IStack stack)
    {
        bool ok = new DropCmd(me, item, game).turn(out float ap);
        if (ok)
        {
            screenpopTakeTurn(stack, ap);
        }
        return ok;
    }
}
