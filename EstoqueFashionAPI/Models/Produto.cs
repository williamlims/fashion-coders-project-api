using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueFashionAPI.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string CategoriaProduto { get; set; }
        public int QuantidadeProduto { get; set; }
        public double CustoProduto { get; set; }
        public string ImagemProduto { get; set; }        
    }
}
