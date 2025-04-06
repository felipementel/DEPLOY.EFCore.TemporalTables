using Asp.Versioning;
using Bogus;
using Bogus.Extensions.Brazil;
using DEPLOY.TemporalTables.API.Infra.Database.Persistence;
using Microsoft.OpenApi.Models;

namespace DEPLOY.TemporalTables.API.Endpoints.v1
{
    public static class PersonEndpoint
    {
        public static void MapPessoaEndpointsV1(this IEndpointRouteBuilder app)
        {
            var apiVersionSetBoats = app
                .NewApiVersionSet("Pessoa")
                .HasApiVersion(new ApiVersion(1, 0))
                .ReportApiVersions()
                .Build();

            var boats = app
                .MapGroup("/api/v{version:apiVersion}/pessoa")
                //.RequireAuthorization()
                .WithApiVersionSet(apiVersionSetBoats);


            boats
                .MapPost("/Create/{qtd:int}",
                async (DeployDbContext dbContext,
                int qtd) =>
                {
                    var PessoaFaker =
                new Faker<Domain.v1.Person>(locale: "pt_BR").CustomInstantiator(p =>
                {
                    return new Domain.v1.Person(
                        p.Random.Guid(),
                        createdAt: DateTime.Now.ToUniversalTime(),
                        updatedAt: DateTime.Now.ToUniversalTime(),
                        name: p.Person.FullName,
                        email: p.Person.Email,
                        phone: p.Person.Phone,
                        document: p.Person.Cpf(),
                        address: p.Person.Address.Street,
                        bornDate: p.Date.PastDateOnly()
                    );
                }).FinishWith((f, u) =>
                {
                    Console.WriteLine("Pessoa criado com id {0}", u.Id);
                })
                .Generate(qtd);

                    dbContext.Person.AddRange(PessoaFaker);
                    await dbContext.SaveChangesAsync();

                    return Results.Ok();
                })
                .Produces(201)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "post-pessoa",
                    Summary = "create a boat",
                    Description = "process do register a new boat",
                    Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
                });

            //boats
            //    .MapGet("/", GetBoat)
            //    .Produces(200)
            //    .Produces(422)
            //    .Produces(500)
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "boat-get",
            //        Summary = "get a boat",
            //        Description = "process do get a boat",
            //        Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
            //    });

            //boats
            //    .MapGet("/all", async (MongoDBContext context) =>
            //    {
            //        var items = await context.Boats.ToListAsync();

            //        return TypedResults.Ok(items);
            //    })
            //    .Produces(200)
            //    .Produces(422)
            //    .Produces(500)
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "boat-get-all",
            //        Summary = "get a boat",
            //        Description = "process to get all boat",
            //        Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
            //    });

            //boats
            //    .MapGet("/{boatName}", async (MongoDBContext context,
            //    [FromRoute] string boatName,
            //    CancellationToken cancellationToken = default) =>
            //    {
            //        var matchingBoats = await context.Boats
            //        .Where(x => x.Name.ToUpper().Contains(boatName.ToUpper()))
            //        .ToListAsync(cancellationToken);

            //        if (matchingBoats.Count == 0)
            //        {
            //            return Results.NotFound();
            //        }

            //        return TypedResults.Ok(matchingBoats);
            //    })
            //    .Produces(200)
            //    .Produces(422)
            //    .Produces(500)
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "boat-get",
            //        Summary = "get a boat by name",
            //        Description = "process to get a boat by name",
            //        Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
            //    });

            //boats
            //    .MapGet("/searchbyid/{id}", async
            //    (MongoDBContext context,
            //    [FromRoute] string id,
            //    CancellationToken cancellationToken = default) =>
            //    {
            //        var boat = await context.Boats
            //        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            //        if (boat == null)
            //        {
            //            return Results.NotFound();
            //        }

            //        return TypedResults.Ok(boat);
            //    })
            //    .Produces(200)
            //    .Produces(422)
            //    .Produces(500)
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "boat-get-by-id",
            //        Summary = "get a boat by id",
            //        Description = "process to get a boat by id",
            //        Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
            //    });

            //boats
            //    .MapDelete("/{id}", async (MongoDBContext context,
            //    [FromRoute] string id,
            //    CancellationToken cancellationToken = default) =>
            //    {
            //        var boat = await context.Boats
            //        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            //        if (boat == null)
            //        {
            //            return Results.NotFound();
            //        }

            //        context.Boats.Remove(boat);
            //        await context.SaveChangesAsync();

            //        return TypedResults.NoContent();
            //    })
            //    .Produces(200)
            //    .Produces(404)
            //    .Produces(500)
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "boat-delete",
            //        Summary = "delete a boat",
            //        Description = "process to delete a boat",
            //        Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
            //    });

            //boats
            //    .MapPut("/{id}", async (MongoDBContext context,
            //    [FromRoute] string id,
            //    [FromBody] Boat boat,
            //    CancellationToken cancellationToken = default) =>
            //    {
            //        var boatActual = await context.Boats
            //        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            //        if (boat == null)
            //        {
            //            return Results.NotFound();
            //        }

            //        boatActual!.Name = boat.Name;
            //        boatActual.Size = boat.Size;
            //        boatActual.License = boat.License;

            //        await context.SaveChangesAsync();

            //        return TypedResults.NoContent();
            //    })
            //    .Produces(204)
            //    .Produces(422)
            //    .Produces(500)
            //    .WithOpenApi(operation => new(operation)
            //    {
            //        OperationId = "boat-put",
            //        Summary = "update a boat",
            //        Description = "process to update a boat",
            //        Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
            //    });

            //async Task<IResult> GetBoat(MongoDBContext context,
            //    string boatName)
            //{
            //    var boat = await context.Boats.FirstOrDefaultAsync(x => x.Name == boatName);

            //    if (boat == null)
            //    {
            //        return TypedResults.NotFound();
            //    }

            //    return TypedResults.Ok(boat);
            //}
        }
    }
}
