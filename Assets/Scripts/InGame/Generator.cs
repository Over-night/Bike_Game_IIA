using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Generator : MonoBehaviour
{
    [SerializeField] VideoPlayer Video;
    [SerializeField] GameObject item;
    [SerializeField] GameObject actGuideline;
    [SerializeField] float itemTerm;
    [SerializeField] float actGateTerm;

    private float itemGenTime;
    private float actGateGenTime;
    private float endGenerateTime;

    // Start is called before the first frame update
    void Start()
    {
        itemGenTime = 30f;
        actGateGenTime = 15f;
        endGenerateTime = (float)Video.length - 25f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Video.time <= endGenerateTime)
        {
            if(itemGenTime < Video.time)
            {
                itemGenTime += itemTerm;
                Vector3 genLocate = gameObject.transform.position;
                genLocate.x = Random.Range(-1.5f, 1.5f);
                Instantiate(item, genLocate, gameObject.transform.rotation);
            }
            if(actGateGenTime < Video.time)
            {
                actGateGenTime += actGateTerm;
                Vector3 genLocate = gameObject.transform.position;
                genLocate.y = -5f;
                Instantiate(actGuideline, genLocate, gameObject.transform.rotation);
            }
        }
    }
}
