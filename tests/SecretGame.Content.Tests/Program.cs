using System;
using System.IO;
using System.Linq;
using SecretGame.Content;

var root = args.Length == 1 ? args[0] : Path.Combine(Directory.GetCurrentDirectory(), "content");
var catalog = new ContentLoader().Load(root);
var validator = new ContentValidator();
var tests = new (string Name, Action Run)[]
{
    ("representative catalog validates", () => Assert(validator.Validate(catalog).IsValid, string.Join("; ", validator.Validate(catalog).Errors))),
    ("catalog has nine abilities", () => Assert(catalog.Abilities.Count == 9, "Ability count changed.")),
    ("catalog has three reactions", () => Assert(catalog.Reactions.Count == 3, "Reaction count changed.")),
    ("nine abilities reuse at most eight motion archetypes", () => Assert(catalog.Abilities.Select(item => item.MotionArchetype).Distinct().Count() <= 8, "Motion vocabulary exceeded budget.")),
    ("gear taxonomy has exactly four indices", () => Assert(Enum.GetValues<GearSlotIndex>().Length == 4, "Gear slot count changed.")),
    ("weapon schema cannot author sets", WeaponCannotAuthorSets),
    ("four gear plus weapon loadout validates", ValidLoadout),
    ("duplicate gear slot is rejected", DuplicateSlotRejected),
    ("two different two-piece bonuses combine", TwoPlusTwoExists)
};

var failures = 0;
foreach (var test in tests)
{
    try { test.Run(); Console.WriteLine($"PASS {test.Name}"); }
    catch (Exception error) { failures++; Console.Error.WriteLine($"FAIL {test.Name}: {error.Message}"); }
}
Console.WriteLine($"{tests.Length - failures}/{tests.Length} content tests passed");
return failures == 0 ? 0 : 1;

void WeaponCannotAuthorSets()
{
    Assert(typeof(WeaponDefinition).GetProperty("SetId") is null, "WeaponDefinition exposes SetId.");
    Throws<ContentValidationException>(() => ContentLoader.EnsureWeaponJsonHasNoSetId("[{\"setId\":\"illegal\"}]"));
}

void ValidLoadout()
{
    var equipped = catalog.Gear.Where(item => item.SetId is not null).ToArray();
    var errors = validator.ValidateLoadout(equipped, catalog.Weapons.Single());
    Assert(errors.Count == 0, string.Join("; ", errors));
}

void DuplicateSlotRejected()
{
    var equipped = catalog.Gear.Where(item => item.SetId is not null).Take(3)
        .Append(catalog.Gear.First()).ToArray();
    Assert(validator.ValidateLoadout(equipped, catalog.Weapons.Single()).Count > 0,
        "Duplicate slot was accepted.");
}

void TwoPlusTwoExists()
{
    var report = validator.Validate(catalog);
    Assert(report.TwoPlusTwoPairings.SequenceEqual(new[] { "anchor+relay" }),
        "Expected anchor+relay pairing.");
}

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

static void Throws<T>(Action action) where T : Exception
{
    try { action(); }
    catch (T) { return; }
    throw new InvalidOperationException($"Expected {typeof(T).Name}.");
}
