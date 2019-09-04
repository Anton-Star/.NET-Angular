using System;
using System.Net;
using System.Runtime.Serialization.Json;

namespace FactWeb.Infrastructure.GeoCoding
{
    public static class GeoCodingHelper
    {
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
            
            var geocodeRestUrl = ConfigManager.GetConfigurationSetting(Constants.GeoCodeConstants.GeoCodeRestUrl);
            var geocodeApiKey = ConfigManager.GetConfigurationSetting(Constants.GeoCodeConstants.GeoCodeApiKey);

                       
            var location1Request = CreateRequest(address1String, geocodeRestUrl, geocodeApiKey);
            var location1Response = MakeRequest(location1Request);

            if (location1Response == null) return 0;

            ProcessResponse(location1Response, out latitude1, out longitude1);
                        
            var location2Request = CreateRequest(address2String, geocodeRestUrl, geocodeApiKey);
            var location2Response = MakeRequest(location2Request);

            if (location2Response == null) return 0;

            ProcessResponse(location2Response, out latitude2, out longitude2);

            distance = CalculateDistance(latitude1, longitude1, latitude2, longitude2, unit);

            return distance;
        }

        public static Tuple<double, double> GetLongLat(string address)
        {
            double latitude1 = 0.0f;
            double longitude1 = 0.0f;

            var geocodeRestUrl = ConfigManager.GetConfigurationSetting(Constants.GeoCodeConstants.GeoCodeRestUrl);
            var geocodeApiKey = ConfigManager.GetConfigurationSetting(Constants.GeoCodeConstants.GeoCodeApiKey);


            var location1Request = CreateRequest(address, geocodeRestUrl, geocodeApiKey);
            var location1Response = MakeRequest(location1Request);

            if (location1Response == null) return null;

            ProcessResponse(location1Response, out latitude1, out longitude1);

            return new Tuple<double, double>(latitude1, longitude1);
        }

        /// <summary>
        /// Create the request URL
        /// </summary>
        /// <param name="addressString"></param>
        /// <param name="apiUrl"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static string CreateRequest(string addressString, string apiUrl, string apiKey)
        {
            return apiUrl.Replace("[APIKEY]", apiKey).Replace("[ADDRESS]", System.Uri.EscapeDataString(addressString));
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
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).",response.StatusCode,response.StatusDescription));

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    Response jsonResponse = objResponse as Response;
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

            int locNum = locationsResponse.ResourceSets[0].Resources.Length;
            string strLatitude, strLongitude = string.Empty;
            latitude = 0.0f;
            longitude = 0.0f;

            for (int i = 0; i < locNum; i++)
            {
                Location location = (Location)locationsResponse.ResourceSets[0].Resources[i];

                int geocodePointNum = location.GeocodePoints.Length;
                for (int j = 0; j < geocodePointNum; j++)
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
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit = 'M')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
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
