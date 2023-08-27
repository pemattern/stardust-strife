using System.Threading.Tasks;
using UnityEngine;

public class Overheat
{
    private float _maxValue, _increment, _coolingRate, _cooldownDelay;
    private float _currentValue;
    private bool _locked;
    private Task _cooldownDelayTask;
    private State _state;

    public bool Locked => _locked;

    public Overheat(State state, float maxValue, float increment, float coolingRate, float cooldownDelay)
    {
        _locked = false;
        _cooldownDelayTask = Task.Delay(0);
        _maxValue = maxValue;
        _increment = increment;
        _coolingRate = coolingRate;
        _cooldownDelay = cooldownDelay;
        _state = state;
    }

    ~Overheat()
    {
        _state.Entered -= Increment;
        _state.Updated -= Decrement;
    }
    void Decrement()
    {
        if (_currentValue > 0f && _cooldownDelayTask.IsCompleted)
        {
            _currentValue -= Time.deltaTime * _coolingRate;
            _currentValue = Mathf.Clamp(_currentValue, 0f, _maxValue);
        } 
        if (_currentValue <= 0f && _locked == true) _locked = false;
    }

    void Increment()
    {
        if (_locked) return;
        _cooldownDelayTask = Task.Delay((int)(_cooldownDelay * 1000));
        _currentValue += _increment;
        if (_currentValue > _maxValue) Lock();
        _currentValue = Mathf.Clamp(_currentValue, 0f, _maxValue);

    }

    void Lock()
    {
        _locked = true;
    }
}
