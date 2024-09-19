using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestContent
{
    public string Title { get; protected set; }
    public string Reward { get; protected set; }
    public string Objective { get; protected set; }
    public int ObjectiveGoalCount { get; protected set; }

    public int PlayerCollectCount { get; set; } = 0;
    public string QuestStringContent { get; protected set; }
}
