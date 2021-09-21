using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Models
{
    public class Category
    {
        /* محصولات باید در گروه های مختلفی دسته بندی شوند 
         مثلا محضولات مربوط به برنامه نویسی وب در یک گروه
         محصولات مربوط به برنتمه نویسی ویندوز در یک گروه دیگر*/

        //کلید اصلی  هر گروه
        public int Id { get; set; }
        //نام  گروه
        public string Name { get; set; }
        // توضیح گروه
        public string Description { get; set; }

        public ICollection<CategoryToProduct> CategoryToProducts { get; set; }
    }
}
