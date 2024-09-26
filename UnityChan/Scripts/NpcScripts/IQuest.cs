using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest
{
    public QuestContent_SO Decline(QuestContent_SO quest);
    public QuestContent_SO Continue(QuestContent_SO quest);
    public QuestContent_SO Complete(QuestContent_SO quest);
}
