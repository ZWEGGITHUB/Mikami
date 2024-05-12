using System;
using UnityEngine;
using UnityEngine.UI;

public class BugBar : MonoBehaviour
{
    [Header("Bar Component")] 
    [SerializeField] private Image imageBarComponent;

    [Header("Entity Component")] 
    [SerializeField] private HeroMovement heroMovement;
    [SerializeField] private Transform characterTransform;
    
    private void Start()
    {
        heroMovement.OnBugging += HeroMovement_OnBugging;
        
        imageBarComponent.fillAmount = 0f;
        
        Hide();
    }

    private void Update()
    {
        KeepFlip();
    }

    private void HeroMovement_OnBugging(object sender, HeroMovement.OnBuggingEventArgs e)
    {
        imageBarComponent.fillAmount = e.buggingNumberEvent;
        
        if (Mathf.Approximately(imageBarComponent.fillAmount, 0f) || Mathf.Approximately(imageBarComponent.fillAmount, 1f))
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void KeepFlip()
    {
        Vector3 localScale = characterTransform.localScale;
        
        if (localScale.x == -1)
        {
            imageBarComponent.fillOrigin = 1;
        }

        if (localScale.x == 1)
        {
            imageBarComponent.fillOrigin = 0;
        }
    }
}
