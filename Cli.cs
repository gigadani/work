using work.Objects;

namespace work;
public static class Cli
{
    public static string? GetFirstWord(string[] args) => args.Length != 0 ? args[0] : null;

    public static string? GetRestAsOneString(string[] args) => string.Join(" ", args.Skip(1));

    public static (string? Ticket, string? Description) GetTicketAndDescription(string? restOfArgs)
    {
        if (restOfArgs == null)
        {
            return (null, null);
        }
        if (restOfArgs.Contains(':'))
        {
            var parts = restOfArgs.Split(':');
            return (parts[0].Trim(), restOfArgs[(restOfArgs.IndexOf(':') + 1)..].Trim());
        }
        else
        {
            return (restOfArgs, null);
        }
    }

    public static void PrintHelpText() => Console.WriteLine(GetHelpText());

    public static void PrintHelpTextFor(string command) => Console.WriteLine(GetHelpTextForCommand(command));

    public static void StartWork(string[] args, string fileName)
    {
        var (Ticket, Description) = GetTicketAndDescription(GetRestAsOneString(args));
        if (Ticket == null)
        {
            Console.WriteLine("Ticket name is required.");
            return;
        }
        new WorkLog(fileName).LogAction(new WorkLogItem(DateTime.Now, new Work(Ticket, Description)));
        Console.WriteLine("Started working.");
    }

    public static void StopWork(string fileName)
    {
        var workLog = new WorkLog(fileName);
        var latestWork = workLog.LatestAction;
        if (latestWork != null && latestWork.Work != null)
        {
            workLog.StopWork();
            Console.WriteLine("Stopped working.");
            Console.WriteLine($"Worked on {latestWork.Work.Ticket}{(string.IsNullOrEmpty(latestWork.Work.Description) ? "" : $": {latestWork.Work.Description}")}");
            Console.WriteLine($"Work started: {latestWork.Timestamp.ToShortTimeString()}");
            Console.WriteLine($"Elapsed: {HumanStringify(DateTime.Now - latestWork.Timestamp)}");
        }
        else
        {
            Console.WriteLine("There's no ongoing work.");
        }
    }

    public static void PrintStatus(string fileName)
    {
        var latestWork = new WorkLog(fileName).LatestAction;
        if (latestWork == null)
        {
            Console.WriteLine("No work logged.");
            return;
        }

        if (latestWork.Work == null)
        {
            Console.WriteLine("No work currently going on.");
            return;
        }

        Console.WriteLine($"Currently working on {latestWork.Work.Ticket}{(string.IsNullOrEmpty(latestWork.Work.Description) ? "" : $": {latestWork.Work.Description}")}");
        Console.WriteLine($"Work started: {latestWork.Timestamp.ToShortTimeString()}");
        Console.WriteLine($"Elapsed: {HumanStringify(DateTime.Now - latestWork.Timestamp)}");
    }

    private static string HumanStringify(TimeSpan timespan)
    {
        var parts = new List<string>();
        if (timespan.Days > 0)
        {
            parts.Add($"{timespan.Days} day{(timespan.Days > 1 ? "s" : "")}");
        }
        if (timespan.Hours > 0)
        {
            parts.Add($"{timespan.Hours} hour{(timespan.Hours > 1 ? "s" : "")}");
        }
        if (timespan.Minutes > 0)
        {
            parts.Add($"{timespan.Minutes} minute{(timespan.Minutes > 1 ? "s" : "")}");
        }
        if (timespan.Seconds > 0)
        {
            parts.Add($"{timespan.Seconds} second{(timespan.Seconds > 1 ? "s" : "")}");
        }
        return string.Join(" and ", parts);
    }

    public static void SetDescriptionOfCurrentWork(string? description, string fileName)
    {
        if (string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Please provide a description for the work.");
            return;
        }

        try
        {
            new WorkLog(fileName).SetDescriptionOfLatest(description);
        }
        catch (NoWorkException)
        {
            Console.WriteLine("No work currently going on.");
            return;
        }
        Console.WriteLine("Description set.");
    }

    public static string GetHelpText()
    {
        return string.Join(Environment.NewLine, new List<string>
        {
            "Available actions:",
            "start <ticket> [description]",
            "stop",
            "desc <description>",
            "status",
            "log",
            "help [command]"
        });
    }

    public static string GetHelpTextForCommand(string command)
    {
        var lines = new List<string>();
        switch (command)
        {
            case "start":
                lines.Add("start <ticket> [description]");
                lines.Add("Starts working on a ticket with an optional description.");
                break;
            case "stop":
                lines.Add("stop");
                lines.Add("Stops working on the current ticket.");
                break;
            case "desc":
                lines.Add("desc <description>");
                lines.Add("Sets the description of the current ticket.");
                break;
            case "status":
                lines.Add("status");
                lines.Add("Shows the current ticket and its description.");
                break;
            case "log":
                lines.Add("log");
                lines.Add("Shows the log of all actions.");
                break;
            case "help":
                lines.Add("help [command]");
                lines.Add("Shows general list of commands, or the help for an optionally specified command.");
                break;
            default:
                lines.Add("Invalid command.");
                break;
        }
        return string.Join(Environment.NewLine, lines);
    }
}
