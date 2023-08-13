using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    [SerializeField] private SpritesPack p;

    private void Start()
    {
        for (int i = 0; i < p.packs.Length; i++)
        {
            for (int j = 0; j < p.packs[i].sprites.Length; j++)
            {
                if (p.packs[i].sprites[j] == null)
                {
                    print("null sprite in: " + p.packs[i].PackName + ", ID: " + p.packs[i].ID);
                    continue;
                }


                for (int g = 0; g < p.packs[i].sprites.Length; g++)
                {
                    if (j==g)
                    {
                        //print(p.packs[i].sprites[j].name);
                        continue;
                    }

                    
                    if (p.packs[i].sprites[j].name == p.packs[i].sprites[g].name)
                    {
                        print("string name: " + p.packs[i].sprites[j].name + ", pack name: " + p.packs[i].name);
                    }
                }
            }
        }
    }
}
