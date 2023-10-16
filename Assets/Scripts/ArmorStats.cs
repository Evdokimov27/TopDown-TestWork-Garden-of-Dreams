using UnityEngine;

public class ArmorStats : MonoBehaviour
{
    [System.Serializable]
    public class ArmorItem
    {
        public GameObject armorObject; // Объект брони
        public float armorValue = 10f; // Значение брони для этого элемента
    }

    public ArmorItem[] armorItems; // Массив объектов брони с их значениями брони

 
    public float CalculateDamageWithArmor(float damage)
    {
        float totalDamageReduction = 0; // Общее снижение урона в процентах
        foreach (var armorItem in armorItems)
        {
            if (armorItem.armorObject.activeSelf)
            {
                totalDamageReduction += armorItem.armorValue;
            }
        }

        float damageMultiplier = 1f - (totalDamageReduction / 100f);

        return damage * damageMultiplier;
    }
}
