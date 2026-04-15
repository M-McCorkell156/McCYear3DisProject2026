using UnityEngine;
using TMPro;

public class GridMoverHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private int damageAmount = 1;   

    [SerializeField] private TextMeshProUGUI thisPlayersHealth;
    [SerializeField] private ChangeSelectedCharacter changeSelectedCharacter;
    [SerializeField] private Animator animitor;
    [SerializeField] private WinConditionUI winConditionUI;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {

        thisPlayersHealth.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }   

    public void TakeDamage()
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerDead();
        }
        else
        {
            animitor.SetBool("IsHurt",true);
        }
        UpdateHealthUI();
        animitor.SetBool("IsHurt", false);
    }

    private void PlayerDead()
    {
        //Debug.Log("Player is Dead");
        winConditionUI.AddDeathCount();

        animitor.SetBool("IsDead", true);
        changeSelectedCharacter.ChangeCharacter();
        changeSelectedCharacter.LockSwitching();
    }
}
