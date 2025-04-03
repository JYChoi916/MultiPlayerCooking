using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have a implemens IHasProgess");
            return;
        }
        hasProgress.OnProgressChanged += HasPrgress_OnProgressChanged;
        barImage.fillAmount = 0;
        gameObject.SetActive(false);
    }

    private void HasPrgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressAmount;
        bool show = e.progressAmount > 0 && e.progressAmount < 1;
        gameObject.SetActive(show);
    }
}
