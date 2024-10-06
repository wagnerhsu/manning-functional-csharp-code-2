using LanguageExt;

namespace OptionDemo
{
    public record Person(string FirstName, string LastName)
    {        
        public Option<string> MiddleName { get; set; }
    }
}