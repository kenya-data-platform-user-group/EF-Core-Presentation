using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI_EFCore.Models;

namespace BlogAPI_EFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> GetBlogs()
        {
            var blogs = await _context.Blogs.Include(b => b.Author).ToListAsync();
            var blogDTOs = blogs.Select(b => new BlogDTO
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                Tags = b.Tags,
                AuthorId = b.AuthorId,
                Author = new AuthorDTO
                {
                    Id = b.Author.Id,
                    Name = b.Author.Name,
                    Email = b.Author.Email
                }
            }).ToList();

            return blogDTOs;
        }

        // GET: api/Blog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDTO>> GetBlog(int id)
        {
            var blog = await _context.Blogs.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            var blogDTO = new BlogDTO
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags,
                AuthorId = blog.AuthorId,
                Author = new AuthorDTO
                {
                    Id = blog.Author.Id,
                    Name = blog.Author.Name,
                    Email = blog.Author.Email
                }
            };

            return blogDTO;
        }

        // PUT: api/Blog/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, BlogDTO blogDTO)
        {
            if (id != blogDTO.Id)
            {
                return BadRequest();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.Title = blogDTO.Title;
            blog.Content = blogDTO.Content;
            blog.Tags = blogDTO.Tags;
            blog.AuthorId = blogDTO.AuthorId;

            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blog
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlogDTO>> PostBlog(BlogDTO blogDTO)
        {
            var blog = new Blog
            {
                Title = blogDTO.Title,
                Content = blogDTO.Content,
                Tags = blogDTO.Tags,
                AuthorId = blogDTO.AuthorId
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            blogDTO.Id = blog.Id;

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blogDTO);
        }

        // DELETE: api/Blog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
