using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueFashionWebApi
{
    public class Produto
    {
        public string descricao { get; set; }
        public int quantidade { get; set; }
        public string data_entrada { get; set; }
        public string data_saida { get; set; }
        public double custo { get; set; }
        public string imagem { get; set; }    

    }
}
