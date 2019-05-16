using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{

	public static Rooms instance;

	private void Awake()
	{
		instance = this;
	}

	public GameObject[] rooms;
}
