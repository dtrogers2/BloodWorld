using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgLog
{
    public Msglist queue = new Msglist();
    public List<Msglist> archive = new List<Msglist>();

    public void msg(Msg s, float time, bool flash = false)
    {
        if (archive.Count == 0)
            archive.Add(newLine(time));
        else
        {
            if (time == archive[archive.Count - 1].time) archive[archive.Count - 1].msgs.Add(s);
            else
            {
                archive.Add(newLine(time));
                archive[archive.Count - 1].msgs.Add(s);
            }
        }
        if (flash) this.queue.msgs.Add(s);
    }

    public Msglist newLine(float time)
    {
        Msglist list = new Msglist { time = time};
        //Msg startLine = new Msg { color = ColorHex.Gray, text = "_" };
        //list.msgs.Add(startLine);
        //archive.Add(list);
        return list;
    }

    //public void addMsg(Msg msg)
    //{
    //    curMsgs.msgs.Add(msg);
    //}


    public void dequeue()
    {
        Msg msg = queue.msgs[0];
        if (queue.msgs.Count > 0) this.queue.msgs.RemoveAt(0);
    }
    public Msg top()
    {
        return this.empty() ? new Msg { color = COLOR.White, text = ""} : this.queue.msgs[0];
    }
    public bool queuedMsgs()
    {
        return queue.msgs.Count > 1;
    }
    public bool empty()
    {
        return queue.msgs.Count == 0;
    }
    public int len() { return queue.msgs.Count; }
    public void clearQueue()
    {
        queue.msgs.Clear();
    }
}


public struct Msg
{
    public COLOR color;
    public string text;
}

public class Msglist
{
    public List<Msg> msgs = new List<Msg>();
    public float time;
}
