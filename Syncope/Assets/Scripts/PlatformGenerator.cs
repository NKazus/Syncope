using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Platforms")]
    [SerializeField] private GameObject[] platforms;//список префабов платформ
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Transform maxHeightPoint;

    [SerializeField] private float distanceMin = 3;
    [SerializeField] private float distanceMax = 5;
    [SerializeField] private float maxHeightChange = 2;

    [SerializeField] private GameObject exitPoint;

    [Header("Healing Points")]
    [SerializeField] private GameObject healingPoint;
    [SerializeField] private float healingSpawnCoefficient = 0.1f;
    [SerializeField] private int intervalAmount = 3;//количество хилок до выхода
    [SerializeField] private float healingSpawnMax = 2;//промежуток спавна хилки
    [SerializeField] private int healingSpawnMin = 4;//минимальное количество платформ между хилками

    [Header("Enemies")]
    [SerializeField] private bool spawnEnemies = false;//спавнить ли врагов
    [SerializeField] private int enemyMin = 0;//диапазон спавна врагов от
    [SerializeField] private int enemyMax = 4;//и до

    [Header("Moving Platforms")]
    [SerializeField] private bool fallingPlatforms = false;

    [SerializeField] private float platformSpeedMin = 2;//скорость для движущейся платформы
    [SerializeField] private float platformSpeedMax = 5;

    [Header("Traps")]
    [SerializeField] private bool spawnTraps = false;//спавнить ли ловушки
    [SerializeField] private int trapMin = 2;//диапазон для ловушек
    [SerializeField] private int trapMax = 5;

    [Header("Essence")]
    [SerializeField] private bool spawnEssence = false;
    [SerializeField] private int essenceMin = 3;
    [SerializeField] private int essenceMax = 4;

    private bool _spawning = true;
    private int _platformSelector;//выбор префаба определенной платформы
    private float[] _platformsWidth;//ширина префабов из массива
    private float[] _platformsHeight;
    private float _minHeight;//высота спавна платформ
    private float _maxHeight;
    private float _heightChange;
    private float distanceBetween;//расстояние между платформами
    private GameObject _currentPlatform;//текущая создаваемая платформа

    private float _healingPointY;//половина высоты хилки
    private float _healingSpawnTime;
    private bool _healingIsDecreasing = true;
    private float _lastHealingPointX;
    private int _healingPointAmount = 0;
    private int _currentPlatformAmount = 0;//количество платформ с предыдущей хилки

    private float _exitY;//половина высоты выхода
    private bool _spawnExit = false;

    private EnemyGenerator _enemyGenerator;
    private int _enemyDensity;//количество платформ, через которое заспавнится враг
    
    private TrapGenerator _trapGenerator;
    private int _trapDensity;//количество платформ между ловушками
    
    

    private EssenceGenerator _essenceGenerator;
    private int _essenceDensity;

    private bool _isEnemyPlatform = false;
    private bool _isExitPlatform = false;
    private bool _isHealingPlatform = false;


    private void Start()
    {
        _platformsWidth = new float[platforms.Length];
        _platformsHeight = new float[platforms.Length];
        for (int i = 0; i < platforms.Length; i++)
        {
            _platformsWidth[i] = platforms[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size.x;
            _platformsHeight[i] = platforms[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        }
        _healingPointY = healingPoint.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        _exitY = exitPoint.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        _minHeight = transform.position.y;
        _maxHeight = maxHeightPoint.position.y;
        _healingSpawnTime = healingSpawnMax;
        if (spawnEnemies)
        {
            _enemyGenerator = GetComponent<EnemyGenerator>();
            _enemyDensity = Random.Range(enemyMin, enemyMax);
        }
        if (spawnTraps)
        {
            _trapGenerator = GetComponent<TrapGenerator>();
            _trapDensity = Random.Range(trapMin, trapMax);
        }
        if (spawnEssence)
        {
            _essenceGenerator = GetComponent<EssenceGenerator>();
            _essenceDensity = Random.Range(essenceMin, essenceMax);
        }
    }

    private void Update()
    {
        if (_spawning)//спавн новых платформ (любых)
        {
            if (_healingIsDecreasing)//запущен ли отсчет для спавна следующей хилки
                _healingSpawnTime -= Time.deltaTime * healingSpawnCoefficient;
            if (transform.position.x < generationPoint.position.x)
            {

                SpawnPlatform();

                if (spawnEnemies)
                {
                    if (_enemyDensity <= 0)//пришло время спавна
                    {
                        SpawnEnemy();
                    }
                    else
                        _enemyDensity--;
                }

                    
                if (_healingSpawnTime <= 0 && _currentPlatformAmount > healingSpawnMin)//спавн платформы с хилкой
                {
                    SpawnHealingPoint();
                }
                if (_spawnExit)//если спавним выход
                {
                    Instantiate(exitPoint, transform.position + new Vector3(Random.Range(-_platformsWidth[_platformSelector] / 4f, _platformsWidth[_platformSelector] / 4f),
                       platforms[_platformSelector].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2f + _exitY / 2f, 0), transform.rotation);
                    _spawning = false;//прекращаем спавн платформ
                    _isExitPlatform = true;
                }

                if (fallingPlatforms)
                {
                    if (!_isExitPlatform && !_isHealingPlatform && !_isEnemyPlatform)
                    {
                        SetPlatformType();                        
                    }
                }

                if (spawnTraps)
                {
                    if (_trapDensity <= 0 && !_isExitPlatform)//если не выход
                    {
                        SpawnTrap();
                    }
                    _trapDensity--;
                }

                if (spawnEssence)
                {
                    if (_essenceDensity <= 0 && !_isExitPlatform)
                    {
                        SpawnEssence();
                    }
                    _essenceDensity--;
                }
                
            }
        }
        
    }
    
    public void SetHealingSpawnTime()
    {
        _healingSpawnTime = healingSpawnMax;
    }
    public void SetHealingDesreasing(bool decrease)
    {
        _healingIsDecreasing = decrease;
    }
    public float GetLastHealingPoint()
    {
        return _lastHealingPointX;
    }

    private void SpawnPlatform()
    {
        _isEnemyPlatform = false;
        _isHealingPlatform = false;
        distanceBetween = Random.Range(distanceMin, distanceMax);
        float currentPlatformWidth = _platformsWidth[_platformSelector];//для сохранения длины предыдущей платформы, для корректного спавна
        _platformSelector = Random.Range(0, platforms.Length);
        _heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);//изменения от положения текущей платформы
        if (_heightChange > _maxHeight)
            _heightChange = _maxHeight;
        if (_heightChange < _minHeight)
            _heightChange = _minHeight;


        transform.position = new Vector3(transform.position.x + _platformsWidth[_platformSelector] / 2f + currentPlatformWidth / 2f + distanceBetween, _heightChange, transform.position.z);
        _currentPlatform = Instantiate(platforms[_platformSelector], transform.position, transform.rotation);
        _currentPlatformAmount++;
    }

    private void SpawnEnemy()
    {
        _enemyGenerator.SetSpawnPosition(transform.position.x, transform.position.y);//место спавна врага
        _enemyGenerator.SetBorders(_platformsWidth[_platformSelector], _platformsHeight[_platformSelector]);//ширина текущей платформы
        _enemyGenerator.SpawnEnemy();
        _enemyDensity = Random.Range(enemyMin, enemyMax);
        _isEnemyPlatform = true;
    }

    private void SpawnEssence()
    {
        _essenceGenerator.SetSpawnPosition(transform.position.x, transform.position.y);
        _essenceGenerator.SetPlatformHeight(_platformsHeight[_platformSelector]);
        _essenceGenerator.SpawnEssence();
        _essenceDensity = Random.Range(essenceMin, essenceMax);
    }

    private void SpawnTrap()
    {
        _trapGenerator.SetSpawnPosition(transform.position.x, transform.position.y);
        _trapGenerator.SetPlatformHeight(_platformsHeight[_platformSelector]);
        _trapGenerator.SpawnTrap();
        _trapDensity = Random.Range(trapMin, trapMax);
    }

    private void SetPlatformType()
    {
        float chance = Random.Range(0f, 3f);
        if (chance > 1.0 && _currentPlatform.transform.GetChild(0).GetComponent<MovingPlatform>() != null)
        {
            _currentPlatform.transform.GetChild(0).GetComponent<MovingPlatform>().SetMoving(_minHeight, _maxHeight, Random.Range(platformSpeedMin, platformSpeedMax));
        }
        else
        {
            _currentPlatform.transform.GetChild(0).GetComponent<FallingPlatform>().SetFalling();
        }
    }

    private void SpawnHealingPoint()
    {
        if (_healingPointAmount > intervalAmount)//если пора спавнить выход
        {
            _spawnExit = true;
        }
        else
        {
            _lastHealingPointX = transform.position.x;
            float delta = Random.Range(0, _platformsWidth[_platformSelector] / 4f);
            _lastHealingPointX += delta;
            Instantiate(healingPoint, transform.position + new Vector3(delta,
                platforms[_platformSelector].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2f + _healingPointY / 2f, 0), transform.rotation);
            _healingPointAmount++;
            _currentPlatformAmount = 0;
            _isHealingPlatform = true;
        }
        _healingSpawnTime = healingSpawnMax;
    }
}
