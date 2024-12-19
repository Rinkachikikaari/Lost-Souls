using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public enum ChestContentType { Item, Equipment, Ability }
    public ChestContentType contentType;

    public ItemData itemData; // Si el contenido es un Item
    public EquipmentData equipmentData; // Si el contenido es un Equipment
    public AbilityData abilityData; // Si el contenido es una Ability

    public int cantidad; // Cantidad para los Items
    public GameObject Abierto;
    public GameObject Cerrado;

    public AudioClip cofreSonido; // Sonido del cofre al abrirse
    private AudioSource audioSource;

    private bool isChestOpened = false;
    private bool isPlayerNear = false;

    private void Start()
    {
        // Configuración inicial del cofre
        Cerrado.SetActive(true);
        Abierto.SetActive(false);

        // Configuración del AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isChestOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isChestOpened = true;

        // Cambiar estado del cofre (cerrado -> abierto)
        Cerrado.SetActive(false);
        Abierto.SetActive(true);

        // Reproducir el sonido del cofre
        if (cofreSonido != null)
        {
            audioSource.PlayOneShot(cofreSonido);
        }

        // Entregar el contenido del cofre
        GiveChestContent();
    }

    private void GiveChestContent()
    {
        switch (contentType)
        {
            case ChestContentType.Item:
                if (itemData != null)
                {
                    InventoryManager.instance.AddItem(itemData, cantidad);
                }
                break;

            case ChestContentType.Equipment:
                if (equipmentData != null)
                {
                    InventoryManager.instance.AddEquipment(equipmentData);
                }
                break;

            case ChestContentType.Ability:
                if (abilityData != null)
                {
                    InventoryManager.instance.UnlockAbility(abilityData);
                }
                break;
        }

        Debug.Log("Contenido entregado: " + contentType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
