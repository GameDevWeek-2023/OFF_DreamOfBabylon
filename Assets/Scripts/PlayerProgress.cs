using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProgress
{
    public int level;
    public float audioVolume;
    public int checkPointInLevel;
    public int dialogIndex;
    public PlayerProgress(int lvl, int checkPointInLevel = 0, float audioVolume = 1, int dialogIndex = 0)
    {
        level = lvl;
        this.checkPointInLevel = checkPointInLevel;
        this.audioVolume = audioVolume;
        this.dialogIndex = dialogIndex;
    }

    //public void LoadPlayerProgress()
}
