using System.Globalization;
using System.Text.Json;

public static class SetsAndMaps
{
    // Problem 2: O(n) symmetric pairs using a set.
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<string>();
        var results = new List<string>();

        foreach (var word in words)
        {
            var reversed = string.Concat(word[1], word[0]);
            if (word != reversed && seen.Contains(reversed))
                results.Add($"{reversed} & {word}");
            seen.Add(word);
        }

        return results.ToArray();
    }

    // Problem 2: count degrees (column 4 of the file).
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            if (fields.Length < 4) continue; // skip blank/short lines

            var degree = fields[3].Trim();
            degrees[degree] = degrees.GetValueOrDefault(degree) + 1;
        }

        return degrees;
    }

    // Problem 3: anagram check using a dictionary, ignoring spaces and case.
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

        return counts.Values.All(v => v == 0);
    }

    // Problem 5: today's earthquakes from USGS.
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
            if (props?.Place == null || props.Mag == null)
                continue;

            var mag = props.Mag.Value.ToString(CultureInfo.InvariantCulture);
            results.Add($"{props.Place} - Mag {mag}");
        }

        return results.ToArray();
    }
}