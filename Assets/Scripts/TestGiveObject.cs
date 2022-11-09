using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGiveObject : MonoBehaviour
{
    ChangeCharacter changeCharacter;
    PlayerController currentAnimal;
    bool held;

    // Start is called before the first frame update
    void Start()
    {
        changeCharacter = GameObject.FindObjectOfType<ChangeCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        currentAnimal = changeCharacter.currentAnimal;

        if (Input.GetKeyDown(KeyCode.K) && !held)
        {
            currentAnimal.HoldObject(gameObject, Vector3.zero);
            held = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && held)
        {
            held = false;
            currentAnimal.RemoveObject(1);
        }
    }
}
