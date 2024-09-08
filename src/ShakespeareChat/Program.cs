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
    ConsoleHelper.SelectFromOptions(
        [Statics.OpenAIKey, Statics.OllamaLocalLLMKey,
        Statics.LlamafileLocalLLMKey]);

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
                Statics.GPT4TurboKey, Statics.GPT4oKey,
                Statics.GPT4oMiniKey]);

        // Initialize the client.
        client = new(openAIModel, new ApiKeyCredential(openAIApiKey));

        break;

    case Statics.OllamaLocalLLMKey:

        // Set variables
        string ollamaApiKey = "ollama";
        Uri ollamaEndpoint = new("http://localhost:11434");

        // Get the local LLM name.
        string ollamaModel =
            ConsoleHelper.GetString(Statics.OllamaModelNamePrompt);

        // Initialize the client.
        client =
            new(ollamaModel,
                new ApiKeyCredential(ollamaApiKey),
                new OpenAIClientOptions { Endpoint = ollamaEndpoint });

        break;

    case Statics.LlamafileLocalLLMKey:

        // Set variables
        string llamafileApiKey = "llamafile";
        Uri llamafileEndpoint = new("http://127.0.0.1:8080");

        // Initialize the client.
        client =
            new("LLaMA_CPP",
                new ApiKeyCredential(llamafileApiKey),
                new OpenAIClientOptions { Endpoint = llamafileEndpoint });

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
    AsyncCollectionResult<StreamingChatCompletionUpdate> chatUpdates =
        client.CompleteChatStreamingAsync(messages, options);

    // Loop through the chat updates.
    await foreach (StreamingChatCompletionUpdate chatUpdate in chatUpdates)
    {
        foreach (ChatMessageContentPart contentPart in chatUpdate.ContentUpdate)
        {
            if (contentPart.Text.Equals("</s>"))
            {
                continue;
            }

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