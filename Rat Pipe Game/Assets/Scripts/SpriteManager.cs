using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
    private static Dictionary<string, Sprite> pipeSprites = null;
    public static Dictionary<string, Sprite> PipeSprites => pipeSprites;

    private static Dictionary<string, Sprite> indicatorSprites = null;
    public static Dictionary<string, Sprite> IndicatorSprites => indicatorSprites;

    public static void LoadSprites() {
        if(pipeSprites != null) {
            return;
        }

        pipeSprites = new Dictionary<string, Sprite>();
        
        Sprite[] sprites = Resources.LoadAll<Sprite>("Pipes");

        pipeSprites.Add("0,1,0,0,1,0", sprites[0]);
        pipeSprites.Add("0,0,1,1,0,0", sprites[1]);
        pipeSprites.Add("1,0,0,0,0,1", sprites[2]);
        pipeSprites.Add("1,0,1,0,0,0", sprites[3]);
        pipeSprites.Add("1,0,0,1,0,0", sprites[4]);
        pipeSprites.Add("0,0,1,0,0,1", sprites[5]);
        pipeSprites.Add("0,0,0,1,0,1", sprites[6]);
        pipeSprites.Add("0,1,1,0,0,0", sprites[7]);
        pipeSprites.Add("1,1,0,0,0,0", sprites[8]);
        pipeSprites.Add("0,1,0,1,0,0", sprites[9]);
        pipeSprites.Add("0,1,0,0,0,1", sprites[10]);
        pipeSprites.Add("0,0,0,1,1,0", sprites[11]);
        pipeSprites.Add("0,0,0,0,1,1", sprites[12]);
        pipeSprites.Add("1,0,0,0,1,0", sprites[13]);
        pipeSprites.Add("0,0,1,0,1,0", sprites[14]);

        if(indicatorSprites != null) {
            return;
        }

        indicatorSprites = new Dictionary<string, Sprite>();
        
        sprites = Resources.LoadAll<Sprite>("Indicators");

        indicatorSprites.Add("0,-1,0", sprites[0]);
        indicatorSprites.Add("0,0,-1", sprites[1]);
        indicatorSprites.Add("1,0,0", sprites[2]);
        indicatorSprites.Add("-1,0,0", sprites[3]);
        indicatorSprites.Add("0,1,0", sprites[4]);
        indicatorSprites.Add("0,0,1", sprites[5]);

        // pipeSprites
        // pipeSprites.Add(new int[] {0,1,0,0,1,0}, sprites[0]);
        // pipeSprites.Add(new int[] {0,0,1,1,0,0}, sprites[1]);
        // pipeSprites.Add(new int[] {1,0,0,0,0,1}, sprites[2]);
        // pipeSprites.Add(new int[] {1,0,1,0,0,0}, sprites[3]);
        // pipeSprites.Add(new int[] {1,0,0,1,0,0}, sprites[4]);
        // pipeSprites.Add(new int[] {0,0,1,0,0,1}, sprites[5]);
        // pipeSprites.Add(new int[] {0,0,0,1,0,1}, sprites[6]);
        // pipeSprites.Add(new int[] {0,1,1,0,0,0}, sprites[7]);
        // pipeSprites.Add(new int[] {1,1,0,0,0,0}, sprites[8]);
        // pipeSprites.Add(new int[] {0,1,0,1,0,0}, sprites[9]);
        // pipeSprites.Add(new int[] {0,1,0,0,0,1}, sprites[10]);
        // pipeSprites.Add(new int[] {0,0,0,1,1,0}, sprites[11]);
        // pipeSprites.Add(new int[] {0,0,0,0,1,1}, sprites[12]);
        // pipeSprites.Add(new int[] {1,0,0,0,1,0}, sprites[13]);
        // pipeSprites.Add(new int[] {0,0,1,0,1,0}, sprites[14]);
    }
}

// https://stackoverflow.com/questions/14663168/an-integer-array-as-a-key-for-dictionary
public class MyEqualityComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[] x, int[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(int[] obj)
    {
        int result = 17;
        for (int i = 0; i < obj.Length; i++)
        {
            unchecked
            {
                result = result * 23 + obj[i];
            }
        }
        return result;
    }
}