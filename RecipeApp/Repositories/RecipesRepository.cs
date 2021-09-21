﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApp.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace RecipeApp.Repositories
{
    public class RecipesRepository : IRecipesRepository
    {
        public async Task<Recipe> Add(Recipe recipe)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                await session.SaveAsync(recipe);
                await transaction.CommitAsync();
                return recipe;
            }
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var a = await session.Query<Recipe>().ToListAsync();
                return a;
            }
        }

        public async Task<Recipe> GetById(Guid recipeId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return await session.GetAsync<Recipe>(recipeId);
        }

        public async Task<List<Recipe>> GetRecipes(Dictionary<string, string> filters)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.Query<Recipe>().Where(b => b.Name.Contains(filters["name"])); 
                if(filters["type"] != "")
                    query = query.Where(b => b.Type.Equals(filters["type"]));
                if(filters["duration"] != "")
                    query = query.Where(b => b.Duration.Equals(Int16.Parse(filters["duration"])));
                return await query.ToListAsync();
            }
        }

        public async Task Remove(Guid id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                var recipe = session.Load<Recipe>(id);
                await session.DeleteAsync(recipe);
                await transaction.CommitAsync();
            }
        }

        public async Task Update(Recipe recipe)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                await session.UpdateAsync(recipe);
                await transaction.CommitAsync();
                //var recipe = await session.GetAsync<Recipe>(recipe.RecipeId)
            }
        }
    }
}