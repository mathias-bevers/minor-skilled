namespace FitMate.Utils;

public class PopupException : Exception
{
    public override string Message { get; }
    public string Title { get; }

    public PopupException(string message, string title = "ERROR")
    {
        Message = message;
        Title = title;
    }
}