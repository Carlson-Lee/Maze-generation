using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]           

public class GameController : MonoBehaviour
{
    private MazeConstructor constructor;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;

    private AIController aIController;

    [SerializeField] private int rows;
    [SerializeField] private int cols;

    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
        //AI movement
        aIController = GetComponent<AIController>(); 
    }
    
    void Start()
    {
        constructor.GenerateNewMaze(rows, cols, OnTreasureTrigger);

        aIController.Graph = constructor.graph;
        aIController.Player = CreatePlayer();
        aIController.Monster = CreateMonster(monstercollisionTrigger);
        aIController.HallWidth = constructor.hallWidth;
        aIController.StartAI();
    }

    private GameObject CreatePlayer()
    {
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";

        return player;
    }

    private GameObject CreateMonster(TriggerEventHandler monstercollisionCallback)
    {
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";

        monster.GetComponent<BoxCollider>().isTrigger = true;

        TriggerEventRouter mc = monster.AddComponent<TriggerEventRouter>();
        mc.callback = monstercollisionCallback;

        return monster;
        
    }

    private void OnTreasureTrigger(GameObject trigger, GameObject other)
    { 
        Debug.Log("You Won!");
        aIController.StopAI();
    }

    private void monstercollisionTrigger(GameObject trigger, GameObject other)
    { 
        Debug.Log("Gotcha!");
        Start();
    }



}