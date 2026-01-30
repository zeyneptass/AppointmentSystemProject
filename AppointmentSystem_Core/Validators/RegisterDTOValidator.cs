using AppointmentSystem_Core.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Validators
{
    public class RegisterDTOValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email giriniz.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur.")
                .Length(2, 50).WithMessage("Ad 2-50 karakter arasında olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur.")
                .Length(2, 50).WithMessage("Soyad 2-50 karakter arasında olmalıdır.");

            RuleFor(x => x.TC)
                .NotEmpty().WithMessage("TC alanı boş olamaz.")
                .Length(11).WithMessage("TC alanı 11 karakter olmalıdır.")
                .Matches(@"^\d{11}$").WithMessage("TC sadece rakamlardan oluşmalıdır.")
                .Must(IsValidTCNumber).WithMessage("Geçersiz TC numarası.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Parola zorunludur.")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Parola en fazla 20 karakter olabilir.")
                .Matches(@"[A-Z]").WithMessage("En az bir büyük harf")
                .Matches(@"[a-z]").WithMessage("En az bir küçük harf")
                .Matches(@"[0-9]").WithMessage("En az bir rakam")
                .Must(IsValidPassword).WithMessage("Şifre çok zayıf.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .Must(IsValidTurkishPhone).WithMessage("Telefon numarası +905XXXXXXXXX formatında olmalıdır.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Doğum tarihi zorunludur.")
                .LessThan(DateTime.Now).WithMessage("Doğum tarihi geçmişte olmalıdır.");
        }

        // TC algoritması
        private bool IsValidTCNumber(string tc)
        {
            // 1. Temel kontroller
            if (string.IsNullOrEmpty(tc) || tc.Length != 11)
                return false;

            // Tüm karakterler rakam mı?
            foreach (char c in tc)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            // İlk hane 0 olamaz
            if (tc[0] == '0')
                return false;

            // 2. 10. haneyi kontrol et
            int sumOdd = 0, sumEven = 0;

            // 1,3,5,7,9. haneler (indeks: 0,2,4,6,8)
            for (int i = 0; i < 9; i += 2)
                sumOdd += tc[i] - '0';

            // 2,4,6,8. haneler (indeks: 1,3,5,7)
            for (int i = 1; i < 9; i += 2)
                sumEven += tc[i] - '0';

            int tenthDigit = (sumOdd * 7 - sumEven) % 10;
            if (tenthDigit < 0) tenthDigit += 10;

            if (tc[9] - '0' != tenthDigit)
                return false;

            // 3. 11. haneyi kontrol et
            int sumFirst10 = 0;
            for (int i = 0; i < 10; i++)
                sumFirst10 += tc[i] - '0';

            int eleventhDigit = sumFirst10 % 10;

            return (tc[10] - '0') == eleventhDigit;
            //Eğer sadece toplama/çıkarma olsaydı, bir hane değiştiğinde sonuç kolay tahmin edilebilirdi. Ama 7 ile çarpma işlemi, küçük değişiklikleri büyütür.
            // 10. hane, hem tek hem çift hanelere bağımlı tek bir haneyi değiştirmek, 10.haneyi etkiler. Bu da birden fazla hatayı yakalama yeteneği sağlar
            // 10. rakam = ( (1+3+5+7+9. rakamların toplamı)×7 - (2+4+6+8. rakamların toplamı) ) mod 10
            // 11.rakam = (1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10.rakamların toplamı) mod 10

            // Geçerli Test TCKN: 12345678903

        }

        // Türkçe telefon formatı
        private bool IsValidTurkishPhone(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;
            return Regex.IsMatch(phoneNumber, @"^(\+90|0)5\d{9}$");
        }

        // Şifre güvenliği
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Ardışık karakterler (aaa, 111)
            if (Regex.IsMatch(password, @"(.)\1{2,}"))
                return false;

            // Klavye düzeni (qwerty, asdfgh)
            // Bu kod, bir parolanın klavyedeki yaygın tuş dizilerini (pattern'ları) içerip içermediğini kontrol ediyor. Amacı, kolay tahmin edilebilir ve güvensiz parolaları engellemek.
            string[] keyboardPatterns = { "qwerty", "asdfgh", "zxcvbn", "12345", "qazwsx" };
            foreach (var pattern in keyboardPatterns)
            {
                if (password.ToLower().Contains(pattern))
                    return false;
            }

            return true;
        }
    }
}
