using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesPack : MonoBehaviour
{
    public Sprite[] pack1;

    public Sprite[] pack2;
     
    public Sprite[] backgrounds;

    public Pack[] packs;

    public Sprite[] GetRandomPack(int Amount, int Diff)
    {
        if (packs.Length == 0)
        {
            return pack2;
        }
        else
        {
            List<Pack> candidates = new List<Pack>();

            for (int i = 0; i < packs.Length; i++)
            {
                if (packs[i].ID == Globals.PreviousPackIDNumber) continue;
                if (packs[i].sprites.Length < Amount) continue;
                if (MathF.Abs(packs[i].PackDifficulty - Diff) > 1) continue;

                candidates.Add(packs[i]);
            }

            int value = UnityEngine.Random.Range(0, candidates.Count);
            Pack pack = candidates[value];
            Globals.PreviousPackIDNumber = pack.ID;

            print("used pack with ID: " + pack.ID + ", difficulty: " + Diff);
            return pack.sprites;
        }
    }

    public Sprite GetRandomBackGround()
    {
        return backgrounds[UnityEngine.Random.Range(0, backgrounds.Length)];
    }

}
