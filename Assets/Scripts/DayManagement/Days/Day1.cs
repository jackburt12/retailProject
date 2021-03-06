﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Day1 : MonoBehaviour {

	private GameObject floor;
	private GameObject wall;
	private GameObject desk;

	private bool continuePressed = false;
	private List<Sprite> floorList = new List<Sprite>();
	private List<Sprite> wallList = new List<Sprite>();
	private List<Sprite> deskList = new List<Sprite>();
	private int currentPosition = 0;
	private GameObject welcomePanel;
	private bool welcomeClicked = false;
	private GameObject floorButtons;
	private GameObject wallButtons;
	private GameObject deskButtons;
	private bool floorClicked;
	private bool wallClicked;
	private bool deskClicked;

	private float typeSpeed = 0.01f;

	public AudioClip letterSound; 
	public AudioClip chaching;
	AudioSource audio;

	public void StartDay () {

		PlayerPrefs.SetInt ("Money", 10000);

		floor = GameObject.Find ("PlayArea");

		floorList.Add (Resources.Load<Sprite>("Floors/TileFloor"));
		floorList.Add (Resources.Load<Sprite> ("Floors/MarbleFloor"));
		floorList.Add (Resources.Load<Sprite> ("Floors/WoodenFloor"));
		floorList.Add (Resources.Load<Sprite> ("Floors/WhiteTileFloor"));
		floorList.Add (Resources.Load<Sprite> ("Floors/DarkTileFloor"));

		wall = GameObject.Find ("BackWall");

		wallList.Add(Resources.Load<Sprite>("Walls/YellowPlain"));
		wallList.Add (Resources.Load<Sprite> ("Walls/WhitePlain"));
		wallList.Add (Resources.Load<Sprite> ("Walls/BrickWall"));

		desk = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/Items/Desk"));
		desk.SetActive (false);

		deskList.Add(Resources.Load<Sprite>("Desks/Desk1"));
		deskList.Add(Resources.Load<Sprite>("Desks/Desk2"));
		deskList.Add(Resources.Load<Sprite>("Desks/Desk3"));
		deskList.Add(Resources.Load<Sprite>("Desks/Desk4"));

		StartCoroutine ("Sequence");


	}
	
	public void EndDay () {

		GameObject dayChanger = GameObject.Find ("DayChanger");
		dayChanger.GetComponent<DayChanger> ().StartCoroutine ("FadeIn");
		dayChanger.GetComponent<DayChanger> ().dayStarted = false;

	}

	IEnumerator Sequence() {
		yield return StartCoroutine ("DestroyPlayer");
		yield return new WaitForSeconds (4);
		yield return StartCoroutine ("WelcomePanel");
		yield return StartCoroutine ("ChooseFloor");
		yield return StartCoroutine ("ChooseWall");
		yield return StartCoroutine ("ChooseDesk");
		yield return StartCoroutine ("OpenInventory");
	}

	public void FloorChangerRight() {

		if (currentPosition == floorList.Count - 1) {
			currentPosition = 0;
		} else {
			currentPosition++;
		}

		floor.GetComponent<SpriteRenderer> ().sprite = floorList [currentPosition];

	}

	public void FloorChangerLeft() {

		if (currentPosition == 0) {
			currentPosition = (floorList.Count - 1);
		} else {
			currentPosition--;
		}
		floor.GetComponent<SpriteRenderer> ().sprite = floorList [currentPosition];

	}

	public void WallChangerRight() {

		if (currentPosition == wallList.Count - 1) {
			currentPosition = 0;
		} else {
			currentPosition++;
		}

		wall.GetComponent<SpriteRenderer> ().sprite = wallList [currentPosition];

	}

	public void WallChangerLeft() {

		if (currentPosition == 0) {
			currentPosition = (wallList.Count - 1);
		} else {
			currentPosition--;
		}
		wall.GetComponent<SpriteRenderer> ().sprite = wallList [currentPosition];

	}

	public void DeskChangerRight() {

		if (currentPosition == deskList.Count - 1) {
			currentPosition = 0;
		} else {
			currentPosition++;
		}

		desk.GetComponent<SpriteRenderer> ().sprite = deskList [currentPosition];

	}

	public void DeskChangerLeft() {

		if (currentPosition == 0) {
			currentPosition = (deskList.Count - 1);
		} else {
			currentPosition--;
		}
		desk.GetComponent<SpriteRenderer> ().sprite = deskList [currentPosition];

	}

	IEnumerator DestroyPlayer() {

		yield return new WaitForSeconds (4);

		if (GameObject.Find ("Player(Clone)") != null) {
			GameObject.Destroy(GameObject.Find("Player(Clone)"));
		}

		yield return null;

	}

	IEnumerator WelcomePanel() {

		yield return new WaitForSeconds (8);

		GameObject.Find ("GameTime").GetComponent<GameTime> ().pauseTime = true;
		welcomePanel = (GameObject)Instantiate(Resources.Load("Prefabs/UI/WelcomePanel"), GameObject.Find("Canvas").transform);

		for (float f = 0f; f <= 1f; f += 0.05f) {
			Color c = new Color (1, 1, 1, 0);
			c.a = f;
			welcomePanel.GetComponent<Image>().color = c;
			yield return null;

		}

		audio = GameObject.Find("DayChanger").GetComponent<AudioSource> ();


		welcomePanel.GetComponentInChildren<Text>().text = (Resources.Load("Dialogue/WelcomeDialogue") as TextAsset).text;

		welcomePanel.GetComponentInChildren<Button> ().onClick.AddListener (CloseWelcomePanel);

		while (!welcomeClicked) {



			yield return null;

		}

		yield return null;

	}

	public void CloseWelcomePanel() {
		GameObject moneyPanel = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/UI/MoneyPanel"), GameObject.Find("Canvas").transform);
		GameObject.Destroy (welcomePanel);
		welcomeClicked = true;

	}

	IEnumerator ChooseFloor() {

		floorButtons = (GameObject)Instantiate(Resources.Load("Prefabs/UI/FloorChangeButtons"), GameObject.Find("Canvas").transform);

		floorButtons.transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener (CloseFloor);
		floorButtons.transform.Find("ChangerButtonLeft").GetComponent<Button>().onClick.AddListener (FloorChangerLeft);
		floorButtons.transform.Find ("ChangerButtonRight").GetComponent<Button> ().onClick.AddListener (FloorChangerRight);

		while (!floorClicked) {
			yield return null;
		}
		currentPosition = 0;
		yield return null;
	}

	public void CloseFloor() {
		audio.PlayOneShot (chaching);
		GameObject.Destroy (floorButtons);
		floorClicked = true;

		PlayerPrefs.SetInt ("Money", PlayerPrefs.GetInt ("Money") - 1500);

	}

	public void CloseWall() {
		audio.PlayOneShot (chaching);
		GameObject.Destroy (wallButtons);
		wallClicked = true;
		PlayerPrefs.SetInt ("Money", PlayerPrefs.GetInt ("Money") - 750);


	}

	public void CloseDesk() {
		audio.PlayOneShot (chaching);
		GameObject.Destroy (deskButtons);
		deskClicked = true;
		PlayerPrefs.SetInt ("Money", PlayerPrefs.GetInt ("Money") - 200);


	}

	IEnumerator ChooseWall() {
		GameObject camera = GameObject.Find ("Main Camera");
		float lerpTime = 1;
		float currentLerpTime = 0;

		Vector3 camPos = camera.transform.position;

		while (camera.transform.position.y < 5) {

			/*
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime >= lerpTime) {
				currentLerpTime = lerpTime;
			}
			float perc = currentLerpTime / lerpTime;
			GameObject.Find ("Main Camera").transform.position = Vector3.Lerp(transform.position, new Vector3(0,5f,-2.5f), perc);
			yield return null;
			*/
			camera.transform.position = new Vector3(0,camera.transform.position.y + Time.deltaTime * 10f, camera.transform.position.z);
			yield return null;
		}
			
		wallButtons = (GameObject)Instantiate(Resources.Load("Prefabs/UI/FloorChangeButtons"), GameObject.Find("Canvas").transform);

		wallButtons.transform.Find ("ConfirmButton").GetComponentInChildren<Text> ().text = "£750";
		wallButtons.transform.Find ("ChooseFloorText").GetComponent<Text> ().text = "Choose Wall";

		wallButtons.transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener (CloseWall);
		wallButtons.transform.Find("ChangerButtonLeft").GetComponent<Button>().onClick.AddListener (WallChangerLeft);
		wallButtons.transform.Find ("ChangerButtonRight").GetComponent<Button> ().onClick.AddListener (WallChangerRight);

		while (!wallClicked) {

			yield return null;
		}
		currentLerpTime = 0;
		while (camera.transform.position.y > 0) {
			
			camera.transform.position = new Vector3 (0, camera.transform.position.y - Time.deltaTime * 10f, camera.transform.position.z);
			yield return null;
 		}

	}


	IEnumerator ChooseDesk() {
		desk.SetActive (true);
		deskButtons = (GameObject)Instantiate(Resources.Load("Prefabs/UI/FloorChangeButtons"), GameObject.Find("Canvas").transform);

		deskButtons.transform.Find ("ConfirmButton").GetComponentInChildren<Text> ().text = "£200";
		deskButtons.transform.Find ("ChooseFloorText").GetComponent<Text> ().text = "Choose Desk";

		deskButtons.transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener (CloseDesk);
		deskButtons.transform.Find("ChangerButtonLeft").GetComponent<Button>().onClick.AddListener (DeskChangerLeft);
		deskButtons.transform.Find ("ChangerButtonRight").GetComponent<Button> ().onClick.AddListener (DeskChangerRight);

		while (!deskClicked) {

			yield return null;
		}

		yield return null;

	}

	IEnumerator OpenInventory() {

		GameObject inventory = (GameObject)Instantiate(Resources.Load("Prefabs/UI/InventoryMoney"), GameObject.Find("Canvas").transform);

		yield return null;
	}
}
