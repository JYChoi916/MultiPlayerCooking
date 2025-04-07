using System.Collections;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    bool isFirstTime = true;

    void Update()
    {
        if (isFirstTime)
        {
            isFirstTime = false;
            Loader.LoadCallback();
        }
    }
}
