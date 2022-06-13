using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerScripts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    public static int level = 0;
    public  static int currentEnemies;
    [SerializeField] 
    private GameObject Skeleton;
    [SerializeField] 
    private GameObject Golem;
    [SerializeField] 
    private GameObject Lich;
    [SerializeField] 
    private GameObject ShopDoor;
    [SerializeField] 
    private GameObject DoorNextLevel;
    [SerializeField] 
    private PlayerController player;
    
    private List<Transform> spawnPoints;
    private bool levelFinished;
    private bool gameFinished;
    private ShopEnter shopEnter;
    private DoorNextLevel doorNextLevel;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private UIController uiController;

    private void Start()
    {
        gameFinished = false;
        shopEnter = ShopDoor.GetComponent<ShopEnter>();
        doorNextLevel = DoorNextLevel.GetComponent<DoorNextLevel>();
        LoadSpawnPoints(); //loads all 6 spawn points into array
        StartGame();
    }

    private void StartGame()
    {
        level = 0;
        NextLevel();
    }

    public static void ReduceEnemies()
    {
        currentEnemies--;
    }
    
    public void NextLevel()
    {
        level++;
        //uiController.SetLevel(level);
        levelFinished = false;
        currentEnemies = 0;
        SetupLevel();
    }

    private void SetupLevel()
    {
        Debug.Log("Level: " + level);
        doorNextLevel.CloseDoor();
        shopEnter.CloseDoor();
        StartCoroutine(SpawnEnemies(GetRandomEnemies(level + 1)));
    }

    private GameObject[] GetRandomEnemies(int numberOfEnemies)
    {
        var result = new List<GameObject>();
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int rand = Random.Range(level/2, level * 2)%7;
            if(rand <= 2)
                result.Add(Skeleton);
            else if(rand <= 4)
                result.Add(Lich);
            else
                result.Add(Golem);

        }

        return result.ToArray();
    }

    private IEnumerator SpawnEnemies(GameObject[] enemies) 
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            currentEnemies++;
            float waitPeriod = 0;
            int rand = Random.Range(1, 6);
            int spawnPoint = rand;
            if (enemies[i] == Skeleton)
                waitPeriod = Random.Range(1.5f, 3.5f);
            if (enemies[i] == Lich)
                waitPeriod = Random.Range(1f, 2f);
            if (enemies[i] == Golem)
                waitPeriod = Random.Range(3, 6);
            yield return new WaitForSeconds(waitPeriod);
            Instantiate(enemies[i], spawnPoints[i + 1].position, Quaternion.identity);
        }
        levelFinished = true;
    }

    private bool CheckSpawnPoint(Transform spawnPoint)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(spawnPoint.position, 1f, enemyLayer);
        Debug.Log(hitEnemies.Length);
        if (hitEnemies?.Length > 0)
            return true;
        return false;
    }

    private void LoadSpawnPoints()
    {
        spawnPoints = GetComponentsInChildren<Transform>().ToList();
    }

    private void Update()
    {
        // foreach (var sp in spawnPoints)
        // {
        //     Debug.Log(sp.name + " " + CheckSpawnPoint(sp));
        // }
        if (currentEnemies == 0 && levelFinished)
        {
            if (level == 5 && !gameFinished)
            {
                gameFinished = true;
                StartCoroutine(this.Delay(1f, () =>
                {
                    Time.timeScale = 0;
                    DeathPopUpUI.Instance.SetTitle("CONGRATULATIONS YOU'VE WON\n But the princess is another tower").Show();
                }));
            }

            doorNextLevel.OpenDoor();
            if(level % 2 == 0)
                shopEnter.OpenDoor();
        }
    }
}
