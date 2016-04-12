using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour 
{
	public void LoadScene(int level)
	{
		// The level being loaded is specified in the inspector
		Application.LoadLevel (level);
	}
}