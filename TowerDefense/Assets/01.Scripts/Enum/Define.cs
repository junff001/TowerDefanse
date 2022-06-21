using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }

    public enum ActType
    {
        Enemy,
        Wait
    }

    public enum PlaceTileType
    {
        Place,
        Road,
    }

    public enum TileType
    {
        None,
        Road,
        Place,
        Tower,
        Obstacle,
        Tunnel,
        Water,

        Road_Tower,
        Place_Tower,
    }

    [System.Flags]
    public enum MonsterType // 몹 추가될 때마다 추가해줘
    {
        None        = 0,
        Normal      = 1 << 0,
        Shadow      = 1 << 1,
        Alchemist   = 1 << 2,
        Armor       = 1 << 3,
        Guardian    = 1 << 4,
        Witch       = 1 << 5,
        Fly         = 1 << 6,
    }

    public enum SpeciesType // 종족
    {
        None,
        Goblin,
        Golem
    }

    public enum PropertyType
    {
        NONE,
        FIRE,
        WATER,
        LIGHTNING,
        SOIL,
        WIND,
        GRASS
    }

    public enum GameMode
    {
        DEFENSE,
        OFFENSE
    }

    public enum BuffType
    {
        BUFF,
        DEBUFF
    }
}
