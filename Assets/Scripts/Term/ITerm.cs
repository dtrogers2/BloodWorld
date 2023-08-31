using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEditorInternal.ReorderableList;
using ColorUtility = UnityEngine.ColorUtility;

public interface ITerm
{
    Vector2Int dim { get; }
    TermChar[,] tpoint { get; }
    void txt(Vector2Int position, string s, string foreground, string background);
    void at(Vector2Int position, char c, string foreground, string background);

    void txt(int x, int y, string s, string foreground, string background);
    void at(int x, int y, char c, string foreground, string background);
    void clear();
    void init();
}

public class Term : ITerm
{
    public Vector2Int dim { get; protected set; }
    public TermChar[,] tpoint { get; protected set; }
    public Term(Vector2Int dim)
    {
        this.dim = dim;
        this.tpoint = new TermChar[dim.x, dim.y];
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                this.tpoint[x, y] = new TermChar { foreground = ColorHex.Black, background = ColorHex.Black, c = '?'};
            }
        }
    }

     public void txt(Vector2Int position, string s, string foreground, string background)
    {
        char[] chars = s.ToCharArray();
        for (int x = 0; x < s.Length; x++)
        {
            this.at(new Vector2Int(position.x + x, position.y), chars[x], foreground, background);
        }
    }

    virtual public void at(Vector2Int position, char c, string foreground, string background)
    {
        if (position.x >= 0 && position.y >= 0 && position.x < this.dim.x && position.y < this.dim.y)
        {
            this.tpoint[position.x, position.y].foreground =  foreground;
            this.tpoint[position.x, position.y].background = background;
            this.tpoint[position.x, position.y].c = c;
        }
    }

     public void txt(int x, int y, string s, string foreground, string background)
    {
        char[] chars = s.ToCharArray();
        for (int c = 0; c < s.Length; c++)
        {
            this.at(new Vector2Int(x + c, y), chars[c], foreground, background);
        }
    }

    virtual public void at(int x, int y, char c, string foreground, string background)
    {
        if (x >= 0 && y >= 0 && x < this.dim.x && y < this.dim.y)
        {
            this.tpoint[x, y].foreground = foreground;
            this.tpoint[x, y].background = background;
            this.tpoint[x, y].c = c;
        }
    }

    virtual public void clear()
    {
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                this.tpoint[x, y].foreground = ColorHex.Black;
                this.tpoint[x, y].background = ColorHex.Black;
                this.tpoint[x, y].c = '?';
            }
        }
    }

    virtual public void init()
    {
        this.clear();
    }

    public static Vector2Int StockDim()
    {
        return new Vector2Int(80, 25);
    }
}

public class GTerm : Term
{
    SpriteRenderer[,] fgSprite { get; }
    SpriteRenderer[,] bgSprite { get; }

    Sprite[] spriteSheet;
    public GTerm(Vector2Int dim, Sprite[] sheet) : base(dim)
    {
        fgSprite = new SpriteRenderer[dim.x, dim.y];
        bgSprite = new SpriteRenderer[dim.x, dim.y];
        spriteSheet = sheet;
        this.init();
    }

    public override void at(Vector2Int position, char c, string foreground, string background)
    {
        base.at(position, c, foreground, background);

        if (position.x >= 0 && position.y >= 0 && position.x < this.dim.x && position.y < this.dim.y)
        {

            fgSprite[position.x, position.y].sprite = spriteSheet[(int)c];
            fgSprite[position.x, position.y].color = HexToColor(foreground);
            bgSprite[position.x, position.y].sprite = spriteSheet[219];
            bgSprite[position.x, position.y].color = HexToColor(background);
        }
    }

    public override void at(int x, int y, char c, string foreground, string background)
    {
        base.at(x, y, c, foreground, background);

        if (x >= 0 && y >= 0 && x < this.dim.x && y < this.dim.y)
        {
            fgSprite[x, y].sprite = spriteSheet[(int)c];
            fgSprite[x, y].color = HexToColor(foreground);
            bgSprite[x, y].sprite = spriteSheet[219];
            bgSprite[x, y].color = HexToColor(background);
        }

    }

    public override void init()
    {
        
        for (int y = 0; y < this.dim.y; y++)
        {
            for (int x = 0; x < this.dim.x; x++)
            {
                fgSprite[x, y] = new GameObject().AddComponent<SpriteRenderer>();
                fgSprite[x, y].transform.position = new Vector3(x * .18f, -y * .32f, 0);
                fgSprite[x, y].name = $"fg {x} {y}";
                bgSprite[x, y] = new GameObject().AddComponent<SpriteRenderer>();
                bgSprite[x, y].transform.position = new Vector3(x * .18f, -y * .32f, 0.1f);
                bgSprite[x, y].name = $"bg {x} {y}";
            }
        }
        base.init();

    }
    public override void clear()
    {
        base.clear();
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                this.at(x, y, this.tpoint[x, y].c, this.tpoint[x, y].foreground, this.tpoint[x, y].background);
                fgSprite[x, y].sprite = spriteSheet[0];
                fgSprite[x, y].color = HexToColor(this.tpoint[x, y].foreground);
                bgSprite[x, y].sprite= spriteSheet[219];
                bgSprite[x, y].color = HexToColor(this.tpoint[x, y].background);
            }
        }
    }

    public static Color HexToColor(string hex)
    {
        Color newCol;
        ColorUtility.TryParseHtmlString(hex, out newCol);
        return newCol;

    }

    public static GTerm StockTerm(Sprite[] sheet)
    {
        return new GTerm(GTerm.StockDim(), sheet);
    }
}

public class TestTerm
{
    public static void test(ITerm term)
    {
        term.init();
        term.at(0, 0, 'c', ColorHex.White, ColorHex.Black);

        term.txt(1, 1, "Testing...", ColorHex.Blue, ColorHex.YellowDark);

    }

    public static void test2(ITerm term, string str)
    {
        //term.init();
        for (int y = 0; y < term.dim.y; y++)
        {
            for (int x = 0; x < term.dim.x; x++)
            {
                int n = (5 * y * x + 3 * x);
                int nc = (n % 26) + 'a';
                char c = (char)nc;
                string bg = '#' + Convert.ToString(((n + 0) % 16),16) + Convert.ToString(((n + 5) % 16),16) + Convert.ToString(((n + 10) % 16),16);
                term.at(x, y, c, ColorHex.White, bg);
            }
        }

        term.txt(2, 1, "##.##", ColorHex.White, ColorHex.Black);
        term.txt(2, 2, "#@.k!", ColorHex.White, ColorHex.Black);
        term.txt(2, 3, str, ColorHex.Yellow, ColorHex.RedDark);
    }
}
