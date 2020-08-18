using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

	public float leftTime;
	public float leftTimeEffect =4f;

	private float lastTime;
	private bool isPlayEffect =true;
	public bool startTime;
	void OnTriggerEnter2D(Collider2D other)
	{
		 
		if (other.tag=="Player")
		{
			Destroy(gameObject);
			GameManager.Instance.coinBoxNum += 1;
		}
        
	}

	void Update()
	{
		if (startTime == false)
		{
			return;
		}

		if (Time.time  - lastTime > leftTimeEffect)
		{
			if (isPlayEffect)
			{
				//Debug.LogError("   Animator     ");
				transform.GetComponent<Animator>().Play("补充1");
				isPlayEffect = false;
			}
		}
		if (Time.time - lastTime>leftTime)
		{
			Destroy(gameObject);
		}
	}

	public void StartTime()
	{
		startTime = true;
		lastTime = Time.time;
	}
	
	
	
}
