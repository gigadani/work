using work.Objects;
using System.Text.Json;

namespace work;

public class WorkLog(string filePath)
{
    public string FilePath { get; set; } = filePath;

    public void LogAction(WorkLogItem item)
    {
        LogInFile = LogInFile.Append(item);
    }

    private IEnumerable<WorkLogItem> LogInFile
    {
        get
        {
            if (!File.Exists(FilePath))
            {
                return [];
            }
            var book = JsonSerializer.Deserialize(File.ReadAllText(FilePath), JsonContext.Default.WorkLogBook);
            var list = book?.WorkLogItems ?? [];
            return list.OrderBy(workLogItem => workLogItem.Timestamp);
        }
        set
        {
            var items = Enumerable
                .Concat(LogInFile, value)
                .OrderBy(workLogItem => workLogItem.Timestamp)
                .ToList();

            var book = new WorkLogBook(items);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(book, JsonContext.Default.WorkLogBook));
        }
    }

    public WorkLogItem? LatestAction
    {
        get
        {
            return LogInFile.LastOrDefault();
        }

        set
        {
            ArgumentNullException.ThrowIfNull(value);
            var logInFile = LogInFile;
            var latestAction = logInFile.LastOrDefault();
            if (latestAction != null)
            {
                logInFile = logInFile.SkipLast(1);
            }
            LogInFile = logInFile.Append(value);
        }
    }

    public void SetDescriptionOfLatest(string newDescription)
    {
        var latestAction = LatestAction;
        if (latestAction == null)
        {
            throw new NoWorkException();
        }
        else if (latestAction.Work == null)
        {
            throw new NoWorkException();
        }
        var newAction = latestAction with { Work = latestAction.Work with { Description = newDescription } };
        LatestAction = newAction;
    }

    public void StopWork()
    {
        var latestAction = LatestAction;
        if (latestAction == null)
        {
            throw new NoWorkException();
        }
        else if (latestAction.Work == null)
        {
            throw new NoWorkException();
        }
        var newAction = latestAction with { Work = null };
        LatestAction = newAction;
    }
}

public class NoWorkException : Exception
{
    public NoWorkException() : base() { }
}
