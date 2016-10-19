using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadProgressBar : MonoBehaviour {

	public Slider progressSlider;
	public Text progressTask;


	public void SetMaxVal(int sliderMaxVal)
	{
		progressSlider.maxValue = sliderMaxVal;
		progressSlider.value = 0;
		progressTask.text = "";
	}


	public void AddTask(string taskName)
	{
		progressSlider.value++;
		progressTask.text = taskName;

//		Debug.Log("Task name: " + taskName + " taskNumber: " + progressSlider.value);

		if (progressSlider.value == progressSlider.maxValue)
		{
//			Debug.Log("Hit max, turning off progress bar");
			gameObject.SetActive(false);
		}
			
	}
}
