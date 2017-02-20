using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SystemSet{

#if UNITY_EDITOR
    public static bool _isDebugBuild = EditorUserBuildSettings.development;
#else
    public static bool _isDebugBuild = Debug.isDebugBuild;
#endif
}
