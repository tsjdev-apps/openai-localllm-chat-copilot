namespace ShakespeareChat.Utils;

internal static class Statics
{
    /// <summary>
    ///     The key for OpenAI.
    /// </summary>
    public const string OpenAIKey
        = "OpenAI";

    /// <summary>
    ///     The key for Ollama Local LLM.
    /// </summary>
    public const string OllamaLocalLLMKey
        = "Ollama (Local LLM)";

    /// <summary>
    ///     The key for Llamafile Local LLM.
    /// </summary>
    public const string LlamafileLocalLLMKey
        = "Llamafile (Local LLM)";

    /// <summary>
    ///     The key for GPT-3.5 Turbo.
    /// </summary>
    public static string GPT35TurboKey
        = "gpt-3.5-turbo";

    /// <summary>
    ///     The key for GPT-4.
    /// </summary>
    public static string GPT4Key
        = "gpt-4";

    /// <summary>
    ///     The key for GPT-4 Turbo.
    /// </summary>
    public static string GPT4TurboKey
        = "gpt-4-turbo";

    /// <summary>
    ///     The key for GPT-4o.
    /// </summary>
    public static string GPT4oKey
        = "gpt-4o";

    /// <summary>
    ///     The key for GPT-4o-mini.
    /// </summary>
    public static string GPT4oMiniKey
        = "gpt-4o-mini";

    /// <summary>
    ///     Prompt for getting the OpenAI API key.
    /// </summary>
    public static string OpenAIKeyPrompt
        = $"Please insert your [yellow]{OpenAIKey}[/] API key:";

    /// <summary>
    ///     Prompt for gettin the local LLM name.
    /// </summary>
    public static string OllamaModelNamePrompt
        = "Please insert your [yellow]Ollama local LLM name[/]:";

    /// <summary>
    ///     System message for William Shakespeare.
    /// </summary>
    public static string SystemMessage
        = "You are William Shakespeare, the English playwright, poet and actor. Pretend to be William Shakespeare.";

    /// <summary>
    ///     User message prompt.
    /// </summary>
    public static string UserMessage
        = "Please introduce yourself.";
}
