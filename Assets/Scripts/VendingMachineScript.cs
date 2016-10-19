using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VendingMachineScript : MonoBehaviour {

    public Text hoverText;
    public GameObject vendingMachine;
    public Image vendingMachineInterface;
    public Button livesPurchase;
    public Button jetpackPurchase;
    public Button shovelPurchase;
    public PlayerCollisionController player;

    private const int PRICE_LIFE_UPGRADE = 25;
    private const int PRICE_JETPACK_UPGRADE = 50;
    private const int PRICE_SHOVEL_UPGRADE = 1;
    private bool isShopOpen;
    private Color buttonHighlight;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<PlayerCollisionController>();
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
    }

    // Update is called once per frame
    void Update () {
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

    void triggerOpenShopInterface()
    {
        if (isShopOpen)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                vendingMachineInterface.enabled = true;
                showAllButtons();
                hoverText.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
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
        if (player.diamonds < PRICE_LIFE_UPGRADE)
        {
            ColorBlock cb = livesPurchase.colors;
            cb.highlightedColor = new Color(1, 0, 0, 70f / 255);
            livesPurchase.colors = cb;
        }
        else
        {
            ColorBlock cb = livesPurchase.colors;
            cb.highlightedColor = new Color(1, 1, 0, 200f / 255);
            livesPurchase.colors = cb;
        }
    }

    public void onLivesPurchase()
    {
        if (player.diamonds < PRICE_LIFE_UPGRADE)
        {
            // do nothing
        }
        else
        {
            print("Player successfully purchased lives upgrade.");
            player.diamonds -= PRICE_LIFE_UPGRADE;
            player.lives++;
            print(player.diamonds);
            print(player.lives);
        }
    }

    public void onJetpackPurchaseButtonHover()
    {
        if (player.diamonds < PRICE_JETPACK_UPGRADE)
        {
            ColorBlock cb = jetpackPurchase.colors;
            cb.highlightedColor = new Color(1, 0, 0, 70f / 255);
            jetpackPurchase.colors = cb;
        }
        else
        {
            ColorBlock cb = jetpackPurchase.colors;
            cb.highlightedColor = new Color(1, 1, 0, 200f / 255);
            jetpackPurchase.colors = cb;
        }
    }

    public void onJetpackPurchase()
    {
        if (player.diamonds < PRICE_JETPACK_UPGRADE)
        {
            // do nothing
        }
        else
        {
            print("Player successfully purchased jetpack upgrade.");
            player.diamonds -= PRICE_JETPACK_UPGRADE;
            print(player.diamonds);
            // 1. add jetpack upgrade to player
            // 2. superimpose 'SOLD OUT' over jetpack
            // 3. must check if this upgrade exist in player for level 2.5 and perform #2 if the upgrade exists
        }
    }

    public void onShovelPurchaseButtonHover()
    {
        if (player.specialDiamonds < PRICE_SHOVEL_UPGRADE)
        {
            ColorBlock cb = shovelPurchase.colors;
            cb.highlightedColor = new Color(1, 0, 0, 70f / 255);
            shovelPurchase.colors = cb;
        }
        else
        {
            ColorBlock cb = shovelPurchase.colors;
            cb.highlightedColor = new Color(1, 1, 0, 200f / 255);
            shovelPurchase.colors = cb;
        }
    }

    public void onShovelPurchase()
    {
        if (player.specialDiamonds < PRICE_SHOVEL_UPGRADE)
        {
            // do nothing
        }
        else
        {
            print("Player successfully purchased shovel upgrade.");
            player.specialDiamonds--;
            print(player.specialDiamonds);
            // 1. add shovel upgrade to player
            // 2. superimpose 'SOLD OUT' over shovel
            // 3. must check if this upgrade exist in player for level 2.5 and perform #2 if the upgrade exists
        }
    }
}
