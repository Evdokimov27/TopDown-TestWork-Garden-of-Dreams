using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public float chaseDistance; // Расстояние, на котором моб начнет преследование
    public float attackDistance; // Расстояние для атаки
    public float attackDelay; // Задержка между атаками
    public int attackDamage; // Урон атаки
    public float distanceToPlayer;
    public float moveSpeed; // Скорость движения 
    public int maxHealth; // Текущее здоровье 
    public int currentHealth; // Текущее здоровье 
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public bool isAlive = true;
    public GameObject DropItemForDeath;

    public Slider slider;
    public Image fill; // Заполняющая часть полоски здоровья

    private bool isChasing = false;
    private bool hasAnimPlay = false;
    private float lastAttackTime;


    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackDelay;
        navMeshAgent.speed = moveSpeed;
    }
    public void SetHealth(float health)
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = health;
    }
    void Update()
    {
        if (isAlive)
        {
            SetHealth(currentHealth);
            Physics2D.IgnoreLayerCollision(8, 8);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition = new Vector3(transform.position.x, transform.position.y, 0);
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < chaseDistance && distanceToPlayer > attackDistance + 0.5f)
            {
                isChasing = true;
                navMeshAgent.destination = player.position;
            }
            else
            {
                isChasing = false;
                navMeshAgent.ResetPath();
            }

            if (distanceToPlayer - 0.5 < attackDistance)
            {
                if (Time.time - lastAttackTime > attackDelay)
                {
                    if (!hasAnimPlay)
                    {
                        hasAnimPlay = true;
                        StartCoroutine(WaitForAnimation());
                    }
                }
            }

            animator.SetBool("Walk", isChasing);

            Vector3 moveDirection = navMeshAgent.velocity;
            if (moveDirection != Vector3.zero)
            {
                // Если движение влево, размер по X на -1
                if (moveDirection.x < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }

            }
        }
    }



    public void DropItem(GameObject itemPrefab)
    {
        float dropChance = 0.6f; // Вероятность выпадения предмета
        if (Random.value <= dropChance)
        {
            if (itemPrefab != null) Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
    }
    IEnumerator WaitForAnimation()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        hasAnimPlay = false;
        Attack();
    }
    void Attack()
    {
        if (distanceToPlayer - 0.5 < attackDistance)
        {
            isChasing = false;
            lastAttackTime = Time.time;
            player.GetComponent<PlayerController>().ReceiveDamage(attackDamage);
        }
    }
    public void TakeDamage(int damage)
    {
        // Отнимаем урон от текущего здоровья
        currentHealth -= damage;

        // Проверяем, не умер ли монстр
        if (currentHealth <= 0)
        {
            StartCoroutine(Die()); // Реализуйте логику смерти монстра
        }
    }
    public IEnumerator Die()
	{
        isAlive = false;
        animator.SetTrigger("Death");
        navMeshAgent.ResetPath();
        this.GetComponent<BoxCollider2D>().enabled = false;
        navMeshAgent.enabled = false;
        this.GetComponent<Rigidbody2D>().simulated = false;
        DropItem(DropItemForDeath);
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
