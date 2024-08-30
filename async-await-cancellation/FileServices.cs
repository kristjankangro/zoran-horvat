class FileServices
{
    public async Task GenerateTextFile(CancellationToken token, string path, int lengthMb)
    {
        try
        {
            using var writer = new StreamWriter(path);

            var buffer = new char[128];
            int buffersCount = lengthMb * 1024 * 1024 / buffer.Length;

            for (int i = 0; i < buffersCount; i++)
            {
                FillRandomText(buffer);
                await writer.WriteAsync(buffer, token);
            }
        }
        catch (OperationCanceledException)
        {
            File.Delete(path);
            throw;
        }
    }

    private void FillRandomText(char[] buffer)
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz\n";
        Random random = new();
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = alphabet[random.Next(alphabet.Length)];
        }
    }
}

