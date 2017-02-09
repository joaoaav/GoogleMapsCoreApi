using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleMapsCoreApi.Entities.DistanceMatrix.Response
{
    [DataContract(Name = "row")]
    public class Row
    {
        /// <summary>
		/// element[] The information about each origin-destination pairing is returned in an element entry
		/// </summary>
		[DataMember(Name = "elements")]
        public IEnumerable<Element> Elements { get; set; }
    }
}
