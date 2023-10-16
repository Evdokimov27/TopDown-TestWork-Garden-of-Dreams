using UnityEngine;

public class AimWithJoystick: MonoBehaviour
{
    public Transform player;
    public Transform hands;
    public VariableJoystick variableJoystick; 
    public float maxSearchDistance = 10f; // Максимальное расстояние для поиска мобов
    public float radius;
    public Transform closestMob;

    private bool angularAll = false; // true - Ограничение движения рук слишком вниз 
    private bool reversedScale = false;
    private string mobTag = "Mob"; // Тег мобов

    private void Update()
    {
        player = this.transform;
        float changeX = (float)hands.localScale.x * -1;
        float changeY = (float)hands.localScale.y * -1;
        // Ближайший моба в пределах maxSearchDistance
        closestMob = FindClosestMob();


        if (closestMob != null)
        {
            // Направление к ближайшему мобу
            Vector3 direction = closestMob.position - hands.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Поворот оружия
            hands.rotation = Quaternion.Euler(0, 0, angle);

            // Поворот персонажа в сторону оружия
            Vector3 playerDirection = closestMob.position - player.position;
            if (angle > -90 && angle < 90)
            {
                if (reversedScale)
                {
                    reversedScale = false;

                    player.localScale = new Vector3(-player.localScale.x, player.localScale.y, player.localScale.z);
                    hands.localScale = new Vector3(changeX, changeY, hands.localScale.z);

                }
            }
            else
            {
                if (!reversedScale)
                {
                    reversedScale = true;

                    player.localScale = new Vector3(-player.localScale.x, player.localScale.y, player.localScale.z);
                    hands.localScale = new Vector3(changeX, changeY, hands.localScale.z);

                }
            }
        }
        else
        {
            Rotate();
            if (variableJoystick.Horizontal > 0 && reversedScale)
            {

                player.localScale = new Vector3(-player.localScale.x, player.localScale.y, player.localScale.z);
                hands.localScale = new Vector3(changeX, changeY, hands.localScale.z);
                reversedScale = false;
            }


            if (variableJoystick.Horizontal < 0 && !reversedScale)
            {
                player.localScale = new Vector3(-player.localScale.x, player.localScale.y, player.localScale.z);
                hands.localScale = new Vector3(changeX, changeY, hands.localScale.z);
                reversedScale = true;
            }

        }
    }
    void Rotate()
    {
        if (variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0)
        {

            float joystickAngle = Mathf.Atan2(variableJoystick.Vertical, variableJoystick.Horizontal) * Mathf.Rad2Deg;

            // радиус перемещения оружия
            float clampedRadius = Mathf.Clamp(radius, 0, radius);


            float radians = joystickAngle * Mathf.Deg2Rad;
            //   Vector3 newPosition = new Vector3(startPos.x + Mathf.Cos(radians) * clampedRadius, startPos.y + Mathf.Sin(radians) * clampedRadius, gunTransform.position.z);
            //   
            // новая позицию оружия
            //   gunTransform.position = newPosition;
            if (angularAll)
            {
                if (!(joystickAngle < -50 && joystickAngle > -130))
                {
                    hands.rotation = Quaternion.Euler(0, 0, joystickAngle);
                }
                else
                {
                    if (reversedScale) hands.rotation = Quaternion.Euler(0, 0, -130);
                    if (!reversedScale) hands.rotation = Quaternion.Euler(0, 0, -50);
                }
            }
            else hands.rotation = Quaternion.Euler(0, 0, joystickAngle);
        }
    }
    private Transform FindClosestMob()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag(mobTag);
        Transform closestMob = null;
        float closestDistance = maxSearchDistance;

        foreach (GameObject mob in mobs)
        {
            float distance = Vector3.Distance(hands.position, mob.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMob = mob.transform;
                if(!closestMob.GetComponent<MonsterAI>().isAlive) closestMob = null;
            }
        }

        return closestMob;
    }

}
