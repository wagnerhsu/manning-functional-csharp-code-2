using LanguageExt;

Option<int> some =42;
Option<int> none = Option<int>.None;

some.Map(x => x + 1).Match(Some: (x)=> Console.WriteLine(x), None:()=>Console.WriteLine("None")); // Some(43)
none.Map(x => x + 1).Match(Some: (x)=> Console.WriteLine(x), None:()=>Console.WriteLine("None")); // None