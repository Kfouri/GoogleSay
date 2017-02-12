using UnityEngine;
using System.Collections;

public class RankingScript : MonoBehaviour {

	private int rank;
	private static string server = "http://kfouri.onlinewebshop.net/";
	private string TopScoresURL = server+"TopScoresGoogleSay.php";
	private string RankURL = server+"GetRankGoogleSay.php";
	private float waitTime = 2.5f;
	private float elapsedTime = 0.0f;

	//The score and username we submit
	private int highscore;
	private string username;

	//We use this to allow the user to start the game again.
	private bool pressspace;

	public GameObject BaseGUIText;

	GameObject MensajeEsperar;

	WWW GetScoresAttempt;
	WWW RankGrabAttempt;

	// Use this for initialization
	void Start () {
	
		//Choose our text positions

		Vector2 CentreTextPosition = new Vector2(0.33f, 0.33f);
		
		MensajeEsperar = Instantiate(BaseGUIText, CentreTextPosition, Quaternion.identity) as GameObject;
		MensajeEsperar.GetComponent<GUIText>().enabled = true;
		MensajeEsperar.GetComponent<GUIText>().text = "Please wait...";
		MensajeEsperar.GetComponent<GUIText>().fontSize = 35;


		StartCoroutine(GrabRank(SystemInfo.deviceUniqueIdentifier));


	}

	void OnEnable()
	{
		pressspace = false; // The user can't press space yet.
	}

	void Update()
	{
		elapsedTime += Time.deltaTime;

		if (pressspace && Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel("menuPrincipal");
		}
		else
		{
			if(elapsedTime >= waitTime)
			{
				Destroy(MensajeEsperar);

				if (!GetScoresAttempt.isDone)
				{
					StopCoroutine("GetTopScores");
					GetScoresAttempt.Dispose();
					Error();
				}

				if (!RankGrabAttempt.isDone)
				{
					StopCoroutine("GrabRank");
					RankGrabAttempt.Dispose();
					Error();
				}


			}
		}
	}

	///Our public access functions
	public void Setscore(int givenscore)
	{
		highscore = givenscore;
	}
	
	public void SetName(string givenname){
		username = givenname;
	}


	IEnumerator GrabRank(string name)
	{
		elapsedTime = 0.0f;
		RankGrabAttempt = new WWW(RankURL+"?p_imei=" + WWW.EscapeURL(name));
		string resultado = "";
		char[] delimiterChars = {';'};

		yield return RankGrabAttempt;
		
		if (RankGrabAttempt.error == null)
		{
			resultado = RankGrabAttempt.text;

			string[] words = resultado.Split(delimiterChars);
			username = words[0];
			rank = int.Parse (words[1]);

			StartCoroutine(GetTopScores());
			
		}
		else
		{
			Error();
		}
	}


