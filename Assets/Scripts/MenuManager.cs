using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    public GameObject panelInicial;
    public GameObject panelLoading;
    public GameObject panelUsername;
    public GameObject panelMenu;

    public TMP_InputField inputUsername;
    public Button readyButton;

    public string idUser;

    public static string user_login;
    private static string playerName;
    private static int playerId;

    private bool modoInicial = true;
    private bool modoConcurso = false;

    public Button ConcursoButton;
    public Button TutorialButton;
    public Button JugarButton;
    public Button ConfiguracionButton;
    public Button salirDeConfigButton;
    public GameObject configPanel;
    public TextMeshProUGUI playerNameText;
    public AudioManager audioManager;

    // Panel dificultad y controles
    public GameObject escogerDificultadJuegoPanel;
    public Toggle toggleFacil, toggleNormal, toggleDificil;
    public Button botonJugarPartida, backButton;
    public TextMeshProUGUI textoJugarPartida;

    void Start()
    {
        panelInicial.SetActive(true);
        panelLoading.SetActive(false);
        panelUsername.SetActive(false);
        panelMenu.SetActive(false);
        escogerDificultadJuegoPanel.SetActive(false);
        configPanel.SetActive(false);
        modoInicial = true;
        modoConcurso = false;

        inputUsername.text = "";
        readyButton.interactable = false;
        inputUsername.onValueChanged.AddListener(OnInputUsernameChanged);
        readyButton.onClick.AddListener(OnReadyButtonPressed);

        // Inicializar toggles y botones de dificultad
        toggleFacil.isOn = false;
        toggleNormal.isOn = false;
        toggleDificil.isOn = false;
        botonJugarPartida.interactable = false;
        textoJugarPartida.gameObject.SetActive(false);
        SetBotonJugarPartidaAltura(183);

        toggleFacil.onValueChanged.AddListener((v) => OnToggleChanged(toggleFacil, v));
        toggleNormal.onValueChanged.AddListener((v) => OnToggleChanged(toggleNormal, v));
        toggleDificil.onValueChanged.AddListener((v) => OnToggleChanged(toggleDificil, v));
        botonJugarPartida.onClick.AddListener(OnBotonJugarPartida);
        backButton.onClick.AddListener(OnBackButton);
        JugarButton.onClick.AddListener(OnJugarButton);
        ConfiguracionButton.onClick.AddListener(OnConfiguracionButton);
        salirDeConfigButton.onClick.AddListener(OnSalirDeConfigButton);
        TutorialButton.onClick.AddListener(OnTutorialButton);
    }
    void OnTutorialButton()
    {
        // Buscar el LoadingScreenManager en la escena
        LoadingScreenManager loadingManager = FindObjectOfType<LoadingScreenManager>();
        if (loadingManager != null)
        {
            loadingManager.LoadSceneAsyncWithLoadingPanelWithIndex(1); // Cambia "1" por el índice de tu escena tutorial
        }
        else
        {
            Debug.LogWarning("No se encontró LoadingScreenManager en la escena.");
        }
    }
    void OnConfiguracionButton()
    {
        panelMenu.SetActive(false);
        configPanel.SetActive(true);
        if (audioManager != null)
        {
            audioManager.LoadAudioSettings();
        }
    }

    void OnSalirDeConfigButton()
    {
        configPanel.SetActive(false);
        panelMenu.SetActive(true);
    }
    void OnJugarButton()
    {
        panelMenu.SetActive(false);
        escogerDificultadJuegoPanel.SetActive(true);
        // Resetear estado
        toggleFacil.isOn = false;
        toggleNormal.isOn = false;
        toggleDificil.isOn = false;
        botonJugarPartida.interactable = false;
        textoJugarPartida.gameObject.SetActive(false);
        SetBotonJugarPartidaAltura(183);
    }

    void OnToggleChanged(Toggle changedToggle, bool isOn)
    {
        if (isOn)
        {
            // Desactivar los otros toggles
            if (changedToggle == toggleFacil)
            {
                toggleNormal.isOn = false;
                toggleDificil.isOn = false;
            }
            else if (changedToggle == toggleNormal)
            {
                toggleFacil.isOn = false;
                toggleDificil.isOn = false;
            }
            else if (changedToggle == toggleDificil)
            {
                toggleFacil.isOn = false;
                toggleNormal.isOn = false;
            }
        }
        // Activar botón si algún toggle está en true
        bool algunoActivo = toggleFacil.isOn || toggleNormal.isOn || toggleDificil.isOn;
        botonJugarPartida.interactable = algunoActivo;
        textoJugarPartida.gameObject.SetActive(algunoActivo);
        SetBotonJugarPartidaAltura(algunoActivo ? 220 : 183);
    }

    void OnBotonJugarPartida()
    {
        // Aquí puedes poner la lógica para iniciar la partida según la dificultad seleccionada
        // Ejemplo: string dificultad = toggleFacil.isOn ? "Facil" : toggleNormal.isOn ? "Normal" : "Dificil";
    }

    void OnBackButton()
    {
        escogerDificultadJuegoPanel.SetActive(false);
        panelMenu.SetActive(true);
        toggleFacil.isOn = false;
        toggleNormal.isOn = false;
        toggleDificil.isOn = false;
        botonJugarPartida.interactable = false;
        textoJugarPartida.gameObject.SetActive(false);
        SetBotonJugarPartidaAltura(183);
    }

    void SetBotonJugarPartidaAltura(float altura)
    {
        var rect = botonJugarPartida.GetComponent<RectTransform>();
        if (rect != null)
        {
            var size = rect.sizeDelta;
            size.y = altura;
            rect.sizeDelta = size;
        }
    }

    void Update()
    {
        if (modoInicial && Input.anyKeyDown)
        {
            modoInicial = false;
            panelInicial.SetActive(false);

            panelLoading.SetActive(true);
            StartCoroutine(FlujoObtenerUsuario());
        }
    }

    void OnInputUsernameChanged(string value)
    {
        readyButton.interactable = !string.IsNullOrEmpty(value);
    }

    void OnReadyButtonPressed()
    {
        playerName = inputUsername.text;
        panelUsername.SetActive(false);
        StartCoroutine(AnimacionPanelLoading());
    }

    IEnumerator AnimacionPanelLoading()
    {

        panelLoading.SetActive(true);
        yield return new WaitForSeconds(2f);
        panelLoading.SetActive(false);
        MostrarPanelMenu();
    }

    void MostrarPanelMenu()
    {
        panelMenu.SetActive(true);
        // Activar todos los botones
        TutorialButton.gameObject.SetActive(true);
        JugarButton.gameObject.SetActive(true);
        ConfiguracionButton.gameObject.SetActive(true);
        // Mostrar el nombre del jugador
        if (playerNameText != null)
            playerNameText.text = playerName;
        // Activar o desactivar el botón de concurso según el modo
        if (ConcursoButton != null)
            ConcursoButton.gameObject.SetActive(modoConcurso);
    }

    IEnumerator FlujoObtenerUsuario()
    {
        yield return GetIdUserFromUrlCoroutine();
    }

    IEnumerator GetIdUserFromUrlCoroutine()
    {
        yield return new WaitForSeconds(2f);
        string url = Application.absoluteURL;
        bool tieneId = false;
        if (string.IsNullOrEmpty(url))
        {
            idUser = null;
        }
        else
        {
            var uri = new System.Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            idUser = query.Get("id_user");
            if (idUser != null)
            {
                tieneId = true;
            }
        }

        if (tieneId)
        {
            yield return ObtenerNicknameCoroutine();
            modoConcurso = true;
            yield return new WaitForSeconds(2f);
            panelLoading.SetActive(false);
            MostrarPanelMenu();
        }
        else
        {
            modoConcurso = false;
            yield return new WaitForSeconds(2f);
            panelLoading.SetActive(false);
            panelUsername.SetActive(true);
        }
    }

    IEnumerator ObtenerNicknameCoroutine()
    {
        var task = InitializeUser(idUser);
        while (!task.IsCompleted)
        {
            yield return null;
        }
    }

    public async Task InitializeUser(string idUser)
    {
        var user = await HighScoreManager.GetUserById(idUser);
        if (user != null)
        {
            playerName = user.user_nickname;
            user_login = user.user_login;
            playerId = int.Parse(idUser);
        }
    }
}