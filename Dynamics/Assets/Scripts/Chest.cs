using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour 
{
	public GameObject player;
	public bool left;
	public float ground, verPos, horPos, gravity;

	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player");

		left = false;
		verPos = 5f;
		horPos = 5f;
		ground = -4f;
		gravity = 2f;
	}

	void FixedUpdate () 
	{
		if (player.transform.position.x > 0)
			left = true;
		else
			left = false;

		if (verPos >= ground)
			verPos -= gravity * Time.fixedDeltaTime;

		if (left)
			horPos = -5f;
		else
			horPos = 5f;
	}

	public void Drop()
	{
		GameObject chest;
		chest = Instantiate(gameObject, new Vector3(horPos, verPos, 0), Quaternion.identity) as GameObject;
	}
}
