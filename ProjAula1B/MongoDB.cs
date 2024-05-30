using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAula1B
{
    internal class MongoDB
    {
        string Conexao = "mongodb://root:Mongo%402024%23@localhost:27017/";

        public MongoDB()
        {

        }
        public string Caminho()
        {
            return Conexao;
        }
    }
}
