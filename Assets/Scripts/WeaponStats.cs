using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public int damage = 10; // Урон оружия
    public float fireRate = 5f; // Скорость стрельбы (выстрелов в секунду)
    public float bulletSpeed = 10f; // Скорость пули
    public Transform firePoint; // Ссылка на точку, откуда будет выпущена пуля
    public GameObject bulletPrefab; // Префаб пули
    public Animator animator; // Анимация
    public AimWithJoystick playerAim; // Скрипт прицела
    public GameObject player; // Персонаж
    public Item ammo; // Патроны
    public TMP_Text ammoCount; // Патроны


    private float nextFireTime = 0f; // Время следующего выстрела
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAim = player.GetComponent<AimWithJoystick>();
        animator = GetComponent<Animator>();
    }

    public void Shooting()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            StartCoroutine(Shoot());
        }
    }
    public void Update()
    {
        if (player.GetComponent<Inventory>().GetItemCount(ammo) > 0)
        {
            ammoCount.text = "Патроны: " + player.GetComponent<Inventory>().GetItemCount(ammo).ToString();
        }
        else ammoCount.text = "Патрон нет";
    }
	IEnumerator Shoot()
    {
        if (player.GetComponent<Inventory>().GetItemCount(ammo) > 0)
        {
            player.GetComponent<Inventory>().DecreaseItemQuantity(ammo, 1);

            animator.SetTrigger("Fire");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);

            Bullet bulletCollisionScript = bullet.GetComponent<Bullet>();
            bulletCollisionScript.weaponStats = this;
            yield return new WaitForSeconds(1f);

            Destroy(bullet);
        }        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            MonsterAI monster = collision.gameObject.GetComponent<MonsterAI>();

            if (monster != null)
            {
                monster.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
