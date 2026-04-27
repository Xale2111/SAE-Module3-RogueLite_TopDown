using FSM;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemiesPrefab = new List<GameObject>();
    Dictionary<GameObject, int> enemiesCountInRoom = new Dictionary<GameObject, int>();
    List<GameObject> enemiesSpawned = new List<GameObject>();
    float currentUsedWeight = 0;
    float roomThreshold = 1f;

    public void SpawnEnemies(float roomWeight)
    {
        DestroyAllEnemies();
        currentUsedWeight = 0;
        enemiesCountInRoom.Clear();
        List<GameObject> availablePrefabs = new List<GameObject>(enemiesPrefab);

        while (currentUsedWeight < roomWeight)
        {
            if (availablePrefabs.Count == 0) break;

            int currentEnemyID = Random.Range(0, availablePrefabs.Count);
            GameObject prefab = availablePrefabs[currentEnemyID];
            FSMEnemy enemyFSM = prefab.GetComponentInChildren<FSMEnemy>();

            if (!enemiesCountInRoom.ContainsKey(prefab))
                enemiesCountInRoom[prefab] = 0;

            bool weightOk = currentUsedWeight + enemyFSM.GetWeight() <= roomWeight;
            bool amountOk = enemiesCountInRoom[prefab] < enemyFSM.GetMaxAmountPerRoom();

            if (weightOk && amountOk)
            {
                Vector3 spawnPosition = GetRandomPositionInBounds();
                GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);

                FSMEnemy[] childEnemies = instance.GetComponentsInChildren<FSMEnemy>();
                if (childEnemies.Length > 0)
                {
                    foreach (FSMEnemy child in childEnemies)
                        enemiesSpawned.Add(child.gameObject);
                }
                else
                {
                    enemiesSpawned.Add(instance);
                }

                enemiesCountInRoom[prefab]++;
                currentUsedWeight += enemyFSM.GetWeight();
            }
            else
            {
                availablePrefabs.RemoveAt(currentEnemyID);
            }
        }
    }

    private Vector3 GetRandomPositionInBounds()
    {
        return new Vector3(
            Random.Range(RoomManager.GetCurrentRoomBounds().xMin + roomThreshold, RoomManager.GetCurrentRoomBounds().xMax - roomThreshold),
            Random.Range(RoomManager.GetCurrentRoomBounds().yMin + roomThreshold, RoomManager.GetCurrentRoomBounds().yMax - roomThreshold),
            0f
        );
    }

    private void DestroyAllEnemies()
    {
        for (int i = enemiesSpawned.Count - 1; i >= 0; i--)
        {
            Destroy(enemiesSpawned[i]);
        }
        enemiesSpawned.Clear();
    }

    public bool AreAllEnemiesDead()
    {
        foreach (GameObject enemy in enemiesSpawned)
        {
            if (enemy != null && enemy.activeSelf)
                return false;
        }
        return true;
    }
}