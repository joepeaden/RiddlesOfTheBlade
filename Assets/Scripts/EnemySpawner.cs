using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Vector3> _spawnPositions = new List<Vector3>();

    private Coroutine spawnCoroutine;

    private void Start()
    {
        GameManager.Instance.OnWaveBegin.AddListener(BeginWave);
        GameManager.Instance.OnWaveEnd.AddListener(EndWave);
        GameManager.Instance.OnGameOver.AddListener(HandleGameOver);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWaveBegin.RemoveListener(BeginWave);
        GameManager.Instance.OnWaveEnd.RemoveListener(EndWave);
        GameManager.Instance.OnGameOver.RemoveListener(HandleGameOver);
    }

    private void BeginWave()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private void EndWave()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private void HandleGameOver(bool playerWon)
    {
        EndWave();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            GameObject enemyObject = ObjectPool.Instance.GetEnemy();
            enemyObject.SetActive(true);

            enemyObject.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)];

            yield return new WaitForSeconds(GameManager.Instance.Data.spawnInterval);
        }
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
