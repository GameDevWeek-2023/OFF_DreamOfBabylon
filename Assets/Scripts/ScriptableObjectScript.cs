using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newProgressHolder", menuName = "ProgessHolder")]
public class ScriptableObjectScript : ScriptableObject
{
    public int level = 1;
    public int checkPointInLevel = 0;
    public bool newGame = true;
    public int dialogIndex = 0;
}
