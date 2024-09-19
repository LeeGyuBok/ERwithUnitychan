using System.Collections;
using System.Collections.Generic;


public enum QuestStatus
{
    Complete,
    Continue,
    Decline
}


public class QuestContent_SO
{
    //제목, 보상, 목표아이템, 아이템 요구 개수
    public Quest_SO Contents { get; private set; }

    public Npc NPC{ get; private set; }
    
    public int PlayerCollectCount { get; set; } = 0;
    
    public QuestStatus Status {get; private set;} = QuestStatus.Decline;

    public QuestContent_SO(Quest_SO contents, Npc npc)
    {
        NPC = npc;
        Contents = contents;
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
