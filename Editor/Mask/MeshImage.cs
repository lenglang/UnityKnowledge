/* ==============================================================================
 * 功能描述：利用修改图片顶点原理的Mask(相比Unity自带Mask少消耗1-2个DrawCall)
 *           与MeshMask配合使用
 * 用法：把该脚本挂到想要修改遮罩的Image组件GO上          
 * 创 建 者：shuchangliu
 * ==============================================================================*/

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Sprites;


[AddComponentMenu("UI/Custom/Mesh Image")]
[RequireComponent(typeof(Image))]
public class MeshImage : BaseMeshEffect
{
    private List<UIVertex> _uiVertices = new List<UIVertex>();
    
    public MeshMask mask;

    private Image _image;
    public Image image
    {
        get
        {
            if (_image == null)
            {
                _image = this.GetComponent<Image>();
            }
            return _image;
        }
    }

    void Update()
    {
        if (this.transform.hasChanged)
        {
            this.graphic.SetVerticesDirty();
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (this.enabled)
        {
            vh.Clear();
            _uiVertices.Clear();

            if (mask)
            {
                if (mask.vertices != null && mask.triangles != null)
                {
                    float tw = image.rectTransform.rect.width;
                    float th = image.rectTransform.rect.height;
                    Vector4 uv = image.overrideSprite != null ? DataUtility.GetOuterUV(image.overrideSprite) : Vector4.zero;
                    float uvCenterX = (uv.x + uv.z) * image.rectTransform.pivot.x;
                    float uvCenterY = (uv.y + uv.w) * image.rectTransform.pivot.y;
                    float uvScaleX = (uv.z - uv.x) / tw;
                    float uvScaleY = (uv.w - uv.y) / th;

                    List<Vector3> vertices = this.mask.vertices.Select(
                        x => { return this.transform.InverseTransformPoint(this.mask.transform.TransformPoint(x)); }).ToList();

                    for (int i = 0; i < mask.vertices.Count; i++)
                    {
                        UIVertex v = new UIVertex();
                        v.color = image.color;
                        v.position = vertices[i];
                        v.uv0 = new Vector2(v.position.x * uvScaleX + uvCenterX, v.position.y * uvScaleY + uvCenterY);
                        _uiVertices.Add(v);
                    }

                    vh.AddUIVertexStream(_uiVertices, mask.triangles);
                }
            }

        }
    }

}
