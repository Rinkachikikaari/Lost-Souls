using UnityEngine;

public class PocionCurativa : MonoBehaviour
{
    [Header("Configuarion")]
    private int cantidadCuracion;
    private string Nombre;
    [SerializeField] VidaJugador VidaJugador;
    private void Update()
    {
        ItemData herramientaActiva = PlayerStats.instance.HerramientaActiva;
        if (Input.GetKeyDown(KeyCode.F) && herramientaActiva.subCategory == ItemSubCategory.Pocion)
        {
            UsarPocion();
        }
    }
    private void UsarPocion()
    {
        ItemData herramientaActiva = PlayerStats.instance.HerramientaActiva;
        if (InventoryManager.instance.HasItem(Nombre) && InventoryManager.instance.PuedoUsarItem(herramientaActiva))
        {
            InventoryManager.instance.HasItem(Nombre);
            VidaJugador.CurarVida(cantidadCuracion);
        }
    }
    public void UpdateCurrentWeapon()
    {
        if (PlayerStats.instance.HerramientaActiva != null)
        {
            Nombre = PlayerStats.instance.HerramientaActiva.itemName;
            cantidadCuracion = PlayerStats.instance.HerramientaActiva.Curacion;
        }
    }
}
