using UnityEngine;
using System.Collections;
// Librerias de Google Ads
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour {
	
	public static AdsManager manager;    // Patron Singleton
	
	void Awake ()
	{
		if(manager == null)
		{
			// Persistente entre escenas
			DontDestroyOnLoad(gameObject);
			manager = this;
		}
		else if (manager != this)
		{
			// Solo puede haber 1
			Destroy (gameObject);
		}    
	}
	
	void Start()
	{
		// Creacion del banner
		BannerView bannerView = new BannerView ("ca-app-pub-8178489486911148/6164637510", AdSize.Banner, AdPosition.Bottom);
		// Creacion de la peticion de anuncio
		AdRequest request = new AdRequest.Builder ().Build ();
		// Carga del anuncio
		bannerView.LoadAd (request);

	}
}
