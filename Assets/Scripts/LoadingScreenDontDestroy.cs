using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenDontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
