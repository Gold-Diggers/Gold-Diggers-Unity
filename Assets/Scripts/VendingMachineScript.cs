using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VendingMachineScript : MonoBehaviour {

    public Text hoverText;
    public GameObject vendingMachine;
    public Image vendingMachineInterface;
    public Image livesSoldout;
    public Image jetpackSoldout;
    public Image shovelSoldout;
    public Button livesPurchase;
    public Button jetpackPurchase;
    public Button shovelPurchase;
    public PlayerCollisionController player;
    public PlayerBaseController player_upgrades;
    public Canvas shopCanvas;
    public AudioSource visitSound;
    public AudioSource buySound;

    private const int PRICE_LIFE_UPGRADE = 25;
    private const int PRICE_JETPACK_UPGRADE = 50;
    private const int PRICE_SHOVEL_UPGRADE = 1;
    private bool isShopOpen;
    private Color buttonHighlight;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<PlayerCollisionController>();
        player_upgrades = FindObjectOfType<PlayerBaseController>();
        loadPlayerState();
        hoverText.enabled = false;
        isShopOpen = false;
        vendingMachineInterface.enabled = false;
        hideAllButtons();
    }

    private void loadPlayerState()
    {
        player.lives = GlobalPlayerScript.Instance.lives;
        player.diamonds = GlobalPlayerScript.Instance.diamonds;
        player.specialDiamonds = GlobalPlayerScript.Instance.specialDiamonds;
        player_upgrades.hasJetpackUpgrade = GlobalPlayerScript.Instance.hasJetpackUpgrade;
        player_upgrades.hasShovelUpgrade = GlobalPlayerScript.Instance.hasShovelUpgrade;
    }

    // Update is called once per frame
    void Update () {
        updateUpgradesState();
        updateButtonStates();
        triggerOpenShopInterface();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        hoverText.enabled = true;
        isShopOpen = true;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        hoverText.enabled = false;
        isShopOpen = false;
    }

    void hideAllButtons()
    {
        livesPurchase.gameObject.SetActive(false);
        jetpackPurchase.gameObject.SetActive(false);
        shovelPurchase.gameObject.SetActive(false);
    }

    void showAllButtons()
    {
        livesPurchase.gameObject.SetActive(true);
        jetpackPurchase.gameObject.SetActive(true);
        shovelPurchase.gameObject.SetActive(true);
    }

    void updateButtonStates()
    {
        if (player.diamonds < PRICE_LIFE_UPGRADE || player.lives >= 6)
        {
            livesPurchase.interactable = false;
        }
        else
        {
            livesPurchase.interactable = true;
        }
        if (player.diamonds < PRICE_JETPACK_UPGRADE || player_upgrades.hasJetpackUpgrade)
        {
            jetpackPurchase.interactable = false;
        }
        else
        {
            jetpackPurchase.interactable = true;
        }
        if (player.specialDiamonds < PRICE_SHOVEL_UPGRADE || player_upgrades.hasShovelUpgrade)
        {
            shovelPurchase.interactable = false;
        }
        else
        {
            shovelPurchase.interactable = true;
        }
    }

    void updateUpgradesState()
    {
        if (player.lives >= 6 && vendingMachineInterface.isActiveAndEnabled)
        {
            livesSoldout.enabled = true;
        }
        else
        {
            livesSoldout.enabled = false;
        }
        if (player_upgrades.hasJetpackUpgrade && vendingMachineInterface.isActiveAndEnabled)
        {
            jetpackSoldout.enabled = true;
        }
        else
        {
            jetpackSoldout.enabled = false;
        }
        if (player_upgrades.hasShovelUpgrade && vendingMachineInterface.isActiveAndEnabled)
        {
            shovelSoldout.enabled = true;
        }
        else
        {
            shovelSoldout.enabled = false;
        }
    }

    void triggerOpenShopInterface()
    {
        if (isShopOpen)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (!vendingMachineInterface.isActiveAndEnabled)
                {
                    visitSound.Play();
                }
                shopCanvas.sortingOrder = 12;
                vendingMachineInterface.enabled = true;
                showAllButtons();
                hoverText.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                shopCanvas.sortingOrder = 2;
                vendingMachineInterface.enabled = false;
                hideAllButtons();
                hoverText.enabled = true;
            }
        }
        else
        {
            vendingMachineInterface.enabled = false;
            hideAllButtons();
        }
    }

    public void onLivesPurchaseButtonHover()
    {
        if (player.diamonds < PRICE_LIFE_UPGRADE || player.lives >= 6)
        {
            livesPurchase.interactable = false;
        }
        else
        {
            livesPurchase.interactable = true;
        }
    }

    public void onLivesPurchase()
    {
        if (player.diamonds < PRICE_LIFE_UPGRADE || player.lives >= 6)
        {
            // do nothing
        }
        else
        {
            buySound.Play();
            player.diamonds -= PRICE_LIFE_UPGRADE;
            player.lives++;
        }
    }

    public void onJetpackPurchaseButtonHover()
    {
        if (player.diamonds < PRICE_JETPACK_UPGRADE || player_upgrades.hasJetpackUpgrade)
        {
            jetpackPurchase.interactable = false;
        }
        else
        {
            jetpackPurchase.interactable = true;
        }
    }

    public void onJetpackPurchase()
    {
        if (player.diamonds < PRICE_JETPACK_UPGRADE || player_upgrades.hasJetpackUpgrade)
        {
            // do nothing
        }
        else
        {
            buySound.Play();
            player.diamonds -= PRICE_JETPACK_UPGRADE;
            player_upgrades.hasJetpackUpgrade = true;
        }
    }

    public void onShovelPurchaseButtonHover()
    {
        if (player.specialDiamonds < PRICE_SHOVEL_UPGRADE || player_upgrades.hasShovelUpgrade)
        {
            shovelPurchase.interactable = false;
        }
        else
        {
            shovelPurchase.interactable = true;
        }
    }

    public void onShovelPurchase()
    {
        if (player.specialDiamonds < PRICE_SHOVEL_UPGRADE || player_upgrades.hasShovelUpgrade)
        {
            // do nothing
        }
        else
        {
            buySound.Play();
            player.specialDiamonds--;
            player_upgrades.hasShovelUpgrade = true;
        }
    }
}
