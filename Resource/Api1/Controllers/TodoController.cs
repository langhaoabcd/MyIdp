using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TodoController : ControllerBase
    {
        [HttpGet("list")]
        public async Task<List<Todo>> GetTodoList()
        {
            var list = new List<Todo>();
            list.Add(new Todo { Id = "111", Subject = "ssss", Complentd = false });
            list.Add(new Todo { Id = "2222", Subject = "aaaaa", Complentd = false });
            return await Task.FromResult(list);
        }
    }


    public class Todo
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public bool Complentd { get; set; }
    }
}
