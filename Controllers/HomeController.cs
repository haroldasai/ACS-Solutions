using acs_chat_app_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azure.Communication;
using Azure.Communication.Identity;
using Azure.Communication.Chat;

namespace acs_chat_app_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> User()
        {
            User user = new User();
            async System.Threading.Tasks.Task UserThreads()
            {
                string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
                var client = new CommunicationIdentityClient(connectionString);
                //var identityResponse = await client.CreateUserAsync();
                //var identity = identityResponse.Value;
                //Console.WriteLine(identity);
                var identity = new Azure.Communication.CommunicationUserIdentifier(user.UserID);
                var tokenResponse = await client.GetTokenAsync(identity, scopes: new[] { CommunicationTokenScope.Chat });
                user.AccessToken = tokenResponse.Value.Token;
                Console.WriteLine(user.AccessToken);

                Uri endpoint = new Uri("https://haroldtest.communication.azure.com");
                CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(user.AccessToken);
                ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
                Azure.Pageable<ChatThreadItem> threadItems = chatClient.GetChatThreads();
                foreach (ChatThreadItem item in threadItems)
                {
                    ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(item.Id);
                    Azure.Pageable<ChatMessage> messages = chatThreadClient.GetMessages();
                    List<ChatMessage> messageList = new List<ChatMessage>();
                    foreach (ChatMessage message in messages)
                    {
                        messageList.Add(message);
                    }
                    ChatThread thread = new ChatThread(item.Id, item.Topic, messageList);
                    user.Threads.Add(thread);
                }
                Console.WriteLine(user.Threads[0].Topic);
                Console.WriteLine(user.Threads[1].Topic);
            }
            await UserThreads();
            Console.WriteLine(user.Threads[0].Topic);
            ViewBag.user = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> User(InputMessage input)
        {
            User user = new User();
            string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
            var client = new CommunicationIdentityClient(connectionString);
            var identity = new Azure.Communication.CommunicationUserIdentifier(user.UserID);
            var tokenResponse = await client.GetTokenAsync(identity, scopes: new[] { CommunicationTokenScope.Chat });
            user.AccessToken = tokenResponse.Value.Token;
            Console.WriteLine(user.AccessToken);

            Uri endpoint = new Uri("https://haroldtest.communication.azure.com");
            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(user.AccessToken);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);

            Azure.Pageable<ChatThreadItem> threadItems = chatClient.GetChatThreads();
            List<ChatThreadClient> ctclients = new List<ChatThreadClient>();

            foreach (ChatThreadItem item in threadItems)
            {
                ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(item.Id);
                ctclients.Add(chatThreadClient);
            }

            SendChatMessageResult sendChatMessageResult = await ctclients[input.ThreadIndex].SendMessageAsync(content: input.Message, type: ChatMessageType.Text, senderDisplayName: user.Name);
            string messageId = sendChatMessageResult.Id;
            Console.WriteLine(messageId);

            foreach (ChatThreadItem item in threadItems)
            {
                ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(item.Id);
                Azure.Pageable<ChatMessage> messages = chatThreadClient.GetMessages();
                List<ChatMessage> messageList = new List<ChatMessage>();
                foreach (ChatMessage message in messages)
                {
                    messageList.Add(message);
                }
                ChatThread thread = new ChatThread(item.Id, item.Topic, messageList);
                user.Threads.Add(thread);
            }
            Console.WriteLine(user.Threads[0].Topic);
            Console.WriteLine(user.Threads[1].Topic);
            ViewBag.user = user;
            return View();
        }

        /*
        public IActionResult UserList()
        {
           async System.Threading.Tasks.Task ListAllUsers()
            {
                string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
                var client = new CommunicationIdentityClient(connectionString);

                client.
                var identity = new Azure.Communication.CommunicationUserIdentifier(user.UserID);
                var tokenResponse = await client.GetTokenAsync(identity, scopes: new[] { CommunicationTokenScope.Chat });
                var token = tokenResponse.Value.Token;
                Console.WriteLine(token);

                Uri endpoint = new Uri("https://haroldtest.communication.azure.com");
                CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(token);
                ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
                Azure.Pageable<ChatThreadItem> threadItems = chatClient.GetChatThreads();
                foreach (ChatThreadItem item in threadItems)
                {
                    Console.WriteLine(item.Topic);
                }
            }
            return View();
        }
        */

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
