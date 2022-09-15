using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ToDoList.Models
{
    public class ProjectList
    {
        public int ProjectListId { get; set; }
        [Required]
        public string Project { get; set; }
        //Relationships
        public virtual List<TodoList> TodoLists { get; set; }

    }
}
