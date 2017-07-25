using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(fileName ="对象池",menuName = "创建对象池",order =1000)]
public class GameObjectPoolList : ScriptableObject
{//继承自ScriptableObject 表示吧类GameObjectPoolList变成可以自定义资源配置的文件
    public List<GameObjectPool> poolList;
}