﻿using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
	public GameObject   poi;
	public GameObject[] panels;
	public float 	    scrollSpeed = -30f;
	public float	    motionMult = .25f;

	private float panelHt;
	private float depth;


	// Use this for initialization
	void Start () {
		panelHt = panels [0].transform.localScale.y;
		depth = panels [0].transform.position.z;

		panels[0].transform.position = new Vector3 (0, 0, depth);
		panels[1].transform.position = new Vector3 (0, panelHt, depth);
	}
	
	// Update is called once per frame
	void Update () {
		float tY, tX = 0;
		tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);

		if (poi != null) {
			tX = -poi.transform.position.x * motionMult;
		}

		panels [0].transform.position = new Vector3 (tX, tY, depth);
		if (tY >= 0) {
			panels [1].transform.position = new Vector3 (tX, tY - panelHt, depth);
		} else {
			panels[1].transform.position = new Vector3(tX, tY+panelHt, depth);
		}
	}
}
