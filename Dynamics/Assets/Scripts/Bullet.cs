using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public bool isInstance;
	public GameObject player, boss;
	public PlayerAction playerAction;
	public BossAction bossAction;
	public float horPos, verPos, dir, bulletSpeed;
	public SpriteRenderer rend;

	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerAction = player.GetComponent<PlayerAction>();
		boss = GameObject.FindGameObjectWithTag("Boss");
		bossAction = boss.GetComponent<BossAction>();

		rend = GetComponent<SpriteRenderer>();

		if (bossAction.phase == 3)
			isInstance = true;
		else
			isInstance = false;

		if (isInstance)
		{
			verPos = transform.position.y;
			horPos = transform.position.x;
			dir = playerAction.dir;
			bulletSpeed = playerAction.bulletSpeed;
		}
	}

	void FixedUpdate () 
	{
		if (isInstance)
		{
			rend.enabled = true;
			horPos += dir * bulletSpeed * Time.fixedDeltaTime;

			HitBoss();
			
			DestroySelf();

			transform.position = new Vector3(horPos, verPos, 0);
		}
	}	

	void HitBoss()
	{
		if ((Mathf.Abs (transform.position.x - boss.transform.position.x) < 0.64f) 
		&& (Mathf.Abs (transform.position.y - boss.transform.position.y) < 0.64f))
		{
			Destroy(gameObject);
			bossAction.isHit = true;
			bossAction.startFlash = true;
			bossAction.lifes--;
		}
	}

	void DestroySelf()
	{
		if (Mathf.Abs(horPos) > 8f)
			Destroy(gameObject);
	}
}
