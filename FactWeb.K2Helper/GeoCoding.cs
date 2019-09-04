using System;
using System.Net;
using System.Runtime.Serialization.Json;

namespace FactWeb.K2Helper
{
    public static class GeoCoding
    {
        const string geocodeRestUrl = "http://dev.virtualearth.net/REST/v1/Locations/[ADDRESS]?key=AvhhGfRYMKCpRw1N-8zsdZEcoozpDBELkfUiZq7N56wGiVkKG8BxbHbfXSLNyT0g";

        /// <summary>
        /// This function takes two address and calculate distance between them
        /// </summary>
        /// <param name="address1String">first address </param>
        /// <param name="address2String">second address </param>
        /// <param name="unit">unit of measure. could be KM, M and N- nutral mile</param>
        /// <returns></returns>
        public static double GetDistanceByLocations(string address1String, string address2String, char unit)
        {
            double latitude1 = 0.0f;
            double longitude1 = 0.0f;
            double latitude2 = 0.0f;
            double longitude2 = 0.0f;
            double distance = 0.0f;

            string location1Request = geocodeRestUrl.Replace("[ADDRESS]", Uri.EscapeDataString(address1String));
            Response location1Response = MakeRequest(location1Request);
            ProcessResponse(location1Response, out latitude1, out longitude1);

            string location2Request = geocodeRestUrl.Replace("[ADDRESS]", Uri.EscapeDataString(address2String));
            Response location2Response = MakeRequest(location2Request);
            ProcessResponse(location2Response, out latitude2, out longitude2);

            distance = CalculateDistance(latitude1, longitude1, latitude2, longitude2, unit);

            return distance;
        }

        /// <summary>
        /// Processes the request object
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private static Response MakeRequest(string requestUrl)
        {
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                    var jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    var objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    var jsonResponse = objResponse as Response;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        /// <summary>
        /// Extract latitude and longitude from response object
        /// </summary>
        /// <param name="locationsResponse"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        private static void ProcessResponse(Response locationsResponse, out double latitude, out double longitude)
        {

            var locNum = locationsResponse.ResourceSets[0].Resources.Length;
            string strLatitude, strLongitude = string.Empty;
            latitude = 0.0f;
            longitude = 0.0f;

            for (var i = 0; i < locNum; i++)
            {
                var location = (Location)locationsResponse.ResourceSets[0].Resources[i];

                var geocodePointNum = location.GeocodePoints.Length;
                for (var j = 0; j < geocodePointNum; j++)
                {
                    strLatitude = location.GeocodePoints[j].Coordinates[0].ToString();
                    strLongitude = location.GeocodePoints[j].Coordinates[1].ToString();

                    latitude = double.Parse(strLatitude);
                    longitude = double.Parse(strLongitude);
                    // we are getting first location found, there could be multiple locations
                    break;

                }
            }
        }

        /// <summary>
        /// Get longitude and latitude of two locations and calculate their distance in given unit
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit = 'M')
        {
            var rlat1 = Math.PI * lat1 / 180;
            var rlat2 = Math.PI * lat2 / 180;
            var theta = lon1 - lon2;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}
