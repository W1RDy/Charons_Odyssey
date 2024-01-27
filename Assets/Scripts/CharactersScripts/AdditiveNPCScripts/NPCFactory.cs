using UnityEngine;
using UnityEngine.Networking.Types;
using Zenject;

public class NPCFactory : INPCFactory
{
    private NPCService _npcService;
    private ICustomInstantiator _instantiator;
    private TalkableFinderOnLevel _talkableFinder;

    public NPCFactory(NPCService npcService, ICustomInstantiator instantiator, TalkableFinderOnLevel talkableFinder)
    {
        _npcService = npcService;
        _instantiator = instantiator;
        _talkableFinder = talkableFinder;
    }

    public NPC Create(string npcId, string dialogId, Direction direction, bool isAvailable, Vector2 position)
    {
        return Create(npcId, dialogId, direction, isAvailable, position, null);
    }

    public NPC Create(string npcId, string dialogId, Direction direction, bool isAvailable, Vector2 position, Transform parent)
    {
        var npcObj = _npcService.GetNPCPrefab(npcId);
        var npc = _instantiator.InstantiatePrefabForComponent<NPC>(npcObj, position, parent);
        npc.InitializeNPC(direction, dialogId, isAvailable);
        _talkableFinder.AddTalkable(npc);
        return npc;
    }
}
