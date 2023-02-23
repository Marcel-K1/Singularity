/*****************************************************************************
* Project: Singularity
* File   : EntitySpawner.cs
* Date   : 16.02.2022
* Author : Martin Stasch (MS)
*
* Item entity spawner - event driven.
* It spawnes Items (Keys and gates) randomly for the level when the event is triggered.
*
* History:
*	16.02.2022	MS	Created
******************************************************************************/


using System.Collections.Generic;
using UnityEngine;


public class EntitySpawner : MonoBehaviour
{
    [Header("Spawn Distance Setting")]
    [SerializeField]
    private float heigthVariationToSpawnPointAreaPoint = 1f;
    [SerializeField]
    private float maxDistanzToSpawnPointAreaPoint = 15f;

    [Header("Required SpawnPoint Fields")]
    [SerializeField]
    private List<Transform> spawnPointsList = new List<Transform>();
    [SerializeField]
    private Transform spawnPointDefault = null;

    [SerializeField]
    private LayerMask whatIsGround = -1;
    [SerializeField]
    private LevelManager levelManager = null;


    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();

        foreach (Transform GO in gameObject.GetComponentsInChildren<Transform>())
        {
            spawnPointsList.Add(GO);         
        };

    }

    //Apply methods to event
    private void OnEnable()
    {
        levelManager.SpawnGateEvent += InstantiateObjects;
        levelManager.SpawnKeyEvent += InstantiateObjects;
    }
    private void OnDisable()
    {
        levelManager.SpawnGateEvent -= InstantiateObjects;
        levelManager.SpawnKeyEvent -= InstantiateObjects;
    }

    /// <summary>
    /// Event driven spawner to spawn several objects at the start of the game 
    /// </summary>
    /// <param name="_objectsToSpawn"></param> number of objects the spawner needs to spawn
    /// <param name="_prefabToSpawn"></param> prefab of the object that needs to be spawned
    private void InstantiateObjects(int _objectsToSpawn, GameObject _prefabToSpawn)
    {

        for (int i = 0; i < _objectsToSpawn; i++)
        {

            Vector3 spawnPointTransform = RandomSpawnPointAtSpawn();

            float rndRotation = UnityEngine.Random.Range(-180f, 180f);

            GameObject spawnedObject = Instantiate(_prefabToSpawn, spawnPointTransform, 
                Quaternion.AngleAxis(rndRotation, Vector3.up));

        }

    }

    /// <summary>
    /// Randomizer of spawnpoints
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomSpawnPointAtSpawn()
    {
        if (spawnPointsList != null)
        {
            Vector3 spawnAreaPoint = spawnPointsList[UnityEngine.Random.Range(0, 
                spawnPointsList.Count)].position;
            Vector3 randomSpawnPointAtSpawnArea = new Vector3();
            bool SpawnPointFound = false;

            int runcount = 5000000; // to prevent infinite loop for following do-while-loop

            do
            {

                float randomZ = UnityEngine.Random.Range(-maxDistanzToSpawnPointAreaPoint, 
                    maxDistanzToSpawnPointAreaPoint);
                float randomX = UnityEngine.Random.Range(-maxDistanzToSpawnPointAreaPoint, 
                    maxDistanzToSpawnPointAreaPoint);
                float randomY = UnityEngine.Random.Range(-heigthVariationToSpawnPointAreaPoint, 
                    heigthVariationToSpawnPointAreaPoint);

                randomSpawnPointAtSpawnArea = new Vector3(spawnAreaPoint.x + randomX, 
                    spawnAreaPoint.y + randomY, spawnAreaPoint.z + randomZ);


                if (Physics.Raycast(randomSpawnPointAtSpawnArea, -transform.up, 0.1f, whatIsGround))
                    SpawnPointFound = true;
                else if(runcount <= 0)
                {
                    Debug.LogWarning("No Spawnarea found");
                    SpawnPointFound = true;
                    randomSpawnPointAtSpawnArea = spawnPointDefault.transform.position;
                }

                runcount--;

            } while (SpawnPointFound == false); ;

            runcount = 10000000;
            SpawnPointFound = false;

            return randomSpawnPointAtSpawnArea;
        }
        else
        {
            // error check in case the list is empty!
            Debug.LogError("No spawnpoint location in the list! " +
                "Please check if spawnpoints use Tag 'SpawnPoint'!");
            return spawnPointDefault.transform.position;
        }
    }
}
