using System.Text.Json;
using System.Text.Json.Serialization;
using Namotion.Reflection;
using NJsonSchema;
using NJsonSchema.Generation;

/// <inheritdoc />
public class UmbracoJsonSchemaGenerator : JsonSchemaGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UmbracoJsonSchemaGenerator" /> class.
    /// </summary>
    public UmbracoJsonSchemaGenerator()
        : base(new SystemTextJsonSchemaGeneratorSettings()
        {
            AlwaysAllowAdditionalObjectProperties = true,
            FlattenInheritanceHierarchy = true,
            IgnoreObsoleteProperties = true,
            ReflectionService = new UmbracoSystemTextJsonReflectionService(),
            SerializerOptions = new JsonSerializerOptions()
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                WriteIndented = true,
            },
        })
    { }

    /// <inheritdoc />
    private class UmbracoSystemTextJsonReflectionService : SystemTextJsonReflectionService
    {
        /// <inheritdoc />
        public override void GenerateProperties(JsonSchema schema, ContextualType contextualType, SystemTextJsonSchemaGeneratorSettings settings, JsonSchemaGenerator schemaGenerator, JsonSchemaResolver schemaResolver)
        {
            base.GenerateProperties(schema, contextualType, settings, schemaGenerator, schemaResolver);

            // Remove read-only properties
            foreach (ContextualPropertyInfo property in contextualType.Properties)
            {
                if (property.CanWrite is false)
                {
                    string propertyName = GetPropertyName(property, settings);

                    schema.Properties.Remove(propertyName);
                }
            }
        }
    }
}
