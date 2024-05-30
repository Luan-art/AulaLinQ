// See https://aka.ms/new-console-template for more information
using ProjAula1B;
using System.Text.Json.Nodes;

var lst = ReadFile.GetData("C:\\Users\\LuanLF\\motoristas_habilitados.json");


//Console.WriteLine("quantidade:" + TestFilters.getCountRecords(lst));

Console.WriteLine("Listar Registros que tenham o número do documento (cpf) iniciando com 237");

//TestFilters.imprimirList(TestFilters.ListByCpf(lst));

Console.WriteLine("Listar Registros que tem o ano de vigencia igual a 2021");

//TestFilters.imprimirList(TestFilters.ListByYear(lst));

Console.WriteLine("Quantas empresas em no nome de razao Social a descrição LTDA");

//Console.WriteLine("quantidade:" + TestFilters.CountLTDA(lst));

Console.WriteLine("ordenar pela Razão Social");

//TestFilters.imprimirList(TestFilters.OrderByRS(lst));

Console.WriteLine("Inserir todos os Registors no SQL Server\n");

 
if (TestFilters.AddSQLbd(lst))
{
    Console.WriteLine("Adicionado");
}
else
{
  Console.WriteLine("Erro ao adicionar");
}
 
Console.WriteLine("Gerar XML");

//Console.WriteLine(TestFilters.GenerateXML(lst)); 

//List<PenalidadesAplicadas> pA = TestFilters.ExtractDataFromSQL();

/*if (TestFilters.AddMongobd(pA))
{
    Console.WriteLine("Adicionado");
}
else
{
    Console.WriteLine("Erro ao adicionar");
}*/