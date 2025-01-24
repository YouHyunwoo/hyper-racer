using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject roadPrefab;

    // UI 관련 코드
    [SerializeField] MoveButton leftMoveButton;
    [SerializeField] MoveButton rightMoveButton;

    [SerializeField] TextMeshProUGUI gasText;

    Queue<GameObject> roadPool = new ();
    int roadPoolSize = 3;

    List<GameObject> activeRoads = new ();

    int roadIndex = 0;

    CarController carController;

    public enum State { Start, Play, End }
    public State GameState { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeRoadPool();
        StartGame();
    }

    void Update()
    {
        switch (GameState)
        {
            case State.Start:
                break;
            case State.Play:
                foreach(var activeRoad in activeRoads)
                {
                    activeRoad.transform.Translate(carController.MoveSpeed * Vector3.back * Time.deltaTime);
                }

                if (carController != null)
                {
                    gasText.text = $"Gas: {carController.Gas}";
                }
                break;
            case State.End:
                break;
        }
    }

    void StartGame()
    {
        SpawnRoad(Vector3.zero);

        carController = Instantiate(carPrefab, new Vector3(0, 0, -3f), Quaternion.identity).GetComponent<CarController>();

        leftMoveButton.OnMoveButtonDown += () => 
        {
            carController.Move(-1);
        };

        rightMoveButton.OnMoveButtonDown += () => 
        {
            carController.Move(1);
        };

        GameState = State.Play;
    }

    public void EndGame()
    {
        GameState = State.End;
        Destroy(carController.gameObject);
        carController = null;
        while (activeRoads.Count > 0)
        {
            ReturnRoad(activeRoads[0]);
        }
    }

    void InitializeRoadPool()
    {
        for (int i = 0; i < roadPoolSize; i++)
        {
            var road = Instantiate(roadPrefab);
            road.SetActive(false);
            roadPool.Enqueue(road);
        }
    }

    public void SpawnRoad(Vector3 position)
    {
        GameObject road;
        if (roadPool.Count > 0)
        {
            road = roadPool.Dequeue();
            road.transform.position = position;
            road.SetActive(true);
        }
        else
        {
            road = Instantiate(roadPrefab, position, Quaternion.identity);
        }

        if (roadIndex > 0 && roadIndex % 2 == 0) {
            road.GetComponent<RoadController>().SpawnGas();
        }
        activeRoads.Add(road);
        roadIndex++;
    }

    public void ReturnRoad(GameObject road)
    {
        road.SetActive(false);
        activeRoads.Remove(road);
        roadPool.Enqueue(road);
    }
}
