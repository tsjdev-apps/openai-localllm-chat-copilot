using OpenAI;
using OpenAI.Chat;
using ShakespeareChat.Utils;
using Spectre.Console;
using System.ClientModel;
using System.Text;

// Show the header.
ConsoleHelper.ShowHeader();

// Select the host.
string host =
    ConsoleHelper.SelectFromOptions([Statics.OpenAIKey, Statics.LocalLLMKey]);

// Initialize the client.
ChatClient? client = null;

// Switch on the host.
switch (host)
{
    case Statics.OpenAIKey:

        // Get the OpenAI API key.
        string openAIApiKey =
            ConsoleHelper.GetString(Statics.OpenAIKeyPrompt);

        // Select the OpenAI model.
        string openAIModel =
            ConsoleHelper.SelectFromOptions(
                [Statics.GPT35TurboKey, Statics.GPT4Key,
                Statics.GPT4TurboKey, Statics.GPT4oKey]);

        // Initialize the client.
        client = new(openAIModel, new ApiKeyCredential(openAIApiKey));

        break;

    case Statics.LocalLLMKey:

        // Set variables
        string localApiKey = "ollama";
        Uri localEndpoint = new("http://localhost:11434/v1");

        // Get the local LLM name.
        string localModel =
            ConsoleHelper.GetString(Statics.LocalLLMNamePrompt);

        // Initialize the client.
        client =
            new(localModel,
                new ApiKeyCredential(localApiKey),
                new OpenAIClientOptions { Endpoint = localEndpoint });

        break;
}

// Check if the client is null.
if (client == null)
{
    return;
}

// Show the header.
ConsoleHelper.ShowHeader();

// Set the options.
ChatCompletionOptions options = new()
{
    MaxTokens = 1000,
    Temperature = 0.7f,
};

// Set the messages.
List<ChatMessage> messages =
    [
        new SystemChatMessage(Statics.SystemMessage),
        new UserChatMessage(Statics.UserMessage)
    ];

// Chat loop.
while (true)
{
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine("[green]Shakespeare:[/]");

    StringBuilder stringBuilder = new();

    // Complete the chat.
    AsyncResultCollection<StreamingChatCompletionUpdate> chatUpdates =
        client.CompleteChatStreamingAsync(messages, options);

    // Loop through the chat updates.
    await foreach (StreamingChatCompletionUpdate chatUpdate in chatUpdates)
    {
        foreach (ChatMessageContentPart contentPart in chatUpdate.ContentUpdate)
        {
            ConsoleHelper.WriteToConsole(contentPart.Text);
            stringBuilder.Append(contentPart.Text);
        }
    }

    ConsoleHelper.WriteToConsole(Environment.NewLine);
    messages.Add(new AssistantChatMessage(stringBuilder.ToString()));

    ConsoleHelper.WriteToConsole(
        $"{Environment.NewLine}[green]User:[/]{Environment.NewLine}");

    string? userMessage = Console.ReadLine();
    messages.Add(new UserChatMessage(userMessage));
}