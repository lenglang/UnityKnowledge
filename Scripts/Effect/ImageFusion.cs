using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 脚本挂载到墙上
/// </summary>

public class ImageFusion : MonoBehaviour
{
    public Texture2D bulletTexture;	// 【图片】弹痕 
    private Texture2D wallTexture;		// 【图片】墙
    private Texture2D NewWallTexture;	// 【图片】墙的备份
    private float wall_height;		// 【获取墙和弹痕图片的宽高信息】 
    private float wall_width;
    private float bullet_height;
    private float bullet_width;
    RaycastHit hit;			        // 获取子弹打击点
    private Queue<Vector2> uiQueues;	// 存储像素点信息

    // Use this for initialization
    void Start()
    {
        uiQueues = new Queue<Vector2>();

        wallTexture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        // 【备份墙的图片】
        NewWallTexture = Instantiate(wallTexture);

        GetComponent<MeshRenderer>().material.mainTexture = NewWallTexture;

        wall_height = wallTexture.height;
        wall_width = wallTexture.width;

        bullet_height = bulletTexture.height;
        bullet_width = bulletTexture.width;

        Debug.Log(wall_width);
        Debug.Log(bullet_width);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.name == "Plane")
                {
                    // 得到打击点的图片UV坐标
                    // UV坐标是当前图片宽高的百分比【左下角(0,0)，右上角(1,1)】
                    Vector2 uv = hit.textureCoord;

                    uiQueues.Enqueue(uv);

                    for (int i = 0; i < bullet_width; i++)
                    {
                        for (int j = 0; j < bullet_height; j++)
                        {
                            // 先减去弹痕宽度的一半得到最左边的坐标，依次向右递增
                            float w = uv.x * wall_width - bullet_width / 2 + i;
                            // 同理，从下到上递增
                            float h = uv.y * wall_height - bullet_height / 2 + j;


                            Color wallColor = NewWallTexture.GetPixel((int)w, (int)h);
                            Color bulletColor = bulletTexture.GetPixel(i, j);
                            // 修改弹痕位置的像素为两图的融合颜色，若不相乘融合会使用弹痕原图
                            NewWallTexture.SetPixel((int)w, (int)h, wallColor * bulletColor);
                        }
                    }
                    // 应用图片
                    NewWallTexture.Apply();
                    Invoke("ReturnWall", 3f);
                }
            }
        }
    }
    void ReturnWall()
    {
        // 还原以打击点为原点的图片像素点
        Vector2 uv = uiQueues.Dequeue();

        for (int i = 0; i < bullet_width; i++)
        {
            for (int j = 0; j < bullet_height; j++)
            {
                float w = uv.x * wall_width - bullet_width / 2 + i;
                float h = uv.y * wall_height - bullet_height / 2 + j;

                // 使用原图的像素进行还原
                Color wallColor = wallTexture.GetPixel((int)w, (int)h);
                NewWallTexture.SetPixel((int)w, (int)h, wallColor);
            }
        }
        NewWallTexture.Apply();
    }
}
