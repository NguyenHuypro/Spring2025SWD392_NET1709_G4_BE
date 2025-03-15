using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.DAL.Repository.Implement
{
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        public TokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetRefreshTokenByUserID(Guid userId)
        {
            // lấy token đúng id và chưa bị thu hồi
            return await _context.RefreshTokens
                .Where(rt => rt.id == userId && !rt.isRevoked)
                .FirstOrDefaultAsync();
        }
        public async Task<RefreshToken?> GetRefreshTokenByKey(string refreshTokenKey)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenKey))
            {
                throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshTokenKey));
            }

            // Thực hiện truy vấn để tìm RefreshToken theo RefreshTokenKey
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.refreshTokenKey == refreshTokenKey);

            return refreshTokenEntity;
        }
    }

}
