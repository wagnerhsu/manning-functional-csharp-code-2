// 定义两个简单的函数
using LanguageExt;

Func<int, int> addOne = x => x + 1;
Func<int, int> multiplyByTwo = x => x * 2;

// 使用 Compose 方法将两个函数组合起来
var addOneThenMultiplyByTwo = addOne.Compose(multiplyByTwo);

// 调用组合后的函数并输出结果
int result = addOneThenMultiplyByTwo(5);
Console.WriteLine(result); // 输出 12