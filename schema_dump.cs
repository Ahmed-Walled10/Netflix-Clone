using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Infrastructure.Persistence;

namespace SchemaDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<NetflixCloneDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;
            
            using var ctx = new NetflixCloneDbContext(options);
            var model = ctx.Model;
            var debugString = model.ToDebugString(Microsoft.EntityFrameworkCore.Infrastructure.MetadataDebugStringOptions.ShortDefault);
            System.IO.File.WriteAllText("schema_dump.txt", debugString);
            Console.WriteLine("Done dumping.");
        }
    }
}