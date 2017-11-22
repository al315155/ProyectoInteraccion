using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class MenuManagment : MonoBehaviour {

	private string nextSceneName;
	private GameManager gameManager;
	public GameObject CanvasMenu;
	public GameObject CanvasMode;
	public GameObject CanvasTeam;

	public GameObject ModePanel;
	private List<Button> modeButtons;
	private string currentButton;
	private string lastButton;

	private GameMode gameMode;
	public Sprite IconHealer;
	public Sprite IconTank;
	public Sprite IconDistDamage;
	public Sprite IconMeleDamage;
	public GameObject TeamMembersPanel;
	private List<Button> buttonMembersList;
	private int cont;
	public Text GameModeLabel;
	public Text UnitDetailsLabel;
	private Text numberOfMembers;
	public Button StartButton;
	private List<string> teamList;

	void Start(){
		//La siguiente escena contendrá la partida.
		nextSceneName = "Game";
		//Necesitamos el game manager para pasarle la información del equipo configurado.
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		//El primer paso será el menú. Por eso el canvas de selección de modo de juego
		// y el canvas de selección de miembros de equipo están inicialmente desactivados.
		CanvasMode.gameObject.SetActive (false);
		CanvasTeam.gameObject.SetActive (false);

		//Se añaden los botones correspondientes a los modos de juego en una lista.
		modeButtons = new List<Button> ();
		foreach (Button button in ModePanel.GetComponentsInChildren<Button>()){
			modeButtons.Add (button);
		}

		//Y se actualiza el botón actual como "botón de control", mientras que el último
		//botón clickado se inicializa a cadena vacía, ya que no se ha clickado nada todavía.
		currentButton = modeButtons[modeButtons.Count - 1].name;
		lastButton = "";
	}

	// MÉTODOS DEL MENÚ INICIAL //
	public void PickGameMode(){
		//Se pasa de menú inicial al menú de selección de modo de juego.
		CanvasMenu.gameObject.SetActive(false);
		CanvasMode.gameObject.SetActive (true);
	}

	public void QuitGame(){
		//Cerrar el juego.
		Application.Quit ();
	}


	// MÉTODOS DEL MENÚ DE SELECCIÓN DE MODO DE JUEGO
	public void ShowGameModeDetails(Button b){
		//Muestra información sobre el modo de juego seleccionado.
		//Si seleccionamos un modo distinto al actual.
		if (lastButton != currentButton) {
			//Actualizamos la pantalla y los valores de los botones
			//anteriormente seleccionados y recién seleccionados.
			lastButton = currentButton;
			currentButton = b.name;
			ActualizeText (b.GetComponentInChildren<Text>());
		}
	}

	private void ActualizeText(Text buttonText){
		//Cambiamos los textos. Cuando un modo esté seleccionado, el texto de 
		//su botón se actualiza a la explicación del modo de juego. Los otros dos
		//botones/modos vuelven a su estado inicial mostrando solamente el título
		//del modo de juego.
		buttonText.fontSize = 18;

		switch (currentButton) {
		case "Team VS Team Button":
			buttonText.text = gameManager.GetTeamVsTeamMode ().Explanation;

			if (lastButton.Equals ("One Kill Button")) {
				modeButtons [1].GetComponentInChildren<Text> ().text = gameManager.GetOneKillMode ().Title;
				modeButtons [1].GetComponentInChildren<Text> ().fontSize = 30;
			} else {
				modeButtons [2].GetComponentInChildren<Text> ().text = gameManager.GetBossFightMode ().Title;
				modeButtons [2].GetComponentInChildren<Text> ().fontSize = 30;
			}
			break;

		case "One Kill Button":
			buttonText.text = gameManager.GetOneKillMode ().Explanation;

			if (lastButton.Equals ("Team VS Team Button")) {
				modeButtons [0].GetComponentInChildren<Text> ().text = gameManager.GetTeamVsTeamMode ().Title;
				modeButtons [0].GetComponentInChildren<Text> ().fontSize = 30;
			} else {
				modeButtons [2].GetComponentInChildren<Text> ().text = gameManager.GetBossFightMode ().Title;
				modeButtons [2].GetComponentInChildren<Text> ().fontSize = 30;
			}
			break;

		case "Boss Fight Button":
			buttonText.text = gameManager.GetTeamVsTeamMode ().Explanation;

			if (lastButton.Equals ("Team VS Team Button")) {
				modeButtons [0].GetComponentInChildren<Text> ().text = gameManager.GetTeamVsTeamMode ().Title;
				modeButtons [0].GetComponentInChildren<Text> ().fontSize = 30;
			} else {
				modeButtons [1].GetComponentInChildren<Text> ().text = gameManager.GetOneKillMode ().Title;
				modeButtons [1].GetComponentInChildren<Text> ().fontSize = 30;
			}
			break;
		}
	}

	public void ConfigureTeam(){
		//Se pasa del menú de selección de modo de juego al menú de selección de equipo.
		CanvasMode.gameObject.SetActive (false);
		CanvasTeam.gameObject.SetActive (true);

		//Equiparando el último botón pulsado antes de pulsar a este botón tipo START
		//obtenemos el modo de juego determinado.
		if (currentButton.Equals(ModePanel.transform.GetChild(0).name)) {
			gameMode = gameManager.GetTeamVsTeamMode ();
		} 
		else if (currentButton.Equals(ModePanel.transform.GetChild(1).name)) {
			gameMode = gameManager.GetOneKillMode ();
		} 
		else {
			gameMode = gameManager.GetBossFightMode ();
		}

		//Una vez tenemos el modo de juego que se había seleccionado, se concretan los
		//objetos de la escena.
		cont = 0;
		GameModeLabel.text = gameMode.Title;

		//Se crea una lista para almacenar los valores de los miembros elegidos, inicialmente
		//vacía.
		teamList = new List<string> ();

		//Se crean las casillas necesarias (número de miembros del equipo), dependiendo
		//del modo de juego escogido. Cuando se añada una unidad se reflejará en esta lista
		//de casililas de un modo visual.
		buttonMembersList = new List<Button> ();
		GameObject b = TeamMembersPanel.transform.GetChild (0).gameObject;
		for (int i = 0; i < gameMode.Members - 1; i++) {
			GameObject newButton = Instantiate (b, TeamMembersPanel.transform);
			newButton.transform.localScale = Vector3.one;
		}
	}


	// MÉTODOS DEL MENÚ DE SELECCIÓN DE EQUIPO
	public void StartGame(){
		//Iniciamos una partida.
		gameManager.SetTeamConfiguration (teamList);
		SceneManager.LoadScene (nextSceneName);
	}

	public void RemoveUnit(Button b){
		//Eliminamos una unidad de la lista de miembros del equipo.
		if (b.GetComponentInChildren<Image> ().sprite != null) {
			//b.GetComponentInChildren<Image> ().sprite = null;

			int index = b.transform.GetSiblingIndex ();

			if (index < TeamMembersPanel.transform.childCount - 1) {
				for (int i = index; i < TeamMembersPanel.transform.childCount - 1; i++) {
					TeamMembersPanel.transform.GetChild(i).GetComponentInChildren<Image> ().sprite = 
						TeamMembersPanel.transform.GetChild (i + 1).GetComponentInChildren<Image> ().sprite;
					
					TeamMembersPanel.transform.GetChild (i + 1).GetComponentInChildren<Image> ().sprite = null;
				}
			}
			else {
				b.GetComponentInChildren<Image> ().sprite = null;
			}

			teamList.Remove(teamList[b.transform.GetSiblingIndex()]);
			cont--;
		}
	}

	public void AddUnit(Button b){
		// Añadimos una unidad a la lista de miembros del equipo.
		if (teamList.Count < gameMode.Members) {
			switch (b.transform.parent.name) {
			case "Healer":
				teamList.Add("Healer");
				TeamMembersPanel.transform.GetChild (cont).GetComponentInChildren<Image> ().sprite = IconHealer;
				break;

			case "Tank":
				teamList.Add("Tank");
				TeamMembersPanel.transform.GetChild (cont).GetComponentInChildren<Image> ().sprite = IconTank;
				break;

			case "Distance Damage":
				teamList.Add("Distance Damage");
				TeamMembersPanel.transform.GetChild (cont).GetComponentInChildren<Image> ().sprite = IconDistDamage;
				break;

			case "Mele Damage":
				teamList.Add("Mele Damage");
				TeamMembersPanel.transform.GetChild (cont).GetComponentInChildren<Image> ().sprite = IconMeleDamage;
				break;
			}

			cont++;
			//Sí ya tenemos los miembros necesarios activamos el botón de empezar partida.
			if (teamList.Count == gameMode.Members) {
				StartButton.gameObject.SetActive (true);
			}
		} 
	}

	public void ShowUnitDetails(Button b){
		// Mostramos información sobre las unidades.
		switch (b.transform.parent.name) {
		case "Healer":
			UnitDetailsLabel.text = Unit.HealerDetails;
			break;

		case "Tank":
			UnitDetailsLabel.text = Unit.TankDetails;
			break;

		case "Distance Damage":
			UnitDetailsLabel.text = Unit.DistDamageDetails;
			break;

		case "Mele Damage":
			UnitDetailsLabel.text = Unit.MeleDamageDetails;
			break;
		}
	}
}
