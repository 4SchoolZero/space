using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SyncCrosshair : MonoBehaviour
{
    [SerializeField] private PlayerShip flight;

    
    [SerializeField] private Image centerDot;
    private Vector3 _centerPos;

    [SerializeField] private Image outerCircle;
    [SerializeField] private Image innerCircle;

    [SerializeField] private float multiplier = 10f;


    private float _crossSize;
    // Start is called before the first frame update
    
    void OnEnable()
    {
        _centerPos = centerDot.rectTransform.anchoredPosition;
        _crossSize = centerDot.rectTransform.sizeDelta.x;
    }

    private void ScaleOuterCircle()
    {
        var outerRadius = Mathf.RoundToInt(flight.ControlSettings.maxClamp * multiplier * 2 + _crossSize);
        outerCircle.rectTransform.sizeDelta = Vector2.one * outerRadius;
        
        var innerRadius = Mathf.RoundToInt(flight.ControlSettings.minMouseMov * multiplier * 2 + _crossSize/2);
        innerCircle.rectTransform.sizeDelta = Vector2.one * innerRadius;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCenterDot();
        ScaleOuterCircle();
    }

    private void MoveCenterDot()
    {
        var newPos = _centerPos;
        newPos.x = flight.LookDir.x * multiplier;
        newPos.y = flight.LookDir.y * multiplier;
        centerDot.rectTransform.anchoredPosition = newPos;
    }
}
