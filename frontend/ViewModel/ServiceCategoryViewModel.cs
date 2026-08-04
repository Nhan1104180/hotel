public class ServiceCategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}

public class AddServiceCategoryViewModel
{
    public string Name { get; set; } 
    public string Description { get; set; }
}

public class UpdateServiceCategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
}