﻿using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(UserProfile userProfile)
        {
            _context.Add(userProfile);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(UserProfile userProfile)
        {
            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<UserProfile>> GetAll()
        {
            return await _context.UserProfile.ToListAsync();
        }

        public async Task<UserProfile> Get(string userId)
        {
            return await _context.UserProfile.AsNoTracking().FirstOrDefaultAsync(x => x.UserID == userId);
        }

        public async Task<UserProfile> Update(UserProfile userProfileChanges)
        {
            var user = _context.UserProfile.Attach(userProfileChanges);
            user.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userProfileChanges;
        }

    }
}
