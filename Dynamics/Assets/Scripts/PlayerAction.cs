using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour 
{
	public GameObject boss, life1, life2, life3, life4, gun, helmet, bullet;
	public BossAction bossAction;
	public GameObject[] tiles, chests;
	public float walkSpeed, jumpSpeed, gravity, horPos, verPos, maxJump, ground, startJumpPos, bulletSpeed, dir;
	public Vector3 lastPos;
	public bool grounded, up, down, jumping, isHit, above, under, hasGun, hasHelmet, coolDown, cannotHarm;
	public SpriteRenderer sprite, gunSprite, helmetSprite, bulletSprite;
	public int lifes;
	public AudioSource auw, bossHit;

	void Awake () 
	{
		bullet = GameObject.FindGameObjectWithTag("Bullet");
		bulletSprite = bullet.GetComponent<SpriteRenderer>();
		bulletSprite.enabled = false;

		gun = GameObject.FindGameObjectWithTag("Gun");
		gunSprite = gun.GetComponent<SpriteRenderer>();
		gunSprite.enabled = false;

		helmet = GameObject.FindGameObjectWithTag("Helmet");
		helmetSprite = helmet.GetComponent<SpriteRenderer>();
		helmetSprite.enabled = false;

		life1 = GameObject.FindGameObjectWithTag("PL_1");
		life2 = GameObject.FindGameObjectWithTag("PL_2");
		life3 = GameObject.FindGameObjectWithTag("PL_3");
		life4 = GameObject.FindGameObjectWithTag("PL_4");

		boss = GameObject.FindGameObjectWithTag("Boss");
		bossAction = boss.GetComponent<BossAction>();
		tiles = GameObject.FindGameObjectsWithTag("Tile");

		sprite = GetComponentInChildren<SpriteRenderer>();

		auw = GetComponent<AudioSource>();
		bossHit = boss.GetComponent<AudioSource>();

		bulletSpeed = 5f;
		dir = 1f;
		walkSpeed = 5f;
		jumpSpeed = 0.75f;
		gravity = 6f;
		maxJump = 2.5f;
		ground = -4f;

		lifes = 4;

		coolDown = true;
		grounded = false;
		up = false;
		down = false;
		jumping = false;
		isHit = false;
		above = false;
		under = false;
		hasHelmet = false;
		hasGun = false;
		cannotHarm = false;

		horPos = transform.position.x;
		verPos = transform.position.y;
	}

	void FixedUpdate () 
	{
		chests = GameObject.FindGameObjectsWithTag("Chest");

		InputHelper();

		BossCol();

		Hit();

		Grounding ();

		UnderAbove();

		ChestPickup();

		transform.position = new Vector3(horPos, verPos, 0);
		transform.localScale = new Vector3(dir, 1, 0);
		lastPos = transform.position;
	}

	void InputHelper()
	{
		if (Input.GetKey(KeyCode.LeftArrow) && !(transform.position.x < -7.6f || Input.GetKey(KeyCode.RightArrow)) && (lifes > 0))
		{
			dir = -1f;
			horPos -= walkSpeed * Time.fixedDeltaTime;
		}
		else if (Input.GetKey(KeyCode.RightArrow) && !(transform.position.x > 7.6f || Input.GetKey(KeyCode.LeftArrow)) && (lifes > 0))
		{
			dir = 1f;
			horPos += walkSpeed * Time.fixedDeltaTime;
		}

		if (Input.GetKey(KeyCode.UpArrow) && grounded && (lifes > 0))
		{
			jumping = true;
			startJumpPos = transform.position.y;
		}

		if (transform.position.y > (startJumpPos + maxJump))
			jumping = false;

		if (verPos >= ground && !jumping)
			verPos -= gravity * Time.fixedDeltaTime;

		if (jumping)
			Jump();

		if (Input.GetKey(KeyCode.Space) && hasGun && coolDown)
			Shoot();
	}

	void Shoot()
	{
		coolDown = false;
		GameObject bulletClone;
		bulletClone = Instantiate(bullet, new Vector3(bullet.transform.position.x, bullet.transform.position.y, 0), Quaternion.identity) as GameObject;
		StartCoroutine(Cool());
	}

	IEnumerator Cool()
	{
		yield return new WaitForSeconds(0.7f);
		coolDown = true;
	}

	void ChestPickup()
	{
		foreach (GameObject chest in chests)
		{
			if (bossAction.phase == 2 && (Mathf.Abs (transform.position.x - chest.transform.position.x) < 0.64f) 
			    && (Mathf.Abs (transform.position.y - chest.transform.position.y) < 0.64f))
			{
				helmetSprite.enabled = true;
				hasHelmet = true;
				Destroy(chest);
			}
			else if (bossAction.phase == 3 && (Mathf.Abs (transform.position.x - chest.transform.position.x) < 0.64f) 
			    && (Mathf.Abs (transform.position.y - chest.transform.position.y) < 0.64f))
			{
				gunSprite.enabled = true;
				hasGun = true;
				Destroy (chest);
			}
		}
	}
	
	void Jump()
	{
		verPos += (jumpSpeed * Time.fixedDeltaTime) / (0.05f + Time.fixedDeltaTime);
	}
	
	void Grounding()
	{
		if (verPos < -4.0f)
			grounded = true;
		else
			grounded = false;
	}

	void BossCol()
	{
		if ((Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.96f)  && !isHit && !above  && (bossAction.phase == 1)
		    && (((transform.position.y - boss.transform.position.y) > -0.8f) && ((transform.position.y - boss.transform.position.y) < 0.8f)))
		{
			isHit = true;
			lifes--;
			auw.Play();
			StartCoroutine(Flash());
		}
		else if (above && ((transform.position.y - boss.transform.position.y) < 0.8f) && !isHit && !bossAction.isHit && (bossAction.phase == 1))
		{
			bossHit.Play();
			bossAction.isHit = true;
			bossAction.startFlash = true;
			jumping = true;
			bossAction.lifes--;
		}
		else if (above && (Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.96f) 
		    && (Mathf.Abs(transform.position.y - boss.transform.position.y) < 0.8f) && !isHit && (bossAction.phase == 2))
			jumping = true;
		else if (!under && !above && (Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.96f) 
		         && (Mathf.Abs(transform.position.y - boss.transform.position.y) < 0.8f) && !isHit && !bossAction.isHit && (bossAction.phase == 2) && !cannotHarm)
		{
			isHit = true;
			lifes--;
			auw.Play();
			StartCoroutine(Flash());
		}
		else if (under && ((transform.position.y - boss.transform.position.y) > -0.8f) && !isHit && !bossAction.isHit && (bossAction.phase == 2) && !hasHelmet)
		{
			isHit = true;
			lifes--;
			auw.Play();
			StartCoroutine(Flash());
		}
		else if (under && ((transform.position.y - boss.transform.position.y) > -0.8f) && !isHit && !bossAction.isHit && (bossAction.phase == 2) && hasHelmet)
		{
			cannotHarm = true;
			bossHit.Play();
			bossAction.isHit = true;
			bossAction.startFlash = true;
			jumping = true;
			bossAction.lifes--;
		}
		else if ((Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.96f) 
		         && (Mathf.Abs(transform.position.y - boss.transform.position.y) < 0.8f) && !isHit && (bossAction.phase == 3) && !cannotHarm)
		{
			isHit = true;
			lifes--;
			auw.Play();
			StartCoroutine(Flash());
		}
	}

	void UnderAbove()
	{
		if ((Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.96f)  && !isHit 
		    && ((transform.position.y - boss.transform.position.y) >= 0.8f))
		{
			above = true;
		}
		else if (Mathf.Abs (transform.position.x - boss.transform.position.x) >= 0.96f)
			above = false;
		
		if ((Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.96f)  && !isHit 
		    && ((transform.position.y - boss.transform.position.y) < -0.8f))
		{
			under = true;
		}
		else if (Mathf.Abs (transform.position.x - boss.transform.position.x) >= 0.96f)
			under = false;
	}

	void Hit()
	{
		switch (lifes) 
		{
		case 0:
			life1.SetActive(false);
			life2.SetActive(false);
			life3.SetActive(false);
			life4.SetActive(false);
			break;
		case 1:
			life1.SetActive(true);
			life2.SetActive(false);
			life3.SetActive(false);
			life4.SetActive(false);
			break;
		case 2:
			life1.SetActive(true);
			life2.SetActive(true);
			life3.SetActive(false);
			life4.SetActive(false);
			break;
		case 3:
			life1.SetActive(true);
			life2.SetActive(true);
			life3.SetActive(true);
			life4.SetActive(false);
			break;
		case 4:
			life1.SetActive(true);
			life2.SetActive(true);
			life3.SetActive(true);
			life4.SetActive(true);
			break;
		default:
			break;
		}
	}

	IEnumerator Flash()
	{
		sprite.enabled = false;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = false;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = true;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = true;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = false;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = false;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = true;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = true;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = false;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = false;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = true;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = true;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = false;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = false;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = true;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = true;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = true;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = false;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = false;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = false;
		yield return new WaitForSeconds(0.2f);
		sprite.enabled = true;
		if (bossAction.phase == 2 && hasHelmet)
			helmetSprite.enabled = true;
		if (bossAction.phase == 3 && hasGun)
			gunSprite.enabled = true;
		isHit = false;
		if (lifes <= 0)
			GameOver();
	}

	void GameOver()
	{
		Application.LoadLevel(2);
	}
}
