using System.Collections;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(_Blinking(GetComponent<SpriteRenderer>()));
    }

    public void StartBlinking()
    {
        StartCoroutine(_Blinking(GetComponent<SpriteRenderer>()));
    }

    public void StopBlinking()
    {
        StopCoroutine(_Blinking(GetComponent<SpriteRenderer>()));
    }

    private IEnumerator _Blinking(SpriteRenderer spriteRenderer)
    {
        Color color = spriteRenderer.color;
        float alpha = 1.0f;
        while (true)
        {
            color.a = Mathf.MoveTowards(color.a, alpha, Time.deltaTime);
            spriteRenderer.color = color;
            if(color.a == alpha)
            {
                if (alpha == 1.0f)
                    alpha = 0.0f;
                else
                    alpha = 1.0f;
            }
            yield return null;
        }
    }
}
