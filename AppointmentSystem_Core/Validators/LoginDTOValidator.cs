using AppointmentSystem_Core.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Core.Validators
{
    public class LoginDTOValidator: AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(u => u.TC)
                .NotEmpty().WithMessage("TC alanı boş olamaz.")
                .Length(11).WithMessage("TC alanı 11 karakter olmalıdır.")
                .Matches(@"^\d{11}$").WithMessage("TC sadece rakamlardan oluşmalıdır.")
                .Must(IsValidTCNumber).WithMessage("Geçersiz TC numarası.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Parola alanı boş olamaz.")
                .MinimumLength(8).WithMessage("Parola en az 8 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Parola en fazla 20 karakter olabilir.")
                .Matches(@"[A-Z]").WithMessage("En az bir büyük harf")
                .Matches(@"[a-z]").WithMessage("En az bir küçük harf")
                .Matches(@"[0-9]").WithMessage("En az bir rakam");
            
        }

        // TC numarası geçerlilik kontrolü
        private bool IsValidTCNumber(string tc)
        {
            if (string.IsNullOrEmpty(tc) || tc.Length != 11)
                return false;

            // İlk hanesi 0 olamaz
            if (tc[0] == '0')
                return false;

            // TC algoritması (Türkiye Cumhuriyet Polis Akademisi)
            int sumOdd = 0;  // 1., 3., 5., 7., 9. haneler
            int sumEven = 0; // 2., 4., 6., 8., 10. haneler

            // Tek pozisyon haneleri topla (1., 3., 5., 7., 9.)
            for (int i = 0; i < 10; i += 2)
            {
                sumOdd += int.Parse(tc[i].ToString());
            }

            // Çift pozisyon haneleri topla (2., 4., 6., 8., 10.)
            for (int i = 1; i < 10; i += 2)
            {
                sumEven += int.Parse(tc[i].ToString());
            }

            // 11. hanenin kontrolü
            int tenthDigit = (sumOdd * 7 - sumEven) % 11;
            if (tenthDigit < 0)
                tenthDigit += 11;

            if (int.Parse(tc[10].ToString()) != tenthDigit)
                return false;

            return true;
        }

    }
}
