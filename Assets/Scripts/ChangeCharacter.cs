using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCharacter : MonoBehaviour
{
    public enum Animal
    {
        Dog, Cat
    }

    public Animal startingAnimal;

    public Transform dogBack;

    public bool allowMovingThroughEachCharacter = false;

    public float catHopHeight = 1f;
    public float catHopSpeed = 1.5f;
    public float hopOnDistance = 5;
    public float hopOffDistance = 2.5f;

    public CinemachineVirtualCamera virtualCamera;

    PlayerController cat;
    PlayerController dog;
    [HideInInspector]public PlayerController currentAnimal;

    bool switching = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();
        CatJumpSpot[] catJumpSpots = GameObject.FindObjectsOfType<CatJumpSpot>();

        foreach (PlayerController pc in playerControllers)
        {
            if (pc.animal == PlayerController.Animal.Dog)
                dog = pc;
            
            else if (pc.animal == PlayerController.Animal.Cat)
                cat = pc;
        }

        foreach (CatJumpSpot cJS in catJumpSpots)
        {
            cJS.cat = cat;
        }

        if (allowMovingThroughEachCharacter)
            Physics.IgnoreCollision(cat.cc, dog.cc);

        if (startingAnimal == Animal.Dog)
        {
            currentAnimal = dog;
            cat.transform.position = dogBack.position;
            cat.transform.parent = dogBack;
            cat.transform.rotation = dog.transform.rotation;
        }

        else
        {
            currentAnimal = cat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PartnerControl();
    }

    void PartnerControl()
    {
        virtualCamera.m_Follow = currentAnimal == dog ? dog.transform : cat.transform;
        if (!switching)
        {
            dog.enabled = currentAnimal == dog ? true : false;
            cat.enabled = currentAnimal == dog ? false : true;
            cat.GetComponent<CharacterController>().enabled = currentAnimal == dog ? false : true;

            if (Input.GetButtonDown("Switch Character") && Vector3.Distance(cat.transform.position, dog.transform.position) < hopOnDistance && currentAnimal.cc.isGrounded && !currentAnimal.coroutinePause)
            {
                RaycastHit hit;
                Vector3 startRay = currentAnimal == dog ? dog.transform.position : cat.transform.position;
                Vector3 endRay = currentAnimal == cat ? dog.transform.position : cat.transform.position;
                Vector3 direction = startRay - endRay;

                if (currentAnimal == dog && !Physics.Raycast(currentAnimal.transform.position, currentAnimal.transform.forward, out hit, hopOffDistance))
                    StartCoroutine(CatHopOff());

                else if (currentAnimal == cat && !Physics.Linecast(startRay, endRay, out hit))
                    StartCoroutine(CatHopOn());

                else
                    return;

                currentAnimal = currentAnimal == dog ? cat : dog;
            }
        }
    }

    IEnumerator CatHopOn()
    {
        dog.enabled = false; 
        cat.enabled = false;
        switching = true;
        cat.transform.LookAt(new Vector3 (dogBack.position.x, cat.transform.position.y, dogBack.position.z));

        Vector3 startPos = cat.transform.position;
        Vector3 endPos = dogBack.position;
        float parabolaTimeline = 0;

        while (parabolaTimeline < 1)
        {
            cat.transform.position = MathParabola.Parabola(startPos, endPos, catHopHeight, parabolaTimeline);
            parabolaTimeline += Time.deltaTime * catHopSpeed;
            Debug.Log(parabolaTimeline);
            yield return new WaitForFixedUpdate();
        }
        cat.transform.position = dogBack.position;
        cat.transform.parent = dogBack;
        cat.transform.rotation = dog.transform.rotation;
        dog.enabled = true;
        switching = false;
        yield return null;
    }

    IEnumerator CatHopOff()
    {
        Vector3 startPos = cat.transform.position;
        Vector3 endPos = dog.transform.position + dog.transform.forward * hopOffDistance;
        endPos.y = dog.transform.position.y;
        float parabolaTimeline = 0;
        dog.enabled = false; 
        cat.enabled = false;
        switching = true;
        cat.transform.parent = null;

        while (parabolaTimeline < 1)
        {
            cat.transform.position = MathParabola.Parabola(startPos, endPos, catHopHeight, parabolaTimeline);
            parabolaTimeline += Time.deltaTime * catHopSpeed;
            yield return new WaitForFixedUpdate();
        }
        
        cat.enabled = true;
        switching = false;

        yield return null;
    }
}
