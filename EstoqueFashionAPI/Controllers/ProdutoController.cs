using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace EstoqueFashionAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProdutoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select idProduto, descricao, categoria, quantidade, custo, imagem  
                            from produto";

            DataTable tabela = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EstoqueAppCon");
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myComando = new MySqlCommand(query, mycon))
                {
                    myReader = myComando.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(tabela);
        }
    }
}
