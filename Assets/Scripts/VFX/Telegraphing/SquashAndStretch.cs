using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    [SerializeField] private Vector3 TargetSquashPercentage;
    [SerializeField] private Vector3 TargetStretchPercentage;
    [SerializeField] private float StretchRate;
    [SerializeField] private float SquashRate;
    [SerializeField] private float ReturnToNormalRate;
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private float _currentRate;
    private bool _isAnimating;


    public System.Action OnAnimComplete;
    public void Init()
    {
        _originalScale = transform.localScale;
    }
    public void ReturnToNormal()
    {
        _targetScale = _originalScale;
        _currentRate = ReturnToNormalRate;
        _isAnimating = true;
    }
    public void DoSquash()
    {
        _targetScale = new Vector3(_originalScale.x * TargetSquashPercentage.x, 
            _originalScale.y * TargetSquashPercentage.y, _originalScale.z * TargetSquashPercentage.z);
        _currentRate = SquashRate;
        _isAnimating = true;
    }
    public void DoStretch()
    {
        _targetScale = new Vector3(_originalScale.x * TargetStretchPercentage.x,
           _originalScale.y * TargetStretchPercentage.y, _originalScale.z * TargetStretchPercentage.z);
        _currentRate = StretchRate;
        _isAnimating = true;
    }

    public void Update()
    {
        if (!_isAnimating)
        {
            return;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * _currentRate);


        if (Vector3.Distance(transform.localScale, _targetScale) < 0.01f)
        {
            _isAnimating = false;
            transform.localScale = _targetScale;
            OnAnimComplete?.Invoke();

        }
    }

    private void OnDisable()
    {
        _isAnimating = false;
        transform.localScale = _originalScale;
    }

    public bool Animating { get { return _isAnimating; } }
}
