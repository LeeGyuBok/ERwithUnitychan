using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest_My
{
    public Npc NPC { get; private set; }
    
    public QuestContent QuestContents { get; private set; }
    public bool Goal { get; private set; } = false;

    public QuestStatus Status {get; private set;} = QuestStatus.Decline;
    public bool QuestEnd { get; private set; }  = false;

    public Quest_My(Npc npc, QuestContent questContents, bool goal, bool questEnd)
    {
        NPC = npc;
        QuestContents = questContents;
        Goal = goal;
        QuestEnd = questEnd;
    }
    
    public void QuestDecline()
    {
        Status = QuestStatus.Decline;
    }
    public void QuestComplete()
    {
        Status = QuestStatus.Complete;
    }
    public void QuestContinue()
    {
        Status = QuestStatus.Continue;
    }
}
