using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField]private ContainerCounter containerCounter;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
