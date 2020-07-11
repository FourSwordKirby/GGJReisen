using UnityEngine;
using System.Collections;

//Enums for various player things
public enum CharacterExpression
{
    normal = 0,
    smile = 1,
    laugh = 2,
    think = 3,
    frown = 4,
    worry = 5
}

public enum Symbol
{
    Interested
}

public enum ItemDesignation
{
    WeatherCharm,
    Soda,
    Coin
}

public enum SoundType
{
    Menu,
    Environment,
    Item,
    Action
}

public enum ConsumableItemType
{
    GreenTeaBottle
}

public enum EquipableItemType
{
    BasicUmbrella
}

public enum BadgeType
{
    AllOutAttack,
    OffensePlus,
    DefensePlus,
    HealingPlus,
    SupportPlus
}

public enum KeyItemType
{
    MagnetGlove,
    RaffleTicket
}
