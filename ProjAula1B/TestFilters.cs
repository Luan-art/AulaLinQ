using DnsClient.Protocol;
using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjAula1B
{
    internal class TestFilters
    {
        public static void imprimirList(List<PenalidadesAplicadas> records)
        {
            foreach (PenalidadesAplicadas p in records)
            {
                Console.WriteLine(p);
            }
        }

        public static int getCountRecords(List<PenalidadesAplicadas> records) { return records.Count; }

        public static List<PenalidadesAplicadas> ListByCpf(List<PenalidadesAplicadas> records) => records.Where(r => r.Cpf.Contains("237")).ToList();

        public static List<PenalidadesAplicadas> ListByYear(List<PenalidadesAplicadas> records) => records.Where(r => r.VigenciaCadastro.Year == 2021).ToList();

        public static int CountLTDA(List<PenalidadesAplicadas> records) => (records.Count(r => r.RazaoSocial.Contains("LTDA")));

        public static List<PenalidadesAplicadas> OrderByRS(List<PenalidadesAplicadas> records) => (records.OrderBy(r => r.RazaoSocial)).ToList();

        internal static string GenerateXML(List<PenalidadesAplicadas> lst)
        {

            if (lst.Count > 0)
            {

                var penalidadeAplicada = new XElement("Root", from data in lst
                                                              where data.Cpf == "399.***.***-05"
                                                              select new XElement("Motorista",
                                                              new XElement("razao_social", data.RazaoSocial),
                                                              new XElement("cnpj", data.Cnpj),
                                                              new XElement("nome_motorista", data.NomeMot),
                                                              new XElement("cpf", data.Cpf),
                                                              new XElement("vigencia_do_cadastro", data.VigenciaCadastro)));

                return penalidadeAplicada.ToString();
            }
            else
            {
                return "Lista não existe registros";
            }


        }

        internal static List<PenalidadesAplicadas> ExtractDataFromSQL()
        {
            List<PenalidadesAplicadas> records = new List<PenalidadesAplicadas>();

            try
            {
                Banco conn = new Banco();
                SqlConnection conexaosql = new SqlConnection(conn.Caminho());
                conexaosql.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT razaoSocial, cnpj, cpf, vigenciaDoCadastro FROM PenalidadesAplicadas";

                cmd.Connection = conexaosql;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        PenalidadesAplicadas p = new PenalidadesAplicadas
                        {
                            RazaoSocial = reader.GetString(0),
                            Cnpj = reader.GetString(1),
                            Cpf = reader.GetString(2),
                            VigenciaCadastro = reader.GetDateTime(3)
                        };

                        records.Add(p);
                    }

                }
                conexaosql.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }

            return records;
        }

        public static bool AddSQLbd(List<PenalidadesAplicadas> records)
        {
            bool isConect = false;
            Banco conn = new Banco();
            SqlConnection conexaosql = new SqlConnection(conn.Caminho());
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            try
            {
                conexaosql.Open();
                isConect = true;

                int count = records.Count;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexaosql;

                for (int i = 0; i <= (int)Math.Floor((double)records.Count / 1000); i++)
                {
                    string insert = "INSERT INTO PenalidadesAplicadas (razaoSocial, cnpj, cpf, vigenciaDoCadastro) VALUES ";

                    foreach (var item in records.Skip(1000 * i).Take(1000))
                    {
                        if (item.Cpf != null)
                        {
                            insert += $"('{item.RazaoSocial.Replace("'", "''")}', " +
                                      $"'{item.Cnpj}', " +
                                      $"'{item.Cpf}', " +
                                      $"'{item.VigenciaCadastro:yyyy-MM-dd}'),";
                        }
                    }

                    if (insert.EndsWith(","))
                    {
                        insert = insert.Substring(0, insert.Length - 1);  
                        cmd.CommandText = insert;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine($"--ERRO AO INSERIR QUERY SQL:\n" +
                                              $"cod.: {e.ErrorCode}\n" +
                                              $"msg: {e.Message}\n");
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"--ERRO AO CONECTAR AO BANCO DE DADOS:\n" +
                                  $"msg: {e.Message}\n");
                return false;
            }
            finally
            {
                if (conexaosql.State == System.Data.ConnectionState.Open)
                {
                    conexaosql.Close();
                }
            }

            watch.Stop();
            Console.WriteLine($"Levou {watch.ElapsedMilliseconds} milissegundos!");

            return true;
        }

        internal static bool AddMongobd(List<PenalidadesAplicadas> pA)
        {
            try
            {
                MongoDB conn = new MongoDB();
                MongoClient client = new MongoClient(conn.Caminho());
                var database = client.GetDatabase("PenalidadesAplicadas");
                var collection = database.GetCollection<BsonDocument>("penalidades");

                List<BsonDocument> documents = new List<BsonDocument>();

                foreach (var record in pA)
                {
                    BsonDocument document = new BsonDocument
                    {
                        { "razaoSocial", record.RazaoSocial },
                        { "cnpj", record.Cnpj },
                        { "cpf", record.Cpf },
                        { "vigenciaDoCadastro", record.VigenciaCadastro }
                    };

                    documents.Add(document);
                }
                collection.InsertMany(documents);

                InsertMetadata("Sucesso", DateTime.Now, pA.Count);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar documentos ao MongoDB: " + ex.Message);
                return false;
            }
        }

        private static void InsertMetadata(string desc, DateTime data, int number)
        {
            Banco conn = new Banco();
            SqlConnection conexaosql = new SqlConnection(conn.Caminho());
            conexaosql.Open();


            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO ControleProcessamento (descricao, datetim, numeroDeLinhas) VALUES (@descricao, @datetim, @numeroDeLinhas);";

                cmd.Parameters.Add("@descricao", System.Data.SqlDbType.VarChar, 50).Value = desc;
                cmd.Parameters.Add("@datetim", System.Data.SqlDbType.DateTime).Value = data;
                cmd.Parameters.Add("@numeroDeLinhas", System.Data.SqlDbType.VarChar, 14).Value = number;

                cmd.Connection = conexaosql;
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar com o banco");
                Console.WriteLine(ex.Message);
            }

            conexaosql.Close();

        }
    }
}
