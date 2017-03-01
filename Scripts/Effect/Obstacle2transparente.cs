using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 脚本功能：当人物主角被障碍物遮挡的时候（从摄像机视角看去），使障碍物半透明化，当主角可见时，恢复障碍物透明度
/// 脚本位置：MainCamera 或者任意一个可以始终存在的游戏对象身上即可
/// 创建时间：2015年12月29日
/// 障碍物Shader使用的是Unity内置的Standard
/// Rendering Mode选择Transparent模式
/// </summary>
public class Obstacle2transparente : MonoBehaviour
{
    // 所有障碍物的Renderer数组
    private List<Renderer> _ObstacleCollider;

    // 人物主角（之后通过名字识别？还是tag？目前手动拖过来）
    public GameObject _target;

    // 临时接收，用于存储
    private Renderer _tempRenderer;
    void Start()
    {
        _ObstacleCollider = new List<Renderer>();
    }
    void Update()
    {
        // 调试使用：红色射线，仅Scene场景可见   
#if UNITY_EDITOR
        Debug.DrawLine(_target.transform.position, transform.position, Color.red);
#endif
        RaycastHit[] hit;
        hit = Physics.RaycastAll(_target.transform.position, transform.position);
        //  如果碰撞信息数量大于0条
        if (hit.Length > 0)
        {   // 设置障碍物透明度为0.5
            for (int i = 0; i < hit.Length; i++)
            {
                _tempRenderer = hit[i].collider.gameObject.GetComponent<Renderer>();
                _ObstacleCollider.Add(_tempRenderer);
                SetMaterialsAlpha(_tempRenderer, 0.5f);
                Debug.Log(hit[i].collider.name);
            }
        }// 恢复障碍物透明度为1
        else
        {
            for (int i = 0; i < _ObstacleCollider.Count; i++)
            {
                _tempRenderer = _ObstacleCollider[i];
                SetMaterialsAlpha(_tempRenderer, 1f);
            }
        }

    }
    // 修改障碍物的透明度
    private void SetMaterialsAlpha(Renderer _renderer, float Transpa)
    {
        // 一个游戏物体的某个部分都可以有多个材质球
        int materialsCount = _renderer.materials.Length;
        for (int i = 0; i < materialsCount; i++)
        {

            // 获取当前材质球颜色
            Color color = _renderer.materials[i].color;

            // 设置透明度（0--1）
            color.a = Transpa;

            // 设置当前材质球颜色（游戏物体上右键SelectShader可以看见属性名字为_Color）
            _renderer.materials[i].SetColor("_Color", color);
        }

    }
}