using System;
using System.Collections.Generic;

public abstract class BaseSchema
{
    public abstract Dictionary<string, object> ToDict();

    protected static string GetCurrentTimestamp()
    {
        return DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
    }
}
