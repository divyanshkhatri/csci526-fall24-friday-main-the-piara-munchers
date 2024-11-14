using System.Collections.Generic;

[System.Serializable]
public class EventData
{
    public string session_id;
    public int level_index;
    public float completion_time;
    public int hit_count;
    public int lives_remaining;
    public int flips;
    public bool finish;
}
