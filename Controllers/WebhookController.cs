using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BotApi.Telegram;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

[ApiController]
[Route("WebHook")]
public class WebHookController : ControllerBase
{
    public static Client Bot { get; set; }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        Update update;
        using(StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            update = JsonConvert.DeserializeObject<Update>(await reader.ReadToEndAsync());
        }
        if (update.Type == UpdateType.Message)
            await Bot.HandleMessage(update.Message);
        else if (update.Type == UpdateType.InlineQuery)
            await Bot.HandleInlineQuery(update.InlineQuery);
        else if (update.Type == UpdateType.ChosenInlineResult)
            Bot.HandleInlineQueryChoosen(update.ChosenInlineResult);
        return Ok();
    }
}