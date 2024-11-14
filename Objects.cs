using System.Text.Json.Serialization;

namespace work.Objects;

[JsonSerializable(typeof(WorkLogBook))]
public partial class JsonContext : JsonSerializerContext {}
public record WorkLogBook(List<WorkLogItem> WorkLogItems);
public record WorkLogItem(DateTime Timestamp, Work? Work);
public record Work(string Ticket, string? Description);
