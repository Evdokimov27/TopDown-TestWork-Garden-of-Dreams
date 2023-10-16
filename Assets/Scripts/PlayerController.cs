using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public VariableJoystick joystick;
    public float moveSpeed = 1f;
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject deathScreen;
    public GameObject ui;
    public ArmorStats armor; // Ссылка на компонент ArmorStats

    public Slider slider;
    public Image fill; // Заполняющая часть полоски здоровья

    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetHealth(float health)
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = health;
    }
    private void Update()
    {
        SetHealth(currentHealth);
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;
        if ((horizontalInput != 0 || verticalInput != 0) && (horizontalInput > 0.5 || verticalInput > 0.5 || horizontalInput < -0.5 || verticalInput < -0.5))
        {
            anim.SetBool("Walk", true);
            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
            rb.velocity = movement;
        }
        else
        {
            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * 0;

            rb.velocity = movement;
            anim.SetBool("Walk", false);
        }
        
    }
   

    public void ReceiveDamage(float damage)
    {
        if (armor != null)
        {
            Debug.Log(armor.CalculateDamageWithArmor(damage));
            // Используем метод CalculateDamageWithArmor() для учета брони
            damage = armor.CalculateDamageWithArmor(damage);
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        anim.SetTrigger("Death");
        this.gameObject.GetComponent<AimWithJoystick>().enabled = false;
        this.enabled = false;
        ui.active = false;
        deathScreen.active = true;
    }
    public void Restart()
	{
        SceneManager.LoadScene(0);
	}
}
