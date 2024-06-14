using UnityEngine;

public class NPCService : IService
{
    #region Const
    private const string Conductor = "Conductor";
    private const string OldDemon = "OldDemon";
    private const string NPC1 = "NPC1";
    private const string NPC2 = "NPC2";
    private const string Trader = "Trader";
    private const string StationManager = "StationManager";
    private const string StationBoss = "Policeman";
    private const string Carpenter = "Carpenter";
    private const string Pawnbroker = "Pawnbroker";
    #endregion

    #region Prefabs
    private NPC _conductorPrefab;
    private NPC _oldDemonPrefab;
    private NPC _npc1Prefab;
    private NPC _npc2Prefab;
    private NPC _traderPrefab;
    private NPC _stationManagerPrefab;
    private NPC _stationBossPrefab;
    private NPC _carpenterPrefab;
    private NPC _pawnbrokerPrefab;
    #endregion

    public void InitializeService()
    {
        _conductorPrefab = Resources.Load<NPC>("NPC/" + Conductor);
        _oldDemonPrefab = Resources.Load<NPC>("NPC/" + OldDemon);
        _npc1Prefab = Resources.Load<NPC>("NPC/" + NPC1);
        _npc2Prefab = Resources.Load<NPC>("NPC/" + NPC2);
        _traderPrefab = Resources.Load<NPC>("NPC/" + Trader);
        _stationManagerPrefab = Resources.Load<NPC>("NPC/" + StationManager);
        _stationBossPrefab = Resources.Load<NPC>("NPC/" + StationBoss);
        _carpenterPrefab = Resources.Load<NPC>("NPC/" + Carpenter);
        _pawnbrokerPrefab = Resources.Load<NPC>("NPC/" + Pawnbroker);
    }

    public NPC GetNPCPrefab(string npcId)
    {
        switch (npcId)
        {
            case "conductor":
                return _conductorPrefab;
            case "old_demon":
                return _oldDemonPrefab;
            case "npc_1":
                return _npc1Prefab;
            case "npc_2":
                return _npc2Prefab;
            case "trader":
                return _traderPrefab;
            case "station_manager":
                return _stationManagerPrefab;
            case "station_boss":
                return _stationBossPrefab;
            case "carpenter":
                return _carpenterPrefab;
            case "pawnbroker":
                return _pawnbrokerPrefab;
        }
        throw new System.InvalidCastException("NPC with id " + npcId + "doesn't exists!");
    }
}
