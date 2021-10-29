using EstoqueFashionAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;

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
                    
                    // API não deixa campo ser nullo. Como tratar o vazio? API ou Front?
                    if (string.IsNullOrEmpty(produto.Descricao))
                    {
                        return new JsonResult("Há campo inválido!");
                    }                  
                    

                    myCommand.Parameters.AddWithValue("@status", produto.Status);
                    //retira espaço inicial e final e deixa minúsculo
                    myCommand.Parameters.AddWithValue("@descricao", produto.Descricao.Trim().ToLower());
                    myCommand.Parameters.AddWithValue("@categoria", produto.Categoria);                    
                    myCommand.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    //duas casas decimais
                    myCommand.Parameters.AddWithValue("@custo", Math.Round(produto.Custo, 2));
                    myCommand.Parameters.AddWithValue("@imagem", produto.Imagem.Trim());

                    myReader = myCommand.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Produto inserido!");
        }
        
        [HttpPut("{id}")]
        public JsonResult Put(Produto produto, int id)
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
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@status", produto.Status);
                    myCommand.Parameters.AddWithValue("@descricao", produto.Descricao.Trim().ToLower());
                    myCommand.Parameters.AddWithValue("@categoria", produto.Categoria);                    
                    myCommand.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    myCommand.Parameters.AddWithValue("@custo", Math.Round(produto.Custo, 2));
                    myCommand.Parameters.AddWithValue("@imagem", produto.Imagem);

                    myReader = myCommand.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Produto atualizado!");
        }
                
        //para inativar produto
        [HttpPut("{id}/status/{status}")]
        public JsonResult Put(int id, int status )
        {

            string query = @"
                            update produto set 
                            status = @status
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
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@status", status);

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
