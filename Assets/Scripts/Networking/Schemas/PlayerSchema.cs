
using System.Collections.Generic;

public class PlayerSchema: BaseSchema
{
    private string created;
    private readonly string name;
    private readonly int icon;
    private readonly int index;

    public PlayerSchema(int index, string name, int icon)
    {
        this.index = index;
        this.name = name;
        this.icon = icon;
        created = GetCurrentTimestamp();
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