public enum ControlState 
{
    Defocused,    // No interactable is focused on 
    UnitsSelected, // A unit is currently focused on
    StructureSelected,
}

public enum UnitType
{
    Alien, //Friendly
    Human, //Human
}

public enum UnitAttackType
{
    Melee, 
    Ranged,
}

public enum InteractableType
{
    Unit,
    Structure,
}

public enum HumanAIControlState
{
    Idle,
    Attack,
}

public enum StructureType
{
    Obstacle,
    Hive,
    Townhouse,
}