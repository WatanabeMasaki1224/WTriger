using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Point")]
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _checkRadius = 3f;

    [Header("Enemy")]
    [SerializeField] private EnemyBase[] _enemyPrefabs;
    [SerializeField] private int _maxEnemyCount = 5;
    [SerializeField] private float _spawnInterval = 3f;

    [Header("Boss")]
    [SerializeField] private EnemyBase _bossPrefab;
    [SerializeField] private Transform _bossSpawnPoint;
    [SerializeField] private int _needKillCount = 20;


    private List<EnemyBase> _currentEnemies = new List<EnemyBase>();

    private float _spawnTimer;
    private int _killCount;

    private bool _bossPhase;

    private void Update()
    {
        // ボス戦なら雑魚スポーン停止
        if (_bossPhase)
        {
            return;
        }


        _spawnTimer += Time.deltaTime;


        if (_spawnTimer < _spawnInterval)
        {
            return;
        }


        _spawnTimer = 0f;


        // 最大数以下ならスポーン
        if (_currentEnemies.Count < _maxEnemyCount)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        List<Transform> candidates = new();


        // 使用可能なスポーン地点を探す
        foreach (Transform point in _spawnPoints)
        {
            Collider[] hit =Physics.OverlapSphere(point.position,_checkRadius);

            bool canSpawn = true;

            foreach (Collider col in hit)
            {
                if (col.GetComponent<EnemyBase>() != null)
                {
                    canSpawn = false;
                    break;
                }
            }


            if (canSpawn)
            {
                candidates.Add(point);
            }
        }


        // 出せる場所がない
        if (candidates.Count == 0)
        {
            return;
        }

        // ランダム地点
        Transform spawnPoint =candidates[Random.Range(0, candidates.Count)];
        // ランダム敵
        EnemyBase prefab =_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        //敵出現
        EnemyBase enemy =Instantiate(prefab,spawnPoint.position,Quaternion.identity);
        _currentEnemies.Add(enemy);
        // 死亡通知用
        enemy.SetSpawner(this);
    }

    /// <summary>
    /// 敵死亡時に呼ばれる
    /// </summary>
    public void EnemyDead(EnemyBase enemy)
    {
        _currentEnemies.Remove(enemy);


        _killCount++;


        if (_killCount >= _needKillCount)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        _bossPhase = true;
        Instantiate(_bossPrefab,_bossSpawnPoint.position,Quaternion.identity);
        Debug.Log("Boss Start");
    }
}
