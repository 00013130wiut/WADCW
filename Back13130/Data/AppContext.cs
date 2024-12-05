using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Back13130;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options)
        : base(options)
    {
    }

    public DbSet<Back13130.User> Users { get; set; } = default!;
    public DbSet<Back13130.Event> Events { get; set; } = default!;
    public DbSet<Back13130.Participant> Participants { get; set; } = default!;

}
