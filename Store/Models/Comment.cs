namespace Store.Models;

public class Comment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Body { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}