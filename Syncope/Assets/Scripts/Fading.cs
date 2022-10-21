using UnityEngine;

public class Fading : MonoBehaviour
{
    [SerializeField] private float _additionCoefficient = 2f;
    [SerializeField] private float _divisionCoefficient = 2.2f;

    private SpriteRenderer _character;
    private Color _color;
    private HealthBar _health;

    private void Start()
    {
        _character = GetComponent<SpriteRenderer>();
        _color = _character.material.color;
        _health = GetComponent<HealthBar>();
    }

    private void Update()
    {
        _color.a = Mathf.Log((_health.getFill()+ _additionCoefficient) * (_health.getFill() + _additionCoefficient))/ _divisionCoefficient;
        _character.material.color = _color;
    }
}
