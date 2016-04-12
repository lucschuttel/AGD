using UnityEngine;
using System.Collections;

public class BossAction : MonoBehaviour 
{ 
	public Chest chestDrop;
	public PlayerAction playerAction;
	public float moveSpeed, horPos, verPos, dir, ground, jumpSpeed, maxJump, gravity;
	public int phase, lifes;
	public GameObject player, chest, life1, life2, life3, body1, body2, body3;
	public bool isHit, startFlash, changePhase, jumping, startJump, waiting;
	public SpriteRenderer sprite1, sprite2, sprite3, lifeSprite1, lifeSprite2, lifeSprite3;

	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerAction = player.GetComponent<PlayerAction>();

		chest = GameObject.FindGameObjectWithTag("Chest");
		chestDrop = chest.GetComponent<Chest>();

		body1 = GameObject.FindGameObjectWithTag("Body1");
		body2 = GameObject.FindGameObjectWithTag("Body2");
		body3 = GameObject.FindGameObjectWithTag("Body3");
		sprite1 = body1.GetComponent<SpriteRenderer>();
		sprite2 = body2.GetComponent<SpriteRenderer>();
		sprite3 = body3.GetComponent<SpriteRenderer>();

		sprite2.enabled = false;
		sprite3.enabled = false;

		life1 = GameObject.FindGameObjectWithTag("B_1");
		life2 = GameObject.FindGameObjectWithTag("B_2");
		life3 = GameObject.FindGameObjectWithTag("B_3");

		gravity = 7f;
		phase = 1;
		moveSpeed = 4f;
		dir = 1f;
		ground = -3.87f;
		lifes = 3;
		jumpSpeed = 0.5f;
		maxJump = 2f;

		isHit = false;
		startFlash = false;
		changePhase = false;
		jumping = false;
		startJump = false;
		waiting = false;

		horPos = transform.position.x;
		verPos = transform.position.y;
	}

	void FixedUpdate ()
	{
		Hit();

		PhaseSwitch();

		if (startFlash)
		{
			StartCoroutine(Flash ());
			startFlash = false;
		}

		if (transform.position.y > (ground + maxJump))
			jumping = false;

		if (verPos >= ground && !jumping)
			verPos -= gravity * Time.fixedDeltaTime;

		transform.position = new Vector3(horPos, verPos, 0);
	}
	
	void PhaseOne()
	{
		horPos += dir * moveSpeed * Time.fixedDeltaTime;
		
		if (transform.position.x > 7.0f)
			dir = -1f;
		else if (transform.position.x < -7.0f)
			dir = 1f;
	}
	
	void PhaseTwo()
	{
		if (changePhase)
		{
			sprite2.enabled = true;
			changePhase = false;
			lifes = 3;
			chestDrop.Drop();
		}

		moveSpeed = 5f;

		horPos += dir * moveSpeed * Time.fixedDeltaTime;
		
		if (transform.position.x > 7.0f)
			dir = -1f;
		else if (transform.position.x < -7.0f)
			dir = 1f;

		if (transform.position.y <= ground && !isHit)
		{
			startJump = true;
			jumping = true;
		}

		if (startJump)
		{
			StartCoroutine(WaitForJump());
			startJump = false;
		}
	}

	IEnumerator WaitForJump()
	{
		yield return new WaitForSeconds(1f);
		verPos += (jumpSpeed * Time.fixedDeltaTime) / (0.05f + Time.fixedDeltaTime);
	}
	
	void PhaseThree()
	{
		if (changePhase)
		{
			sprite2.enabled = false;
			sprite3.enabled = true;
			changePhase = false;
			lifes = 3;
			
			chestDrop.Drop();

			playerAction.hasHelmet = false;
		}
		
		moveSpeed = 8f;

		horPos += dir * moveSpeed * Time.fixedDeltaTime;

		if (transform.position.x > 7.0f && lifes >= 1)
			dir = -1f;
		else if (transform.position.x < -7.0f && lifes >= 1)
			dir = 1f;
		else if (lifes <= 0)
			dir = 0f;
	}

	void Hit()
	{
		switch (lifes) 
		{
		case 0:
			life1.SetActive(false);
			life2.SetActive(false);
			life3.SetActive(false);
			break;
		case 1:
			life1.SetActive(true);
			life2.SetActive(false);
			life3.SetActive(false);
			break;
		case 2:
			life1.SetActive(true);
			life2.SetActive(true);
			life3.SetActive(false);
			break;
		case 3:
			life1.SetActive(true);
			life2.SetActive(true);
			life3.SetActive(true);
			break;
		default:
			break;
		}
	}

	void PhaseSwitch()
	{
		if (lifes <= 0 && phase < 3)
		{
			phase++;
			changePhase = true;
		}

		switch(phase)
		{
		case 1:
			PhaseOne();
			break;
		case 2:
			PhaseTwo();
			break;
		case 3:
			PhaseThree ();
			break;
		}
	}
	
	IEnumerator Flash()
	{
		sprite1.enabled = false;
		if (phase == 2)
			sprite2.enabled = false;
		if (phase == 3)
			sprite3.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = true;
		if (phase == 2)
			sprite2.enabled = true;
		if (phase == 3)
			sprite3.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = false;
		if (phase == 2)
			sprite2.enabled = false;
		if (phase == 3)
			sprite3.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = true;
		if (phase == 2)
			sprite2.enabled = true;
		if (phase == 3)
			sprite3.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = false;
		if (phase == 2)
			sprite2.enabled = false;
		if (phase == 3)
			sprite3.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = true;
		if (phase == 2)
			sprite2.enabled = true;
		if (phase == 3)
			sprite3.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = false;
		if (phase == 2)
			sprite2.enabled = false;
		if (phase == 3)
			sprite3.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = true;
		if (phase == 2)
			sprite2.enabled = true;
		if (phase == 3)
			sprite3.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = false;
		if (phase == 2)
			sprite2.enabled = false;
		if (phase == 3)
			sprite3.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite1.enabled = true;
		if (phase == 2)
			sprite2.enabled = true;
		if (phase == 3)
			sprite3.enabled = true;
		isHit = false;
		playerAction.cannotHarm = false;

		if (lifes <= 0 && phase >= 3)
			Win ();
	}

	void Win()
	{
		Application.LoadLevel(3);
	}
}
