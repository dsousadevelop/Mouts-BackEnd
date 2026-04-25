using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories;

public abstract class RepositoryTestsBase : IDisposable
{
    protected readonly DefaultContext Context;

    protected RepositoryTestsBase()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new DefaultContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
