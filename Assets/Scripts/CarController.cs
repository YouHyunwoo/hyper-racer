using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] int gas = 100;
    [SerializeField] float moveSpeed = 1f;

    public int Gas { get => gas; }
    public float MoveSpeed { get => moveSpeed; }

    public void Move(float direction)
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2f, 2f), transform.position.y, transform.position.z);
    }

    void Start()
    {
        StartCoroutine(GasCoroutine());
    }

    IEnumerator GasCoroutine()
    {
        while (true)
        {
            gas -= 10;
            if (gas <= 0) break;
            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.EndGame();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gas"))
        {
            gas += 30;
            other.gameObject.SetActive(false);
        }
    }
}
