using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : IQuest
{
    public string Name { get; private set; }
    public int FriendShip { get; private set; }
    public Sprite SDCharacterIcon { get; private set; }
    public Quest_My[] questArray { get; private set; }
    public Npc(string name, int friendShip)
    {
        Name = name;
        questArray = new Quest_My[10];
        FriendShip = friendShip;
        SDCharacterIcon = Resources.Load<Sprite>($"NPC/OriginPlayable/{name}");
        /*Debug.Log(SDCharacterIcon.name);*/
    }

    public Quest_My Decline(Quest_My quest)
    {
        quest.QuestDecline();
        return quest;
    }

    public Quest_My Continue(Quest_My quest)
    {
        quest.QuestContinue();
        return quest;
    }

    public Quest_My Complete(Quest_My quest)
    {
        quest.QuestComplete();
        return quest;
    }
}
