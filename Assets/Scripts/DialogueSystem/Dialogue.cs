using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
  public List<string> toSay;

  public float velocityOfDisplay;

  public float timeBetweenSentences;

  public bool hideAtEnd;

  private string displayText;

  private float actualTime;

  private float timePerLetter;

  private int actualLetter;

  private int actualSentence;

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public string getDisplayText()
  {
    return displayText;
  }

  public void Reset()
  {
    displayText = "";
    timePerLetter = 1.0f / velocityOfDisplay;
    actualTime = timePerLetter;
    actualLetter = 0;
    actualSentence = 0;
  }

  public void Tick()
  {
    actualTime += Time.deltaTime;

    if (
      actualTime > timePerLetter &&
      actualSentence < toSay.Count &&
      actualLetter < toSay[actualSentence].Length
      )
    {
      actualTime -= timePerLetter;

      displayText += toSay[actualSentence][actualLetter];

      ++actualLetter;
    }

    if (
      actualLetter == toSay[actualSentence].Length &&
      actualTime > timeBetweenSentences
      )
    {
      if (actualSentence < toSay.Count - 1)
      {
        actualTime -= timeBetweenSentences;

        ++actualSentence;

        actualLetter = 0;

        displayText = "";

      }
    }
  }

  public bool HasFinished()
  {
    return actualSentence == toSay.Count - 1 && actualLetter == toSay[actualSentence].Length;
  }
  public bool WaitingForClear()
  {
    return HasFinished() && actualTime > timeBetweenSentences;
  }
}

