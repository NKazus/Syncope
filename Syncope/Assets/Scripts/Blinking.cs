using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
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

    IEnumerator _Blinking(SpriteRenderer sR)
    {
        Color c = sR.color;
        float alpha = 1.0f;
        while (true)
        {
            c.a = Mathf.MoveTowards(c.a, alpha, Time.deltaTime);
            sR.color = c;
            if(c.a == alpha)
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
