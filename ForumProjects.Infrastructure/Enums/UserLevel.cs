using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumProjects.Infrastructure.Enums
{
    public enum UserLevel
    {
        Admin = 1,  // Yönetici, en yüksek seviye
        Member = 2, // Üye, standart kullanıcı
        Guest = 3   // Misafir, sınırlı erişim
    }
}
