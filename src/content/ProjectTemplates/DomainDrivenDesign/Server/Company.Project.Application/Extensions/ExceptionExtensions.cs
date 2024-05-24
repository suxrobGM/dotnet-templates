using System.Text;

namespace Company.Project.Application.Extensions;

/// <summary>
/// Provides extension methods for Exception objects.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Retrieves a concatenated string of all exception messages, including those of inner exceptions.
    /// </summary>
    /// <param name="exception">The exception to extract messages from.</param>
    /// <returns>A concatenated string containing all exception and inner exception messages.</returns>
    public static string GetAllMessages(this Exception exception)
    {
        var stringBuilder = new StringBuilder();
        AppendAllMessages(exception, stringBuilder);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Recursively appends messages from the exception and its inner exceptions to the string builder.
    /// </summary>
    /// <param name="exception">The exception to extract messages from.</param>
    /// <param name="stringBuilder">The StringBuilder to which the messages are appended.</param>
    private static void AppendAllMessages(Exception? exception, StringBuilder stringBuilder)
    {
        if (exception is null)
        {
            return;
        }

        stringBuilder.AppendLine(exception.Message);                                   
        
        if (exception.InnerException is not null)
        {
            AppendAllMessages(exception.InnerException, stringBuilder);
        }
    }
}
