
using System.Collections.Generic;

public class PlayerSchema: BaseSchema
{
    public const string pathKey = "players";
    
    private readonly string created;
    private readonly string name;
    private readonly int icon;
    private readonly int index;

    public PlayerSchema(int index, string name, int icon) : this(index, name, icon, GetCurrentTimestamp()) {}
    
    public PlayerSchema(int index, string name, int icon, string created)
    {
        this.index = index;
        this.name = name;
        this.icon = icon;
        this.created = created;
    }

    public override Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>()
        {
            ["created"] = created,
            ["name"] = name,
            ["icon"] = icon,
        };
    }
}