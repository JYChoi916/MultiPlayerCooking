using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisaul : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO data;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> machingDataList;

    void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach(var machingData in machingDataList)
        {
            machingData.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach(var machingData in machingDataList)
        {
            if (machingData.data == e.kitchenObjectSO)
            {
                machingData.gameObject.SetActive(true);
            }
        }
    }
}
