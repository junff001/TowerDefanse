using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,  // �ƹ��͵� �ƴ�. �׳� Sound enum�� ���� ���� ���� �߰�. (0, 1, '2' �̷��� 2��) 
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

    public enum MonsterType // �� �߰��� ������..
    {
        None,
        Goblin,
        ShadowGoblin,
        GuardianGoblin,
        AlchemistGoblin,
        ArmorGoblin,
        FlyingGoblin,
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
