using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {


	public GameObject mainCamera;

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

	public void ResetGame()
	{
		Application.LoadLevel("Escena1");
	}
	
	public void ExitGame()
	{
		Application.LoadLevel("menuPrincipal");
	}
}