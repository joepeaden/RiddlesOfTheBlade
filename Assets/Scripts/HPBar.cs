using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] RectTransform foreground;

    private float maxWidth;

    private void Start()
    {
        maxWidth = foreground.rect.width;

        Player.Instance.Character.OnGetHit.AddListener(UpdateBar);
    }

    private void OnDestroy()
    {
        Player.Instance.Character.OnGetHit.RemoveListener(UpdateBar);
    }

    public void UpdateBar(int currentVal, int maxVal)
    {
        float ratio = ((float)currentVal) / ((float) maxVal);
        //foreground.rect.Set(foreground.rect.x, foreground.rect.y, ratio*maxWidth, foreground.rect.height);

        foreground.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ratio*maxWidth);

    }
}
