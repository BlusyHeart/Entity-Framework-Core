using StudentSystem.Data;

class Program
{
    public static void Main()
    {
        var context = new StudentSystemContext();

        context.Database.EnsureCreated();
    }
}
