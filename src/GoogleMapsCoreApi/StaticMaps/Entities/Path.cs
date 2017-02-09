using GoogleMapsCoreApi.Entities.Common;
using System.Collections.Generic;

namespace GoogleMapsCoreApi.StaticMaps.Entities
{
    public class Path
    {
        public PathStyle Style { get; set; }

        public IList<ILocationString> Locations { get; set; }
    }
}
