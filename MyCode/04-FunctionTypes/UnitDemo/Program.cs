using LaYumba.Functional;
using UnitDemo;

Action action = () => Console.WriteLine("Hello, World!");
Func<ValueTuple> func = action.ToFunc();
func();


Func<string> greet = () => "Hello, World!";
Instrumentation.Time("greet", greet);
Action greetAction = () => Console.WriteLine("Hello, World!");
Instrumentation.Time("greet with action", greetAction.ToFunc());