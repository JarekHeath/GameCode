using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float dayLength;
    public float begin = 0.5f;
    private float cycleSpeed;
    public Vector3 noon;
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    public AnimationCurve lightingIntensity;
    public AnimationCurve reflections;

    void Start()
    {
        cycleSpeed = 1.0f / dayLength;
        time = begin;
    }

    // Update is called once per frame
    void Update()
    {
        //Increment time
        time += cycleSpeed * Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
        }

        //Light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        //Light intensity
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        //Change colors
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        //enable and disable sun
        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
        }
        else if (sun.intensity > 0  && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
        }

        //enable and disable moon
        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
        }

        //Lighting and reflections intensity
        RenderSettings.ambientIntensity = lightingIntensity.Evaluate(time);
        RenderSettings.reflectionIntensity = reflections.Evaluate(time);

    }
}
