using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public static bool AddSQLbd(List<PenalidadesAplicadas> records)
        {
            bool isConect = false;
            Banco conn = new Banco();
            SqlConnection conexaosql = new SqlConnection(conn.Caminho());
            conexaosql.Open();

            foreach (PenalidadesAplicadas p in records)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO PenalidadesAplicadas (razaoSocial, cnpj, cpf, vigenciaDoCadastro) VALUES (@razaoSocial, @cnpj, @cpf, @vigenciaDoCadastro);";

                    cmd.Parameters.Add("@razaoSocial", System.Data.SqlDbType.VarChar, 50).Value = p.RazaoSocial;
                    cmd.Parameters.Add("@cnpj", System.Data.SqlDbType.VarChar, 18).Value = p.Cnpj;
                    cmd.Parameters.Add("@cpf", System.Data.SqlDbType.VarChar, 14).Value = p.Cpf;
                    cmd.Parameters.Add("@vigenciaDoCadastro", System.Data.SqlDbType.Date).Value = p.VigenciaCadastro;

                    cmd.Connection = conexaosql;
                    cmd.ExecuteNonQuery();

                    isConect = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao conectar com o banco");
                    Console.WriteLine(ex.Message);
                    break;
                }
     


            }

            conexaosql.Close();

            return isConect;

        }

        internal static string GenerateXML(List<PenalidadesAplicadas> lst)
        {

            if(lst.Count > 0) {

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
    }
}

