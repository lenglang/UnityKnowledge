using UnityEngine;
using System.Collections;

public class CancelWarning : MonoBehaviour
{
    void Cancle()
    {
        if (SystemSet._isDebugBuild) Debug.Log("DebugModel");
    }
}
