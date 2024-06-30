using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class StatusValue {
    public StatusType statusType;
    public int value;

    public StatusValue(StatusType statusType, int value) {
        this.statusType = statusType;
        this.value = value;
    }

    public override string ToString() {
        return $"{statusType}: {value}";
    }
}

[System.Serializable]
public class CharaStatus
{
    public int level;
    public long exp;

    public List<StatusValue> statusValueList = new();

    public CharaStatus() {
        level = 1;
        exp = 0 ;

        statusValueList = new() {
            new(StatusType.Strength, 0),
            new(StatusType.Intelligence, 0),
            new(StatusType.Dexterity, 0),
            new(StatusType.Charm, 0),
            new(StatusType.Luck, 0)
        };
    }

    public CharaStatus(List<int> values) {
        level = 1;
        exp = 0;

        for (int i = 0; i < values.Count; i++) {
            // i ‚ª—LŒø‚È StatusType ‚Å‚ ‚é‚±‚Æ‚ðŠm”F
            if (Enum.IsDefined(typeof(StatusType), i)) {
                statusValueList.Add(new((StatusType)i, values[i]));
            } else {
                throw new ArgumentOutOfRangeException($"Invalid StatusType index: {i}");
            }
        }
    }

    public StatusValue GetStatusValue(StatusType searchStatusType) {
        return statusValueList.FirstOrDefault(data => data.statusType == searchStatusType);
    }


    public void SetStatusValue(StatusType statusType, int value) {
        StatusValue statusValue = GetStatusValue(statusType);
        if (statusValue != null) {
            statusValue.value = value;
        } else {
            statusValueList.Add(new(statusType, value));
        }
    }
}