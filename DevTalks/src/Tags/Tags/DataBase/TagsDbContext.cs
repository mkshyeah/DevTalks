using Microsoft.EntityFrameworkCore;
using Tags.Domain;

namespace Tags.DataBase;

public class TagsDbContext: DbContext
{
    public DbSet<Tag> Tags { get; set; }
    
}