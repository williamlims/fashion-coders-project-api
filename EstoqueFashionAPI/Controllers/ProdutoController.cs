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
        
        //Retornar lista de todos os produtos
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
        
        //Adicionar produto na lista
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

                    //VALIDAÇÕES com retorno de mensagens                    
                    if (!((produto.Status).Equals(0)) && !((produto.Status).Equals(1)))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Status' com 1(Produto ativado) ou 0(Produto desativado)!");
                    }
                    else if (string.IsNullOrEmpty(produto.Descricao))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Descrição'!");
                    }
                    else if (string.IsNullOrEmpty(produto.Categoria))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Categoria'!");
                    }
                    else if ((produto.Categoria == "Feminino") ||(produto.Categoria == "Masculino") || (produto.Categoria == "Infantil")))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Categoria' com Feminino, Masculino ou Infantil!");
                    }                       
                    else if (produto.Quantidade <= 0)
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Quantidade' com valor maior que 0!");
                    } 
                    else if (produto.Custo <= 0)
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Custo' com valor maior que 00.00!");
                    } 

                    //Valores enviados por Json atribuidos aos campos da query
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
            return new JsonResult("Produto adicionado!");
        }
        
        //Editar um produto
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
                    //VALIDAÇÕES com retorno de mensagens
                    if (produto == null)
                    {
                        return new JsonResult("É obrigatório o preenchimento ds campos!");
                    }                    
                    else if (!((produto.Status).Equals(0)) && !((produto.Status).Equals(1)))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Status' com 1(Produto ativado) ou 0(Produto desativado)!");
                    }
                    else if(string.IsNullOrEmpty(produto.Descricao))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Descrição'!");
                    }
                    else if (string.IsNullOrEmpty(produto.Categoria) ||
                       produto.Categoria == "Feminino" ||
                       produto.Categoria == "Masculino" ||
                       produto.Categoria == "Infantil")
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Categoria' com Feminino, Masculino ou Infantil!");
                    }
                    else if (produto.Quantidade <= 0)
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Quantidade' com valor maior que 0!");
                    }
                    else if (produto.Custo <= 0)
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Custo' com valor maior que 00.00!");
                    }                    

                    //Valores enviados por Json atribuidos aos campos da query
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@status", produto.Status);
                    //retira espaço inicial e final e deixa minúsculo
                    myCommand.Parameters.AddWithValue("@descricao", produto.Descricao.Trim().ToLower());
                    myCommand.Parameters.AddWithValue("@categoria", produto.Categoria);                    
                    myCommand.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    //duas casas decimais
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
                
        //Inativar produto
        [HttpPut("{id}/status={status}")]
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
                    if (!(status.Equals(0)) && !(status.Equals(1)))
                    {
                        return new JsonResult("É obrigatório o preenchimento do campo 'Status' com 1(Produto ativado) ou 0(Produto desativado)!");
                    }
                    
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@status", status);

                    myReader = myCommand.ExecuteReader();
                    tabela.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            //Retorno com mensagem correspondente
            if (status == 0)
            {
                return new JsonResult("Produto DESATIVADO");
            }
            return new JsonResult("Produto ATIVADO");
        }              
    }   
}
