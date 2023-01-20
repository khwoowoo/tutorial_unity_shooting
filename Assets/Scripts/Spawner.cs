using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemyRemainingToSpawn;
    int enemyRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    float timeBetweenCampingChecks = 2f;
    float nextCampCheckTime;
    float campThresholdDistance = 2.5f;
    Vector3 campPositionOld;
    bool isCamping;
    bool isDisabled;

    public event System.Action<int> OnNewWave;


    private void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerEntity.OnDeath += OnPlayerDeath;
        playerT = playerEntity.transform;
        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    private void Update()
    {
        //플레이어가 죽은 경우
        if (isDisabled)
        {
            return;
        }

        if(Time.time > nextCampCheckTime)
        {
            nextCampCheckTime = timeBetweenCampingChecks + Time.time;

            isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
            campPositionOld = playerT.position;
        }

        if (enemyRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemyRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine(SpawnEntity());
        }
    }

    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    IEnumerator SpawnEntity()
    {
        //스폰 되는 시간
        float spawnDelay = 1f;
        //타일이 반짝이는 속도
        float tileFlashSpeed = 4f;

        Transform randomTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            randomTile = map.GetTileFromPosition(playerT.position);
        }
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color initColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, randomTile.position + Vector3.up, Quaternion.identity);
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    void NextWave()
    {
        currentWaveNumber++;

        //print("Wave: " + currentWaveNumber);
        if(currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemyRemainingToSpawn = currentWave.enemyCount;
            enemyRemainingAlive = enemyRemainingToSpawn;

            if(OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }

            ResetPlayerPosition();
        }

     
    }

    void OnEnemyDeath()
    {
        enemyRemainingAlive--;

        if(enemyRemainingAlive == 0)
        {
            NextWave();
        }
    }


    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
