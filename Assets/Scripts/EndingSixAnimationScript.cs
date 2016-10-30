using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSixAnimationScript : MonoBehaviour {

    public Text dialogueText;

    private const string dialogue1 = "...";
    private const string dialogue2 = "... ...";
    private const string dialogue3 = "... ... ...";
    private const string dialogue4 = "What a pitiful collection of diamonds you have...";
    private const string dialogue5 = "And you made me wait so long for you...";
    private const string dialogue6 = "Remind me why you even entered this place, again?";
    private const string dialogue7 = "...";
    private const string dialogue8 = "Maybe... this was all some kind of a game to you?";
    private const string dialogue9 = "Were you looking to be ENTERTAINED?";
    private const string dialogue10 = "I DON'T THINK SO.";
    private const string dialogue11 = "STEVE! GET DOWN HERE!";
    private const string dialogue12 = "Huh. Whassup, boss?"; // color this grey
    private const string dialogue13 = "Go and have a vacation.";
    private const string dialogue14 = "Gold-head here is taking your place.";
    private const string dialogue15 = "I'm getting paid leave for that, right?"; // color this grey
    private const string dialogue16 = "...";
    private const string dialogue17 = "Steve.";
    private const string dialogue18 = "Yes, boss?"; // color this grey
    private const string dialogue19 = "Nobody cares.";
    private const string dialogue20 = "..."; // color this grey
    private const string dialogue21 = "He won't be missed.";
    private const string dialogue22 = "As for you, my SWEET, SWEET UNDERLING...";
    private const string dialogue23 = "... Get to work at Level 1-";
    private const string dialogue24 = "PRONTO!";
    private const string dialogue25 = "Don't worry, I've sealed your mouth shut...";
    private const string dialogue26 = "... so you can't protest for now! :D";
    private const string dialogue27 = "Enjoy your job forever!";

    private GameObject player;
    private GameObject devil;
    private GameObject steve;
    private GameObject bumper;
    private Color defaultTextColor;
    public Image blackPanel;

    private bool movePlayer;
    private bool moveLeft;
    private bool moveRight;
    private bool moveBumper;
    private bool fadeToBlack;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        devil = GameObject.Find("Devil");
        steve = GameObject.Find("Steve");
        bumper = GameObject.Find("Bumper");
        blackPanel = GameObject.Find("BlackScreen").GetComponentsInChildren<Image>()[0];
        defaultTextColor = dialogueText.color;
        movePlayer = true;
        StartCoroutine(triggerDialogues());
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
        else if (moveBumper)
        {
            triggerMoveBumper();
        }
        else if (fadeToBlack)
        {
            triggerFadeToBlack();
        }
    }

    IEnumerator triggerDialogues()
    {
        yield return new WaitForSeconds(2.5f);
        dialogueText.text = dialogue1;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue2;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue3;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue4;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue5;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue6;
        yield return new WaitForSeconds(3.5f);
        yield return StartCoroutine(triggerNextDialogue());
    }

    IEnumerator triggerNextDialogue()
    {
        dialogueText.text = "";
        movePlayer = true;
        moveLeft = true;
        player.GetComponent<SpriteRenderer>().flipX = true;
        player.GetComponent<Animator>().Play("Running");
        yield return new WaitForSeconds(1.5f);
        movePlayer = !movePlayer;
        moveLeft = !moveLeft;
        player.GetComponent<Animator>().Play("dig");
        yield return new WaitForSeconds(0.75f);
        movePlayer = !movePlayer;
        player.GetComponent<SpriteRenderer>().flipX = false;
        moveRight = true;
        player.GetComponent<Animator>().Play("Running");
        yield return new WaitForSeconds(1.25f);
        movePlayer = !movePlayer;
        moveRight = !moveRight;
        player.GetComponent<Animator>().Play("dig");
        yield return new WaitForSeconds(0.75f);
        movePlayer = !movePlayer;
        player.GetComponent<SpriteRenderer>().flipX = true;
        moveLeft = !moveLeft;
        player.GetComponent<Animator>().Play("Running");
        yield return new WaitForSeconds(0.5f);
        movePlayer = !movePlayer;
        moveLeft = !moveLeft;
        player.GetComponent<Animator>().Play("idle");
        player.GetComponent<SpriteRenderer>().flipX = false;
        dialogueText.text = dialogue7;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue8;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue9;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue10;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue11;
        yield return new WaitForSeconds(3.5f);
        yield return StartCoroutine(activateSteve());
    }

    IEnumerator activateSteve()
    {
        steve.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = Color.gray;
        dialogueText.text = dialogue12;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = defaultTextColor;
        dialogueText.text = dialogue13;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue14;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = Color.gray;
        dialogueText.text = dialogue15;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = defaultTextColor;
        dialogueText.text = dialogue16;
        yield return new WaitForSeconds(3.5f);
        devil.GetComponent<SpriteRenderer>().flipX = true;
        dialogueText.text = dialogue17;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = Color.gray;
        dialogueText.text = dialogue18;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = defaultTextColor;
        dialogueText.text = dialogue19;
        yield return new WaitForSeconds(3.5f);
        dialogueText.color = Color.gray;
        dialogueText.text = dialogue20;
        yield return new WaitForSeconds(0.5f);
        steve.GetComponent<Animator>().Play("steveopen");
        yield return new WaitForSeconds(0.5f);
        moveBumper = true;
        dialogueText.text = "";
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(triggerLastDialogues());
    }

    IEnumerator triggerLastDialogues()
    {
        yield return new WaitForSeconds(1.0f);
        dialogueText.color = defaultTextColor;
        dialogueText.text = dialogue21;
        yield return new WaitForSeconds(3.5f);
        devil.GetComponent<SpriteRenderer>().flipX = false;
        dialogueText.text = dialogue22;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue23;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue24;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue25;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue26;
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = dialogue27;
        yield return new WaitForSeconds(3.5f);
        fadeToBlack = true;
    }

    void triggerMoveBumper()
    {
        if (bumper.transform.position.y < 15f)
        {
            steve.transform.position += (Vector3.up);
            bumper.transform.position += (Vector3.up);
        }
        else
        {
            moveBumper = false;
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
            SceneManager.LoadScene("Ending 6a", LoadSceneMode.Single);
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
