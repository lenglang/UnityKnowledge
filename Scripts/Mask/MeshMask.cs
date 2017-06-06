/* ==============================================================================
 * 功能描述：利用修改图片顶点原理的Mask(相比Unity自带Mask少消耗1-2个DrawCall)
 *           与MeshImage配合使用
 * 用法：1、类似Unity自带的Mask，在Image组件拖放好Mask图片后，点击MeshMask组件菜单->生成Mesh Mask即可。子GO的Image将会被自动附上Mesh Image组件并修改顶点，变成遮罩后的形状。
 *       2、可以对PolygonCollider2D组件进行形状微调，点击MeshMask组件菜单->更新Mesh Mask。遮罩形状将刷新
 * 创 建 者：shuchangliu
 * ==============================================================================*/

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Sprites;

[AddComponentMenu("UI/Custom/Mesh Mask")]
[RequireComponent(typeof(Image), typeof(PolygonCollider2D))]
public class MeshMask : BaseMeshEffect{

    //----------------------Inspector字段-------------
    [ReadOnly]
    [Tooltip("根据PolygonCollider2D自动算出")]
    public List<int> triangles;
    [ReadOnly]
    [Tooltip("根据PolygonCollider2D自动算出")]
    public List<Vector2> vertices;
    //-----------------------------------------------

    //private List<UIVertex> _uiVertices = new List<UIVertex>();

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

    private PolygonCollider2D _polygon2d;
    public PolygonCollider2D polygon2d
    {
        get
        {
            if (_polygon2d == null)
            {
                _polygon2d = this.GetComponent<PolygonCollider2D>();
            }
            return _polygon2d;
        }
    }
    

    public override void ModifyMesh(VertexHelper vh)
    {
        if (this.enabled)
        {
            vertices = polygon2d.GetPath(0).ToList();
            //vh.Clear();
            //if (vertices != null && triangles != null)
            //{
            //    float tw = image.rectTransform.rect.width;
            //    float th = image.rectTransform.rect.height;
            //    Vector4 uv = image.overrideSprite != null ? DataUtility.GetOuterUV(image.overrideSprite) : Vector4.zero;
            //    float uvCenterX = (uv.x + uv.z) * 0.5f;
            //    float uvCenterY = (uv.y + uv.w) * 0.5f;
            //    float uvScaleX = (uv.z - uv.x) / tw;
            //    float uvScaleY = (uv.w - uv.y) / th;

            //    for (int i = 0; i < vertices.Count; i++)
            //    {
            //        UIVertex v = new UIVertex();
            //        v.color = image.color;
            //        //v.position = new Vector2(vertices[i].x * tw, vertices[i].y * tw);
            //        v.position = vertices[i];
            //        v.uv0 = new Vector2(v.position.x * uvScaleX + uvCenterX, v.position.y * uvScaleY + uvCenterY);
            //        _uiVertices.Add(v);
            //    }

            //    vh.AddUIVertexStream(_uiVertices, triangles);
            //}

            List<Image> childs = this.GetComponentsInChildren<Image>().ToList();
            foreach (var child in childs)
            {
                if (child.gameObject != this.gameObject)
                {
                    MeshImage mi = null;
                    if (child.gameObject.GetComponent<MeshImage>() != null)
                    {
                        mi = child.gameObject.GetComponent<MeshImage>();
                        mi.mask = this;
                    }

                    MeshButton mb = null;
                    if (child.gameObject.GetComponent<MeshButton>() != null)
                    {
                        mb = child.gameObject.GetComponent<MeshButton>();
                        mb.mask = this;
                    }
                    
                }
            }
        }
    }

    #region ContextMenu
    [ContextMenu("根据Image组件生成Mask")]
    private void CreateMeshMaskByImage()
    {
#if UNITY_EDITOR
        GameObject o = Selection.activeGameObject;
        if (o != null && o.GetComponent<Image>() != null)
        {
            Image img = o.GetComponent<Image>();
            string path = AssetDatabase.GetAssetPath(img.mainTexture);
            if (!string.IsNullOrEmpty(path))
            {
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                TextureImporterSettings cacheSettings = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(cacheSettings);

                //将Texture临时设置为可读写
                TextureImporterSettings tmp = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(tmp);
                tmp.readable = true;
                textureImporter.SetTextureSettings(tmp);
                AssetDatabase.ImportAsset(path);

                SobelEdgeDetection sobel = new SobelEdgeDetection();

                Texture2D targetT2d = sobel.Detect(img.mainTexture as Texture2D);

                List<Vector2> vertices = EdgeUtil.GetPoints(targetT2d);

                for (int i = 0; i < vertices.Count; i++)
                {
                    Vector3 vec = vertices[i];
                    vec.x -= targetT2d.width * 0.5f;
                    vec.y -= targetT2d.height * 0.5f;
                    //vec.x /= targetT2d.width;
                    //vec.y /= targetT2d.height;
                    vertices[i] = vec;
                }

                //恢复Texture设置
                textureImporter.SetTextureSettings(cacheSettings);
                AssetDatabase.ImportAsset(path);

                MeshMask mm = o.GetComponent<MeshMask>();
                PolygonCollider2D polygon2d = o.GetComponent<PolygonCollider2D>();
                if (mm == null)
                {
                    mm = o.AddComponent<MeshMask>();
                }
                if (polygon2d == null)
                {
                    polygon2d = o.AddComponent<PolygonCollider2D>();
                }

                vertices = vertices.Select(p =>
                {
                    return new Vector2(p.x / img.mainTexture.width * image.rectTransform.rect.width, p.y / img.mainTexture.height * image.rectTransform.rect.height);
                }).ToList();
                polygon2d.SetPath(0, vertices.ToArray());

                mm.vertices = polygon2d.GetPath(0).ToList();
                if (mm.vertices[0] == mm.vertices[vertices.Count - 1])
                    mm.vertices.RemoveAt(mm.vertices.Count - 1); 

                Stopwatch sw = new Stopwatch();
                sw.Start();
                TriangleMath tr = new TriangleMath(mm.vertices.ToArray());
                mm.triangles = tr.Triangulate().ToList();
                sw.Stop();
                UnityEngine.Debug.Log("三角化耗时:" + sw.ElapsedMilliseconds + "ms");

                AssetDatabase.Refresh();
            }
            else
            {
                UnityEngine.Debug.LogError("Image组件还没设置图片");
            }
        }
        else
        {
            UnityEngine.Debug.LogError("选中物体需要有Image组件才能生成PolygonImage");
        }
#endif
    }

    /// <summary>
    /// 刷新Mesh数据
    /// </summary>
    [ContextMenu("根据Collider组件生成Mask")]
    private void CreateMeshMaskByPolygonCollider2D()
    {
#if UNITY_EDITOR
        //把顶点归一化
        vertices = polygon2d.GetPath(0).ToList();
        if(vertices[0] == vertices[vertices.Count-1])
            vertices.RemoveAt(vertices.Count - 1);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        TriangleMath tr = new TriangleMath(vertices.ToArray());
        triangles = tr.Triangulate().ToList();
        sw.Stop();
        UnityEngine.Debug.Log("三角化耗时:" + sw.ElapsedMilliseconds + "ms");
#endif
    }
    #endregion

}
