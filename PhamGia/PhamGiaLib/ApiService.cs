

using Newtonsoft.Json;
using System.Text.Json;

namespace PhamGia.PhamGiaLib
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<District>> GetDistrictsAsync()
        {
            var response = await _httpClient.GetAsync("api/province/district/01");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);

            return apiResponse.Results;
        }

        public async Task<List<Ward>> GetWardsAsync(string districtId)
        {
            var response = await _httpClient.GetAsync($"api/province/ward/{districtId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var wards = JsonConvert.DeserializeObject<ApiResponse2>(json);

            return wards.Results;
        }
        public class District
        {

            [JsonProperty("district_id")]
            public string DistrictId { get; set; }

            [JsonProperty("district_name")]
            public string DistrictName { get; set; }

            [JsonProperty("district_type")]
            public string DistrictType { get; set; }

            [JsonProperty("lat")]
            public double? Lat { get; set; } // Có thể null

            [JsonProperty("lng")]
            public double? Lng { get; set; } // Có thể null

            [JsonProperty("province_id")]
            public string ProvinceId { get; set; }
        }
        public class Ward
        {
            [JsonProperty("district_id")]
            public string DistrictId { get; set; }
            [JsonProperty("ward_id")]
            public string WardId { get; set; }
            [JsonProperty("ward_name")]
            public string WardName { get; set; }
            [JsonProperty("ward_type")]
            public string WardType { get; set; }
        }
        public class ApiResponse
        {
            [JsonProperty("results")]
            public List<District> Results { get; set; }
        }

        public class ApiResponse2
        {
            [JsonProperty("results")]
            public List<Ward> Results { get; set; }
        }
    }
}
