using UnityEngine;
using UnityEngine.UI; // Necesario para manejar elementos de UI

public class SelectorDeMagia : MonoBehaviour
{
    private AirSpell airSpell;
    private FireSpell fireSpell;
    private LightningSpell lightningSpell;

    public Image magiaUI; // Referencia a la imagen en la UI
    public Sprite rayoSprite; // Sprite para el rayo
    public Sprite fuegoSprite; // Sprite para el fuego
    public Sprite aireSprite; // Sprite para el aire

    private int magiaSeleccionada;

    void Start()
    {
        airSpell = GetComponent<AirSpell>();
        fireSpell = GetComponent<FireSpell>();
        lightningSpell = GetComponent<LightningSpell>();

        ActualizarUI(); // Actualizar la imagen al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && InventoryManager.instance.IsAbilityUnlocked("Trueno"))
        {
            magiaSeleccionada = 1;
            ActualizarUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && InventoryManager.instance.IsAbilityUnlocked("Fuego"))
        {
            magiaSeleccionada = 2;
            ActualizarUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && InventoryManager.instance.IsAbilityUnlocked("Viento"))
        {
            magiaSeleccionada = 3;
            ActualizarUI();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (magiaSeleccionada == 1)
            {
                lightningSpell.LanzarRayo();
            }
            if (magiaSeleccionada == 2)
            {
                fireSpell.LanzarFuego();
            }
            if (magiaSeleccionada == 3)
            {
                airSpell.LanzarAire();
            }
        }
    }

    private void ActualizarUI()
    {
        switch (magiaSeleccionada)
        {
            case 1:
                magiaUI.sprite = rayoSprite;
                break;
            case 2:
                magiaUI.sprite = fuegoSprite;
                break;
            case 3:
                magiaUI.sprite = aireSprite;
                break;
            default:
                break;
        }
    }
}
