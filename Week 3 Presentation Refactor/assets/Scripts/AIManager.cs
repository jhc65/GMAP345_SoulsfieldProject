using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public List<GameObject> mushObjPool;
    public List<Transform> aiNodePool;
    public List<MushController> mushScriptPool;

    public List<GameObject> inactivePool;

    //move to resources
    [SerializeField]
    private GameObject mushPrefab;
    
    //number of Ai 
    private int aiNum;

    // Use this for initialization
    void Start()
    {   //get # of spawn Nodes
        aiNum = transform.childCount;
        //get every spawn path node transform
        for (int i = 0; i < aiNum; i++)
        {
            aiNodePool.Add(transform.GetChild(i));
        }
        //spawn a enemy mush at every node
        for (int i = 0; i < aiNum; i++)
        {
            mushObjPool.Add(Instantiate(mushPrefab, aiNodePool[i].transform.position, Quaternion.identity));
            mushScriptPool.Add(mushObjPool[i].GetComponent<MushController>());
            //log info on each Mushroom
            mushScriptPool[i].spawnPos = aiNodePool[i].transform.position;
            mushScriptPool[i].aiManager = GetComponent<AIManager>();
        }
    }

    //we should change this to a custom event
    void Update()
    {
        if (inactivePool.Count == aiNum)
        {
            Debug.Log("inative pool");
            for (int i = 0; i < aiNum; i ++)
            {
                //set AI back to starting Position
                mushScriptPool[i].transform.position = mushScriptPool[i].spawnPos;
                //set AI back to active
                mushObjPool[i].SetActive(true);
                // Clear Inactive pool
                
            }
            inactivePool = new List<GameObject>();


        }

    }
}
