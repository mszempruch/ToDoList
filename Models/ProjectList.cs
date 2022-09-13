using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ToDoList.Models
{
    public class ProjectList
    {
        public int Id { get; set; }
        [Required]
        public string Project { get; set; }
        public TodoList TodoList { get; set; }

    }
}
