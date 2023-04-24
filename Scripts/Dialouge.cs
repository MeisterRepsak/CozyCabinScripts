using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Dialouge : MonoBehaviour
{
    [TextArea(0,6)]
    public string[] introduction;
    public string[] tellToChopWood;
    public string[] NextTime;
    public string[] helpWithWood;
    public string[] gatheringAreaUnlocked;

    AudioSource source;

    public AudioClip[] clips;
    int count;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public IEnumerator GoThroughDialouge(string[] dialouge)
    {
        GameObject.Find("Canvas").transform.Find("DialougeUI").gameObject.SetActive(true);
        TextMeshProUGUI txt_dialouge = GameObject.Find("Canvas").transform.Find("DialougeUI").GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        for (int i = 0; i < dialouge.Length; i++)
        {
            string text = string.Empty;
            for (int k = 0; k < dialouge[i].Length; k++)
            {
                
                char n = dialouge[i][k];
                text += n;
                source.PlayOneShot(clips[count]);
                count++;
                if (count == clips.Length - 1)
                    count = 0;
                yield return new WaitForSeconds(0.04f);
                txt_dialouge.text = text;
                if (FindObjectOfType<InputManager>().doGoInCabin)
                {
                    FindObjectOfType<InputManager>().doGoInCabin = false;
                    txt_dialouge.text = dialouge[i];
                    break;
                }
                
            }
            yield return new WaitUntil(() => FindObjectOfType<InputManager>().doGoInCabin);
            FindObjectOfType<InputManager>().doGoInCabin = false;
        }

        if(dialouge == helpWithWood)
        {
            GetComponent<AIChopTrees>().goChop = true;
        }

        GameObject.Find("Canvas").transform.Find("DialougeUI").gameObject.SetActive(false);
        GetComponent<Companion>().hasIntroduced = true;
    }
}
