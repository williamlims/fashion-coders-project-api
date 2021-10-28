using EstoqueFashionAPI.Models;
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
                            select id, status, descricao, categoria, quantidade, custo, imagem  
                            from produto;
                           ";

            DataTable tabela = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EstoqueAppCon");
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(tabela);
        }

        [HttpPost]
        public JsonResult Post(Produto produto)
        {
            string query = @"
                            insert into produto (status, descricao, categoria, quantidade, custo, imagem)
                            values (@status, @descricao, @categoria, @quantidade, @custo, @imagem);
                            ";

            DataTable tabela = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EstoqueAppCon");
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@status", produto.Status);
                    myCommand.Parameters.AddWithValue("@descricao", produto.Descricao);
                    myCommand.Parameters.AddWithValue("@categoria", produto.Categoria);
                    myCommand.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    myCommand.Parameters.AddWithValue("@custo", produto.Custo);
                    myCommand.Parameters.AddWithValue("@imagem", produto.Imagem);

                    myReader = myCommand.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Produto inserido!");
        }

        [HttpPut]
        public JsonResult Put(Produto produto)
        {
            string query = @"
                            update produto set 
                            status = @status, 
                            descricao = @descricao, 
                            categoria = @categoria, 
                            quantidade = @quantidade, 
                            custo = @custo,  
                            imagem = @imagem
                            where id = @id;
                            ";
            DataTable tabela = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EstoqueAppCon");
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", produto.Id);
                    myCommand.Parameters.AddWithValue("@status", produto.Status);
                    myCommand.Parameters.AddWithValue("@descricao", produto.Descricao);
                    myCommand.Parameters.AddWithValue("@categoria", produto.Categoria);
                    myCommand.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    myCommand.Parameters.AddWithValue("@custo", produto.Custo);
                    myCommand.Parameters.AddWithValue("@imagem", produto.Imagem);
                   
                    myReader = myCommand.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Produto atualizado!");
        }
    }
}
