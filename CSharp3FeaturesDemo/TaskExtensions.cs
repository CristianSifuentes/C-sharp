using System.IO.Compression;
using System.Net.NetworkInformation;

public static class TaskExtensions
{
    // Extension method fo filter task by priority
    public static IEnumerable<Task> FilterByPriority(this IEnumerable<Task> tasks, string priority){
        return tasks.Where(task => task.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));
    }
    // Extension method to get recent task (last N days)
    public static IEnumerable<Task> GetRecentTasks(this IEnumerable<Task> tasks, int days){
        DateTime cutoffDate = DateTime.Now.AddDays(-days);
        return tasks.Where(task => task.CreatedAt >= cutoffDate);
    }

}