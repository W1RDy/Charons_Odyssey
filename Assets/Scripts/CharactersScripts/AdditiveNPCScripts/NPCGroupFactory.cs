using UnityEngine;
using Zenject;

public class NPCGroupFactory : INPCGroupFactory
{
    private ICustomInstantiator _instantiator;
    private INPCFactory _npcFactory;
    private NPCGroup _npcGroupPrefab;
    private TalkableFinderOnLevel _talkableFinder;

    public NPCGroupFactory(ICustomInstantiator instantiator, INPCFactory npcFactory, NPCGroup npcGroupPrefab, TalkableFinderOnLevel talkableFinder)
    {
        _instantiator = instantiator;
        _npcFactory = npcFactory;
        _npcGroupPrefab = npcGroupPrefab;
        _talkableFinder = talkableFinder;
    }

    public NPCGroup Create(string[] npcsID, string dialogId, bool isAvailable, Vector2 position)
    {
        var npcGroup = _instantiator.InstantiatePrefabForComponent<NPCGroup>(_npcGroupPrefab, position);
        npcGroup.InitializeGroup(CreateNPCs(npcsID, npcGroup), dialogId, isAvailable);
        _talkableFinder.AddTalkable(npcGroup);
        return npcGroup;
    }

    private NPC[] CreateNPCs(string[] npcsID, NPCGroup group)
    {
        var npcs = new NPC[npcsID.Length];
        var spawnPoints = group.GetNPCPoints();
        if (spawnPoints.Length != npcs.Length) throw new System.ArgumentException("NPC count in group doesn't equals group points count!");

        for (int i = 0; i < npcsID.Length; i++)
        {
            npcs[i] = _npcFactory.Create(npcsID[i], "", (Direction)i, true, spawnPoints[i].position, group.transform);
        }

        return npcs;
    }
}
