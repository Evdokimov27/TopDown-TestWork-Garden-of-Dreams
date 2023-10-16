using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Ссылка на объект, за которым следит камера
    public float smoothSpeed = 0.125f; // Скорость следования камеры (чем меньше, тем плавнее)

    private Vector3 offset;  // Смещение между камерой и персонажем

    void Start()
    {
        offset = transform.position - target.position;  // Вычисляем начальное смещение
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;  // Желаемая позиция для камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // Используем Lerp для плавного движения

        transform.position = smoothedPosition;  // Обновляем позицию камеры
    }
}
