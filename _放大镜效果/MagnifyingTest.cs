using UnityEngine;
using System.Collections;

public class MagnifyingTest : MonoBehaviour
{
    public Camera m_MagnifyingCamera;
    public GameObject m_cloth;
    public GameObject m_Magnifier;
    public Transform m_MagnifierCenter;
    Camera m_MainCamera;
    float mZ;
    //float camera_z;
    // Use this for initialization
    void Start()
    {
        m_MainCamera = Camera.main;
        mZ = m_cloth.transform.position.z - m_MainCamera.transform.position.z;
        //camera_z = m_MagnifyingCamera.transform.position.z;
    }

    Vector3 tMainPos;
    //void Update()
    //{
    //    tMainPos = Input.mousePosition;
    //    tMainPos.z = mZ;
    //    tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
    //    tMainPos.z = camera_z;
    //    m_MagnifyingCamera.transform.position = tMainPos;

    //    tMainPos = Input.mousePosition;
    //    tMainPos.z = -7.5f - m_MainCamera.transform.position.z;
    //    tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
    //    m_Magnifier.transform.position = tMainPos - Vector3.up * 0.065f;
    //}

    void Update()
    {
        tMainPos = Input.mousePosition;
        tMainPos.z = -7.5f - m_MainCamera.transform.position.z;
        tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
        m_Magnifier.transform.position = tMainPos;

        tMainPos = m_MagnifierCenter.position;
        tMainPos = m_MainCamera.WorldToScreenPoint(tMainPos);
        tMainPos.z = mZ;
        tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
        tMainPos.z = -7f;
        m_MagnifyingCamera.transform.position = tMainPos;
    }

    //void Update()
    //{
    //    tMainPos = Input.mousePosition;
    //    tMainPos.z = 0f;
    //    tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
    //    tMainPos.z = -7.9f;
    //    m_MagnifyingCamera.transform.position = tMainPos;

    //    tMainPos = Input.mousePosition;
    //    tMainPos.z = -7.5f - m_MainCamera.transform.position.z;
    //    tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
    //    m_Magnifier.transform.position = tMainPos - Vector3.up * 0.065f;
    //}

    //void Update()
    //{
    //    tMainPos = Input.mousePosition;
    //    tMainPos.z = mZ - 0.5f;
    //    tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
    //    m_MagnifyingCamera.transform.position = tMainPos;
    //    aa(m_MainCamera.transform.position, m_MagnifyingCamera.transform.position);

    //    tMainPos = Input.mousePosition;
    //    tMainPos.z = -7.5f - m_MainCamera.transform.position.z;
    //    tMainPos = m_MainCamera.ScreenToWorldPoint(tMainPos);
    //    m_Magnifier.transform.position = tMainPos - Vector3.up * 0.065f;

    //}

    void SetLook(Vector3 pStart, Vector3 pMiddle)
    {
        var tEnd = new Vector3(2f * pMiddle.x - pStart.x, 2f * pMiddle.y - pStart.y, 2f * pMiddle.z - pStart.z);
        m_MagnifyingCamera.transform.LookAt(tEnd);
    }

}
