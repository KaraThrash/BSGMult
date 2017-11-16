using UnityEngine;
using System.Collections;

public class RPGCamera : MonoBehaviour
{
    public Transform Target;
    public float ftlCameraEffect;
    public float MaximumDistance;
    public float MinimumDistance;

    public float ScrollModifier;
    public float TurnModifier;

    Transform m_CameraTransform;

    Vector3 m_LookAtPoint;
    Vector3 m_LocalForwardVector;
    float m_Distance;
    public bool canNotRotateTarget;
    void Start()
    {
        m_CameraTransform = transform.GetChild(0);
        m_LocalForwardVector = m_CameraTransform.forward;

        m_Distance = -m_CameraTransform.localPosition.z / m_CameraTransform.forward.z;
        m_Distance = Mathf.Clamp(m_Distance, MinimumDistance, MaximumDistance);
        m_LookAtPoint = m_CameraTransform.localPosition + m_LocalForwardVector * m_Distance;
    }

    void LateUpdate()
    {
        UpdatePosition();
        if (ftlCameraEffect < 0)
        {
            UpdateDistance();
            UpdateZoom();

            UpdateRotation();
        }
        else {
            ftlCameraEffect -= Time.deltaTime;
        }
    }

    void UpdateDistance()
    {
        m_Distance = Mathf.Clamp(m_Distance - Input.GetAxis("Mouse ScrollWheel") * ScrollModifier, MinimumDistance, MaximumDistance);
    }

    void UpdateZoom()
    {
        m_CameraTransform.localPosition = m_LookAtPoint - m_LocalForwardVector * m_Distance;
    }

    void UpdatePosition()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.transform.position;
    }

    void UpdateRotation()
    {
        if (Input.GetMouseButton(0) == true || Input.GetMouseButton(1) == true || Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            //transform.Rotate(Input.GetAxis("Mouse Y") * TurnModifier, Input.GetAxis( "Mouse X" ) * TurnModifier, 0 );
            transform.localEulerAngles = new Vector3((transform.localEulerAngles.x - Input.GetAxis("Mouse Y")), (transform.localEulerAngles.y + Input.GetAxis("Mouse X")), 0);
        }

        if ((Input.GetMouseButton(1) || Input.GetButton("Fire2")) && Target != null)
        {
            //TODO: 
            if (canNotRotateTarget == false) {

                Target.rotation = Quaternion.Euler(Target.parent.eulerAngles.x, transform.localEulerAngles.y, 0);

            }

        }
    }

    public void SetLookTarget(Transform newTarget)
    {
        Target = newTarget;
    }


}
