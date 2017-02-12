using UnityEngine;
using System.Collections;

public class ColorPlateClickHandler : MonoBehaviour 
{
	SimonLightPlate.eType GetColorPlate(string clickPlateStr)
	{
		
		//if (this.name == "BlueClickPlane")
		if (this.tag == "Azul")
		{
			return SimonLightPlate.eType.BLUE;
		}
		
		//if (this.name == "GreenClickPlane")
		if (this.tag == "Verde")
		{
			return SimonLightPlate.eType.GREEN;
		}
		
		//if (this.name == "RedClickPlane")
		if (this.tag == "Rojo")
		{
			return SimonLightPlate.eType.RED;
		}
		
		//if (this.name == "YellowClickPlane")
		if (this.tag == "Amarillo")
		{
			return SimonLightPlate.eType.YELLOW;
		}

		return SimonLightPlate.eType.INVALID_TYPE;
	}
	
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			//GetComponent<AudioSource>().Play();
			this.SendMessageUpwards("OnLeftClickDown",GetColorPlate(this.name),SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnMouseUp()
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			this.SendMessageUpwards("OnLeftClickUp",GetColorPlate(this.name),SendMessageOptions.DontRequireReceiver);
		}


	}
}
