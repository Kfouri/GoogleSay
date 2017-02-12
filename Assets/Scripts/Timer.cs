using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {


	public TextMesh tiempo;
    
	// Use this for initialization
	void Start () {
		tiempo.text = "0";
		InvokeRepeating("IncrementarTiempo", 1, 1); 
	}
	
	void IncrementarTiempo()
	{

		tiempo.text = (int.Parse(tiempo.text) + 1).ToString();
	}
}
