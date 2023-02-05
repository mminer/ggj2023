using System.Linq;
using System.Collections.Generic;

public class GameSchema: BaseSchema
{
    public const string pathKey = "Games";
    
    public readonly string gameCode;
    private readonly string created;
      
    public string ended;

    public GameSchema(string gameCode)
    {
        this.gameCode = gameCode;
        created = GetCurrentTimestamp();
        ended = null;
    }

    public void EndGame()
    {
        ended = GetCurrentTimestamp();
    } 

    public override Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>()
        {
            ["created"] = created,
            ["ended"] = ended,
            ["players"] = new Dictionary<string, object>(),
            ["history"] = new Dictionary<string, object>(),
        };
    }
}
