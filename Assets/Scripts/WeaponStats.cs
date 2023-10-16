using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public int damage = 10; // ���� ������
    public float fireRate = 5f; // �������� �������� (��������� � �������)
    public float bulletSpeed = 10f; // �������� ����
    public Transform firePoint; // ������ �� �����, ������ ����� �������� ����
    public GameObject bulletPrefab; // ������ ����
    public Animator animator; // ��������
    public AimWithJoystick playerAim; // ������ �������
    public GameObject player; // ��������
    public Item ammo; // �������
    public TMP_Text ammoCount; // �������


    private float nextFireTime = 0f; // ����� ���������� ��������
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
            ammoCount.text = "�������: " + player.GetComponent<Inventory>().GetItemCount(ammo).ToString();
        }
        else ammoCount.text = "������ ���";
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
