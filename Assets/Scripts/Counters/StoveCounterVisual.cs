using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesObject;

    void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveOnGameObject.SetActive(false);
        particlesObject.SetActive(false);
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesObject.SetActive(showVisual);
    }
}
