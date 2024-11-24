using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Vector3> _spawnPositions = new List<Vector3>();

    //private int currentSpawnCap;

    private Coroutine spawnCoroutine;
    //private bool waveInProgress;

    private void Start()
    {
        GameManager.Instance.OnWaveBegin.AddListener(BeginWave);
        GameManager.Instance.OnWaveEnd.AddListener(EndWave);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWaveBegin.RemoveListener(BeginWave);
        GameManager.Instance.OnWaveEnd.RemoveListener(EndWave);
    }

    private void BeginWave()
    {
        //waveInProgress = true;

        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private void EndWave()
    {
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
    }

    //public void StartGame()
    //{
        //currentSpawnCap = GameManager.Instance.Data.baseAmountToSpawn;
    //    StartWave();
    //}

    //public void StartWave()
    //{
    //    StartCoroutine(SpawnEnemies());
    //}

    private IEnumerator SpawnEnemies()
    {
        //int enemiesSpawned = 0;
        while (true)//GameManager.Instance.WaveTimer > GameManager.Instance.Data.)//enemiesSpawned < currentSpawnCap)
        {
            GameObject enemyObject = ObjectPool.Instance.GetEnemy();
            enemyObject.SetActive(true);
            //enemiesSpawned++;

            enemyObject.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)];

            yield return new WaitForSeconds(GameManager.Instance.Data.spawnInterval);
        }

        //currentSpawnCap = GameManager.Instance.Data.baseAmountToSpawn + (GameManager.Instance.Data.amountToAddEachWave * GameManager.Instance.CurrentWave);
    }

    /// <summary>
    /// Draw the spawn positions in the editor for dev purposes
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Set the color for the gizmos
        Gizmos.color = Color.red;

        // Draw a sphere at each position in the list
        foreach (Vector3 position in _spawnPositions)
        {
            Gizmos.DrawSphere(transform.position + position, 0.2f);
        }
    }
}
