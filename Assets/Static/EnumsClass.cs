using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnumsClass
    {
    }
    
    public enum p_ResourceTypeEnum
    {
        Wood,
        Stone,
        Gold
    }

    public enum p_GeneratorTypeEnum
    {
        Wood_Gen,
        Stone_Gen,
        Gold_Gen,
        HQ
    }

    public enum p_BuildingTypeEnum
    {
        HQ,
        Wood_Build,
        Stone_Build,
        Gold_Build,
        Tower_Build
    }

    public enum p_GhostTypeEnum
    {
        Arrow,
        Generator,
        Protector
    }

    public enum G_SceneTypeEnum
    {
        PersistentScene,
        MainMenuScene,
        EasyGameScene,
        NormalGameScene,
        HardGameScene
    }
}