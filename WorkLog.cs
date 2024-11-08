using hssl.Models;
using System.Text.Json;

namespace hssl;

public class WorkLog
{
    public string FilePath { get; set; }

    public WorkLog(string filePath)
    {
        FilePath = filePath;
    }

    public void LogAction(WorkLogAction action)
    {
        LogInFile = LogInFile.Append(action);
    }

    private IEnumerable<WorkLogAction> LogInFile
    {
        get
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("File not found", FilePath);
            }
            return JsonSerializer.Deserialize<WorkLogAction[]>(File.ReadAllText(FilePath)) ?? [];
        }
        set
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(value));
        }
    }

    public WorkLogAction? LatestAction => LogInFile.LastOrDefault();
}
