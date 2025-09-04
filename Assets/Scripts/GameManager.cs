using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Buscar el LoadingScreenManager en la escena
        LoadingScreenManager loadingManager = FindObjectOfType<LoadingScreenManager>();
        if (loadingManager != null)
        {
            loadingManager.RemoveLoadingPanel();
        }
        else
        {
            Debug.LogWarning("No se encontró LoadingScreenManager en la escena.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
