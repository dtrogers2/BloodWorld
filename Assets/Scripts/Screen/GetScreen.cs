
using System.Collections.Generic;
using UnityEngine;

public class GetScreen : OptScreen
{
    public string name { get; set; } = "getscreen";
    public uint me;
    public List<uint> items = new List<uint>();
    public GetScreen(IGame game, IScreenMaker m, uint me, List<uint> items, bool wrapX = false, bool wrapY = false) : base(game, m, wrapX, wrapY)
    {
        this.me = me;
        this.options = new bool[items.Count][];
        this.items = items;
        for (int i = 0; i < items.Count; i++)
        {
            this.options[i] = new bool[1] { false };
        }
    }

    override public void draw(ITerm term)
    {
        term.clear();
        term.txt(0, 0, "Here:", COLOR.White, COLOR.Black);
        int pos = 0;
        foreach (var item in items)
        {

            char c = pos2char(pos);
            Item i = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
            term.txt(0, 1 + pos, $"{c} {(options[pos][0] ? "+" : "-")} {i.name}", COLOR.White, (pos == curY) ? COLOR.GrayDark : COLOR.Black);
            pos++;
        }
    }

    override public void onKey(KeyCode keycode, IStack stack)
    {
        base.onKey(keycode, stack);
        int pos = char2pos((char)keycode);
        if ((pos >= 0 && pos < items.Count) || keycode == KeyCode.Return || keycode == KeyCode.KeypadEnter)
        {
            if (keycode == KeyCode.Return || keycode == KeyCode.KeypadEnter)
            {
                pos = curY;
            }
            this.getItem(pos, stack);
        }
    }

    public bool getItem(int pos, IStack stack)
    {
        bool ok = new GetCmd(me, items[pos], game).turn(out float ap);
        if (ok)
        {
            screenpopTakeTurn(stack, ap);
        }
        return ok;
    }
}
