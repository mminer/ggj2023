using System.Collections.Generic;

public class HistorySchema: BaseSchema
{
    public const string pathKey = "history";
    
    private readonly string created;
    private readonly int playerId;
    private readonly int actionId;
    private readonly List<int> data;

    public HistorySchema(int playerId, int actionId, List<int> data) : this(playerId, actionId, data, GetCurrentTimestamp()) {}

    public HistorySchema(int playerId, int actionId, List<int> data, string created)
    {
        this.playerId = playerId;
        this.actionId = actionId;
        this.data = data;
        this.created = created;
    }

    public override Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>()
        {
            ["created"] = created,
            ["player_id"] = playerId,
            ["action_id"] = actionId,
            ["data"] = data,
        };
    }
}