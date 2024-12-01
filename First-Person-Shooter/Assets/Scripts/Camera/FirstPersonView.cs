
using UnityEngine;
using Cinemachine;

public class FirstPersonView : MonoBehaviour
{
    public Transform characterBody; // Тело персонажа
    public CinemachineVirtualCamera cinemachineCamera; // Ссылка на виртуальную камеру

    [Header("Camera Rotation Limits")]
    public float minVerticalAngle = -45f; // Минимальный угол наклона камеры
    public float maxVerticalAngle = 45f;  // Максимальный угол наклона камеры

    private CinemachinePOV cinemachinePOV; // Контроллер вращения камеры

    private void Start()
    {
        // Получаем компонент POV из Cinemachine Virtual Camera
        cinemachinePOV = cinemachineCamera.GetCinemachineComponent<CinemachinePOV>();
        if (cinemachinePOV == null)
        {
            Debug.LogError("CinemachinePOV компонент не найден! Проверьте настройку камеры.");
        }
    }

    private void LateUpdate()
    {
        // Ограничиваем вертикальный угол вращения камеры
        if (cinemachinePOV != null)
        {
            cinemachinePOV.m_VerticalAxis.Value = Mathf.Clamp(cinemachinePOV.m_VerticalAxis.Value, minVerticalAngle, maxVerticalAngle);
        }

        // Поворачиваем тело персонажа только по горизонтальной оси
        RotateCharacterWithCamera();
    }

    private void RotateCharacterWithCamera()
    {
        // Получаем текущий горизонтальный угол вращения камеры
        float cameraYaw = cinemachinePOV.m_HorizontalAxis.Value;

        // Поворачиваем тело персонажа по горизонтальной оси (Y)
        characterBody.rotation = Quaternion.Euler(0, cameraYaw, 0);
    }
}
