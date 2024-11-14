using work;
using work.Objects;

var fileName = Path.Combine(AppContext.BaseDirectory, "worklog.json");
var actionWord = Cli.GetFirstWord(args);
switch (actionWord)
{
    case null:
    case "":
        {
            Cli.PrintHelpText();
            break;
        }
    case "start":
        {
            var (Ticket, Description) = Cli.GetTicketAndDescription(Cli.GetRestAsOneString(args));
            if (string.IsNullOrEmpty(Ticket))
            {
                Console.WriteLine("Ticket name is required.");
                return;
            }
            new WorkLog(fileName).LogAction(new WorkLogItem(DateTime.Now, new Work(Ticket, Description)));
            Console.WriteLine("Started working.");
            break;
        }
    case "stop":
        {
            Cli.StopWork(fileName);
            break;
        }
    case "desc":
        {
            Cli.SetDescriptionOfCurrentWork(Cli.GetRestAsOneString(args), fileName);
            break;
        }
    case "status":
        {
            Cli.PrintStatus(fileName);
            break;
        }
    case "help":
        {
            var restOfArgs = Cli.GetRestAsOneString(args);
            if (string.IsNullOrEmpty(restOfArgs))
            {
                Cli.PrintHelpText();
            }
            else
            {
                Cli.PrintHelpTextFor(restOfArgs);
            }
            break;
        }
    case "log":
        {
            Console.WriteLine("Not implemented yet.");
            break;
        }
    default:
        {
            Console.WriteLine("Invalid command.");
            break;
        }
}