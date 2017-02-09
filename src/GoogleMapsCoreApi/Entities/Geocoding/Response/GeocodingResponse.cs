﻿using GoogleMapsCoreApi.Entities.Common;
using GoogleMapsCoreApi.Entities.Geocoding.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GoogleMapsCoreApi.Entities.Geocoding.Response
{
    [DataContract]
    public class GeocodingResponse : IResponseFor<GeocodingRequest>
    {
        [DataMember(Name = "status")]
        internal string StatusStr
        {
            get
            {
                return Status.ToString();
            }
            set
            {
                Status = (Status)Enum.Parse(typeof(Status), value);
            }
        }

        public Status Status { get; set; }

        [DataMember(Name = "results")]
        public IEnumerable<Result> Results { get; set; }

        public override string ToString()
        {
            return string.Format("GeocodingResponse - Status: {0}, Results count: {1}", Status, Results != null ? Results.Count() : 0);
        }
    }
}
