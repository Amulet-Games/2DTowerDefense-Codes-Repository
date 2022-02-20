using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class ResourceCost
    {
        public ResourceSO referedResource;
        public int cost;

        public bool CheckIsEnoughResource()
        {
            switch (referedResource.resourceTypeEnum)
            {
                case p_ResourceTypeEnum.Wood:

                    if (cost > ResourceManager.singleton.cur_woodAmt)
                        return false;
                    else
                        return true;

                case p_ResourceTypeEnum.Stone:

                    if (cost > ResourceManager.singleton.cur_stoneAmt)
                        return false;
                    else
                        return true;

                case p_ResourceTypeEnum.Gold:

                    if (cost > ResourceManager.singleton.cur_goldAmt)
                        return false;
                    else
                        return true;

                default:
                    return false;
            }
        }

        public void DepleteResources()
        {
            switch (referedResource.resourceTypeEnum)
            {
                case p_ResourceTypeEnum.Wood:

                    ResourceManager.singleton.RemoveResource_Wood(cost);
                    break;

                case p_ResourceTypeEnum.Stone:

                    ResourceManager.singleton.RemoveResource_Stone(cost);
                    break;

                case p_ResourceTypeEnum.Gold:

                    ResourceManager.singleton.RemoveResource_Gold(cost);
                    break;
            }
        }

        public void ReturnPortionResources()
        {
            switch (referedResource.resourceTypeEnum)
            {
                case p_ResourceTypeEnum.Wood:

                    ResourceManager.singleton.AddResource_Wood(Mathf.FloorToInt(cost * .6f));
                    break;

                case p_ResourceTypeEnum.Stone:

                    ResourceManager.singleton.AddResource_Stone(Mathf.FloorToInt(cost * .6f));
                    break;

                case p_ResourceTypeEnum.Gold:

                    ResourceManager.singleton.AddResource_Gold(Mathf.FloorToInt(cost * .6f));
                    break;
            }
        }
    }
}