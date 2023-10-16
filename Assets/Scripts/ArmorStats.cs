using UnityEngine;

public class ArmorStats : MonoBehaviour
{
    [System.Serializable]
    public class ArmorItem
    {
        public GameObject armorObject; // ������ �����
        public float armorValue = 10f; // �������� ����� ��� ����� ��������
    }

    public ArmorItem[] armorItems; // ������ �������� ����� � �� ���������� �����

 
    public float CalculateDamageWithArmor(float damage)
    {
        float totalDamageReduction = 0; // ����� �������� ����� � ���������
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
