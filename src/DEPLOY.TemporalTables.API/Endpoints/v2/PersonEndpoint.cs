using Asp.Versioning;
using Bogus;
using Bogus.Extensions.Brazil;
using DEPLOY.TemporalTables.API.Domain.v2;
using DEPLOY.TemporalTables.API.Infra.Database.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DEPLOY.TemporalTables.API.Endpoints.v2
{
    public static class PersonEndpoint
    {
        public static void MapPessoaEndpointsV2(this IEndpointRouteBuilder app)
        {
            var apiVersionSetPessoa = app
                .NewApiVersionSet("People")
                .HasApiVersion(new ApiVersion(2, 0))
                .ReportApiVersions()
                .Build();

            var people = app
                .MapGroup("/api/v{version:apiVersion}/people")
                //.RequireAuthorization()
                .WithApiVersionSet(apiVersionSetPessoa);


            people
                  .MapPost("/create/{qtd:int}",
                  async (DeployDbContext dbContext,
                  int qtd) =>
                  {
                      var PessoaFaker =
                      new Faker<Domain.v2.Person>(locale: "pt_BR")
                      .CustomInstantiator(p =>
                      {
                          return new Domain.v2.Person(
                              id: p.Random.Guid(),
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

                      dbContext.Person_Versioned.AddRange(PessoaFaker);
                      await dbContext.SaveChangesAsync();

                      return Results.Ok();
                  })
                  .Produces(201)
                  .Produces(422)
                  .Produces(500)
                  .WithOpenApi(operation => new(operation)
                  {
                      OperationId = "boat-post",
                      Summary = "create a boat",
                      Description = "process do register a new boat",
                      Tags = new List<OpenApiTag> { new() { Name = "Boats" } }
                  });

            people.MapGet("/TemporalAsOf/{id}", async
                (DeployDbContext dbContext,
                [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
                {
                    var person = await dbContext.Person_Versioned
                    .TemporalAsOf(DateTime.UtcNow)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                    if (person == null)
                    {
                        return Results.NotFound();
                    }
                    return TypedResults.Ok(person);
                })
                .Produces(200)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "person-get",
                    Summary = "get a person by id",
                    Description = "process to get a person by id",
                    Tags = new List<OpenApiTag> { new() { Name = "People" } }
                });

            // crie um get para TemporalAll
            people.MapGet("/TemporalAll", async
                (DeployDbContext dbContext,
                CancellationToken cancellationToken = default) =>
                {
                    var people = await dbContext.Person_Versioned
                    .TemporalAll()
                    .ToListAsync(cancellationToken);
                    if (people.Count == 0)
                    {
                        return Results.NotFound();
                    }
                    return TypedResults.Ok(people);
                })
                .Produces(200)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "person-get-all",
                    Summary = "get all people",
                    Description = "process to get all people",
                    Tags = new List<OpenApiTag> { new() { Name = "People" } }
                });

            // crie um get para TemporalFromTo
            people.MapGet("/TemporalFromTo/{id}", async
                (DeployDbContext dbContext,
                [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
                {
                    var person = await dbContext.Person_Versioned
                    .TemporalFromTo(DateTime.UtcNow, DateTime.UtcNow)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                    if (person == null)
                    {
                        return Results.NotFound();
                    }
                    return TypedResults.Ok(person);
                })
                .Produces(200)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "person-get",
                    Summary = "get a person by id",
                    Description = "process to get a person by id",
                    Tags = new List<OpenApiTag> { new() { Name = "People" } }
                });

            // crie um get para TemporalBetween
            people.MapGet("/TemporalBetween/{id}", async
                (DeployDbContext dbContext,
                [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
                {
                    var person = await dbContext.Person_Versioned
                    .TemporalBetween(DateTime.UtcNow, DateTime.UtcNow)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                    if (person == null)
                    {
                        return Results.NotFound();
                    }
                    return TypedResults.Ok(person);
                })
                .Produces(200)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "person-get",
                    Summary = "get a person by id",
                    Description = "process to get a person by id",
                    Tags = new List<OpenApiTag> { new() { Name = "People" } }
                });

            // crie um get para TemporalContainedIn
            people.MapGet("/TemporalContainedIn/{id}", async
                (DeployDbContext dbContext,
                [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
                {
                    var person = await dbContext.Person_Versioned
                    .TemporalContainedIn(DateTime.UtcNow, DateTime.UtcNow)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                    if (person == null)
                    {
                        return Results.NotFound();
                    }
                    return TypedResults.Ok(person);
                })
                .Produces(200)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "person-get",
                    Summary = "get a person by id",
                    Description = "process to get a person by id",
                    Tags = new List<OpenApiTag> { new() { Name = "People" } }
                });

            // crie um put
            people.MapPut("/{id}", async
                (DeployDbContext dbContext,
                [FromRoute] Guid id,
                [FromBody] Domain.v2.Person person,
                CancellationToken cancellationToken = default) =>
                {
                    var personActual = await dbContext.Person_Versioned
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                    
                    if (personActual == null)
                    {
                        return Results.NotFound();
                    }

                    personActual.Name = person.Name;
                    personActual.Email = person.Email;
                    personActual.Phone = person.Phone;
                    personActual.Document = person.Document;
                    personActual.Address = person.Address;
                    personActual.BornDate = person.BornDate;

                    await dbContext.SaveChangesAsync();
                    return TypedResults.NoContent();
                })
                .Produces(204)
                .Produces(422)
                .Produces(500)
                .WithOpenApi(operation => new(operation)
                {
                    OperationId = "person-put",
                    Summary = "update a person",
                    Description = "process to update a person",
                    Tags = new List<OpenApiTag> { new() { Name = "People" } }
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




        

        //            try
        //            {
        //                var PessoaCanalDEPLOY = await dbContext.Pessoas
        //                .SingleAsync(product => product.Nome.Contains(PessoaFaker[1].Nome));
        //    }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }

        ////O EF Core suporta vários operadores de consulta de tabela temporal:

        ////TemporalAsOf:   Retorna linhas que estavam ativas (atuais) no horário UTC fornecido. 
        ////                Esta é uma única linha da tabela de histórico para uma determinada chave primária.

        //var PessoaTemporalAsOf = dbContext.Pessoas
        //.TemporalAsOf(DateTime.UtcNow)
        //.SingleOrDefaultAsync(p => p.Nome.Contains(PessoaFaker[0].Nome));


        ///*TemporalAll:    Retorna todas as linhas nos dados históricos. 
        //                Normalmente, são muitas linhas da tabela de histórico para uma
        //                determinada chave primária.*/

        ////Crie um exemplo
        //var PessoaTemporalAll = dbContext.Pessoas
        //.TemporalAll()
        //.Where(p => p.Nome.Contains(PessoaFaker[0].Nome));

        ///*TemporalFromTo: Retorna todas as linhas que estavam ativas entre dois horários UTC fornecidos.
        //                Podem ser muitas linhas da tabela de histórico para uma determinada chave primária.*/

        ////Crie um exemplo
        //var PessoaTemporalFromTo = await dbContext.Pessoas
        //.TemporalFromTo(DateTime.UtcNow, DateTime.UtcNow)
        //.SingleAsync(product => product.Nome.Contains(PessoaFaker[0].Nome));


        ///*TemporalBetween: O mesmo que TemporalFromTo, exceto que as linhas
        //                incluídas se tornaram ativas no limite superior.*/

        //var PessoaTemporalBetween = await dbContext.Pessoas
        //.TemporalBetween(DateTime.UtcNow, DateTime.UtcNow)
        //.SingleAsync(product => product.Nome.Contains(PessoaFaker[0].Nome));


        ///*TemporalContainedIn: : Retorna todas as linhas que começaram a ser ativas e terminaram a ser ativas 
        //                entre dois horários UTC fornecidos. Podem ser muitas linhas da tabela de histórico 
        //                para uma determinada chave primária.*/

        //var PessoaTemporalContainedIn = await dbContext.Pessoas
        //.TemporalContainedIn(DateTime.UtcNow, DateTime.UtcNow)
        //.SingleAsync(product => product.Nome.Contains("CANAL"));

    }
}
