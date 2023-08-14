using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgLog
{
    public List<string> queue = new List<string>();
    public List<string> archive = new List<string>();

    public void msg(string s, bool flash = false)
    {
        this.archive.Insert(0, s);
        if (flash) this.queue.Insert(0, s);
    }

    public void dequeue()
    {
        string msg = queue[0];
        if (queue.Count > 0) this.queue.RemoveAt(0);
    }
    public string top()
    {
        return this.empty() ? "" : this.queue[0];
    }
    public bool queuedMsgs()
    {
        return queue.Count > 1;
    }
    public bool empty()
    {
        return queue.Count == 0;
    }
    public int len() { return queue.Count; }
    public void clearQueue()
    {
        queue.Clear();
    }
}
