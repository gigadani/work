# work
### The simplest and the most efficient.

If you are a developer who works on one ticket at a time and you want to track your work hours, and for some reason want to plurge it only twice or even once a month into the time tracking system of your company, then you will love `work`.

`work` is a simple command line application for you to track your work hours you spend on tickets. It is designed to be simple and efficient. It is written in C# and uses a json file to store the data. You can get monthly or daily reports of these workings which come in the simple and concise format of:

```Ticket LOL-1032: "Refactoring unit tests", 4h 49min```

Let's say you start your work in the morning and are about to work on the ticket LOL-1032. You simply type:

```bash
work start LOL-1032
```

If you happen to already know what you are really doing there, you can add a description to it:

```bash
work desc Refactoring unit tests
```

When you go to lunch, you type:

```bash
work stop
```

When you come back from lunch, you type:

```bash
work start LOL-1032: Refactoring unit tests
```

Maybe during your endless Reddit browsing you forgot what you were working on, so you type:

```bash
work status
```

## Installation

### From source

Note: To build from source, you must have .NET 9.0 SDK installed on your machine.

1. Clone the repository

```bash
git clone https://github.com/gigadani/work.git
````

2. Build the project

```bash
cd work

dotnet publish
```

3. Add the path to the `work` executable to your PATH environment variable.

```bash
