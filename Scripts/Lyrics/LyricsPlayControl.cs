using UnityEngine;
using System.Collections;

public class LyricsPlayControl:MonoBehaviour
{
    private static LyricsPlayControl instance;
    public static LyricsPlayControl Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("LyricsPlayControl");
                instance=obj.AddComponent<LyricsPlayControl>();
            }
            return instance;
        }
    }
}
