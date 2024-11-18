using UnityEngine;

public class SelectorDeMagia : MonoBehaviour
{
    private AirSpell airSpell;
    private FireSpell fireSpell;
    private LightningSpell lightningSpell;


    private int MagiaSeleccionada;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        airSpell = GetComponent<AirSpell>();
        fireSpell = GetComponent<FireSpell>();
        lightningSpell = GetComponent<LightningSpell>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MagiaSeleccionada = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MagiaSeleccionada = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MagiaSeleccionada = 3;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (MagiaSeleccionada == 1)
            {
                lightningSpell.LanzarRayo();
            }
            if (MagiaSeleccionada == 2)
            {
                fireSpell.LanzarFuego();
            }
            if (MagiaSeleccionada == 3)
            {
                airSpell.LanzarAire();
            }
        }
    }
}
