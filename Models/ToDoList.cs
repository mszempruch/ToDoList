using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace ToDoList.Models
{
    public class TodoList
    {
        public int TodoListId { get; set; }
        [Required]
        public string Content { get; set; }
        //ProjectList
        public int ProjectListId { get; set; }
        [ForeignKey("ProjectListId")]
        public virtual ProjectList Projectlist { get; set; }
    }
}
