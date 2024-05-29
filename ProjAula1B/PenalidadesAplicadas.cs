using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjAula1B
{
    public class PenalidadesAplicadas
    {
        [JsonProperty("razao_social")]
        public string RazaoSocial {  get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }
        
        [JsonProperty("nome_motorista")]
        public string NomeMot {  get; set; }
        
        [JsonProperty("cpf")]
        public string Cpf { get; set; }
        
        [JsonProperty("vigencia_do_cadastro")]
        public DateTime VigenciaCadastro { get; set; }

        public override string? ToString() => $"RazaoSocial: {RazaoSocial}, Cnpj: {Cnpj}, NomeMot: {NomeMot}, Cpf: {Cpf}, VigenciaCadastro: {VigenciaCadastro}\n";
        
    }


}
