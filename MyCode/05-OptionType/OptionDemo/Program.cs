using System.Text.Json;
using Dumpify;
using OptionDemo;

Person person = new("John", "Doe");
person.MiddleName = "Smith";
var options = new JsonSerializerOptions();
options.Converters.Add(new LangExtOptionConverter());
var json = JsonSerializer.Serialize(person, options);
json.Dump();


Person? newPerson = JsonSerializer.Deserialize<Person>(json, options);
newPerson?.Dump();