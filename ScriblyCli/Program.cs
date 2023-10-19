static void ProcessUserInput(char input)
{

}


Console.WriteLine("Type something (press Esc to exist):");

while (true)
{
    var keyInfo = Console.ReadKey(intercept: true);

    if (keyInfo.Key == ConsoleKey.Escape)
        break;

    ProcessUserInput(keyInfo.KeyChar);
}

Console.WriteLine("Fin.");
