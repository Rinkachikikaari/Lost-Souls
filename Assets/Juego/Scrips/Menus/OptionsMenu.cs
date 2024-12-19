using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;          // Referencia al menú de pausa en el Canvas
    public GameObject pauseAjustesUI;          // Referencia al menú de pausa en el Canvas

    [Header("Control de Volumen")]
    public Slider volumenSlider;      // Slider para controlar el volumen
    public TMP_Text volumenText;          // Texto para mostrar el valor del volumen

    [Header("Opciones de FPS")]
    public Button fps30Button;        // Botón para 30 FPS
    public Button fps60Button;        // Botón para 60 FPS
    public Button fps120Button;       // Botón para 120 FPS
    public Button fpsUnlimitedButton; // Botón para sin límite de FPS

    public Image Ifps30Button;        // Botón para 30 FPS
    public Image Ifps60Button;        // Botón para 60 FPS
    public Image Ifps120Button;       // Botón para 120 FPS
    public Image IfpsUnlimitedButton; // Botón para sin límite de FPS

    [Header("Opciones de VSync")]
    public Toggle vsyncToggle;        // Toggle para activar/desactivar VSync

    private void Start()
    {
        pauseAjustesUI.SetActive(false);
        // Cargar configuraciones guardadas
        CargarConfiguraciones();

        // Asignar listeners a los botones de FPS
        fps30Button.onClick.AddListener(() => SetFPS(30));
        fps60Button.onClick.AddListener(() => SetFPS(60));
        fps120Button.onClick.AddListener(() => SetFPS(120));
        fpsUnlimitedButton.onClick.AddListener(() => SetFPS(-1)); // -1 para sin límite

        // Asignar listener al slider de volumen
        volumenSlider.onValueChanged.AddListener(SetVolumen);

        // Asignar listener al toggle de VSync
        vsyncToggle.onValueChanged.AddListener(SetVSync);
    }

    private void CargarConfiguraciones()
    {
        // Cargar el volumen
        float volumen = PlayerPrefs.GetFloat("Volumen", 1f); // 1f por defecto
        AudioListener.volume = volumen;
        volumenSlider.value = volumen;
        volumenText.text = Mathf.RoundToInt(volumen * 100) + "%";

        // Cargar límite de FPS
        int fps = PlayerPrefs.GetInt("FPSLimit", -1); // Sin límite por defecto
        Application.targetFrameRate = fps;
        ActualizarFPSValueText(fps);

        // Cargar configuración de VSync
        bool vsync = PlayerPrefs.GetInt("VSync", 1) == 1; // VSync activado por defecto
        QualitySettings.vSyncCount = vsync ? 1 : 0;
        vsyncToggle.isOn = vsync;
    }

    public void SetVolumen(float volumen)
    {
        // Ajustar el volumen global del juego
        AudioListener.volume = volumen;
        volumenText.text = Mathf.RoundToInt(volumen * 100) + "%";

        // Guardar el volumen en PlayerPrefs
        PlayerPrefs.SetFloat("Volumen", volumen);
    }

    public void salir()
    {
        pauseAjustesUI.SetActive(false);
        pauseMenuUI.SetActive(true);

    }

    public void SetFPS(int fps)
    {
        // Aplicar límite de FPS
        Application.targetFrameRate = fps;

        // Guardar la configuración en PlayerPrefs
        PlayerPrefs.SetInt("FPSLimit", fps);

        // Actualizar el texto del FPS seleccionado
        ActualizarFPSValueText(fps);

        Debug.Log("Límite de FPS establecido en: " + (fps == -1 ? "Sin Límite" : fps + " FPS"));
    }

    private void ActualizarFPSValueText(int fps)
    {
        /*
        if (fps == 30)
        {
            Ifps30Button.color.WithAlpha(225);
            Ifps120Button.color.WithAlpha(100);
            Ifps60Button.color.WithAlpha(100);
            IfpsUnlimitedButton.color.WithAlpha(100);
        }
        if (fps == 60)
        {
            Ifps30Button.color.WithAlpha(100);
            Ifps120Button.color.WithAlpha(100);
            Ifps60Button.color.WithAlpha(225);
            IfpsUnlimitedButton.color.WithAlpha(100);
        }
        if (fps == 120)
        {
            Ifps30Button.color.WithAlpha(100);
            Ifps120Button.color.WithAlpha(225);
            Ifps60Button.color.WithAlpha(100);
            IfpsUnlimitedButton.color.WithAlpha(100);
        }
        if (fps == -1)
        {
            Ifps30Button.color.WithAlpha(100);
            Ifps120Button.color.WithAlpha(100);
            Ifps60Button.color.WithAlpha(100);
            IfpsUnlimitedButton.color.WithAlpha(225);
        }
        */
    }

    public void SetVSync(bool vsync)
    {
        // Activar o desactivar VSync
        QualitySettings.vSyncCount = vsync ? 1 : 0;

        // Guardar la configuración en PlayerPrefs
        PlayerPrefs.SetInt("VSync", vsync ? 1 : 0);

        Debug.Log("VSync " + (vsync ? "activado" : "desactivado"));
    }
}
