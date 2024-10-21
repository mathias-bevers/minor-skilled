namespace FitMate.Utils;

public class PopupException : Exception
{
    public PopupException(string message, string title = "ERROR") : base(message) { }
}