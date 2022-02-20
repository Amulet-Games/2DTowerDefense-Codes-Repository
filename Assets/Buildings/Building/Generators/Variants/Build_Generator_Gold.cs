using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Build_Generator_Gold : Build_Generator_Base
    {
        protected override void AddResource()
        {
            _buildingManager._resourceManager.AddResource_Gold(1);
        }
    }
}
