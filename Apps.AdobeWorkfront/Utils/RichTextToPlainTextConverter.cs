using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace Apps.AdobeWorkfront.Utils;

public class RichTextToPlainTextConverter
{
    public static bool IsRichText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        try
        {
            var document = JsonSerializer.Deserialize<RichTextDocument>(text);
            return document?.Blocks != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static string ConvertToPlainText(string richTextJson)
    {
        if (string.IsNullOrWhiteSpace(richTextJson))
            return string.Empty;

        try
        {
            var document = JsonSerializer.Deserialize<RichTextDocument>(richTextJson);
            return document != null ? ConvertBlocksToPlainText(document.Blocks) : string.Empty;
        }
        catch (JsonException)
        {
            return richTextJson;
        }
    }

    private static string ConvertBlocksToPlainText(RichTextBlock[] blocks)
    {
        if (blocks == null || blocks.Length == 0)
            return string.Empty;

        var result = new StringBuilder();
        var listItemCounter = 1;
        var previousBlockType = string.Empty;

        foreach (var block in blocks)
        {
            var text = block.Text ?? string.Empty;

            switch (block.Type)
            {
                case "header-one":
                case "header-two":
                case "header-three":
                case "header-four":
                case "header-five":
                case "header-six":
                    result.AppendLine(text);
                    break;

                case "unordered-list-item":
                    result.AppendLine($"• {text}");
                    break;

                case "ordered-list-item":
                    if (previousBlockType != "ordered-list-item")
                        listItemCounter = 1;
                    result.AppendLine($"{listItemCounter}. {text}");
                    listItemCounter++;
                    break;

                case "blockquote":
                    result.AppendLine($"> {text}");
                    break;

                case "code-block":
                    result.AppendLine($"```");
                    result.AppendLine(text);
                    result.AppendLine($"```");
                    break;

                case "unstyled":
                default:
                    if (!string.IsNullOrEmpty(text))
                        result.AppendLine(text);
                    break;
            }

            previousBlockType = block.Type;
        }

        return result.ToString().TrimEnd();
    }

    private class RichTextDocument
    {
        [JsonPropertyName("blocks")]
        public RichTextBlock[] Blocks { get; set; } = Array.Empty<RichTextBlock>();
        
        [JsonPropertyName("entityMap")]
        public object EntityMap { get; set; } = new object();
    }

    private class RichTextBlock
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;
        
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
        
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        
        [JsonPropertyName("depth")]
        public int Depth { get; set; }
        
        [JsonPropertyName("inlineStyleRanges")]
        public InlineStyleRange[] InlineStyleRanges { get; set; } = Array.Empty<InlineStyleRange>();
        
        [JsonPropertyName("entityRanges")]
        public EntityRange[] EntityRanges { get; set; } = Array.Empty<EntityRange>();
        
        [JsonPropertyName("data")]
        public object Data { get; set; } = new object();
    }

    private class InlineStyleRange
    {
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
        
        [JsonPropertyName("length")]
        public int Length { get; set; }
        
        [JsonPropertyName("style")]
        public string Style { get; set; } = string.Empty;
    }

    private class EntityRange
    {
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
        
        [JsonPropertyName("length")]
        public int Length { get; set; }
        
        [JsonPropertyName("key")]
        public int Key { get; set; }
    }
}
