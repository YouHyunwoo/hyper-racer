using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [SerializeField] GameObject[] gasObjects;

    void OnDisable()
    {
        foreach (var gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SpawnRoad(transform.position + Vector3.forward * 10f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ReturnRoad(gameObject);
        }
    }

    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }
}
