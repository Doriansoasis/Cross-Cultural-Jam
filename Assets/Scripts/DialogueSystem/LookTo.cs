using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTo : MonoBehaviour
{
  public GameObject lookToObject;

  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      //Vector3 delta = transform.position - lookToObject.transform.position;
      //delta.y = 0;
      //Quaternion q = new Quaternion();
      //q.SetLookRotation(delta, new Vector3(0, 1, 0));
      transform.rotation = lookToObject.transform.rotation;
    }
}
