using UnityEngine;
using TMPro;

public class GridMoverHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private int damageAmount = 1;   

    [SerializeField] private TextMeshProUGUI thisPlayersHealth;

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
        UpdateHealthUI();
    }

    private void PlayerDead()
    {
        // Handle player death (e.g., trigger game over, respawn, etc.)
    }
}
