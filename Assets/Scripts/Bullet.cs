using UnityEngine;

public class Bullet: MonoBehaviour
{
    public int damage;
    public WeaponStats weaponStats;

	private void Update()
	{
        damage = weaponStats.damage;
	}
	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            MonsterAI monster = collision.gameObject.GetComponent<MonsterAI>();

            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}