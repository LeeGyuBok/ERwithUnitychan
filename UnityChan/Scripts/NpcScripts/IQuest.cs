using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest
{
    public Quest_My Decline(Quest_My quest);
    public Quest_My Continue(Quest_My quest);
    public Quest_My Complete(Quest_My quest);
}
