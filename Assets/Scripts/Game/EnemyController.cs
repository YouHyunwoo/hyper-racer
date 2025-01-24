using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public bool isHit;

    void Start()
    {
        moveSpeed = Random.Range(1f, 2f);

        StartCoroutine(AccelerateRoutine());
        StartCoroutine(MoveLineRoutine());
    }

    void Update()
    {
        if (isHit) {
            moveSpeed -= 10 * Time.deltaTime;
        }

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        

        if (transform.position.z < -10f || transform.position.z > 10f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AccelerateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            moveSpeed += 2 * (Random.value - 0.5f);
        }
    }

    IEnumerator MoveLineRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (transform.position.z < 0f) { continue; }
            if (Random.value >= 0.3f) { continue; }

            var direction = Random.value < 0.5 ? -1 : 1;
            var destination = transform.position + new Vector3(direction, 0f, 0f);
            
            if (destination.x < -1f || destination.x > 1f) { continue; }

            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, 1.5f * Time.deltaTime);
                yield return null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Hit();
            if (other.TryGetComponent<EnemyController>(out var enemyController))
            {
                enemyController.Hit();
            }

        }
    }

    public void Hit()
    {
        isHit = true;
    }
}
