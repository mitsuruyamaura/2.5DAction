using System.Collections.Generic;

[System.Serializable]
public class UserData {
    public long money;
    public int walkCount;
    public int stageCount;
    public int floorCount;
    public int challengeCount;
    public List<int> clearedStageNoList = new();
    public int maxFloorCount;

    public int lifePotionCount;
    public int cleansingPotionCount;
    public int exploreCount;

}