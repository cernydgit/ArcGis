using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using FluentAssertions;
using System.Security.Cryptography.X509Certificates;

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
            List<Feature> features = queryResult.ToList();

            features.Should().NotBeNull();
            features.Should().NotBeEmpty();
        }
    }
}