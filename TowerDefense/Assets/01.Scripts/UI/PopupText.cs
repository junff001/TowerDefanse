using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText
{
    public string text;

    public int maxSize;
    public Color textColor;

    public Vector2 dir;
    public float moveTime;

    public float duration;

    public PopupText()
    {
        maxSize = 30;
        textColor = new Color(1, 252 / 255f, 172 / 255f); // ¿¬ÇÑ ³ë¶û»ö

        dir = new Vector2(0, 100);
        moveTime = 1.5f;

        duration = 3;
    }

    public PopupText(string _text)
    {
        text = _text;

        maxSize = 30;
        textColor = new Color(1, 252 / 255f, 172 / 255f); // ¿¬ÇÑ ³ë¶û»ö

        dir = new Vector2(0, 100);
        moveTime = 1.5f;

        duration = 3;
    }
}
