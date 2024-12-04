using BanhXeoProject.Data;
using BanhXeoProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace BanhXeoProject.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            // Tìm người dùng trong cơ sở dữ liệu
            var user = _context.User.SingleOrDefault(u => u.Email == username);

            if (user == null)
            {
                return null;  // Người dùng không tồn tại
            }

            // So sánh mật khẩu đã mã hóa trong cơ sở dữ liệu với mật khẩu người dùng nhập vào
            if (!BCrypt.Net.BCrypt.Verify(password, user.PassWordHash))
            {
                return null;  // Mật khẩu không đúng
            }

            return user;  // Đăng nhập thành công
        }
    }
}
