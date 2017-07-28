using UnityEngine;


public static class RectTransformExtension
{
    public static void CopyFrom(this RectTransform rectTransform, RectTransform other)
    {
        rectTransform.anchoredPosition = other.anchoredPosition;
        rectTransform.anchoredPosition3D = other.anchoredPosition3D;
        rectTransform.anchorMax = other.anchorMax;
        rectTransform.anchorMin = other.anchorMin;
        rectTransform.offsetMax = other.offsetMax;
        rectTransform.offsetMin = other.offsetMin;
        rectTransform.pivot = other.pivot;

        rectTransform.localScale = other.localScale;

        rectTransform.sizeDelta = other.sizeDelta;
    }

    public static void SetAnchoredX(this RectTransform rectTransform, float x)
    {
        var anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.x = x;
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public static void SetAnchoredY(this RectTransform rectTransform, float y)
    {
        var anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.y = y;
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public static void SetWidth(this RectTransform rectTransform, float x)
    {
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x = x;
        rectTransform.sizeDelta = sizeDelta;
    }

    public static void SetHeight(this RectTransform rectTransform, float y)
    {
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = y;
        rectTransform.sizeDelta = sizeDelta;
    }
}
