using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using Microsoft.Bot.Connector;

using OpsJobBot.Ops;

namespace OpsJobBot.Dialogs
{
    [Serializable]
    public class SimpleOpsDialog : IDialog<object>
    {
        #region CONSTANTS

        public const string CONST_JOB_NAME = "jobname";

        public const string CONST_JOB_DIRECTORY = "C:\\tmp\\";

        #endregion

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text.ToLower().StartsWith("run "))
            {
                bool QuestionAnswered = false;

                if (message.Text.Contains(" "))
                {
                    string[] ProvidedCommand = message.Text.Split(new char[1] { ' ' });

                    if (ProvidedCommand.Length == 2)
                    {
                        string sJobName = ProvidedCommand[1];

                        QuestionAnswered = true;

                        context.UserData.SetValue(CONST_JOB_NAME, sJobName);

                        PromptDialog.Confirm(
                            context,
                            AfterRunJobAsync,
                            $"Are you sure you want to run the job {sJobName}?",
                            "Didn't get that!",
                            promptStyle: PromptStyle.None);
                    }
                }

                if (!QuestionAnswered)
                {
                    await context.PostAsync($"I did not understand the request : {message.Text}");
                    context.Wait(MessageReceivedAsync);
                }
            }
            else if (message.Text.ToLower().StartsWith("check "))
            {
                bool QuestionAnswered = false;

                if (message.Text.Contains(" "))
                {
                    string[] ProvidedCommand = message.Text.Split(new char[1] { ' ' });

                    if (ProvidedCommand.Length == 2)
                    {
                        string sJobName = ProvidedCommand[1];

                        QuestionAnswered = true;

                        bool bIsJobRunning = SystemOps.IsJobRunning(CONST_JOB_DIRECTORY, sJobName);

                        if (bIsJobRunning)
                            await context.PostAsync("Yes, it is currently running.");
                        else 
                            await context.PostAsync("No, it's not currently running.");
                    }
                }

                if (!QuestionAnswered)
                {
                    await context.PostAsync($"I did not understand the request : {message.Text}");
                    context.Wait(MessageReceivedAsync);
                }
            }
            else
            {
                await context.PostAsync($"I did not understand the request : {message.Text}");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterRunJobAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;

            if (confirm)
            {
                string JobName     = context.UserData.Get<string>(CONST_JOB_NAME);
                string JobFilepath = CONST_JOB_DIRECTORY + JobName;

                FileInfo JobFileInfo = new FileInfo(JobFilepath);
                if (JobFileInfo.Exists)
                {
                    try
                    {
                        SystemOps.ExecuteCommand(JobFileInfo);

                        await context.PostAsync("And there it goes...off and running!");
                    }
                    catch (Exception ex)
                    {
                        await context.PostAsync(ex.ToString());
                    }
                }
                else
                    await context.PostAsync($"Job ({JobName}) does not exist.");
            }
            else
            {
                await context.PostAsync("Did not run the job.");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}