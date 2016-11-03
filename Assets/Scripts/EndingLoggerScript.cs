using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndingLoggerScript : MonoBehaviour {

	public Image end1;
	public Image end2;
	public Image end3;
	public Image end4;
	public Image end5;
	public Image end6;
	public string header = "Ending:\n";
	public Text endingCounter;

	// Use this for initialization
	void Start () {
		int count = 0;
		end1.enabled = false;
		end2.enabled = false;
		end3.enabled = false;
		end4.enabled = false;
		end5.enabled = false;
		end6.enabled = false;
		if (GlobalPlayerScript.Instance.hasEndings[0])
		{
			end1.enabled = true;
			count += 1;
		}
		if (GlobalPlayerScript.Instance.hasEndings[1])
		{
			end2.enabled = true;
			count += 1;
		}
		if (GlobalPlayerScript.Instance.hasEndings[2])
		{
			end3.enabled = true;
			count += 1;
		}
		if (GlobalPlayerScript.Instance.hasEndings[3])
		{
			end4.enabled = true;
			count += 1;
		}
		if (GlobalPlayerScript.Instance.hasEndings[4])
		{
			end5.enabled = true;
			count += 1;
		}
		if (GlobalPlayerScript.Instance.hasEndings[5])
		{
			end6.enabled = true;
			count += 1;
		}
		endingCounter.text = (header + "" + count + "/6");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
