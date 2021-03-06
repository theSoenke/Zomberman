﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class FlyingCandy : MonoBehaviour {
	private int zombieHits = 0;
	public int maxZombieHits = 3;

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
            return;
		var enemyController = collision.gameObject.GetComponent<EnemyController>();
		if (enemyController != null) {
			enemyController.Die();
			zombieHits ++;
			if (zombieHits >= maxZombieHits) {
				Destroy (gameObject);
			}
		}
        //var tilemap = collision.gameObject.GetComponent<Tilemap>();
        //if(tilemap != null)
        //{
        //    Destroy(gameObject, 0.05f);
        //}
    }
}
