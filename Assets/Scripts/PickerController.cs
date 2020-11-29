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

            if (Input.GetMouseButton(0))
            {
                var mouseX = Input.GetAxis("Mouse X");
                var mouseY = Input.GetAxis("Mouse Y");

                var touch = Vector3.Slerp(transform.position, transform.position + new Vector3(mouseX, 0f, mouseY), TouchSpeed * Time.deltaTime);
                transform.position = new Vector3(Mathf.Clamp(touch.x, -Clamps.x, Clamps.x), transform.position.y, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndPoint")
        {
            Debug.Log(other.tag);
            other.gameObject.SetActive(false);
            GameManager.Instance.LockPicker = true;
            StartCoroutine(GameManager.Instance.CheckGameOver());
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
