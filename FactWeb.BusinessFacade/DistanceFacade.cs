using FactWeb.BusinessLayer;
using FactWeb.Infrastructure.GeoCoding;
using FactWeb.Model;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessFacade
{
    public class DistanceFacade
    {
        private readonly Container container;

        public DistanceFacade(Container container)
        {
            this.container = container;
        }

        public void SetDistanceForInspectors()
        {
            var distanceManager = this.container.GetInstance<DistanceManager>();
            var dict = new Dictionary<string, Tuple<double, double>>();

            var records = distanceManager.GetInspectorsWithoutDistance();

            if (records.Count == 0)
            {
                return;
            }

            foreach (var address in records)
            {
                Tuple<double, double> userLatLong = null;
                Tuple<double, double> siteLatLong = null;

                if (dict.ContainsKey(address.UserAddress))
                {
                    userLatLong = dict[address.UserAddress];
                }
                else
                {
                    userLatLong = GeoCodingHelper.GetLongLat(address.UserAddress);
                    dict.Add(address.UserAddress, userLatLong);
                }

                if (dict.ContainsKey(address.SiteAddress))
                {
                    siteLatLong = dict[address.SiteAddress];
                }
                else
                {
                    siteLatLong = GeoCodingHelper.GetLongLat(address.SiteAddress);
                    dict.Add(address.SiteAddress, siteLatLong);
                }

                if (userLatLong == null || siteLatLong == null) continue;

                var distance = GeoCodingHelper.CalculateDistance(userLatLong.Item1, userLatLong.Item2, siteLatLong.Item1, siteLatLong.Item2, 'M');

                distanceManager.Add(new Distance
                {
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    DistanceInMiles = distance,
                    SiteAddressId = address.SiteAddressId,
                    UserAddressId = address.UserAddressId
                });
            }
        }
    }
}
