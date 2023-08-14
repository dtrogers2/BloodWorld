using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D spriteSheet;
    private EventManager eventManager;
    private bool keyReleased = true;
    void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheet.name);
        //Term term = new GTerm(Term.StockDim(), sprites);
        //TestTerm.test(term);
        //this.eventManager = EventManager.runScreen(new RawTestScreen(), sprites);
        //this.eventManager = MapScreen.runMapScreen(TestMap.test(Term.StockDim(), new Rng(42)), sprites);
        this.eventManager = ScreenMaker.Gfirst(new Build(), sprites);

    }

    private void Update()
    {
        if (!keyReleased && !Input.anyKey) { keyReleased = true; }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && keyReleased)
        {
            keyReleased = false;
            if (e.keyCode != KeyCode.None) this.eventManager.onKey(e.keyCode);
        }
    }

}
