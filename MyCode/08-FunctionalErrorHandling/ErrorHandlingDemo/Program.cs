using LanguageExt;
using static LanguageExt.Prelude;
var text = Console.ReadLine();
var result = SafeWriteLine(text);

result.Match(
    Right: _ => Console.WriteLine("Message written successfully."),
    Left: ex => Console.WriteLine($"An error occurred: {ex.Message}")
);
static Either<Exception, Unit> SafeWriteLine(string message)
{
    try
    {
        Console.WriteLine(message);
        return Right(unit);
    }
    catch (Exception ex)
    {
        return Left(ex);
    }
}