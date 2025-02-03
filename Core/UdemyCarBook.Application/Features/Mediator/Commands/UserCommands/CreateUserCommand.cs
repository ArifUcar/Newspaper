using System;
using System.Collections.Generic;
using MediatR;
using UdemyCarBook.Domain.Enums;

namespace UdemyCarBook.Application.Features.Mediator.Commands.UserCommands
{
    public class CreateUserCommand : IRequest
    {
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// E-posta adresi
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Şifre
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Ad
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Soyad
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Telefon numarası
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Kullanıcı tipi (1: SuperAdmin, 2: Admin, 3: Editor, 4: Author, 5: User)
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// Aktif/Pasif durumu
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Kullanıcı rolleri
        /// </summary>
        public List<string> Roles { get; set; } = new List<string> { "USER" };
    }
} 