using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    public GameObject[] platforms;//список префабов платформ
    public Transform generationPoint;
    public float distanceBetween;//расстояние между платформами
    public float distanceMin;
    public float distanceMax;
    public Transform maxHeightPoint;
    public float maxHeightChange;
    public float healingSpawnCoefficient;
    public GameObject exitPoint;
    public GameObject healingPoint;
    public int intervalAmount;//количество хилок до выхода
    public bool spawnEnemies = false;//спавнить ли врагов
    public int enemyMin;//диапазон спавна врагов от
    public int enemyMax;//и до
    public float healingSpawnMax;//промежуток спавна хилки
    public int healingSpawnMin;//минимальное количество платформ между хилками
    public bool fallingPlatforms;
    public float platformSpeedMin;//скорость для движущейся платформы
    public float platformSpeedMax;
    public bool spawnTraps;//спавнить ли ловушки
    public int trapMin;//диапазон для ловушек
    public int trapMax;
    public bool spawnEssence;
    public int essenceMin;
    public int essenceMax;


    //float healingPlatformWidth;
    int platformSelector;//выбор префаба определенной платформы
    float[] platformsWidth;//ширина префабов из массива
    float[] platformsHeight;
    private float healingPointY;//половина высоты хилки
    private float exitY;//и выхода
    float minHeight;//высоты спавна платформ
    float maxHeight;
    float heightChange;
    float healingSpawnTime;
    bool healingIsDecreasing=true;
    private float lastHealingPointX;
    private int healingPointAmount=0;
    private bool spawning = true;
    private bool spawnExit = false;
    private EnemyGenerator enemyGenerator;
    private int enemyDensity;//количество платформ, через которое заспавнится враг
    private int currentPlatformAmount;//количество платформ с предыдущей хилки
    private TrapGenerator trapGenerator;
    private int trapDensity;//количество платформ между ловушками
    private GameObject currentPlatform;//текущая создаваемая платформа
    private bool isEnemyPlatform = false;
    private bool isExitPlatform = false;
    private bool isHealingPlatform = false;
    private int essenceDensity;
    private EssenceGenerator essenceGenerator;


    // Start is called before the first frame update
    void Start()
    {
        //healingPlatformWidth = healingPlatform.GetComponent<BoxCollider2D>().size.x;//получаем размер ребенка, то есть самой платформы
        platformsWidth = new float[platforms.Length];
        platformsHeight = new float[platforms.Length];
        for (int i = 0; i < platforms.Length; i++)
        {
            platformsWidth[i] = platforms[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size.x;//аналогично
            platformsHeight[i] = platforms[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        }
        healingPointY = healingPoint.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        exitY = exitPoint.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;
        healingSpawnTime = healingSpawnMax;
        currentPlatformAmount = 0;
        if (spawnEnemies)
        {
            enemyGenerator = GetComponent<EnemyGenerator>();
            enemyDensity = Random.Range(enemyMin, enemyMax);
            print("enemy dens" + enemyDensity);
        }
        if (spawnTraps)
        {
            trapGenerator = GetComponent<TrapGenerator>();
            trapDensity = Random.Range(trapMin, trapMax);
        }
        if (spawnEssence)
        {
            essenceGenerator = GetComponent<EssenceGenerator>();
            essenceDensity = Random.Range(essenceMin, essenceMax);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)//спавн новых платформ (любых)
        {
            if (healingIsDecreasing)//запущен ли отсчет для спавна следующей хилки
                healingSpawnTime -= Time.deltaTime * healingSpawnCoefficient;
            if (transform.position.x < generationPoint.position.x)
            {
                isEnemyPlatform = false;
                isHealingPlatform = false;
                distanceBetween = Random.Range(distanceMin, distanceMax);
                float currentPlatformWidth = platformsWidth[platformSelector];//для сохранения длины предыдущей платформы, для корректного спавна
                platformSelector = Random.Range(0, platforms.Length);
                heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);//изменения от положения текущей платформы
                if (heightChange > maxHeight)
                    heightChange = maxHeight;
                if (heightChange < minHeight)
                    heightChange = minHeight;

                //начало переписывания
                transform.position = new Vector3(transform.position.x + platformsWidth[platformSelector]/2f + currentPlatformWidth/2f + distanceBetween, heightChange, transform.position.z);
                currentPlatform = Instantiate(platforms[platformSelector], transform.position, transform.rotation);
                currentPlatformAmount++;
                if (spawnEnemies)
                {
                    if (enemyDensity <= 0)//пришло время спавна
                    {
                        enemyGenerator.SetSpawnPosition(transform.position.x, transform.position.y);//место спавна врага
                        enemyGenerator.SetBorders(platformsWidth[platformSelector], platformsHeight[platformSelector]);//ширина текущей платформы
                        enemyGenerator.SpawnEnemy();
                        enemyDensity = Random.Range(enemyMin, enemyMax);
                        isEnemyPlatform = true;
                    }
                    else
                        enemyDensity--;
                }

                    
                if (healingSpawnTime <= 0 && currentPlatformAmount>healingSpawnMin)//спавн платформы с хилкой
                {
                    if (healingPointAmount > intervalAmount)//если пора спавнить выход
                    {
                        spawnExit = true;//бахаем истину и идем на следующую итерацию апдейта
                    }
                    else
                    {
                        lastHealingPointX = transform.position.x;
                        print("Last healing plat " + lastHealingPointX);
                        float delta = Random.Range(0, platformsWidth[platformSelector] / 4f);
                        lastHealingPointX += delta;
                        Instantiate(healingPoint, transform.position + new Vector3(delta,
                            platforms[platformSelector].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2f + healingPointY/2f, 0), transform.rotation);
                        healingPointAmount++;
                        currentPlatformAmount = 0;
                        isHealingPlatform = true;
                    }
                    healingSpawnTime = healingSpawnMax;
                }
                if (spawnExit)//если спавним выход
                {
                    Instantiate(exitPoint, transform.position + new Vector3(Random.Range(-platformsWidth[platformSelector] / 4f, platformsWidth[platformSelector] / 4f),
                       platforms[platformSelector].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2f + exitY/2f, 0), transform.rotation);
                    spawning = false;//прекращаем спавн платформ
                    isExitPlatform = true;
                }

                if (fallingPlatforms)
                {
                    if (!isExitPlatform && !isHealingPlatform && !isEnemyPlatform)
                    {
                        float chance = Random.Range(0f, 3f);

                        if (chance > 1.0 && currentPlatform.transform.GetChild(0).GetComponent<MovingPlatform>() != null)
                        {
                            currentPlatform.transform.GetChild(0).GetComponent<MovingPlatform>().SetMoving(minHeight, maxHeight, Random.Range(platformSpeedMin, platformSpeedMax));
                        }
                        else
                            currentPlatform.transform.GetChild(0).GetComponent<FallingPlatform>().SetFalling();
                        
                    }
                }

                if (spawnTraps)
                {
                    if (trapDensity <= 0 && !isExitPlatform)//если не выход
                    {
                        trapGenerator.SetSpawnPosition(transform.position.x, transform.position.y);
                        trapGenerator.SetPlatformHeight(platformsHeight[platformSelector]);
                        trapGenerator.SpawnTrap();
                        trapDensity = Random.Range(trapMin, trapMax);
                    }
                    trapDensity--;
                }

                if (spawnEssence)
                {
                    if(essenceDensity <=0 && !isExitPlatform)
                    {
                        essenceGenerator.SetSpawnPosition(transform.position.x, transform.position.y);
                        essenceGenerator.SetPlatformHeight(platformsHeight[platformSelector]);
                        essenceGenerator.SpawnEssence();
                        essenceDensity = Random.Range(essenceMin, essenceMax);
                    }
                    essenceDensity--;
                }
                //конец

                
            }
        }
        
    }

    public void SetHealingSpawnTime()
    {
        healingSpawnTime = healingSpawnMax;
    }
    public void SetHealingDesreasing(bool decrease)
    {
        healingIsDecreasing = decrease;
    }
    public float GetLastHealingPoint()
    {
        return lastHealingPointX;
    }
}
