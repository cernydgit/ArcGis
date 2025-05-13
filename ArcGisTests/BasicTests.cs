using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using FluentAssertions;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace ArcGisTests
{
    public class BasicTests
    {
        private string _statesUrl = "https://services.arcgis.com/jIL9msH9OI208GCb/arcgis/rest/services/USA_Daytime_Population_2016/FeatureServer/0";

        [Fact]
        public async Task QueryFeatureLayer()
        {
            ArcGISRuntimeEnvironment.ApiKey = "AAPTxy8BH1VEsoebNVZXo8HurEMzeuUawPxlfsqLe9tkCTCPP7xYcjZerHR-HQtBPihv-TqncKNuRxGAgA4lokP50as-2TcrK_FtUlK8ttRan-QbDZ8w7Zja7mZ83UlPR6cTeyLJ5Nd0uH1KgtLe6xBg-f7YVxQL4f8WL9YXiLwifUeCVPtiOsHjvTPsiZxWa7IUnSFQExHloRHkuvNKTgC3Di_aBfxWEO0loAPTD43KhPU.AT1_I7JIrk9I";

            // Create feature table using a URL.
            var featureTable = new ServiceFeatureTable(new Uri(_statesUrl));

            var stateName = "A";
            // Create a query parameters that will be used to Query the feature table.
            QueryParameters queryParams = new QueryParameters();

            // Trim whitespace on the state name to prevent broken queries.
            string formattedStateName = stateName.Trim().ToUpper();

            // Construct and assign the where clause that will be used to query the feature table.
            queryParams.WhereClause = "upper(STATE_NAME) LIKE '%" + formattedStateName + "%'";

            // Query the feature table.
            FeatureQueryResult queryResult = await featureTable.QueryFeaturesAsync(queryParams);

            // Cast the QueryResult to a List so the results can be interrogated.
            List<Esri.ArcGISRuntime.Data.Feature> features = queryResult.ToList();

            features.Should().NotBeNull();
            features.Should().NotBeEmpty();
        }

        [Fact]
        public async Task QueryFeatureLayer_REST()
        {
            // Set the API key for authorization.
            ArcGISRuntimeEnvironment.ApiKey = "AAPTxy8BH1VEsoebNVZXo8HurEMzeuUawPxlfsqLe9tkCTCPP7xYcjZerHR-HQtBPihv-TqncKNuRxGAgA4lokP50as-2TcrK_FtUlK8ttRan-QbDZ8w7Zja7mZ83UlPR6cTeyLJ5Nd0uH1KgtLe6xBg-f7YVxQL4f8WL9YXiLwifUeCVPtiOsHjvTPsiZxWa7IUnSFQExHloRHkuvNKTgC3Di_aBfxWEO0loAPTD43KhPU.AT1_I7JIrk9I";

            var stateName = "A";
            string formattedStateName = stateName.Trim().ToUpper();

            // Construct the query URL by appending /query to the layer URL.
            var queryUrl = $"{_statesUrl}/query";

            // Build query parameters following the ArcGIS REST API schema, including the token for authorization.
            var parameters = new Dictionary<string, string>
                {
                    { "where", "upper(STATE_NAME) LIKE '%" + formattedStateName + "%'" },
                    { "outFields", "*" },
                    { "f", "json" },
                    { "token", ArcGISRuntimeEnvironment.ApiKey } // added API key parameter for authorization
                };

            // Construct query string.
            var queryString = string.Join("&", parameters.Select(kvp =>
                $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

            var fullUri = $"{queryUrl}?{queryString}";

            using var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(fullUri);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response.
            using JsonDocument jsonDoc = JsonDocument.Parse(jsonResponse);
            JsonElement root = jsonDoc.RootElement;

            // Ensure the response contains a "features" property.
            root.TryGetProperty("features", out JsonElement featuresElement)
                .Should().BeTrue("JSON response should contain features property");

            // Validate that the features array is not empty.
            featuresElement.GetArrayLength().Should().BeGreaterThan(0, "There should be at least one feature in the result.");
        }
    }
}