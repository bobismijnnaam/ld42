using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour {
    public abstract string getDescription();

    public abstract void activate();
}
