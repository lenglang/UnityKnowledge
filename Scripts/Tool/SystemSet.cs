using UnityEngine;
using System.Collections;
using UnityEditor;

public class SystemSet{

#if UNITY_EDITOR
    public static bool _isDebugBuild = EditorUserBuildSettings.development;
#else
    public static bool _isDebugBuild = Debug.isDebugBuild;
#endif
}
