﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Part{
	public string name;
	public float  health;
	public string[] protectedBy;

	public GameObject go;
	public Material   mat;
}

public class Emeny4 : Enemy {

	public Vector3[] points;
	public float	 timeStart;
	public float 	 duration = 4;

	public Part[]	 parts;

	// Use this for initialization
	void Start () {
		points [0] = pos;
		points [1] = pos;
		InitMovement ();

		Transform t;
		foreach (Part prt in parts) {
			t = transform.Find (prt.name);
			if(t != null)
			{
				prt.go = t.gameObject;
				prt.mat = prt.go.renderer.material;
			}
		}

	}

	void InitMovement(){
		Vector3 p1 = Vector3.zero;
		float esp = Main.S.enemySpawnPadding;
		Bounds cBounds = Utils.camBounds;
		p1.x = Random.Range (cBounds.min.x + esp, cBounds.max.x - esp);
		p1.y = Random.Range (cBounds.min.y + esp, cBounds.max.y - esp);

		points [0] = points [1];
		points [1] = p1;

		timeStart = Time.time;
	}

	public override void Move () {
		float u = (Time.time - timeStart) / duration;
		if (u > 1) {
			InitMovement();
			u=0;
		}
		u = 1 - Mathf.Pow (1 - u, 2);
		pos = (1 - u) * points [0] + u * points [1];
	}

	void OnCollisionEnter(Collision coll)
	{
		GameObject other = coll.gameObject;
		switch (other.tag) {
		case "ProjecileHero":
			Projectile p = other.GetComponent<Projectile>();
			bounds.center = transform.position + boundsCenterOffset;
			if(bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen)!= Vector3.zero)
			{
				Destroy(other);
				break;
			}

			GameObject goHit = coll.contacts[0].thisCollider.gameObject;
			Part prtHit = FindPart(goHit);
		

			if (prtHit.protectedBy != null) {
				foreach(string s in prtHit.protectedBy)
				{
					if (!Destroy(s)){
						Destroy(other);
						return;
					}
				}
			}

			prtHit.health -= Main.W_DEFS [pos.type].damageOnHit;
			ShowLocalizedDanage (prtHit.mat);
			if (prtHit.health <= 0) {
				prtHit.go.SetActive(false);		
			}
			bool allDestroyed = true;
			foreach (Part prt in parts) {
				if(!Destroy(prt)){
					allDestroyed = false;
					break;
				}		
			}
			if (allDestroyed) {
				Main.S.ShipDestroyed(this);
				Destroy(this.gameObject);
			}
			Destroy (other);
			break;
		}
	}

	Part FindPart(string n){
		foreach(part prt in parts){
			if(prt.name == n){
				return(prt);
			}
		}
		return(null);
	}
	Part FindPart(GameObject go){
		foreach(part prt in parts){
			if (prt.go == go){
				return(prt);
			}
		}
		return(null);
	}

	bool Destroyed(GameObject go){
		return(Destroyed(FindPart(go)));
	}
	bool Destroyed(string n){
		return(Destroyed(FindPart(n)));
	}
	bool Destroyed(Part prt){
		if(prt == null){
			return true;
		}
		return (prt.health <= 0);
	}

	void ShowLocalizedDanage(Material m){
		m.color = Color.red;
		remainingDamageFrames = showDamageForFrames;
	}
}
