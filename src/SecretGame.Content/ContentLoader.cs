using System.Text.Json;
using System.Text.Json.Serialization;

namespace SecretGame.Content;

public sealed class ContentLoader
{
    private static readonly JsonSerializerOptions Options = CreateOptions();

    public ContentCatalog Load(string root)
    {
        var abilitiesPath = Path.Combine(root, "abilities", "spike-abilities.json");
        var reactionsPath = Path.Combine(root, "reactions", "spike-reactions.json");
        var gearPath = Path.Combine(root, "gear", "cyborg-spike-gear.json");
        var weaponsPath = Path.Combine(root, "weapons", "cyborg-spike-weapon.json");

        foreach (var path in new[] { abilitiesPath, reactionsPath, gearPath, weaponsPath })
            EnsureIntegerNumbers(path);
        EnsureWeaponJsonHasNoSetId(File.ReadAllText(weaponsPath));
        return new ContentCatalog(
            Read<AbilityDefinition>(abilitiesPath),
            Read<ReactionDefinition>(reactionsPath),
            Read<GearDefinition>(gearPath),
            Read<WeaponDefinition>(weaponsPath));
    }

    private static T[] Read<T>(string path)
    {
        var value = JsonSerializer.Deserialize<T[]>(File.ReadAllText(path), Options);
        return value ?? throw new ContentValidationException($"Content file {path} deserialized to null.");
    }

    public static void EnsureWeaponJsonHasNoSetId(string json)
    {
        using var document = JsonDocument.Parse(json);
        foreach (var weapon in document.RootElement.EnumerateArray())
        {
            if (weapon.TryGetProperty("setId", out _))
                throw new ContentValidationException("Weapon JSON must not contain setId.");
        }
    }

    private static void EnsureIntegerNumbers(string path)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        Visit(document.RootElement, path);
    }

    private static void Visit(JsonElement element, string path)
    {
        if (element.ValueKind == JsonValueKind.Number && !element.TryGetInt64(out _))
            throw new ContentValidationException($"Content file {path} contains a non-integer number.");
        if (element.ValueKind == JsonValueKind.Array)
            foreach (var child in element.EnumerateArray()) Visit(child, path);
        if (element.ValueKind == JsonValueKind.Object)
            foreach (var property in element.EnumerateObject()) Visit(property.Value, path);
    }

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        return options;
    }
}
