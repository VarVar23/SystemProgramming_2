using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int _health;
    private int _maxHp = 100;
    private int _countAddHp = 5;

    private float _timeBoost = 3;
    private float _timeWait = 0.5f;


    private void Start() => ReceiveHealth();

    private void ReceiveHealth()
    {
        StopCoroutine(AddHealth()); // Выполнение задачи: "Не может действовать более 1 эффекта"
        StartCoroutine(AddHealth());
    }

    private IEnumerator AddHealth()
    {
        for(float time = 0; time < _timeBoost; time += _timeWait)
        {

            if (_health + _countAddHp > _maxHp) _health = _maxHp;
            else _health += _countAddHp;

            if (_health == _maxHp) yield break;
            yield return new WaitForSeconds(_timeWait);
        }
    }
}
