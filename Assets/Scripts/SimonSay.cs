using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct SimonLightPlate
{

    public enum eType
	{
		INVALID_TYPE = -1,
		BLUE,
		GREEN,
		RED,
		YELLOW,
		NUM_TYPES
	}

	public SimonLightPlate(string plateName)
	{
		plate = GameObject.Find(plateName);
	}

	public GameObject plate;
}

public class SimonSay : MonoBehaviour
{
	private string formText = "";

	public TextMesh score; 
	public TextMesh txthightScore; 
	public TextMesh txtResultado;
	public TextMesh tiempo;
	public TextMesh txtTiempo;
	public AudioClip SonidoError;

	public float displaySequenceRepeatInterval = 1.0f;

	SimonLightPlate[] lightPlates = new SimonLightPlate[(int)SimonLightPlate.eType.NUM_TYPES];

	enum eState
	{
		INVALID_STATE = 1,
		THINKING,
		DISPLAY_SEQUENCE,
		WAITING_FOR_USER,
		NUM_STATES,
		MOSTRANDO
	}

	eState currentState = eState.INVALID_STATE;

	public List<int> sequence = new List<int>(100);
	public int sequeceCount = 0;

	public List<int> clickedSequence = new List<int>(100);

	int oldHighscore = 0;
	int oldTime = 0;

	// Use this for initialization
	void Start () 
	{
		//PlayerPrefs.SetInt ("highscore", 0);
		//PlayerPrefs.SetInt ("Time", 0);

		oldHighscore = PlayerPrefs.GetInt("highscore", 0);
		txthightScore.text = oldHighscore.ToString();
		oldTime = PlayerPrefs.GetInt ("Time", 0);
		txtTiempo.text = oldTime.ToString ();
 
		lightPlates[(int)SimonLightPlate.eType.BLUE] = new SimonLightPlate("AzulClaro"); 
		lightPlates[(int)SimonLightPlate.eType.GREEN] = new SimonLightPlate("VerdeClaro"); 
		lightPlates[(int)SimonLightPlate.eType.RED] = new SimonLightPlate("RojoClaro"); 
		lightPlates[(int)SimonLightPlate.eType.YELLOW] = new SimonLightPlate("AmarilloClaro"); 

		ResetGame ();

		InvokeRepeating ("DisplaySequence", 1, displaySequenceRepeatInterval);
	}

	void DisplaySequence()
	{
		if (currentState == eState.DISPLAY_SEQUENCE) 
		{
			if (sequeceCount == sequence.Count)
			{
				currentState = eState.WAITING_FOR_USER;
				txtResultado.text = "YOUR TURN";
				txtResultado.GetComponent<Renderer> ().enabled = true;
			}
			else
			{
				txtResultado.text = "WAIT";
				txtResultado.GetComponent<Renderer> ().enabled = true;

				//txtResultado.GetComponent<Renderer> ().enabled = false;
				lightPlates[sequence[sequeceCount]].plate.GetComponent<Renderer> ().enabled = true;
				//play audio
				lightPlates[sequence[sequeceCount]].plate.GetComponent<AudioSource>().Play();
				++sequeceCount;
				currentState = eState.MOSTRANDO;
			}
		}
		else if (currentState == eState.MOSTRANDO)
		{
			lightPlates[sequence[sequeceCount-1]].plate.GetComponent<Renderer> ().enabled = false;
			currentState = eState.DISPLAY_SEQUENCE;
		}
	}
	

	// Update is called once per frame
	void Update () 
	{
		if (currentState == eState.THINKING) 
		{

			//agregar nueva secuencia
			AddNewSequence();

		}
	}

	void OnLeftClickDown(SimonLightPlate.eType color)
	{
		if (currentState == eState.WAITING_FOR_USER) 
		{
			lightPlates [(int)color].plate.GetComponent<Renderer> ().enabled = true;
			clickedSequence.Add ((int)color);
		}
	}

	void OnLeftClickUp(SimonLightPlate.eType color)
	{
		if (currentState == eState.WAITING_FOR_USER) 
		{
			lightPlates [(int)color].plate.GetComponent<Renderer> ().enabled = false;
			if (!VerifySequence())
			{

				txtResultado.text = "GAME OVER";
				txtResultado.GetComponent<Renderer> ().enabled = true;
				AudioSource.PlayClipAtPoint(SonidoError, transform.position);

				Time.timeScale = 0;

				if (int.Parse (score.text) > oldHighscore)
				{
					StartCoroutine(subirInternet());
				}
				//ResetGame();
			}
			else
			{
				lightPlates[(int)color].plate.GetComponent<AudioSource>().Play();
				////////////////////////////////////////////////////////////////////////////////
				if (clickedSequence.Count == sequence.Count)
				{
					currentState = eState.THINKING;
					clickedSequence.Clear();

					score.text = (int.Parse(score.text) + 1).ToString();

					if (displaySequenceRepeatInterval>0.22f)
					{
					   displaySequenceRepeatInterval = displaySequenceRepeatInterval - 0.04f;
					}

					//Debug.Log("score.text: "+score.text);
					//Debug.Log("oldHighscore: "+oldHighscore);
					//Debug.Log("tiempo: "+tiempo.text);
					//Debug.Log("oldTime: "+oldTime);

					if (oldTime==0)
					{
						oldTime = 1000;
					}

					if (int.Parse (score.text) >= oldHighscore)
						//&& int.Parse(tiempo.text)<oldTime) 
					{
						PlayerPrefs.SetInt ("highscore", int.Parse (score.text));
						PlayerPrefs.SetInt ("Time", int.Parse (tiempo.text));
						txtTiempo.text = tiempo.text;
						txthightScore.text = score.text;
					}

					txtResultado.text = "GOOD";
					txtResultado.GetComponent<Renderer> ().enabled = true;

					CancelInvoke("DisplaySequence"); 
					InvokeRepeating ("DisplaySequence", 1, displaySequenceRepeatInterval);
				}
			}
		}
	}


	bool VerifySequence()
	{
		for (int i = 0; i < clickedSequence.Count; i++) 
		{

			if (clickedSequence[i] != sequence[i])
			{
				return false;
			}
		}
		return true;
	}


	void AddNewSequence()
	{
		sequence.Add (Random.Range (0, 4));
		currentState = eState.DISPLAY_SEQUENCE;
		sequeceCount = 0;
	}

	void ResetGame()
	{
		tiempo.text = "0";
		Time.timeScale = 1;
		displaySequenceRepeatInterval = 0.5f;
		score.text = "0";
		currentState = eState.THINKING;
		sequence.Clear ();
		clickedSequence.Clear ();
		sequeceCount = 0;

		for (int i = 0; i<(int)SimonLightPlate.eType.NUM_TYPES; ++i) 
		{
			lightPlates[i].plate.GetComponent<Renderer>().enabled = false;
		}
	}

	IEnumerator  subirInternet()
	{
		string url = "http://kfouri.onlinewebshop.net/nuevo_record_google_say.php";
		
		WWWForm form = new WWWForm();
		form.AddField( "p_imei", SystemInfo.deviceUniqueIdentifier);
		form.AddField( "p_puntos", score.text);
		form.AddField( "p_tiempo", txtTiempo.text);
		
		WWW download = new WWW( url, form );
		yield return download;
		if((!string.IsNullOrEmpty(download.error)))
		{
			print( "Error downloading: " + download.error );
		} 
		else 
		{
			formText = download.text;
			
			Debug.Log("Resultado WWW: "+formText);
			
		}
	}

}
