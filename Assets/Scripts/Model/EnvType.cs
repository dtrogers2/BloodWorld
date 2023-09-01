using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public enum ENV
{
    EMPTY = 0,
    WALL = 1,
    FLOOR = 2,
    WALL_NS = 3,
    WALL_WE = 4,
    WALL_NW = 5,
    WALL_NE = 6,
    WALL_SW= 7,
    WALL_SE= 8,
    WALL_N = 9,
    WALL_E = 10,
    WALL_S = 11,
    WALL_W = 12,
    WALL_C = 13,
    WATER = 14,
    PATH = 15,
}

public class Env
{
    public static uint[] envEntities = new uint[Enum.GetNames(typeof(ENV)).Length];
    public static enventry[] entries = new enventry[]
    {
        new enventry { eid = ENV.EMPTY, data = new object [1] {new Glyph { c = ' ', color = ColorHex.Black } }, components =new COMPONENT[1] {  COMPONENT.GLYPH } },
        // 5+ neighbor wall
        new enventry { eid = ENV.WALL, data = new object [1] {new Glyph { c = (char) 219, color = ColorHex.White } }, components = new COMPONENT[1] { COMPONENT.GLYPH }  },
        new enventry { eid = ENV.FLOOR, data = new object [1] {new Glyph { c = '.', color = ColorHex.White }}, components =new COMPONENT[1] {  COMPONENT.GLYPH } },
        // 2 neighbor walls
        new enventry { eid = ENV.WALL_NS, data = new object [1] {new Glyph { c = (char) 205, color = ColorHex.White } },components = new COMPONENT[1] {  COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_WE, data = new object [1]  {new Glyph { c = (char) 186, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_NW, data = new object [1] {new Glyph { c = (char) 201, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_NE, data = new object [1]  {new Glyph { c = (char) 187, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_SW, data = new object [1] {new Glyph { c = (char) 200, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_SE, data = new object [1]  {new Glyph { c = (char) 188, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        // 3 neighbor walls
        new enventry { eid = ENV.WALL_N, data = new object [1] {new Glyph { c = (char) 203, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_S, data = new object [1] {new Glyph { c = (char) 202, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_E, data = new object [1] {new Glyph { c = (char) 204, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WALL_W, data = new object [1] {new Glyph { c = (char) 185, color = ColorHex.White }}, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        // 4 neighbor wall
        new enventry { eid = ENV.WALL_C, data = new object [1] {new Glyph { c = (char) 206, color = ColorHex.White } }, components = new COMPONENT[1] { COMPONENT.GLYPH } },
        new enventry { eid = ENV.WATER, data = new object[2]{ new Glyph {c = (char) 247, color = ColorHex.Blue}, new Stain { stainflags = (uint) STAIN.WATER, isPool = true, amount = 300} }, components = new COMPONENT[2] { COMPONENT.GLYPH, COMPONENT.STAIN } },
        new enventry { eid = ENV.PATH, data = new object [1] {new Glyph { c = ',', color = ColorHex.White }}, components =new COMPONENT[1] {  COMPONENT.GLYPH } },
    };
    public static void init()
    {
        envEntities = new uint[Enum.GetNames(typeof(ENV)).Length];
        for (int i = 0; i < envEntities.Length; i++)
        {
            if (EntityManager.curLength != i) { throw new Exception("Index for stock env entities does not match!!"); }
            uint id = EntityManager.create();
            
            envEntities[i] = id;
            enventry e = getEntry((ENV) id);
            ENTITY.subscribe(id, e.data, e.components);
        }
    }

    public static bool isEnv(uint id)
    {
        return id < envEntities.Length;
    }

    public static uint get(ENV env)
    {
        int index = Array.IndexOf(Enum.GetValues(env.GetType()), env);
        return envEntities[index];
    }

    public static enventry getEntry(ENV id)
    {
        foreach (enventry entry in entries)
        {
            if (entry.eid == id) return entry;
        }

        return entries[0];
    }
}

public struct enventry
{
    public ENV eid;
    public object[] data;
    public COMPONENT[] components;
}
