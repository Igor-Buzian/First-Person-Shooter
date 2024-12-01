
using UnityEngine;
using Cinemachine;

public class FirstPersonView : MonoBehaviour
{
    public Transform characterBody; // ���� ���������
    public CinemachineVirtualCamera cinemachineCamera; // ������ �� ����������� ������

    [Header("Camera Rotation Limits")]
    public float minVerticalAngle = -45f; // ����������� ���� ������� ������
    public float maxVerticalAngle = 45f;  // ������������ ���� ������� ������

    private CinemachinePOV cinemachinePOV; // ���������� �������� ������

    private void Start()
    {
        // �������� ��������� POV �� Cinemachine Virtual Camera
        cinemachinePOV = cinemachineCamera.GetCinemachineComponent<CinemachinePOV>();
        if (cinemachinePOV == null)
        {
            Debug.LogError("CinemachinePOV ��������� �� ������! ��������� ��������� ������.");
        }
    }

    private void LateUpdate()
    {
        // ������������ ������������ ���� �������� ������
        if (cinemachinePOV != null)
        {
            cinemachinePOV.m_VerticalAxis.Value = Mathf.Clamp(cinemachinePOV.m_VerticalAxis.Value, minVerticalAngle, maxVerticalAngle);
        }

        // ������������ ���� ��������� ������ �� �������������� ���
        RotateCharacterWithCamera();
    }

    private void RotateCharacterWithCamera()
    {
        // �������� ������� �������������� ���� �������� ������
        float cameraYaw = cinemachinePOV.m_HorizontalAxis.Value;

        // ������������ ���� ��������� �� �������������� ��� (Y)
        characterBody.rotation = Quaternion.Euler(0, cameraYaw, 0);
    }
}
