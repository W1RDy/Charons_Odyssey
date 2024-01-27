using System.Collections.Generic;
using System.Reflection;

public class TalkableFinderOnLevel
{
    private Dictionary<string, ITalkable> _talkableDictionary;

    public void AddTalkable(ITalkable talkable)
    {
        if (_talkableDictionary == null) _talkableDictionary = new Dictionary<string, ITalkable>();

        if (!_talkableDictionary.ContainsKey(talkable.GetTalkableIndex())) _talkableDictionary.Add(talkable.GetTalkableIndex(), talkable);
    }

    public ITalkable GetTalkable(string talkableId)
    {
        talkableId = talkableId.Replace(" ", "");
        if (!_talkableDictionary.ContainsKey(talkableId)) throw new System.ArgumentNullException("Talkable with index " + talkableId + " doesn't exist!");
        return _talkableDictionary[talkableId];
    }
}
