using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingFiveAnimationScript : MonoBehaviour {
    public Text dialogueText;

    private GameObject devil;
    private GameObject player;
    private GameObject soul;
    private GameObject whiteFade;

    private bool isDevilFadingIn;
    private bool hasPlayerLanded;
    private bool isPlayerMovingToCaptives;
    private bool isSceneEnded;

    private Color defaultTextColor;

    private const string dialogue1 = "I see...";
    private const string dialogue2 = "You didn't hoard any diamonds...";
    private const string dialogue3 = "...and you came down here as fast as you could.";
    private const string dialogue4 = "You're no ordinary miner, are you?";
    private const string dialogue5 = "You came down to find THEM.";
    private const string dialogue6 = "Well. Too. Bad.";
    private const string dialogue7 = "Their souls were mine the moment that accident\n killed them all.";
    private const string dialogue8 = "And so was yours, the moment you fell down here.";
    private const string dialogue9 = "What are you going to do?";
    private const string dialogue10 = "Fight me for their freedom?!";
    private const string dialogue11 = "Damn right I am.";
    private const string dialogue12 = "...oh my";
    private const string dialogue13 = "Heh...";
    private const string dialogue14 = "KEHAHAHAHAHAHAHAHAHA!";
    private const string dialogue15 = "A demon slayer!";
    private const string dialogue16 = "In this day and age!";
    private const string dialogue17 = "Looks like I was mistaken about you, redhead...";
    private const string dialogue18 = "I've not had a dance like this in CENTURIES!";

    // Use this for initialization
    void Start () {
        devil = GameObject.Find("Devil");
        soul = GameObject.Find("Soul");
        soul.SetActive(false);
        isDevilFadingIn = false;
        hasPlayerLanded = false;
        isPlayerMovingToCaptives = false;
        defaultTextColor = dialogueText.color;
        isSceneEnded = false;
        whiteFade = GameObject.Find("WhiteFade/Panel");
    }
	
	// Update is called once per frame
	void Update () {
        fadeDevil();
        movePlayerToCaptives();
        fadeSceneOut();
	}

    private void fadeSceneOut()
    {
        if (isSceneEnded)
        {
            Color color = whiteFade.GetComponent<Image>().color;
            if (color.a < 1f)
            { // fading in
                color.a += 0.02f;
                whiteFade.GetComponent<Image>().color = color;
            } else
            { // finish fading in white screen
                isSceneEnded = false;
                SceneManager.LoadScene("Ending 5a", LoadSceneMode.Single);
            }
        }
    }

    private void playDialogues(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    private void playHeroDialogue(string dialogue)
    {
        dialogueText.color = Color.gray;
        dialogueText.text = dialogue;
    }

    private void fadeDevil()
    {
        if (isDevilFadingIn)
        {
            Color color = devil.GetComponent<SpriteRenderer>().color;
            if (color.a >= 1f)
            { // done fade in
                isDevilFadingIn = false;
                StartCoroutine(triggerDialoguePartOne());
            } else
            { // fading in
                color.a += 0.025f;
                devil.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!hasPlayerLanded && coll.gameObject.tag == "Player")
        {
            hasPlayerLanded = true;
            player = coll.gameObject;
            player.GetComponent<Animator>().Play("idle");
            StartCoroutine(devilBlockPlayerAnim());
        }
    }

    IEnumerator triggerDialoguePartOne()
    {
        playDialogues(dialogue1);
        yield return new WaitForSeconds(1.5f);
        playDialogues(dialogue2);
        yield return new WaitForSeconds(3f);
        playDialogues(dialogue3);
        yield return new WaitForSeconds(3f);
        playDialogues(dialogue4);
        yield return new WaitForSeconds(1.5f);
        player.GetComponent<Animator>().Play("helmetremove");
        yield return new WaitForSeconds(2.5f);
        playDialogues(dialogue5);
        StartCoroutine(devilLookAtCaptivesAndBack());

    }

    IEnumerator triggerDialoguePartTwo()
    {
        yield return new WaitForSeconds(0.5f);
        playDialogues(dialogue6);
        yield return new WaitForSeconds(3f);
        playDialogues(dialogue7);
        yield return new WaitForSeconds(3f);
        playDialogues(dialogue8);
        StartCoroutine(devilShowSoulAnim());
    }

    IEnumerator triggerDialoguePartThree()
    {
        yield return new WaitForSeconds(2f);
        playDialogues(dialogue10);
        devil.GetComponent<Animator>().Play("devil_cackle");
        yield return new WaitForSeconds(2.7f);
        playHeroDialogue(dialogue11);
        player.GetComponent<Animator>().Play("herotalking");
        yield return new WaitForSeconds(3.5f);
        player.GetComponent<Animator>().Play("hero_sword");
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(transformHeroSword());
    }

    IEnumerator triggerDialoguePartFour()
    {
        yield return new WaitForSeconds(2f);
        playDialogues(dialogue15);
        yield return new WaitForSeconds(2f);
        playDialogues(dialogue16);
        yield return new WaitForSeconds(2f);
        playDialogues(dialogue17);
        yield return new WaitForSeconds(2f);
        devil.GetComponent<Animator>().Play("devilsteal");
        playDialogues(dialogue18);
        yield return new WaitForSeconds(3f);
        isSceneEnded = true;
    }

    IEnumerator transformHeroSword()
    {
        GameObject whiteFlash = GameObject.Find("WhiteFlash");
        // Flash effect
        whiteFlash.GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().Play("herotransformed");
        whiteFlash.GetComponent<Canvas>().enabled = false;
        yield return new WaitForSeconds(1f);
        devil.GetComponent<Animator>().Play("devil_upset");
        dialogueText.color = defaultTextColor;
        playDialogues(dialogue12);
        yield return new WaitForSeconds(2f);
        devil.GetComponent<Animator>().Play("Idle");
        playDialogues(dialogue13);
        yield return new WaitForSeconds(2f);
        devil.GetComponent<Animator>().Play("devil_cackle");
        playDialogues(dialogue14);
        StartCoroutine(triggerDialoguePartFour());
    }

    IEnumerator devilBlockPlayerAnim()
    {
        yield return new WaitForSeconds(1f);
        askPlayerToMoveToCaptives();
        yield return new WaitForSeconds(0.1f);
        isDevilFadingIn = true;
        yield return new WaitForSeconds(0.5f);
        isPlayerMovingToCaptives = false; // stop moving
        player.GetComponent<Animator>().SetBool("isRunning", false);
    }

    IEnumerator devilLookAtCaptivesAndBack()
    {
        yield return new WaitForSeconds(0.3f);
        devil.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(1.7f);
        devil.GetComponent<SpriteRenderer>().flipX = false;
        StartCoroutine(triggerDialoguePartTwo());
    }

    IEnumerator devilShowSoulAnim()
    {
        soul.SetActive(true);
        devil.GetComponent<Animator>().Play("devilsteal");
        yield return new WaitForSeconds(3f);
        devil.GetComponent<Animator>().Play("Idle");
        soul.SetActive(false);
        playDialogues(dialogue9);
        StartCoroutine(triggerDialoguePartThree());
    }

    void askPlayerToMoveToCaptives()
    {
        isPlayerMovingToCaptives = true;
    }

    void movePlayerToCaptives()
    {
        if (isPlayerMovingToCaptives)
        {
            player.GetComponent<Animator>().SetBool("isRunning", true);
            Vector3 playerPos = player.transform.position;
            playerPos.x += 0.05f;
            player.transform.position = playerPos;
        }
    }



}
