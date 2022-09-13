﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ToDoList.Models
{
    public class TodoList
    {
        public int TodoListId { get; set; }
        [Required]
        public string Content { get; set; }
        public ICollection<ProjectList> ProjectLists { get; set; }


    }
}
