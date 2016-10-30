using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingThreeAnimationScript : MonoBehaviour {

    public Sprite loverAwake;
    public Sprite loverStand;
    public Sprite loverWorried;

    public Sprite playerAngry;
    public Sprite playerSurprised;
    public Sprite playerConfident;

    public Sprite devilAngry;
    public Text dialogueText;

    public Canvas whiteFlash;
    public Image soulDiamond1;
    public Image soulDiamond2;
    public Image soulDiamond3;

    public AudioSource endingThreeMusic;

    private const string dialogue1 = "Finally got your girl back, eh?\nDon't you think you're celebrating this too early?";
    private const string dialogue2 = "I STILL HAVE YOUR SOUL, YOU FOOL!";
    private const string dialogue3 = "Let's see you leave WITHOUT it!";
    private const string dialogue4 = "KEEHAAHAAHAAHAAHAAAAAAA!";
    private const string dialogue5 = "Th-th-those...";
    private const string dialogue6 = "Those are Soul Gems...";
    private const string dialogue7 = "How did you-?!";
    private const string dialogue8 = "They were part of MY hidden stash!";
    private const string dialogue9 = "GRAH!!!!!! You lucky little-!";
    private const string dialogue10 = "FINE! Take back your filthy little soul in exchange!";
    private const string dialogue11 = "I hope you're happy now.";
    private const string dialogue12 = "Now BEGONE-";
    private const string dialogue13 = "AND NEVER COME BACK.";

    private GameObject lover;
    private GameObject cage;
    private GameObject player;
    private GameObject devil;
    private GameObject soul;
    private GameObject exitDoor;
    public Image blackPanel;

    private bool moveToPlayer;
    private bool devilFadein;
    private bool devilAppear;
    private bool performExchange;
    private bool fadeDoorIn;
    private bool fadeToBlack;

	// Use this for initialization
	void Start () {
        lover = GameObject.Find("Lover");
        cage = GameObject.Find("cagebroken");
        player = GameObject.Find("Player");
        devil = GameObject.Find("Devil");
        soul = GameObject.Find("Soul");
        exitDoor = GameObject.Find("Give Up Door");
        blackPanel = GameObject.Find("BlackScreen").GetComponentsInChildren<Image>()[0];
        Color hideDoorColor = exitDoor.GetComponent<SpriteRenderer>().color;
        hideDoorColor.a = 0;
        exitDoor.GetComponent<SpriteRenderer>().color = hideDoorColor;
        devil.SetActive(false);
        soul.SetActive(false);
        whiteFlash.enabled = false;
        soulDiamond1.enabled = false;
        soulDiamond2.enabled = false;
        soulDiamond3.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        //print(devilFadein);
        animateDevil();
        if (moveToPlayer)
        {
            moveLoverToPlayer();
        }
        else if (devilFadein)
        {
            Color newColor = devil.GetComponent<SpriteRenderer>().color;
            if (newColor.a >= 1.0f)
            {
                devilFadein = false;
                devilAppear = true;
            }
            else
            {
                newColor.a += 0.025f;
                devil.GetComponent<SpriteRenderer>().color = newColor;
            }
        }
        else if (performExchange)
        {
            performItemExchange();
        }
        else if (fadeDoorIn)
        {
            revealDoor();
        }
        else if (fadeToBlack)
        {
            triggerFadeToBlack();
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            cage.GetComponent<Animator>().Play("cagebreak");
            Destroy(cage.GetComponent<Collider2D>());
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-190f, 190f));
            StartCoroutine(makeLoverReact());
        }
    }

    IEnumerator makeLoverReact()
    {
        lover.transform.Translate(new Vector3(0, -0.15f, 0));
        yield return new WaitForSecondsRealtime(0.5f);
        lover.GetComponent<Animator>().Play("lover_awake");
        yield return StartCoroutine(makeLoverRunToPlayer());
    }

    IEnumerator makeLoverRunToPlayer()
    {
        yield return new WaitForSeconds(3f);
        lover.GetComponent<Animator>().Play("lover_run");
        moveToPlayer = true;
    }

    void moveLoverToPlayer()
    {
        if (lover.transform.position.x > -6.8)
        {
            lover.transform.Translate(new Vector3(-0.05f, 0, 0));
        }
        else
        {
            StartCoroutine(fadeDevilIn());
            StartCoroutine(triggerDialogues());
        }
    }

    IEnumerator triggerDialogues()
    {
        yield return new WaitForSeconds(2.5f);
        lover.GetComponent<Animator>().Stop();
        lover.GetComponent<SpriteRenderer>().sprite = loverWorried;
        player.GetComponent<Animator>().Stop();
        player.GetComponent<SpriteRenderer>().sprite = playerAngry;
        dialogueText.text = dialogue1;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue2;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue3;
        yield return new WaitForSeconds(2.6f);
        dialogueText.text = dialogue4;
        yield return StartCoroutine(triggerWhiteFlash());
    }

    IEnumerator triggerWhiteFlash()
    {
        yield return new WaitForSeconds(5f);
        whiteFlash.enabled = true;
        dialogueText.text = "";
        player.GetComponent<SpriteRenderer>().sprite = playerSurprised;
        soulDiamond1.enabled = true;
        soulDiamond2.enabled = true;
        soulDiamond3.enabled = true;
        yield return new WaitForSeconds(1f);
        whiteFlash.enabled = false;
        yield return new WaitForSeconds(3f);
        devil.GetComponent<Animator>().Play("devil_upset");
        yield return new WaitForSeconds(1f);
        player.GetComponent<SpriteRenderer>().sprite = playerConfident;
        yield return StartCoroutine(triggerNextDialogues());
        
    }

    IEnumerator triggerNextDialogues()
    {
        dialogueText.text = dialogue5;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue6;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue7;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue8;
        yield return new WaitForSeconds(2.5f);
        devil.GetComponent<Animator>().Play("devil_upset2");
        dialogueText.text = dialogue9;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue10;
        yield return StartCoroutine(triggerItemsExchange());
    }

    IEnumerator triggerItemsExchange()
    {
        performExchange = true;
        soul.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue11;
        yield return new WaitForSeconds(2.5f);
        fadeDoorIn = true;
        dialogueText.text = dialogue12;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue13;
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = "";
        yield return new WaitForSeconds(1.0f);
        fadeToBlack = true;
    }

    IEnumerator fadeDevilIn()
    {
        devilFadein = true;
        moveToPlayer = false;
        lover.GetComponent<Animator>().Play("lover_idle");
        devil.SetActive(devilFadein);
        yield return StartCoroutine(makeDevilLaugh());
    }

    void triggerFadeToBlack()
    {
        Color newColor = blackPanel.color;
        if (newColor.a >= 1.0f)
        {
            fadeToBlack = false;
            SceneManager.LoadScene("Ending 3a", LoadSceneMode.Single);
        }
        else
        {
            newColor.a += 0.05f;
            blackPanel.color = newColor;
        }
    }

    IEnumerator makeDevilLaugh()
    {
        yield return new WaitForSeconds(10f);
        devil.GetComponent<Animator>().Play("devil_cackle");
    }

    void performItemExchange()
    {
        if (soul.transform.position.x < -7.56f)
        {
            performExchange = false;
            soulDiamond1.enabled = false;
            soulDiamond2.enabled = false;
            soulDiamond3.enabled = false;
            soul.SetActive(false);
        }
        else
        {
            soulDiamond1.rectTransform.Translate(new Vector3(8f * Time.deltaTime, 0, 0));
            soulDiamond2.rectTransform.Translate(new Vector3(8f * Time.deltaTime, 0, 0));
            soulDiamond3.rectTransform.Translate(new Vector3(8f * Time.deltaTime, 0, 0));
            soul.transform.Translate(new Vector3(-8f * Time.deltaTime, 0, 0));
        }
    }

    void animateDevil()
    {
        devil.transform.Translate(new Vector3(0, 0.0075f * Mathf.Sin(Time.time), 0));
    }

    void revealDoor()
    {
        Color newColor = exitDoor.GetComponent<SpriteRenderer>().color;
        if (newColor.a >= 1.0f)
        {
            fadeDoorIn = false;
        }
        else
        {
            newColor.a += 0.0075f;
            exitDoor.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

}
