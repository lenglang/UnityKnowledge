using UnityEngine;
namespace WZK
{
    public static class SkinnedMeshRendererExtension
    {
        public static void SetBlendShapeWeight(this SkinnedMeshRenderer skinnedMeshRenderer, string blendShapeName, float value)
        {
            int index = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
            skinnedMeshRenderer.SetBlendShapeWeight(index, value);
        }

        public static float GetBlendShapeWeight(this SkinnedMeshRenderer skinnedMeshRenderer, string blendShapeName)
        {
            int index = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
            return skinnedMeshRenderer.GetBlendShapeWeight(index);
        }
    }
}
