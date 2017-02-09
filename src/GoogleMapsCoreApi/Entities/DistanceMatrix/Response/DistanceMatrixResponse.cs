using GoogleMapsCoreApi.Entities.Common;
using GoogleMapsCoreApi.Entities.Directions.Response;
using GoogleMapsCoreApi.Entities.DistanceMatrix.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleMapsCoreApi.Entities.DistanceMatrix.Response
{
    [DataContract(Name = "DistanceMatrixResponse")]
    public class DistanceMatrixResponse : IResponseFor<DistanceMatrixRequest>
    {
        [DataMember(Name = "status")]
        public string StatusStr
        {
            get
            {
                return Status.ToString();
            }
            set
            {
                Status = (DirectionsStatusCodes)Enum.Parse(typeof(DirectionsStatusCodes), value);
            }
        }

        /// <summary>
        /// "status" contains metadata on the request. See Status Codes below.
        /// </summary>
        public DirectionsStatusCodes Status { get; set; }

        [DataMember(Name = "rows")]
        public IEnumerable<Row> Rows { get; set; }

        [DataMember(Name = "destination_addresses")]
        public IEnumerable<string> DestinationAddresses { get; set; }


        [DataMember(Name = "origin_addresses")]
        public IEnumerable<string> OriginAddresses { get; set; }
    }
}
