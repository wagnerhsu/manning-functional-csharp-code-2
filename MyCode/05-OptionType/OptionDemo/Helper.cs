using LanguageExt;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using static LanguageExt.Prelude;

namespace OptionDemo;

public class LangExtMapConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;

        if (typeToConvert.GetGenericTypeDefinition() == typeof(Map<,>))
            return true;

        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type keyType = typeToConvert.GetGenericArguments()[0];
        Type valueType = typeToConvert.GetGenericArguments()[1];

        return (JsonConverter)Activator.CreateInstance(
                typeof(MapConverter<,>).MakeGenericType(
                new Type[] { keyType, valueType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null)!;
    }

    private class MapConverter<K, V> : JsonConverter<Map<K, V>>
    {
        public MapConverter(JsonSerializerOptions _) { }
        public override Map<K, V> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            JsonSerializer.Deserialize<IEnumerable<(K, V)>>(ref reader, options).ToMap();

        public override void Write(Utf8JsonWriter writer, Map<K, V> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToArray(), options);
        }
    }
}

public class LangExtSetConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;

        if (typeToConvert.GetGenericTypeDefinition() == typeof(Set<>))
            return true;

        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type type = typeToConvert.GetGenericArguments()[0];

        return (JsonConverter)Activator.CreateInstance(
                typeof(SetConverter<>).MakeGenericType(
                new Type[] { type }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null)!;
    }

    private class SetConverter<T> : JsonConverter<Set<T>>
    {
        public SetConverter(JsonSerializerOptions _) { }

        public override Set<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            toSet(JsonSerializer.Deserialize<IEnumerable<T>>(ref reader, options));

        public override void Write(Utf8JsonWriter writer, Set<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToArray(), options);
        }
    }
}

public class LangExtOptionConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;

        if (typeToConvert.GetGenericTypeDefinition() == typeof(Option<>))
            return true;

        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type type = typeToConvert.GetGenericArguments()[0];

        return (JsonConverter)Activator.CreateInstance(
                typeof(OptionConverter<>).MakeGenericType(
                new Type[] { type }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null)!;
    }

    private class OptionConverter<T> : JsonConverter<Option<T>>
    {
        public OptionConverter(JsonSerializerOptions _) { }

        public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            JsonSerializer.Deserialize<IEnumerable<T>>(ref reader, options) switch
            {
                IEnumerable<T> ienum => ienum.ToSeq() switch
                {
                    Seq<T> { Count: 1 } => Some<T>(ienum.First()),
                    _ => None
                },

                _ => None,
            };

        public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToArray(), options);
        }
    }
}
/*
public class SerializationIssueWithSystemTextJson
{
    readonly Map<int, string> map = Map((1, "One"),
                    (2, "Two"));

    readonly Set<string> set = Set("One", "Two");

    readonly Option<int> optional = Some(1);

    readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        IncludeFields = true, //serialization returns empty without this
    };

    /// <summary>
    /// Exception encountered during de-serialization of
    /// LangExt types.
    /// 
    /// System.NotSupportedException: 'The collection 
    /// type 'LanguageExt.Option`1[System.Int32]' is 
    /// abstract, an interface, or is read only, and 
    /// could not be instantiated and populated
    /// </summary>
    [Fact]
    public void BreaksWithoutConverter()
    {
        var mapS = JsonSerializer.Serialize(map, options);
        var mapD = Try(() => JsonSerializer.Deserialize<Map<int, string>>(mapS, options));

        var setS = JsonSerializer.Serialize(set, options);
        var setD = Try(() => JsonSerializer.Deserialize<Set<string>>(setS, options));

        var optionalS = JsonSerializer.Serialize(optional, options);
        var optionalD = Try(() => JsonSerializer.Deserialize<Option<int>>(optionalS, options));

        Assert.False(mapD.IsSucc());
        Assert.False(setD.IsSucc());
        Assert.False(optionalD.IsSucc());
    }

    [Fact]
    public void WorksWithConverter()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new LangExtMapConverter(),
               new LangExtSetConverter(),
               new LangExtOptionConverter() }
        };

        var mapS = JsonSerializer.Serialize(map, options);
        var mapD = JsonSerializer.Deserialize<Map<int, string>>(mapS, options);

        var setS = JsonSerializer.Serialize(set, options);
        var setD = JsonSerializer.Deserialize<Set<string>>(setS, options);

        var optionalS = JsonSerializer.Serialize(optional, options);
        var optionalD = JsonSerializer.Deserialize<Option<int>>(optionalS, options);

        Assert.True(map == mapD);
        Assert.True(set == setD);
        Assert.True(optional == optionalD);
    }

    [Fact]
    public void MapSerializationUsingNewtonSoftWorksCorrectly()
    {
        var mapS = Newtonsoft.Json.JsonConvert.SerializeObject(map);
        var mapD = Newtonsoft.Json.JsonConvert.DeserializeObject<Map<int, string>>(mapS);

        var setS = Newtonsoft.Json.JsonConvert.SerializeObject(set);
        var setD = Newtonsoft.Json.JsonConvert.DeserializeObject<Set<string>>(setS);

        Assert.True(map == mapD);
        Assert.True(set == setD);
    }
*/