using System.Diagnostics;
using LaYumba.Functional;

namespace UnitDemo
{
    public static class Instrumentation
    {
        public static T Time<T>(string op, Func<T> f)
        {
            var sw = new Stopwatch();
            sw.Start();

            T t = f();

            sw.Stop();
            Console.WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
            return t;
        }

        public static void Time(string op, Action act)=> Time(op, act.ToFunc());
    }
}