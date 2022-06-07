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

        Road_Tower,
        Place_Tower,
    }

    public enum MonsterType // 몹 추가될 때마다 추가해줘
    {
        None,
        Goblin,
        ShadowGoblin,
        GuardianGoblin,
        AlchemistGoblin,
        ArmorGoblin,
        FlyingGoblin,
    }

    public enum MagicType // 마법 추가될 때 마다 추가해줘
    {
        None, 
        GoblinBarrel,
        FireBall, // 뭔가 미래가 보여서 넣어봤어..
    }

    public enum PropertyType
    {
        NONE,
        FIRE,
        WATER,
        LIGHTNING,
        LIGHT,
        DARKNESS
    }

    public enum GameMode
    {
        DEFENSE,
        OFFENSE
    }
}
