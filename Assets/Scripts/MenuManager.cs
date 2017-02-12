using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public void StartGame()
	{
		Application.LoadLevel("Escena1");
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
	
	public void Ranking()
	{
		Application.LoadLevel("ranking");
	}
}
