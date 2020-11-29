using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerController : MonoBehaviour
{
    [SerializeField]
    private float TouchSpeed = 10f;

    [SerializeField]
    private Vector3 Clamps;

    private Touch _touch;
    private float _pickerSpeed;

    private void Start()
    {
        _pickerSpeed = GameManager.Instance.PickerSpeed;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.LockPicker)
        {
            transform.Translate(transform.forward * Time.deltaTime * _pickerSpeed);

#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                var mouseX = Input.GetAxis("Mouse X");
                var mouseY = Input.GetAxis("Mouse Y");

                var touch = Vector3.Slerp(transform.position, transform.position + new Vector3(mouseX, 0f, mouseY), TouchSpeed * Time.deltaTime);
                transform.position = new Vector3(Mathf.Clamp(touch.x, -Clamps.x, Clamps.x), transform.position.y, transform.position.z);
            }
#else
if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Moved)
                transform.position = new Vector3(transform.position.x + _touch.deltaPosition.x * Speed, transform.position.y, transform.position.z + _touch.deltaPosition.y * Speed);
        }
#endif
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "EndPoint")
        {
            GameManager.Instance.LockPicker = true;
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (other.tag == "Modifier")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            if (GameManager.Instance.LockPicker)
                other.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
        }
    }
}
