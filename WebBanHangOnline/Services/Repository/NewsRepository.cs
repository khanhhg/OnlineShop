﻿using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(News news)
        {
            _context.Add(news);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(News news)
        {
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<News>> GetAll()
        {
            return await _context.News.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IList<News>> GetNewHome()
        {
            return await _context.News.OrderByDescending(x => x.CreatedDate).Take(3).ToListAsync();
        }

        public async Task<News> Get(int newsId)
        {
            return await _context.News.FirstOrDefaultAsync(x => x.NewsId == newsId);
        }

        public async Task<News> Update(News newsChanges)
        {
            var news = _context.News.Attach(newsChanges);
            news.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return newsChanges;
        }

        public async Task IsActive(News news)
        {
            news.IsActive = !news.IsActive;
            _context.Entry(news).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
