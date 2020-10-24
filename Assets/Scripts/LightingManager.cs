using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

[ExecuteAlways]
[RequireComponent(typeof(Light2D))]
public class LightingManager : MonoSingleton<LightingManager>
{
    private const float FullDay = 24f;
    private const float HalfDay = 12f;
    [SerializeField] private Light2D globalLight;
    [SerializeField, Range(0, 24)] private float currentTime = 0;
    [SerializeField] private float dayCycleTime = 60;
    [SerializeField] private float dayIntensity;
    [SerializeField] private float nightIntensity;
    [SerializeField] private IEnumerable<Light2D> _mapLights;
    private bool _day;
    private float _secondCounter;

    private void Start()
    {
        globalLight.intensity = nightIntensity;
        _day = false;
        _mapLights = new List<Light2D>();
    }

    private void Update()
    {
        SetDayTime();
    }

    private void SetDayTime()
    {
        currentTime += Time.deltaTime / dayCycleTime * FullDay;

        _day = !(currentTime < HalfDay);

        if (_day)
            globalLight.intensity = dayIntensity - ((dayIntensity - nightIntensity) * ((currentTime - HalfDay) / HalfDay));
        else
            globalLight.intensity = nightIntensity + ((dayIntensity - nightIntensity) * (currentTime / HalfDay));


        if (currentTime >= FullDay)
            currentTime = 0;
    }


    private void ControlLightMaps(bool status)
    {
        if (_mapLights == null || _mapLights?.Count() <= 0) return;
        foreach (var mappedLight in _mapLights)
            mappedLight.gameObject.SetActive(status);
    }
}