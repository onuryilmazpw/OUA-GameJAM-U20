using UnityEngine;

public class GameValues : MonoBehaviour
{
    public Levels[] levels;
}


[System.Serializable]
public class Levels
{
    public int remainTourVal;
    public int codeWorkVal, drawWorkVal, pmWorkVal;
}