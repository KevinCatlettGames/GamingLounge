using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior; 

public class RandomizeCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject[] characterMeshes;
    [SerializeField] private int[] headIndexes;
    [SerializeField] private int[] clothIndexes;
    [SerializeField] private BehaviorGraphAgent graphAgent;
    public float waitTime = 1; 
    int maxNPCTime = 9;
    private int currentNPCTime = 0;
    private int alienToUse = 0; 
    private void Awake()
    {
        StartCoroutine(NewLook());
    }

    private IEnumerator NewLook()
    {
        for(int i = 0; i < characters.Length;i++) 
        {
            if(i != alienToUse)
                characters[i].SetActive(false);
        }
        
        characters[alienToUse].SetActive(true);
        graphAgent.BlackboardReference.SetVariableValue("Anim", characters[alienToUse].GetComponent<Animator>());
        
        characterMeshes[alienToUse].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[alienToUse]]
            .SetFloat("_Hue", currentNPCTime);
        characterMeshes[alienToUse].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[alienToUse]]
            .SetFloat("_Hue", currentNPCTime);
        yield return new WaitForSeconds(waitTime);

        currentNPCTime++;
        if (currentNPCTime >= maxNPCTime)
        {
            if (alienToUse == characters.Length-1)
            {
                alienToUse = 0;
            }
            else
            {
                alienToUse++;
                foreach (GameObject character in characters)
                {
                    character.SetActive(false);
                }
            }
            currentNPCTime = 0; 
        }
        
        StartCoroutine(NewLook());
    }
}