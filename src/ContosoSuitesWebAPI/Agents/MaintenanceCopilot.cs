﻿using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace ContosoSuitesWebAPI.Agents
{
    // Exercise 5 Task 2 TODO #2: Inject the Kernel service into the MaintenanceCopilot class.
    /// <summary>
    /// The maintenance copilot agent for assisting with maintenance requests.
    /// </summary>
    public class MaintenanceCopilot(Kernel kernel)
    {
        // Exercise 5 Task 2 TODO #3: Uncomment the two lines below to declare the Kernel and ChatHistory objects.
        public readonly Kernel _kernel = kernel;
        private ChatHistory _history = new();

        /// <summary>
        /// Chat with the maintenance copilot.
        /// </summary>
        public async Task<string> Chat(string userPrompt)
        {
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            _history.AddUserMessage(userPrompt);

            var result = await chatCompletionService.GetChatMessageContentAsync(
               _history,
               executionSettings: openAIPromptExecutionSettings,
               _kernel
            );

            _history.AddAssistantMessage(result.Content!);

            return result.Content!;
        }
    }
}
