using LanguageExt;

namespace OptionTypeTests;

public class OptionTests
{
    [Fact]
    public void Smart_Constructor_Age_Test()
    {
        // 测试合法年龄值
        var result = Age.Create(25);
        var v = GetAge(result);
        Assert.True(v == 25);

        // 测试非法年龄值
        result = Age.Create(-100);
        v = GetAge(result);
        Assert.True(v == -1);
    }

    // 辅助方法，用于从 Option<Age> 中提取年龄值
    int GetAge(Option<Age> optionAge) => optionAge.Match((age) => age.Value, () => -1);
}