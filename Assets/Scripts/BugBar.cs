using System;
using UnityEngine;
using UnityEngine.UI;

public class BugBar : MonoBehaviour
{
    [Header("Bar Component")] 
    [SerializeField] private Image imageBarComponent;

    [Header("Entity Component")] 
    [SerializeField] private HeroMovement heroMovement;
    [SerializeField] private HeroAttack heroAttack;
    [SerializeField] private Transform characterTransform;

    [Header("Teleporter")] 
    [SerializeField] private Transform teleporterArene;
    
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
        if (!heroAttack.AttackMode)
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
        
            if(Mathf.Approximately(imageBarComponent.fillAmount, 1f))
            {
                characterTransform.position = teleporterArene.position;
                heroAttack.AttackMode = true;
            }   
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
