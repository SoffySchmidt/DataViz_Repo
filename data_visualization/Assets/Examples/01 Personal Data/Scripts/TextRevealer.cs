/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using UnityEngine;

public class TextRevealer : MonoBehaviour
{
	public GameObject textObject = null;

	void Start()
	{
		textObject.SetActive( false );
	}

	void OnMouseEnter()
	{
		textObject.SetActive( true );
	}

	void OnMouseExit()
	{
		textObject.SetActive( false );
	}
}