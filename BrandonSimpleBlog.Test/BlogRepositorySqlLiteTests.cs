using BrandonSimpleBlog.Data;
using BrandonSimpleBlog.Test.TestFixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BrandonSimpleBlog.Test
{
    public class BlogRepositorySqlLiteTests: IClassFixture<SqlLiteTestFixture>
    {
        ApplicationDbContext _context;
        BlogPost testBlogPost = new BlogPost()
        {
            Title = "Test Post",
            Slug="test-post"
        };

        public BlogRepositorySqlLiteTests(SqlLiteTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async void AddPostTest()
        {    
            var blogRepository = new BlogRepository(_context);
            var wasAdded=blogRepository.AddPost(testBlogPost);
            Assert.True(wasAdded);
            Assert.Equal(1, await _context.BlogPosts.CountAsync());            
        }

        [Fact]
        public async void DeletePostTest()
        {
            var blogRepository = new BlogRepository(_context);
            _context.Add(testBlogPost);
            _context.SaveChanges();
            bool wasDeleted = blogRepository.DeletePost(testBlogPost.PostId);
            Assert.True(wasDeleted);
            Assert.Equal(0, await _context.BlogPosts.CountAsync());
        }

        [Fact]
        public async void GetPublishedArchiveMonthsTest()
        {
            var blogRepository = new BlogRepository(_context);
            for (int month = 0; month < 10; month++)
            {
                _context.Add(new BlogPost()
                {
                    DatePublished = DateTime.Now.AddMonths(month),
                    Title="Test Blog " + month + " #1",
                    Slug= "Test-Blog-" + month + "-1",
                    IsPublished =true

                });
                _context.Add(new BlogPost()
                {
                    DatePublished = DateTime.Now.AddMonths(month),
                    Title = "Test Blog " + month +" #2",
                    Slug = "Test-Blog-" + month + "-2",
                    IsPublished =true
                    
                });
                _context.Add(new BlogPost()
                {
                    DatePublished = DateTime.Now.AddMonths(month),
                    Title = "Unpublished Test Blog " + month + " #3",
                    Slug = "Test-Blog-" + month + "-3",
                    IsPublished = false

                });
            }
            _context.SaveChanges();
            var archive = blogRepository.GetPublishedArchiveMonths();
            Assert.Equal(10, archive.Cast<ArchiveEntry>().ToList().Count());
            Assert.Equal(30, await _context.BlogPosts.CountAsync());
        }

        [Fact]
        public void GetPublishedCategories()
        {
            var blogRepository = new BlogRepository(_context);
            _context.Add(new BlogPost()
            {
                DatePublished = DateTime.Now,
                Title = "Test Blog",
                IsPublished = true,
                Categories="First",
                Slug = "test-post"

            });
            _context.Add(new BlogPost()
            {
                DatePublished = DateTime.Now,
                Title = "Test Blog",
                IsPublished = true,
                Categories="First,Second",
                Slug = "test-post"

            });
            _context.Add(new BlogPost()
            {
                DatePublished = DateTime.Now,
                Title = "Test Blog",
                IsPublished = true,
                Categories="Second,Third",
                Slug = "test-post"

            });
            _context.Add(new BlogPost()
            {
                DatePublished = DateTime.Now,
                Title = "Test Blog",
                IsPublished = true,
                Categories = "Fourth",
                Slug = "test-post"

            });
            _context.Add(new BlogPost()
            {
                DatePublished = DateTime.Now,
                Title = "Unpublished Test Blog",
                IsPublished = false,
                Categories = "Fifth",
                Slug = "test-post"

            });
            _context.SaveChanges();
            var catItems = blogRepository.GetCategories(true);
            var catItems2 = blogRepository.GetCategories(false);
            Assert.Equal(4, catItems.Cast<CategoryEntry>().ToList().Count());
            Assert.Equal(5, catItems2.Cast<CategoryEntry>().ToList().Count());
            Assert.Equal(2,catItems.Where(c => c.CategoryName == "First").First().Total);
            Assert.Equal(2, catItems.Where(c => c.CategoryName == "Second").First().Total);
            //test for handling unpublished items
            Assert.Null(catItems.Where(c => c.CategoryName == "Fifth").FirstOrDefault());

        }
    }
}
