string path = "text.txt";
FileServices files = new();
CancellationTokenSource tokenSource = new();

try
{
    Task fileWriter = files.GenerateTextFile(tokenSource.Token, path, 700);
    while (!fileWriter.IsCompleted)
    {
        Console.Write("Enter some text: ");
        string? text = Console.ReadLine();
        if (string.IsNullOrEmpty(text))
        {
            if (text is null) tokenSource.Cancel();
            break;
        }
        Console.WriteLine($"You entered: {text}");
    }
    Console.WriteLine("Ended interaction.");
    await fileWriter;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Sorry to see you going early.");
}
finally
{
    File.Delete(path);
}
