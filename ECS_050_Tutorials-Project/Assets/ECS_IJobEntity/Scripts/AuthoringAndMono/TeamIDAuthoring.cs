using Unity.Entities;
using UnityEngine;

namespace TMG.IJE
{
    public class TeamIDAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private int _teamID;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var newTeamID = new TeamID { Value = _teamID };
            if (!dstManager.AddSharedComponentData(entity, newTeamID))
            {
                Debug.LogError("Error: Unable to add shared component", gameObject);
            }
        }
    }
}