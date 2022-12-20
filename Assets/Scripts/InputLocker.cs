using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputLocker
{
    public static bool IsLocked
    {
        get
        {
            return tags.Count > 0;
        }
    }

    private static List<string> tags = new List<string>();

    public static void Lock(string tag)
    {
        if (!tags.Contains(tag))
            tags.Add(tag);
    }

    public static void Unlock(string tag)
    {
        tags.Remove(tag);
    }
}
