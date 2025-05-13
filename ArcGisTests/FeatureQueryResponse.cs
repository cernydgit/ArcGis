using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArcGisTests
{
    public class FeatureQueryResponse
    {
        [JsonPropertyName("objectIdFieldName")]
        public string ObjectIdFieldName { get; set; }

        [JsonPropertyName("uniqueIdField")]
        public UniqueIdField UniqueIdField { get; set; }

        [JsonPropertyName("globalIdFieldName")]
        public string GlobalIdFieldName { get; set; }

        [JsonPropertyName("geometryProperties")]
        public GeometryProperties GeometryProperties { get; set; }

        [JsonPropertyName("geometryType")]
        public string GeometryType { get; set; }

        [JsonPropertyName("spatialReference")]
        public SpatialReference SpatialReference { get; set; }

        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }

        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }

    public class UniqueIdField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isSystemMaintained")]
        public bool IsSystemMaintained { get; set; }
    }

    public class GeometryProperties
    {
        [JsonPropertyName("shapeAreaFieldName")]
        public string ShapeAreaFieldName { get; set; }

        [JsonPropertyName("shapeLengthFieldName")]
        public string ShapeLengthFieldName { get; set; }

        [JsonPropertyName("units")]
        public string Units { get; set; }
    }

    public class SpatialReference
    {
        [JsonPropertyName("wkid")]
        public int Wkid { get; set; }

        [JsonPropertyName("latestWkid")]
        public int LatestWkid { get; set; }
    }

    public class Field
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("sqlType")]
        public string SqlType { get; set; }

        [JsonPropertyName("domain")]
        public object Domain { get; set; }

        [JsonPropertyName("defaultValue")]
        public object DefaultValue { get; set; }

        // "length" property is optional, so use nullable int
        [JsonPropertyName("length")]
        public int? Length { get; set; }
    }

    public class Feature
    {
        [JsonPropertyName("attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("rings")]
        public List<List<List<double>>> Rings { get; set; }
    }
}
