using LanguageExt;

namespace OptionTypeTests;

public class UnitTest1
{
    [Fact]
    public void Smart_Constructor_Age_Test()
    {
        var result = Age.Create(25);
        var v = GetAge(result);
        Assert.True( v== 25);
       
        
    }
    int GetAge(Option<Age> optionAge) => optionAge.Match((age)=>age.Value,()=>-1);
}