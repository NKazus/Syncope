using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Platforms")]
    [SerializeField] private GameObject[] _platforms;//список префабов платформ
    [SerializeField] private Transform _generationPoint;
    [SerializeField] private Transform _maxHeightPoint;

    [SerializeField] private float _distanceMin;
    [SerializeField] private float _distanceMax;
    [SerializeField] private float _maxHeightChange;

    [SerializeField] private GameObject _exitPoint;

    [Header("Healing Points")]
    [SerializeField] private GameObject _healingPoint;
    [SerializeField] private float _healingSpawnCoefficient;
    [SerializeField] private int _intervalAmount;//количество хилок до выхода
    [SerializeField] private float _healingSpawnMax;//промежуток спавна хилки
    [SerializeField] private int _healingSpawnMin;//минимальное количество платформ между хилками

    [Header("Enemies")]
    [SerializeField] private bool _spawnEnemies = false;//спавнить ли врагов
    [SerializeField] private int _enemyMin;//диапазон спавна врагов от
    [SerializeField] private int _enemyMax;//и до

    [Header("Moving Platforms")]
    [SerializeField] private bool _fallingPlatforms = false;

    [SerializeField] private float _platformSpeedMin;//скорость для движущейся платформы
    [SerializeField] private float _platformSpeedMax;

    [Header("Traps")]
    [SerializeField] private bool _spawnTraps = false;//спавнить ли ловушки
    [SerializeField] private int _trapMin;//диапазон для ловушек
    [SerializeField] private int _trapMax;

    [Header("Essence")]
    [SerializeField] private bool _spawnEssence = false;
    [SerializeField] private int _essenceMin;
    [SerializeField] private int _essenceMax;

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
        _platformsWidth = new float[_platforms.Length];
        _platformsHeight = new float[_platforms.Length];
        for (int i = 0; i < _platforms.Length; i++)
        {
            _platformsWidth[i] = _platforms[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size.x;
            _platformsHeight[i] = _platforms[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        }
        _healingPointY = _healingPoint.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        _exitY = _exitPoint.transform.GetChild(0).GetComponent<BoxCollider2D>().size.y;
        _minHeight = transform.position.y;
        _maxHeight = _maxHeightPoint.position.y;
        _healingSpawnTime = _healingSpawnMax;
        if (_spawnEnemies)
        {
            _enemyGenerator = GetComponent<EnemyGenerator>();
            _enemyDensity = Random.Range(_enemyMin, _enemyMax);
        }
        if (_spawnTraps)
        {
            _trapGenerator = GetComponent<TrapGenerator>();
            _trapDensity = Random.Range(_trapMin, _trapMax);
        }
        if (_spawnEssence)
        {
            _essenceGenerator = GetComponent<EssenceGenerator>();
            _essenceDensity = Random.Range(_essenceMin, _essenceMax);
        }
    }

    private void Update()
    {
        if (_spawning)//спавн новых платформ (любых)
        {
            if (_healingIsDecreasing)//запущен ли отсчет для спавна следующей хилки
                _healingSpawnTime -= Time.deltaTime * _healingSpawnCoefficient;
            if (transform.position.x < _generationPoint.position.x)
            {

                SpawnPlatform();

                if (_spawnEnemies)
                {
                    if (_enemyDensity <= 0)//пришло время спавна
                    {
                        SpawnEnemy();
                    }
                    else
                        _enemyDensity--;
                }

                    
                if (_healingSpawnTime <= 0 && _currentPlatformAmount > _healingSpawnMin)//спавн платформы с хилкой
                {
                    SpawnHealingPoint();
                }
                if (_spawnExit)//если спавним выход
                {
                    Instantiate(_exitPoint, transform.position + new Vector3(Random.Range(-_platformsWidth[_platformSelector] / 4f, _platformsWidth[_platformSelector] / 4f),
                       _platforms[_platformSelector].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2f + _exitY / 2f, 0), transform.rotation);
                    _spawning = false;//прекращаем спавн платформ
                    _isExitPlatform = true;
                }

                if (_fallingPlatforms)
                {
                    if (!_isExitPlatform && !_isHealingPlatform && !_isEnemyPlatform)
                    {
                        SetPlatformType();                        
                    }
                }

                if (_spawnTraps)
                {
                    if (_trapDensity <= 0 && !_isExitPlatform)//если не выход
                    {
                        SpawnTrap();
                    }
                    _trapDensity--;
                }

                if (_spawnEssence)
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
        _healingSpawnTime = _healingSpawnMax;
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
        distanceBetween = Random.Range(_distanceMin, _distanceMax);
        float currentPlatformWidth = _platformsWidth[_platformSelector];//для сохранения длины предыдущей платформы, для корректного спавна
        _platformSelector = Random.Range(0, _platforms.Length);
        _heightChange = transform.position.y + Random.Range(_maxHeightChange, -_maxHeightChange);//изменения от положения текущей платформы
        if (_heightChange > _maxHeight)
            _heightChange = _maxHeight;
        if (_heightChange < _minHeight)
            _heightChange = _minHeight;


        transform.position = new Vector3(transform.position.x + _platformsWidth[_platformSelector] / 2f + currentPlatformWidth / 2f + distanceBetween, _heightChange, transform.position.z);
        _currentPlatform = Instantiate(_platforms[_platformSelector], transform.position, transform.rotation);
        _currentPlatformAmount++;
    }

    private void SpawnEnemy()
    {
        _enemyGenerator.SetSpawnPosition(transform.position.x, transform.position.y);//место спавна врага
        _enemyGenerator.SetBorders(_platformsWidth[_platformSelector], _platformsHeight[_platformSelector]);//ширина текущей платформы
        _enemyGenerator.SpawnEnemy();
        _enemyDensity = Random.Range(_enemyMin, _enemyMax);
        _isEnemyPlatform = true;
    }

    private void SpawnEssence()
    {
        _essenceGenerator.SetSpawnPosition(transform.position.x, transform.position.y);
        _essenceGenerator.SetPlatformHeight(_platformsHeight[_platformSelector]);
        _essenceGenerator.SpawnEssence();
        _essenceDensity = Random.Range(_essenceMin, _essenceMax);
    }

    private void SpawnTrap()
    {
        _trapGenerator.SetSpawnPosition(transform.position.x, transform.position.y);
        _trapGenerator.SetPlatformHeight(_platformsHeight[_platformSelector]);
        _trapGenerator.SpawnTrap();
        _trapDensity = Random.Range(_trapMin, _trapMax);
    }

    private void SetPlatformType()
    {
        float chance = Random.Range(0f, 3f);
        if (chance > 1.0 && _currentPlatform.transform.GetChild(0).GetComponent<MovingPlatform>() != null)
        {
            _currentPlatform.transform.GetChild(0).GetComponent<MovingPlatform>().SetMoving(_minHeight, _maxHeight, Random.Range(_platformSpeedMin, _platformSpeedMax));
        }
        else
        {
            _currentPlatform.transform.GetChild(0).GetComponent<FallingPlatform>().SetFalling();
        }
    }

    private void SpawnHealingPoint()
    {
        if (_healingPointAmount > _intervalAmount)//если пора спавнить выход
        {
            _spawnExit = true;
        }
        else
        {
            _lastHealingPointX = transform.position.x;
            float delta = Random.Range(0, _platformsWidth[_platformSelector] / 4f);
            _lastHealingPointX += delta;
            Instantiate(_healingPoint, transform.position + new Vector3(delta,
                _platforms[_platformSelector].transform.GetChild(0).GetComponent<BoxCollider2D>().size.y / 2f + _healingPointY / 2f, 0), transform.rotation);
            _healingPointAmount++;
            _currentPlatformAmount = 0;
            _isHealingPlatform = true;
        }
        _healingSpawnTime = _healingSpawnMax;
    }
}
