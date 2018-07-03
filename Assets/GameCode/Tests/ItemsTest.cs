using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemsTest : MonoBehaviour
{
    private List<GameObject> _inventory = new List<GameObject>();
    private List<GameObject> _combatants = new List<GameObject>();

    private void Start()
    {
        CreateItems();
        CreateCombatants();
        StartCoroutine(SimulateBattle());
    }

    private void OnEnable()
    {
        this.AddObserver(OnItemEquipped, Equipment.EquipMessage);
        this.AddObserver(OnItemUnequipped, Equipment.UnequipMessage);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnItemEquipped, Equipment.EquipMessage);
        this.RemoveObserver(OnItemUnequipped, Equipment.UnequipMessage);
    }

    private void OnItemEquipped(object sender, object args)
    {
        var eq = sender as Equipment;
        var item = args as Equippable;
        _inventory.Remove(item.gameObject);
        Debug.Log($"{eq.name} equipped {item.name}");
    }

    private void OnItemUnequipped(object sender, object args)
    {
        var eq = sender as Equipment;
        var item = args as Equippable;
        _inventory.Add(item.gameObject);
        Debug.Log($"{eq.name} un-equipped {item.name}");
    }

    private GameObject CreateItem(string title, StatTypes type, int modifier)
    {
        var item = new GameObject(title);
        var attribute = item.AddComponent<StatModifierAttribute>();
        attribute.Stat = type;
        attribute.Modifier = modifier;
        return item;
    }

    private GameObject CreateConumableItem(string title, StatTypes type, int modifier)
    {
        var item = CreateItem(title, type, modifier);
        item.AddComponent<Consumable>();
        return item;
    }

    private GameObject CreateEquippableItem(string title, StatTypes type, int modifier, EquipmentSlots slot)
    {
        var item = CreateItem(title, type, modifier);
        var equip = item.AddComponent<Equippable>();
        equip.DefaultSlot = slot;
        return item;
    }

    private GameObject CreateHero()
    {
        var actor = CreateActor("Hero");
        actor.AddComponent<Equipment>();
        return actor;
    }

    private GameObject CreateActor(string title)
    {
        var actor = new GameObject(title);
        var stats = actor.AddComponent<Stats>();
        stats[StatTypes.HP] = stats[StatTypes.MHP] = Random.Range(500, 1000);
        stats[StatTypes.ATK] = Random.Range(30, 50);
        stats[StatTypes.DEF] = Random.Range(30, 50);
        return actor;
    }

    private void CreateItems()
    {
        _inventory.Add(CreateConumableItem("Health Potion", StatTypes.HP, 300));
        _inventory.Add(CreateConumableItem("Bomb", StatTypes.HP, -150));
        _inventory.Add(CreateEquippableItem("Sword", StatTypes.ATK, 10, EquipmentSlots.Primary));
        _inventory.Add(CreateEquippableItem("Broad Sword", StatTypes.ATK, 15, (EquipmentSlots.Primary | EquipmentSlots.Secondary)));
        _inventory.Add(CreateEquippableItem("Shield", StatTypes.DEF, 10, EquipmentSlots.Secondary));
    }

    private void CreateCombatants()
    {
        _combatants.Add(CreateHero());
        _combatants.Add(CreateActor("Monster"));
    }

    private IEnumerator SimulateBattle()
    {
        while (VictoryCheck() == false)
        {
            LogCombatants();
            HeroTurn();
            EnemyTurn();
            yield return new WaitForSeconds(1);
        }
        LogCombatants();
        Debug.Log("Battle Completed");
    }

    private void HeroTurn()
    {
        int rnd = Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                Attack(_combatants[0], _combatants[1]);
                break;
            default:
                UseInventory();
                break;
        }
    }

    private void EnemyTurn()
    {
        Attack(_combatants[1], _combatants[0]);
    }

    private void Attack(GameObject attacker, GameObject defender)
    {
        var attackerStats = attacker.GetComponent<Stats>();
        var defenderStats = defender.GetComponent<Stats>();
        var damage = Mathf.FloorToInt((attackerStats[StatTypes.ATK] * 4 - defenderStats[StatTypes.DEF] * 2) * Random.Range(0.9f, 1.1f));
        defenderStats[StatTypes.HP] -= damage;
        Debug.Log($"{attacker.name} hits {defender.name} for {damage} damage!");
    }

    private void UseInventory()
    {
        int rnd = Random.Range(0, _inventory.Count);

        var item = _inventory[rnd];
        if (item.GetComponent<Consumable>() != null)
            ConsumeItem(item);
        else
            EquipItem(item);
    }

    private void ConsumeItem(GameObject item)
    {
        _inventory.Remove(item);
        var attribute = item.GetComponent<StatModifierAttribute>();
        if (attribute.Modifier > 0)
        {
            item.GetComponent<Consumable>().Consume(_combatants[0]);
            Debug.Log("Ah... a potion!");
        }
        else
        {
            item.GetComponent<Consumable>().Consume(_combatants[1]);
            Debug.Log("Take this you stupid monster!");
        }
    }

    private void EquipItem(GameObject item)
    {
        Debug.Log("Perhaps this will help...");
        var toEquip = item.GetComponent<Equippable>();
        var equipment = _combatants[0].GetComponent<Equipment>();
        equipment.Equip(toEquip, toEquip.DefaultSlot);
    }

    private bool VictoryCheck()
    {
        for (int i = 0; i < 2; i++)
        {
            var stats = _combatants[i].GetComponent<Stats>();
            if (stats[StatTypes.HP] <= 0) return true;
        }
        return false;
    }

    private void LogCombatants()
    {
        Debug.Log("============");
        for (int i = 0; i < 2; i++) LogToConsole(_combatants[i]);
        Debug.Log("============");
    }

    private void LogToConsole(GameObject actor)
    {
        var stats = actor.GetComponent<Stats>();
        Debug.Log($"Name:{actor.name} HP:{stats[StatTypes.HP]}/{stats[StatTypes.MHP]} ATK:{stats[StatTypes.ATK]} DEF:{stats[StatTypes.DEF]}");
    }
}