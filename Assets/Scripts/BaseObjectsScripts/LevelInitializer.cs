using UnityEngine;
using Zenject;

public class LevelInitializer : MonoBehaviour 
{
    #region Markers
    [SerializeField] private EnemyMarker[] _enemyMarkers;
    [SerializeField] private Transform _playerMarker;
    [SerializeField] private NPCMarker[] _npcMarkers;
    [SerializeField] private NPCGroupMarker[] _npcGroupMarkers;
    [SerializeField] private ItemMarker[] _itemMarkers;
    #endregion

    #region Factories
    private IEnemyFactory _enemyFactory;
    private INPCFactory _npcFactory;
    private INPCGroupFactory _npcGroupFactory;
    private IItemFactory _itemFactory;
    #endregion

    [Inject]
    private void Construct(IEnemyFactory enemyFactory, INPCFactory npcFactory, INPCGroupFactory npcGroupFactory, IItemFactory itemFactory)
    {
        _enemyFactory = enemyFactory;
        _npcFactory = npcFactory;
        _npcGroupFactory = npcGroupFactory;
        _itemFactory = itemFactory;
    }

    public void SpawnAllLevelObjects()
    {
        SpawnNPCs();
        SpawnNPCsGroup();
        SpawnEnemies();
        SpawnItems();
    }

    private void SpawnEnemies()
    {
        foreach (var enemyMarker in _enemyMarkers) 
        {
            _enemyFactory.Create(enemyMarker.EnemyType, enemyMarker.Direction, enemyMarker.IsAvailable, enemyMarker.transform.position);
        }
    }

    private void SpawnNPCs()
    {
        foreach (var npcMarker in _npcMarkers)
        {
            _npcFactory.Create(npcMarker.NameId, npcMarker.DialogId, npcMarker.Direction, npcMarker.IsAvailable, npcMarker.transform.position);
        }
    }

    private void SpawnNPCsGroup()
    {
        foreach (var npcGroupMarker in _npcGroupMarkers)
        {
            _npcGroupFactory.Create(npcGroupMarker.NpcsId, npcGroupMarker.DialogId, npcGroupMarker.IsAvailable, npcGroupMarker.transform.position);
        }
    }

    private void SpawnItems()
    {
        foreach (var itemMarker in _itemMarkers)
        {
            _itemFactory.Create(itemMarker.ItemType, itemMarker.transform.position);
        }
    }
}
