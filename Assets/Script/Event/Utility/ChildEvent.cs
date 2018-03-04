using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildEvent {
    public GameObject childObject = null;
    public int childWeightChange = 0;
    public bool childNextTurnReset = false;
    public bool childHappenNext = false;
}
