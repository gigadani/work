#pragma warning disable IDE0130
namespace hssl.Models;
#pragma warning restore IDE0130

public record WorkLogAction(DateTime Timestamp, Status Status, string? TicketId, string? Description);

public enum Status
{
    Work,
    Idle
}