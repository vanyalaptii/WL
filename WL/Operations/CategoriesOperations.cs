using System;
using System.Collections.Generic;
using System.Linq;
using WL.Context;
using WL.Model;

namespace WL.Operations
{
    public class CategoriesOperations
    {
        public CategoriesOperations() { }

        public void AddCategory(Category _category)
        {
            using (var Context = new WLContext())
            {
                Context.Categories.Add(_category);
                Context.SaveChanges();
            }
        }

        public void RemoveCategory(Category _category)
        {
            using (var Context = new WLContext())
            {
                Context.Categories.Remove(_category);
                Context.SaveChanges();
            }
        }

        public List<Category> LoadAllCategories()
        {
            using (var Context = new WLContext())
            {
                return Context.Categories.ToList();
            }
        }
    }
}
