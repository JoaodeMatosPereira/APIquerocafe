var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var cafes = new List<cafeDto>
{
    new cafeDto(1, "3 corações"),
    new cafeDto(2, "nescafé"),
    new cafeDto(3, "melitta")
};

app.MapGet("/", () => "API de cafés está no ar");

app.MapGet("/api/querocafe", () =>
{
   return Results.Ok(cafes); 
});

app.MapGet("/api/querocafe/{id:int}", (int id) =>
{
   var cafe = cafes.Find(cafeDaLista => cafeDaLista.id == id);

   if (cafe is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(cafe);
});

app.MapPost("/api/querocafe", (cafeEntradaDto dados) =>
{
    int proximoId = cafes.Count + 1;
    var novocafe = new cafeDto(proximoId, dados.Titulo);
    cafes.Add(novocafe);

    return Results.Created($"/api/querocafe/{novocafe.id}", novocafe);
});

app.MapPut("/api/querocafe/{id:int}", (int id, cafeEntradaDto dados) =>
{
    int indice = cafes.FindIndex(cafeDaLista => cafeDaLista.id == id);

    if (indice == -1)
    {
        return Results.NotFound(new { mensagem = "Café não encontrada." });
    }

    var atualizado = new cafeDto(id, dados.Titulo);
    cafes[indice] = atualizado;

    return Results.Ok(atualizado);
});

app.MapDelete("/api/querocafe/{id:int}", (int id) =>
{
    int indice = cafes.FindIndex(cafeDaLista => cafeDaLista.id == id);

    if (indice == -1)
    {
        return Results.NotFound(new { mensagem = "Café não encontrada." });
    }

    cafes.RemoveAt(indice);

    return Results.NoContent();
});


app.Run();

record cafeDto(int id, string Titulo);
record cafeEntradaDto(string Titulo);