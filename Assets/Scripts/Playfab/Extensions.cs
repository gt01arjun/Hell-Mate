public static class Extensions
{
    public static string GetFromJSON(this string json, string fieldName)
    {
        var index = json.IndexOf(fieldName);
        if (index < 0) return "NOPE";

        var colonIndex = json.IndexOf(':', index) + 1;
        var commaIndex = json.IndexOf(',', index);

        if (commaIndex >= 0)
        {
            return json.Substring(colonIndex, commaIndex - colonIndex).Replace('"', ' ').Trim();
        }
        else
        {
            var braceIndex = json.IndexOf('}', index);
            return json.Substring(colonIndex, braceIndex - colonIndex).Replace('"', ' ').Trim();
        }
    }
}
