using System.Runtime.Serialization;

namespace GoogleMapsCoreApi.Entities.DistanceMatrix.Request
{
    [DataContract]
    public enum DistanceMatrixRestrictions
    {
        [EnumMember]
        tolls,
        [EnumMember]
        highways,
        [EnumMember]
        ferries,
        [EnumMember]
        indoor,
    }
}
