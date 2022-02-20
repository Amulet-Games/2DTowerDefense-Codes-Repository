using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Data/Resources/Resource Type")]
    public class ResourceSO : ScriptableObject
    {
        public string nameString;
        public string colorHex;
        public p_ResourceTypeEnum resourceTypeEnum;
    }
}