using System.Collections.Generic;

public class HistorySchema: BaseSchema
{
    public const string pathKey = "history";
    
    private string created;
    private readonly int playerId;
    private readonly int actionId;
    private List<int> data;

    public HistorySchema(int playerId, int actionId, List<int> data)
    {
        this.playerId = playerId;
        this.actionId = actionId;
        this.data = data;
        created = GetCurrentTimestamp();
    }

    public override Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>()
        {
            ["created"] = created,
            ["player_id"] = playerId,
            ["actionId"] = actionId,
            ["data"] = data,
        };
    }
}