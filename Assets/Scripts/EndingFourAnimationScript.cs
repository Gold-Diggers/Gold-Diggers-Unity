using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingFourAnimationScript : MonoBehaviour {

    public Image contract;
    public AudioSource music;
    public Text dialogueText;

    private const string dialogue1 = "End of the road, Helmeted One.";
    private const string dialogue2 = "Unfortunately, it looks like you don't have enough\ncapital to claim back your soul...";
    private const string dialogue3 = "... Too bad for you, I guess.";
    private const string dialogue4 = "Although...";
    private const string dialogue5 = "You seem to have amassed a fortune in diamonds for\nyourself!";
    private const string dialogue6 = "Consider me impressed.";
    private const string dialogue7 = "Work for me, my dear.";
    private const string dialogue8 = "You get to keep 50 percent commission...";
    private const string dialogue9 = "... with health insurance and a house down in\nBurn Avenue.";
    private const string dialogue10 = "So... what say you?";
    private const string dialogue11 = "Your eternal soul, for a job in Hell?";
    private const string dialogue12 = "... that pays way better than your crappy\njob right now.";
    private const string dialogue13 = "Do we have a deal? *winks*";
    
    private GameObject player;
    private GameObject devil;
    public Image blackPanel;
    private Color colorRef;

    private bool movePlayer;
    private bool moveLeft;
    private bool moveRight;
    private bool devilFadein;
    private bool devilAppear;
    private bool fadeContractIn;
    private bool moveContract;
    private bool fadeToBlack;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        devil = GameObject.Find("Devil");
        blackPanel = GameObject.Find("BlackScreen").GetComponentsInChildren<Image>()[0];
        devil.SetActive(false);
        colorRef = Color.white;
        colorRef.a = 0;
        //Time.timeScale = 2.0f;
        movePlayer = true;
        StartCoroutine(movePlayerAbout());
    }

    // Update is called once per frame
    void Update()
    {
        animateDevil();
        if (movePlayer)
        {
            if (moveLeft) triggerPlayerMoveLeft();
            else if (moveRight) triggerPlayerMoveRight();
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
        else if (fadeContractIn)
        {
            triggerContractFade();
        }
        else if (moveContract)
        {
            triggerContractMove();
        }
        else if (fadeToBlack)
        {
            triggerFadeToBlack();
        }
    }

    IEnumerator fadeDevilIn()
    {
        devilFadein = true;
        devil.SetActive(devilFadein);
        yield return StartCoroutine(triggerDialogues());
    }

    IEnumerator movePlayerAbout()
    {
        //print("Commencing movement");
        yield return new WaitForSeconds(1.5f);
        music.Play();
        moveLeft = true;
        player.GetComponent<SpriteRenderer>().flipX = true;
        player.GetComponent<Animator>().Play("Running");
        yield return new WaitUntil(() => player.transform.position.x <= -6.51f);
        moveLeft = !moveLeft;
        player.GetComponent<Animator>().Play("idle");
        yield return new WaitForSeconds(1f);
        moveRight = true;
        player.GetComponent<Animator>().Play("Running");
        player.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitUntil(() => player.transform.position.x >= 3.49f);
        moveRight = !moveRight;
        player.GetComponent<Animator>().Play("idle");
        yield return new WaitForSeconds(1f);
        moveLeft = !moveLeft;
        player.GetComponent<Animator>().Play("Running");
        player.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitUntil(() => player.transform.position.x <= -2.51f);
        movePlayer = false;
        player.GetComponent<Animator>().Play("idle");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(fadeDevilIn());
    }

    IEnumerator triggerDialogues()
    {
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue1;
        player.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue2;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue3;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue4;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue5;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue6;
        yield return StartCoroutine(triggerHelmetRemoval());
    }

    IEnumerator triggerHelmetRemoval()
    {
        player.GetComponent<Animator>().Play("helmetremove");
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Animator>().Play("thief_idle");
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(triggerNextDialogue());
    }

    IEnumerator triggerNextDialogue()
    {
        dialogueText.text = dialogue7;
        fadeContractIn = true;
        yield return new WaitForSeconds(1.65f);
        player.GetComponent<Animator>().Play("thief_contract");
        yield return new WaitForSeconds(0.3f);
        dialogueText.text = dialogue8;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue9;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue10;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue11;
        yield return new WaitForSeconds(3f);
        /*dialogueText.text = dialogue12;
        yield return new WaitForSeconds(3f);
        dialogueText.text = dialogue13;
        yield return new WaitForSeconds(1.5f);*/
        player.GetComponent<Animator>().Play("thief_agree");
        dialogueText.text = "";
        yield return new WaitForSeconds(4f);
        music.Stop();
        fadeToBlack = true;
    }

    void triggerContractFade()
    {
        if (contract.color.a < 1f)
        {
            colorRef.a += 0.05f;
            contract.color = colorRef;
        }
        else
        {
            fadeContractIn = false;
            moveContract = true;
        }
    }

    void triggerContractMove()
    {
        if (contract.rectTransform.position.x > -1.9f)
        {
            contract.rectTransform.Translate(new Vector3(-0.05f, -0.009f, 0));
        }
        else
        {
            moveContract = false;
            contract.enabled = false;
        }
    }

    void triggerPlayerMoveLeft()
    {
        player.transform.Translate(0.075f * Vector3.left);
    }

    void triggerPlayerMoveRight()
    {
        player.transform.Translate(0.075f * Vector3.right);
    }

    void triggerFadeToBlack()
    {
        Color newColor = blackPanel.color;
        if (newColor.a >= 1.0f)
        {
            fadeToBlack = false;
            SceneManager.LoadScene("Ending 4a", LoadSceneMode.Single);
        }
        else
        {
            newColor.a += 0.05f;
            blackPanel.color = newColor;
        }
    }

    void animateDevil()
    {
        devil.transform.Translate(new Vector3(0, 0.0075f * Mathf.Sin(Time.time), 0));
    }
}
