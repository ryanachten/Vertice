using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Import_VerifyDateType : MonoBehaviour {

	public InputField dateInputField;

	public void VerifyDateInput()
	{
		if (dateInputField.text.Length > 0)
		{
			try 
			{
				System.DateTime dateType = System.DateTime.Parse(dateInputField.text);
				dateType.ToString("yyyy/mm/dd");
			}
			catch (System.Exception ex) 
			{
				StartCoroutine(ErrorFeedback());
			}
		}
	}

	IEnumerator ErrorFeedback()
	{
		dateInputField.text = "Date format must be [YYYY/MM/DD]"; 

		yield return new WaitForSeconds(3);

		dateInputField.text = "";
	}
}
