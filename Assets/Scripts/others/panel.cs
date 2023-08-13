using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject blinkEffect;
    [SerializeField] private GameObject matchEffect;

    public int ID;
    public Transform panelTransform;

    public bool IsFaceOn { get 
        {
            return (panelTransform.localEulerAngles.y != 0);
        } }

    public bool IsCompleted { get; private set; }
    public bool IsOpening { get; private set; }
    public bool IsClosing { get; private set; }

    private AudioManager _audioPack;

    public void SetPanelData(int _id, Sprite sprite)
    {
        _audioPack = GameManager.Instance.GetAudio;
        panelTransform = transform;
        Material newMaterial = Instantiate(_renderer.material);
        newMaterial.SetTexture("_MainTex", sprite.texture);
        ID = _id;
        _renderer.material = newMaterial;
        IsCompleted = false;
        blinkEffect.SetActive(false);
        matchEffect.SetActive(false);
    }

    public bool MakeCompleted()
    {
        if (IsCompleted) { return false; }
        IsCompleted = true;
        _audioPack.PlaySound_Success();
        matchEffect.SetActive(true);
        //panelTransform.localEulerAngles = new Vector3(0, 180, 0);
        StartCoroutine(playShowFace());
        return true;
    }

    public bool TryShowFace()
    {
        if (IsFaceOn || IsCompleted) { return false; }
        StartCoroutine(playShowFace());
        return true;
    }

    private IEnumerator playShowFace()
    {
        _audioPack.PlaySound_Click();
        blinkEffect.SetActive(true);

        IsOpening = true;
        IsClosing = false;

        panelTransform.DORotate(new Vector3(0, 180, 0), Globals.PanelSimpleRotationSpeed).SetEase(Ease.Linear);        
        yield return new WaitForSeconds(Globals.PanelTimeForShowing);
        blinkEffect.SetActive(false);

        IsOpening = false;

        if (!IsCompleted)
        {
            StartCoroutine(playHideFace());
        }
    }
    private IEnumerator playHideFace()
    {
        _audioPack.PlaySound_BackRotate();

        IsOpening = false;
        IsClosing = true;
        
        panelTransform.DORotate(Vector3.zero, Globals.PanelSimpleRotationSpeed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Globals.PanelTimeForShowing);

        IsClosing = false;
    }

    public void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public bool GetVisibility() => gameObject.activeSelf;

    public static void ArrangePanels(Sprite[] source, int panelsAmount, int similarPanelsAmount, ref List<Panel> panels)
    {
        int uniques = panelsAmount / similarPanelsAmount;

        if (panelsAmount % similarPanelsAmount != 0)
        {
            Debug.LogError("количество карточек не четно количеству одинаковых");
        }

        if (panelsAmount < (similarPanelsAmount * 2))
        {
            Debug.LogError("количество панелей и пар для них не сщвпадают");
        }

        if (uniques > source.Length)
        {
            Debug.LogError("не хватает уникальных текстур");
        }

        List<int> panelsNumberToMark = new List<int>();
        for (int i = 0; i < panelsAmount; i++)
        {
            panelsNumberToMark.Add(i);
        }

        List<int> spritesNumberToSet = new List<int>();
        for (int i = 0; i < source.Length; i++)
        {
            spritesNumberToSet.Add(i);
        }

        for (int i = 0; i < uniques; i++)
        {
            int uniqueID = UnityEngine.Random.Range(0, 1000000);

            int spriteRND = UnityEngine.Random.Range(0, spritesNumberToSet.Count);
            Sprite sprite = source[spritesNumberToSet[spriteRND]];
            spritesNumberToSet.Remove(spritesNumberToSet[spriteRND]);

            for (int j = 0; j < similarPanelsAmount; j++)
            {
                int panelNumber = -1;
                if (panelsNumberToMark.Count > 1)
                {
                    int rnd = UnityEngine.Random.Range(0, panelsNumberToMark.Count);
                    panelNumber = panelsNumberToMark[rnd];
                    panelsNumberToMark.Remove(panelsNumberToMark[rnd]);
                }
                else
                {
                    panelNumber = panelsNumberToMark[0];
                    panelsNumberToMark.Remove(panelsNumberToMark[0]);
                }

                panels[panelNumber].SetPanelData(uniqueID, sprite);
            }
        }

    }

    public static int CreatePanels(int horizontaly, int vertically, GameObject basicPanel, Transform PanelsLocation, ref List<Panel> panels, int additionalPanels)
    {
        int panelsAmount = 0;

        float xAxis = 0;
        float yAxis = 0;
        float zAxis = 0;

        if (horizontaly <= 4)
        {
            zAxis = -4.5f;
        }
        else if (horizontaly <= 5)
        {
            zAxis = -4.7f;
        }
        else if (horizontaly <= 6 && vertically <= 3)
        {
            zAxis = -4.6f;
            //xAxis = 0.2f;
        }
        else if (horizontaly <= 6 && vertically <= 4)
        {
            zAxis = -4.4f;
            //xAxis = 0.2f;
        }        
        else if (horizontaly <= 6 && vertically <= 5)
        {
            zAxis = -4f;
            //xAxis = 0.2f;
        }
        else if (horizontaly <= 7 && vertically <= 3)
        {
            zAxis = -4.4f;
            //xAxis = 0.2f;
        }
        else if (horizontaly <= 8 && vertically <= 5)
        {
            zAxis = -3.1f;
            xAxis = 0.1f;
        }
        else if (horizontaly <= 8 && vertically <= 6)
        {
            zAxis = -3.2f;
            xAxis = 0.1f;
        }
        else if (horizontaly <= 10 && vertically <= 6)
        {
            zAxis = -3.2f;
            xAxis = 0.7f;
        }


        if (horizontaly == 3 && vertically == 2 && additionalPanels == 2)
        {
            zAxis -= zAxis * 0.1f;
        }
        else if (horizontaly == 4 && vertically == 2 && additionalPanels == 2)
        {
            yAxis -= 0.5f;
        }
        else if (horizontaly == 4 && vertically == 2 && additionalPanels == 4)
        {
            zAxis -= zAxis * 0.1f;
        }
        else if (horizontaly == 4 && vertically == 3 && additionalPanels == 2)
        {
            zAxis -= zAxis * 0.1f;
        }
        else if (horizontaly == 4 && vertically == 3 && additionalPanels == 4)
        {
            zAxis -= zAxis * 0.075f;
            yAxis -= 0.5f;
        }
        else if (horizontaly == 4 && vertically == 4 && additionalPanels == 4)
        {
            zAxis -= zAxis * 0.075f;            
        }
        else if (horizontaly == 5 && vertically == 4 && additionalPanels == 4)
        {
            zAxis -= zAxis * 0.25f;
        }
        else if (horizontaly == 6 && vertically == 4 && additionalPanels == 4)
        {
            zAxis -= zAxis * 0.25f;
        }
        else if (horizontaly == 6 && vertically == 5 && additionalPanels == 6)
        {
            zAxis -= zAxis * 0.25f;
        }
        else if (horizontaly == 7 && vertically == 3 && additionalPanels == 3)
        {            
            yAxis -= 0.5f;
        }
        else if (horizontaly == 7 && vertically == 3 && additionalPanels == 6)
        {
            //yAxis -= 0.5f;
        }

        float startX = (float)horizontaly / 2 - 0.5f;
        float startY = (float)vertically / 2 - 0.5f;

        for (int x = 0; x < horizontaly; x++)
        {
            for (int y = 0; y < vertically; y++)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(x - startX + xAxis, y - startY + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
        }

        if (additionalPanels > 0)
        {
            if (horizontaly == 3 && vertically == 2 && additionalPanels == 2)
            {                
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis, startY - 2 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
            else if (horizontaly == 4 && vertically == 2 && additionalPanels == 2)
            {                
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 0.5f, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 0.5f, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
            else if (horizontaly == 4 && vertically == 2 && additionalPanels == 4)
            {                
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 0.5f, startY - 2 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 0.5f, startY - 2 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 0.5f, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 0.5f, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
            else if (horizontaly == 4 && vertically == 3 && additionalPanels == 2)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 2.5f, yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 2.5f, yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
            else if (horizontaly == 4 && vertically == 3 && additionalPanels == 4)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 2.5f, yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 2.5f, yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 0.5f, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 0.5f, startY + 1 + yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
            else if (horizontaly == 4 && vertically == 4 && additionalPanels == 4)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 2.5f, yAxis-0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 2.5f, yAxis+0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 2.5f, yAxis - 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 2.5f, yAxis + 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

            }
            else if (horizontaly == 5 && vertically == 4 && additionalPanels == 4)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 3f, yAxis - 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 3f, yAxis + 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3f, yAxis - 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3f, yAxis + 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

            }
            else if (horizontaly == 6 && vertically == 4 && additionalPanels == 4)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 3.5f, yAxis - 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 3.5f, yAxis + 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3.5f, yAxis - 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3.5f, yAxis + 0.5f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

            }
            else if (horizontaly == 6 && vertically == 5 && additionalPanels == 6)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis - 3.5f, yAxis - 1f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 3.5f, yAxis + 1f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
                g = Instantiate(basicPanel, new Vector3(xAxis - 3.5f, yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3.5f, yAxis - 1f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3.5f, yAxis + 1f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 3.5f, yAxis, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

            }
            else if (horizontaly == 7 && vertically == 3 && additionalPanels == 3)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis, yAxis + 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 1f, yAxis + 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 1f, yAxis + 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;                                
            }
            else if (horizontaly == 7 && vertically == 3 && additionalPanels == 6)
            {
                GameObject g = Instantiate(basicPanel, new Vector3(xAxis, yAxis + 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                Panel p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 1f, yAxis + 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 1f, yAxis + 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis, yAxis - 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis - 1f, yAxis - 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;

                g = Instantiate(basicPanel, new Vector3(xAxis + 1f, yAxis - 2f, zAxis), Quaternion.identity, PanelsLocation);
                g.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-1.5f, 1.5f));
                p = g.GetComponent<Panel>();
                panels.Add(p);
                panelsAmount++;
            }
        }

        return panelsAmount;
    }

}
