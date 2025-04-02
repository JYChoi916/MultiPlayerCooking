using System;
using UnityEditor.Search;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualSelectedObjects;

    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        ShowSelectedCounterVisual(e.selectedCounter == baseCounter);
    }

    private void ShowSelectedCounterVisual(bool show)
    {
        foreach(GameObject obj in visualSelectedObjects)
        {
            obj.SetActive(show);
        }
    }
}