	IEnumerator GetTopScores()
	{
		elapsedTime = 0.0f;
		GetScoresAttempt = new WWW(TopScoresURL);
		yield return GetScoresAttempt;
		
		if (GetScoresAttempt.error != null)
		{
			Error();
		}
		else
		{
			Destroy (MensajeEsperar);
			//Collect up all our data

			string[] textlist = GetScoresAttempt.text.Split(new string[] { "\n", "\t" }, System.StringSplitOptions.RemoveEmptyEntries);

			//Split it into two smaller arrays
			string[] Names = new string[Mathf.FloorToInt(textlist.Length/3)];
			string[] Scores = new string[Names.Length];
			string[] Tiempos = new string[Names.Length];

			for (int i = 0; i < textlist.Length; i++)
			{

				if (i % 3 == 0)
				{     
					Names[Mathf.FloorToInt(i / 3)] = textlist[i];
				}
				else if (i % 3 == 1)
				{
					Scores[Mathf.FloorToInt(i / 3)] = textlist[i];
				}
				else
				{
					Tiempos[Mathf.FloorToInt(i / 3)] = textlist[i];
				}
			}
			
			//Choose our text positions
			Vector2 RankPosition = new Vector2(0.15f,0.85f);
			Vector2 NamePosition = new Vector2(0.25f, 0.85f);
			Vector2 ScorePosition = new Vector2(0.60f, 0.85f);
			Vector2 TimeTPosition = new Vector2(0.85f, 0.85f);

			///All our headers
			GameObject Scoresheader = Instantiate(BaseGUIText, new Vector2(0.5f,0.94f), Quaternion.identity) as GameObject;
			Scoresheader.tag = "Score";
			Scoresheader.GetComponent<GUIText>().text = "High Scores";
			Scoresheader.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
			Scoresheader.GetComponent<GUIText>().fontSize = 40;
			
			GameObject PressSpace = Instantiate(BaseGUIText, new Vector2(0.5f, 0.35f), Quaternion.identity) as GameObject;
			PressSpace.tag = "Score";
			PressSpace.GetComponent<GUIText>().text = "Click to close windows!";
			PressSpace.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
			PressSpace.GetComponent<GUIText>().fontSize = 35;

			//---------------------------------------------------------------
			GameObject Rankheader = Instantiate(BaseGUIText, RankPosition, Quaternion.identity) as GameObject;
			Rankheader.tag = "Score";
			Rankheader.GetComponent<GUIText>().text = "Rank";
			Rankheader.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
			Rankheader.GetComponent<GUIText>().fontSize = 35;

			GameObject Nameheader = Instantiate(BaseGUIText, NamePosition, Quaternion.identity) as GameObject;
			Nameheader.tag = "Score";
			Nameheader.GetComponent<GUIText>().text = "Name";
			Nameheader.GetComponent<GUIText>().fontSize = 35;

			GameObject Scoreheader = Instantiate(BaseGUIText, ScorePosition, Quaternion.identity) as GameObject;
			Scoreheader.tag = "Score";
			Scoreheader.GetComponent<GUIText>().text = "Score";
			Scoreheader.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
			Scoreheader.GetComponent<GUIText>().fontSize = 35;

			GameObject Timeheader = Instantiate(BaseGUIText, TimeTPosition, Quaternion.identity) as GameObject;
			Timeheader.tag = "Score";
			Timeheader.GetComponent<GUIText>().text = "Time";
			Timeheader.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
			Timeheader.GetComponent<GUIText>().fontSize = 35;


			//Increment the positions
			RankPosition -= new Vector2(0,0.04f);
			ScorePosition -= new Vector2(0,0.04f);
			NamePosition -= new Vector2(0,0.04f);
			TimeTPosition -= new Vector2(0,0.04f);


			///Our top 10 scores
			for(int i=0;i<Names.Length;i++)
			{

				GameObject Rank = Instantiate(BaseGUIText, RankPosition, Quaternion.identity) as GameObject;
				Rank.tag = "Score";
				Rank.GetComponent<GUIText>().text = "" + (i + 1);
				Rank.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;

				GameObject Score = Instantiate(BaseGUIText, ScorePosition, Quaternion.identity) as GameObject;
				Score.tag = "Score";
				Score.GetComponent<GUIText>().text = Scores[i];
				Score.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;
				
				GameObject Name = Instantiate(BaseGUIText, NamePosition, Quaternion.identity) as GameObject;
				Name.tag = "Score";
				Name.GetComponent<GUIText>().text = Names[i];
				
				GameObject Tiempo = Instantiate(BaseGUIText, TimeTPosition, Quaternion.identity) as GameObject;
				Tiempo.tag = "Score";
				Tiempo.GetComponent<GUIText>().text = Tiempos[i];

				if (i + 1 == rank) //If the player is within the top 10 colour their score yellow.
				{
					Score.GetComponent<GUIText>().material.color = Color.yellow;
					Name.GetComponent<GUIText>().material.color = Color.yellow;
					Rank.GetComponent<GUIText>().material.color = Color.yellow;
					Tiempo.GetComponent<GUIText>().material.color = Color.yellow;
				}
				
				//Increment the positions again
				RankPosition -= new Vector2(0,0.04f);
				ScorePosition -= new Vector2(0,0.04f);
				NamePosition -= new Vector2(0,0.04f);
				TimeTPosition -= new Vector2(0,0.04f);
			}

			//If our player isn't in the top 10, add them to the bottom.
			if (rank > 10)
			{

				GameObject Rank = Instantiate(BaseGUIText, RankPosition, Quaternion.identity) as GameObject;
				Rank.tag = "Score";
				Rank.GetComponent<GUIText>().text = "" + (rank);
				Rank.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;

				GameObject Name = Instantiate(BaseGUIText, NamePosition, Quaternion.identity) as GameObject;
				Name.tag = "Score";
				Name.GetComponent<GUIText>().text = username;

				GameObject Score = Instantiate(BaseGUIText, ScorePosition, Quaternion.identity) as GameObject;
				Score.tag = "Score";
				Score.GetComponent<GUIText>().text = ""+PlayerPrefs.GetInt("highscore", 0);
				Score.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;

				GameObject Tiempo = Instantiate(BaseGUIText, TimeTPosition, Quaternion.identity) as GameObject;
				Tiempo.tag = "Score";
				Tiempo.GetComponent<GUIText>().text = ""+PlayerPrefs.GetInt ("Time", 0);
				Tiempo.GetComponent<GUIText>().anchor = TextAnchor.MiddleCenter;

				Score.GetComponent<GUIText>().material.color = Color.yellow;
				Name.GetComponent<GUIText>().material.color = Color.yellow;
				Rank.GetComponent<GUIText>().material.color = Color.yellow;
				Tiempo.GetComponent<GUIText>().material.color = Color.yellow;
			}

			//Allows the user to restart the game
			pressspace = true;
		}
	}

	void Error()
	{
		Destroy (MensajeEsperar);
		//Choose our text positions
		Vector2 CentreTextPosition = new Vector2(0.33f, 0.33f);

		GameObject Scoreheader = Instantiate(BaseGUIText, CentreTextPosition, Quaternion.identity) as GameObject;
		Scoreheader.GetComponent<GUIText>().enabled = true;
		Scoreheader.GetComponent<GUIText>().text = "Connection error. Try later";
		Scoreheader.GetComponent<GUIText>().fontSize = 35;
		pressspace = true;
	}
}
