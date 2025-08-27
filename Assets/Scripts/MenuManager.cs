using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    public Button highScoresPanelButton, startGameButton, closeHSPanel;
    public GameObject gameMenuPanel, highScoresPanel, chooseDificultPanel, howToPlayPanel;
    public GameObject firstPlacePrefab, otherPlacesPrefab; //Prefabs de posiciones
    public Transform highScoreContent;

    private static string playerName;
    private static int playerId;
    private string currentDificultad = "Facil";

    public TextMeshProUGUI playerNameDisplay, dificultadActual, messageDisplay;

    public AudioManager audioManager;

    public List<Sprite> fondos;
    public SpriteRenderer fondoMenu;

    public GameObject nextButton, returnButton;
    public List<Sprite> indicaciones;
    public Image indicacion;

    private static int id_juegos;
    public string idUser;

    private bool isUpdatingHighScores = false; // Variable de control
    public Button closeButton,facilButton,normalButton,dificilButton;
    public static string user_login;

    void Start()
    {
        Time.timeScale = 1f;
        audioManager.ApplyAudioSettings();
        gameMenuPanel.SetActive(false);
        highScoresPanel.SetActive(false);
        chooseDificultPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        id_juegos = 3;
        messageDisplay.text = "";
        // Obtener id_user del URL
        GetIdUserFromUrl();
        
    
        

    }

    void Awake()
    {
        Application.targetFrameRate = 60;

    }

    void Update()
    {
        if (Application.targetFrameRate != 60)
        {
            Application.targetFrameRate = 60;
        }
        switch(currentDificultad)
        {
            case "Facil":
                facilButton.interactable = false;
                normalButton.interactable = true;
                dificilButton.interactable = true;
                break;
            case "Normal":
                facilButton.interactable = true;
                normalButton.interactable = false;
                dificilButton.interactable = true;
                break;
            case "Dificil":
                facilButton.interactable = true;
                normalButton.interactable = true;
                dificilButton.interactable = false;
                break;
        }
    }

    public void GetIdUserFromUrl()
    {
        string url = Application.absoluteURL;
        if (string.IsNullOrEmpty(url))
        {
            idUser = null;
            messageDisplay.text = "Error: No se pudo obtener el ID de usuario.";
        }
        else
        {
            var uri = new System.Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            idUser = query.Get("id_user");
            if (idUser == null)
            {
                messageDisplay.text = "Error: No se pudo obtener el ID de usuario.";
            }
            else
            {
                messageDisplay.text = "Cargando...";
                ObtenerNickname();
            }
        }



    }

    public async void ObtenerNickname()
    {
        messageDisplay.text = "Cargando...";
        await InitializeUser(idUser);
    }

    public async Task InitializeUser(string idUser)
    {
        messageDisplay.text = "Cargando...";
        var user = await HighScoreManager.GetUserById(idUser);

        if (user != null)
        {
            playerName = user.user_nickname;
            user_login = user.user_login;
            playerId = int.Parse(idUser);
            messageDisplay.text = "";
            ShowGameMenu();
        }
        else
        {
            if (messageDisplay.text == "Cargando...")
            {
                messageDisplay.text = "Error: No se pudo obtener el nombre de usuario.";
            }
            else
            {
                messageDisplay.text = messageDisplay.text;
            }
        }
    }

    void ShowGameMenu()
    {
        Debug.Log("Showing game menu.");
        fondoMenu.sprite = fondos[1];
        gameMenuPanel.SetActive(true);
        playerNameDisplay.text = playerName;
    }

    public void StartGame()
    {
        gameMenuPanel.SetActive(false);
        chooseDificultPanel.SetActive(true);
        fondoMenu.sprite = fondos[3];
    }

    public void ShowHighScores()
    {
        fondoMenu.sprite = fondos[2];
        gameMenuPanel.SetActive(false);
        highScoresPanel.SetActive(true);
        currentDificultad = "Facil";
        dificultadActual.text = "Viendo Dificultad: " + currentDificultad;
        ActualizarDificultad(currentDificultad);
    }

    public async void ActualizarDificultad(string dificultad)
    {
        if (isUpdatingHighScores) return; // Si ya se está actualizando, no hacer nada

        isUpdatingHighScores = true; // Marcar que se está actualizando
        closeButton.interactable = false;
        facilButton.interactable = false;
        normalButton.interactable = false;
        dificilButton.interactable = false;
        await UpdateHighScores(dificultad);
        isUpdatingHighScores = false; // Marcar que la actualización ha terminado
        closeButton.interactable = true;
        facilButton.interactable = true;
        normalButton.interactable = true;
        dificilButton.interactable = true;
    }

    public async Task UpdateHighScores(string dificultad)
    {
        currentDificultad = dificultad;
        dificultadActual.text = "Viendo Dificultad: " + currentDificultad;

        foreach (Transform child in highScoreContent)
        {
            Destroy(child.gameObject);
        }

        List<HighScoreEntry> highScores = await HighScoreManager.GetHighScores(id_juegos, dificultad);

        if (highScores == null || highScores.Count == 0)
        {
            Debug.Log("No high scores found.");
            return;
        }

        Debug.Log("High Scores:");
        foreach (var highScore in highScores)
        {
            Debug.Log($"Position: {highScore.puesto}, User: {highScore.nickname_user}, Score: {highScore.puntaje}");
        }

        int position = 1;

        foreach (HighScoreEntry highScore in highScores)
        {
            GameObject newEntry;
            if (position == 1)
            {
                newEntry = Instantiate(firstPlacePrefab, highScoreContent);
            }
            else
            {
                newEntry = Instantiate(otherPlacesPrefab, highScoreContent);
            }

            TextMeshProUGUI[] textFields = newEntry.GetComponentsInChildren<TextMeshProUGUI>();
            textFields[0].text = position.ToString();
            textFields[1].text = highScore.nickname_user;
            textFields[2].text = highScore.puntaje.ToString();

            position++;
        }
    }

    public void CloseHighScores()
    {
        fondoMenu.sprite = fondos[1];
        highScoresPanel.SetActive(false);
        gameMenuPanel.SetActive(true);
    }

    public void SetDificultadFacil()
    {
        PlayerPrefs.SetString("Dificultad", "Facil");
        
        /*GameManager.playerName = playerName;
        GameManager.id_juegos = id_juegos;
        GameManager.id_user = playerId;*/
        SceneManager.LoadScene(1);
    }

    public void SetDificultadNormal()
    {
        PlayerPrefs.SetString("Dificultad", "Normal");
        
        /*GameManager.playerName = playerName;
        GameManager.id_juegos = id_juegos;
        GameManager.id_user = playerId;*/
        SceneManager.LoadScene(1);
    }

    public void SetDificultadDificil()
    {
        PlayerPrefs.SetString("Dificultad", "Dificil");

        /*GameManager.playerName = playerName;
        GameManager.id_juegos = id_juegos;
        GameManager.id_user = playerId;*/
        SceneManager.LoadScene(1);
    }

    public void OnMusicButtonPressed()
    {
        audioManager.ToggleMusic();
    }

    public void OnSFXButtonPressed()
    {
        audioManager.ToggleSFX();
    }

    public void HowToPlay()
    {
        indicacion.sprite = indicaciones[0];
        nextButton.SetActive(true);
        returnButton.SetActive(false);
        gameMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void NextImage()
    {
        nextButton.SetActive(false);
        returnButton.SetActive(true);
        indicacion.sprite = indicaciones[1];
    }

    public void returnMenuPanel()
    {
        howToPlayPanel.SetActive(false);
        gameMenuPanel.SetActive(true);
    }

    public void CloseDificults()
    {
        fondoMenu.sprite = fondos[1];
        chooseDificultPanel.SetActive(false);
        gameMenuPanel.SetActive(true);
    }
}