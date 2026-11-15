using System.Globalization;
using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// Finds all symmetric pairs of two character words in O(n) time using a set.
    /// Example: [am, at, ma, if, fi] -> ["am & ma", "if & fi"]
    /// </summary>
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<string>();
        var results = new List<string>();

        foreach (var word in words)
        {
            var reversed = string.Concat(word[1], word[0]);

            // Same-letter words (e.g. "aa") never pair with anything.
            // If the reverse was seen earlier, report earlier word first.
            if (word != reversed && seen.Contains(reversed))
                results.Add($"{reversed} & {word}");

            seen.Add(word);
        }

        return results.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (4th column).
    /// </summary>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            var degree = fields[3].Trim();

            degrees[degree] = degrees.GetValueOrDefault(degree) + 1;
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams, ignoring spaces and case.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        var counts = new Dictionary<char, int>();

        foreach (var ch in word1)
        {
            if (ch == ' ') continue;
            var c = char.ToLower(ch);
            counts[c] = counts.GetValueOrDefault(c) + 1;
        }

        foreach (var ch in word2)
        {
            if (ch == ' ') continue;
            var c = char.ToLower(ch);
            if (!counts.TryGetValue(c, out var count) || count == 0)
                return false;
            counts[c] = count - 1;
        }

        foreach (var count in counts.Values)
        {
            if (count != 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Reads today's earthquake data from USGS and returns "place - Mag X" strings.
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        if (featureCollection?.Features == null)
            return [];

        var results = new List<string>();
        foreach (var feature in featureCollection.Features)
        {
            var props = feature.Properties;
            // Some events have no magnitude; skip those so every line has one.
            if (props?.Place == null || props.Mag == null)
                continue;

            var mag = props.Mag.Value.ToString(CultureInfo.InvariantCulture);
            results.Add($"{props.Place} - Mag {mag}");
        }

        return results.ToArray();
    }
}