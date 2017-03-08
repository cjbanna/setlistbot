using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Setlistbot.Core;
using Setlistbot.Phish;
using Setlistbot;
using Microsoft.Azure;

namespace SetlistbotWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("SetlistbotWorker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("SetlistbotWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SetlistbotWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("SetlistbotWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");

                Setlistbot.Setlistbot bot = new Setlistbot.Setlistbot();
                // TODO: Interfaces could be refactored to use dependency injection.
                IBotSettings settings = new BotSettings
                {
                    Username = CloudConfigurationManager.GetSetting(SettingsConstants.Username),
                    Password = CloudConfigurationManager.GetSetting(SettingsConstants.Password),
                    Key = CloudConfigurationManager.GetSetting(SettingsConstants.Key),
                    Secret = CloudConfigurationManager.GetSetting(SettingsConstants.Secret),
                    Subreddit = CloudConfigurationManager.GetSetting(SettingsConstants.Subreddit),
                    MaxComments = int.Parse(CloudConfigurationManager.GetSetting(SettingsConstants.MaxComments)),
                    MaxSetlists = int.Parse(CloudConfigurationManager.GetSetting(SettingsConstants.MaxSetlists))
                };
                ISetlistAgent setlistAgent = new PhishinWebAgent();
                ICommentBuilder commentBuilder = new CommentBuilder(settings.MaxSetlists);
                // Use subreddit as partition key
                ICommentRepository commentRepository = new CommentRepository(settings.Subreddit);
                bot.Run(settings, setlistAgent, commentBuilder, commentRepository);

                await Task.Delay(10000);
            }
        }
    }
}
