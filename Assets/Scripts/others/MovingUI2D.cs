using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI2D : MonoBehaviour
{
    public float delta = 10f;
    public float _time = 3f;
    public float angleLimit = 3f;

    private Vector2 prevVector;
    private Vector2 nativeVector;
    private RectTransform rectTransform;    
    

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        nativeVector = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        StartCoroutine(playMoving());
    }

    private IEnumerator playMoving()
    {
        Vector2 _vector = Vector2.zero;
        prevVector = Vector2.zero;
        float angle = 0;

        while (true)
        {
            while (_vector == prevVector)
            {
                int rnd = UnityEngine.Random.Range(0, 5);

                switch (rnd)
                {
                    case 0:
                        _vector = Vector2.down;
                        angle = UnityEngine.Random.Range(-angleLimit, angleLimit);
                        break;

                    case 1:
                        _vector = Vector2.up;
                        angle = 0;
                        break;

                    case 2:
                        _vector = Vector2.left;
                        angle = UnityEngine.Random.Range(-angleLimit, angleLimit);
                        break;

                    case 3:
                        _vector = Vector2.right;
                        angle = UnityEngine.Random.Range(-angleLimit, angleLimit);
                        break;

                    case 4:
                        _vector = Vector2.zero;
                        angle = 0;
                        break;
                }
            }

            prevVector = _vector;

            rectTransform.DORotate(new Vector3(0,0,angle), _time);
            rectTransform.DOAnchorPos(nativeVector + _vector * delta, _time);
            yield return new WaitForSeconds(_time);
        }
    }

    
}
