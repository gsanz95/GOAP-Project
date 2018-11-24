using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actorStats : MonoBehaviour{

	public float maxHealth;
	public float maxBulletCount;
	public float shootingCoolDown;
	private float currentHealth;
	private float currentBulletCount;
	private bool isAlive;
	private bool hasKey;

	void Start() {
		currentHealth = maxHealth;
		currentBulletCount = maxBulletCount;
		isAlive = true;
	}

	public void takeDamage(float damage) {
		currentHealth -= damage;
		if(currentHealth <= 0f)
			isAlive = false;
	}

	public bool hasBullet() {
		if(currentBulletCount > 0f)
			return true;
		return false;
	}

	public bool HasKey
	{ get; set; }
	
	void useBullet() {
		currentBulletCount -= 1f;
	}
}
