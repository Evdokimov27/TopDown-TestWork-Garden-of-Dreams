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

    private Vector3 movement;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
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

        if (Mathf.Abs(horizontalInput) > 0.5f || Mathf.Abs(verticalInput) > 0.5f)
        {
            anim.SetBool("Walk", true);
            movement = new Vector3(horizontalInput, verticalInput, 0).normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
        else
        {
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
        ui.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
