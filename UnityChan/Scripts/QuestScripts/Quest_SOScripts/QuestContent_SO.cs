using System.Collections;
using System.Collections.Generic;

public class QuestContent_SO
{
    //제목, 보상, 목표아이템, 아이템 요구 개수
    public Quest_SO contents;

    public string QuestDetail;

    public QuestContent_SO(Quest_SO _contents, string detail)
    {
        contents = _contents;
        QuestDetail = detail;
    }

}
