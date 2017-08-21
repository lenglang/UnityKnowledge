using UnityEngine;
using UnityEngine.UI;
namespace WZK
{
    public static class RawImageExtension
    {
        public static void SetuvRectX(this RawImage rawImage, float x)
        {
            Rect rect = rawImage.uvRect;
            rect.x = x;
            rawImage.uvRect = rect;
        }

        public static void SetuvRectY(this RawImage rawImage, float y)
        {
            Rect rect = rawImage.uvRect;
            rect.y = y;
            rawImage.uvRect = rect;
        }

        public static void SetuvRectWidth(this RawImage rawImage, float width)
        {
            Rect rect = rawImage.uvRect;
            rect.width = width;
            rawImage.uvRect = rect;
        }

        public static void SetuvRectHeight(this RawImage rawImage, float height)
        {
            Rect rect = rawImage.uvRect;
            rect.height = height;
            rawImage.uvRect = rect;
        }
    }
}
