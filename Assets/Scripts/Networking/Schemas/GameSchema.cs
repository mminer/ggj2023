using System.Linq;
using System.Collections.Generic;

public class GameSchema: BaseSchema
{
    public const string parentPath = "Games";
    
    public readonly string gameCode;
    private readonly string created;
      
    public string ended;
    public List<PlayerSchema> players;
    public List<HistorySchema> history;

    public GameSchema(string gameCode, List<PlayerSchema> players, List<HistorySchema> history)
    {
        this.gameCode = gameCode;
        created = GetCurrentTimestamp();
        ended = null;
        this.players = players;
        this.history = history;
    }

    public override Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>()
        {
            ["created"] = created,
            ["ended"] = ended,
            ["players"] = players.Select((player) => player.ToDict()).ToList(),
            ["history"] = history.Select((action) => action.ToDict().ToList()),
        };
    }
}
