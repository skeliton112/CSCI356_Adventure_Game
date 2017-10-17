using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class volumeSlider : MonoBehaviour {

	public Slider slider;
	public AudioSource volumeAudio;

	public void VolumeController(){
		slider.value = volumeAudio.volume;
	}
}
