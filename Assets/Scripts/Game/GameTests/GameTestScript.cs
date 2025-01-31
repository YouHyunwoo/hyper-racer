using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class GameTestScript
{
    float elapsedTime = 0f;
    float targetTime = 10f;

    // A Test behaves as an ordinary method
    [Test]
    public void GameTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
        yield return waitForSceneLoad();

        var gameManagerObject = GameObject.Find("Game Manager");
        Assert.IsNotNull(gameManagerObject, "Game Manager object not found");

        var gameManager = gameManagerObject.GetComponent<GameManager>();
        Assert.IsNotNull(gameManager, "Game Manager component not found");

        var startButtonObject = GameObject.Find("Start Button");
        Assert.IsNotNull(startButtonObject, "Start Button object not found");

        var startButton = startButtonObject.GetComponent<UnityEngine.UI.Button>();
        Assert.IsNotNull(startButton, "Start Button component not found");

        startButton.onClick.Invoke();

        yield return new WaitForSeconds(0.5f);

        var carController = GameObject.Find("Car(Clone)").GetComponent<CarController>();
        Assert.IsNotNull(carController, "Car Controller component not found");

        var leftMoveButtonObject = GameObject.Find("Left Move Button");
        Assert.IsNotNull(leftMoveButtonObject, "Left Move Button object not found");
        var leftMoveButton = leftMoveButtonObject.GetComponent<MoveButton>();
        Assert.IsNotNull(leftMoveButton, "Left Move Button component not found");

        var rightMoveButtonObject = GameObject.Find("Right Move Button");
        Assert.IsNotNull(rightMoveButtonObject, "Right Move Button object not found");
        var rightMoveButton = rightMoveButtonObject.GetComponent<MoveButton>();
        Assert.IsNotNull(rightMoveButton, "Right Move Button component not found");

        var positions = new Vector3[] {
            new (-1f, 0.2f, -3f),
            new ( 0f, 0.2f, -3f),
            new (+1f, 0.2f, -3f),
        };
        float rayDistance = 10f;
        var p = new Vector3(0, 0, 0);

        while (gameManager.GameState == GameManager.State.Play)
        {
            // bool isFound = false;
            var set = new List<int>();
            var enemyPositions = new Vector3[] {
                new (-1f, 0.2f, -10f),
                new (+1f, 0.2f, -10f),
                new ( 0f, 0.2f, -10f),
            };
            bool isFound = false;
            for (int i = 0; i < enemyPositions.Length; i++) {
                var position = enemyPositions[i];
                RaycastHit hit;
                if (!Physics.Raycast(position, Vector3.forward, out hit, 20, LayerMask.GetMask("Enemy")))
                {
                    set.Add((int)position.x);
                    p.x = position.x;
                    isFound = true;
                }
            }
            if (isFound)
            {
                Debug.Log(p.x);
                MoveCar(p, leftMoveButton, rightMoveButton, carController);
            }
            else {
                for (int i = 0; i < positions.Length; i++) {
                    if (!set.Contains((int)positions[i].x)) { continue; }
                    var position = positions[i];
                    RaycastHit hit;
                    if (Physics.Raycast(position, Vector3.forward, out hit, rayDistance, LayerMask.GetMask("Gas")))
                    {
                        p.x = position.x;
                        MoveCar(position, leftMoveButton, rightMoveButton, carController);
                    }
                }
            }

            Debug.DrawRay(enemyPositions[0], Vector3.forward * 20, Color.red);
            Debug.DrawRay(enemyPositions[1], Vector3.forward * 20, Color.blue);
            Debug.DrawRay(enemyPositions[2], Vector3.forward * 20, Color.green);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Assert.Less(elapsedTime, targetTime, "Game did not end in time");

        yield return null;
    }

    IEnumerator waitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }

    void MoveButtonDown(GameObject moveButtonObject)
    {
        var pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButtonObject, pointerEventData, ExecuteEvents.pointerDownHandler);
    }

    void MoveButtonUp(GameObject moveButtonObject)
    {
        var pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButtonObject, pointerEventData, ExecuteEvents.pointerUpHandler);
    }

    void MoveCar(Vector3 targetPosition, MoveButton leftMoveButton, MoveButton rightMoveButton, CarController carController)
    {
        if (targetPosition.x < carController.transform.position.x)
        {
            MoveButtonDown(leftMoveButton.gameObject);
            MoveButtonUp(rightMoveButton.gameObject);
        }
        else if (targetPosition.x > carController.transform.position.x)
        {
            MoveButtonDown(rightMoveButton.gameObject);
            MoveButtonUp(leftMoveButton.gameObject);
        }
        else
        {
            MoveButtonUp(leftMoveButton.gameObject);
            MoveButtonUp(rightMoveButton.gameObject);
        }
    }
}
