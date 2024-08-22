using System.Diagnostics;

public static class ApplicationDiagnostics
{
    public const string ActivitySourceName = "Console.Tool.Diagnostics";
    public static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName);
}