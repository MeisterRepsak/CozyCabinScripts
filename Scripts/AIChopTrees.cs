using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIChopTrees : MonoBehaviour
{
    NavMeshAgent ai;
    public bool goChop;
    bool isChopping;
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goChop && !isChopping)
        {
            StartCoroutine(ChopTrees());
        }
    }
    IEnumerator ChopTrees()
    {
        isChopping = true;
        int count = GameManager.instance.trees.Count;
        for (int i = 0; i < GameManager.instance.trees.Count; i++)
        {
            

            GameObject temp = GameManager.instance.trees[i];

            if (GameManager.instance.trees.Count == 0)
                yield break;

            ai.SetDestination(GameManager.instance.trees[i].transform.position);
            Vector3 target = GameManager.instance.trees[i].transform.position;
            yield return new WaitUntil(() => Vector3.Distance(target, transform.position) < 0.5f);

            if (count != GameManager.instance.trees.Count)
            {
                StartCoroutine(ChopTrees());
                yield break;
            }

                StartCoroutine(GameManager.instance.trees[i].GetComponent<TreeGone>().ChopTree());

        }
    }
}
