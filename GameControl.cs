using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

    public GameObject gemstone;



    void Start()
    {

        GameObject c = Instantiate(gemstone);
    }

    // Update is called once per frame
    void Update()
    {

    }
}