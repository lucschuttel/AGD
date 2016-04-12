using UnityEngine;
using System.Collections;

public class LoadAfterSeconds : MonoBehaviour 
{
	void Update () 
	{
		StartCoroutine(StartGame());
	}
	
	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(3f);
		Application.LoadLevel(0);
	}
}
