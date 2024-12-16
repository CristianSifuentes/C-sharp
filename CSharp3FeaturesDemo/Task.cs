public class Task
{
    //Automatic property for handling data
    public int Id { get; set; }
    public string Name { get; set; }
    public string Priority { get; set; } // High, Medium, Low
    public DateTime CreatedAt { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Name} - Priority: {Priority}, Created: {CreatedAt}";
    }
}