using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy {

	public Vector3[] points;
	public float birthTime;
	public float lifeTime = 10;
	public float sinEccentricity = 0.6f;;
	

	
	// Use this for initialization
	void Start () {
		points = new Vector3[2];

		Vector3 cbMin = Utils.camBounds.min;
		Vector3 cbMax = Utils.camBounds.max;

		Vector3 v = Vector3.zero;
		v.x = cbMin.x - Main.S.enemySpawnPadding;
		v.y = Random.Range (cbMin.y, cbMax.y);
		points [0] = v;
		v = Vector3.zero;
		v.x = cbMax.x + Main.S.enemySpawnPadding;
		v.y = Random.Range (cbMin.y, cbMax.y);
		points [1] = v;

		if (Random.value < .5f) {
			points[0].x*= -1;
			points[1].x*= -1;
		}
		birthTime = Time.time;
	}
	
	public override void Move(){

		//start here
		Vector3 tempPos = pos;
		float age = Time.time - birthTime;
		float theta = Mathf.PI * 2 * age / waveFrequency;
		float sin = Mathf.Sin (theta);
		tempPos.x = x0 + waveWidth * sin;
		pos = tempPos;
		
		Vector3 rot = new Vector3 (0, sin * waveRotY, 0);
		this.transform.rotation = Quaternion.Euler (rot);
		base.Move ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

