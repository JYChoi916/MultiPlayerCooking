using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum Mode {
        LookAt,
        LookAtInverse,
        CameraForward,
        CameraForwardInverse
    }

    [SerializeField] private Mode mode;

    // Update is called once per frame
    void LateUpdate()
    {
        switch(mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform.position);
                break;
            case Mode.LookAtInverse:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverse:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
