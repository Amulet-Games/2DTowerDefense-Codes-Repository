using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class runtimeHQ : Build_Generator_Base
    {
        protected override void AddResource()
        {
            _buildingManager._resourceManager.AddResource_Wood(1);
        }
    }
}