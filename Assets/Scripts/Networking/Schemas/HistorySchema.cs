using System.Collections.Generic;

public class HistorySchema: BaseSchema
{
    public const string pathKey = "history";
    
    private readonly string created;
    private readonly int playerId;
    private readonly int actionId;
    private readonly string phase;
    private readonly List<int> data;

    public HistorySchema(int playerId, int actionId, string phase, List<int> data) : this(playerId, actionId, phase, data, GetCurrentTimestamp()) {}

    public HistorySchema(int playerId, int actionId, string phase, List<int> data, string created)
    {
        this.playerId = playerId;
        this.actionId = actionId;
        this.phase = phase;
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
            ["phase"] = phase,
            ["data"] = data,
        };
    }
}