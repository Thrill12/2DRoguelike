using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public string parameter;

    public AudioMixer mixer;
    private Slider slider;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(parameter, 0.75f);
        mixer.SetFloat(parameter, Mathf.Log10(slider.value) * 20);
    }

    private void OnLevelWasLoaded(int level)
    {
        Start();
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(parameter, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(parameter, slider.value);
    }
}
