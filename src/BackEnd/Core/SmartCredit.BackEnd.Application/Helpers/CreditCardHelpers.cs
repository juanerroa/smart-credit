using System;
using SmartCredit.BackEnd.Domain.Enums;

namespace SmartCredit.BackEnd.Application.Helpers
{
    public static class CreditCardHelpers
    {
        public static CreditCardType? GetCreditCardType(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return null;

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            // Validación para Visa
            if (cardNumber.StartsWith("4") && (cardNumber.Length == 13 || cardNumber.Length == 16))
                return CreditCardType.Visa;

            // Validación para MasterCard
            if (cardNumber.Length == 16 && int.TryParse(cardNumber.Substring(0, 2), out var prefix) &&
                prefix >= 51 && prefix <= 55)
                return CreditCardType.MasterCard;

            // Validación para American Express
            if (cardNumber.Length == 15 &&
                (cardNumber.StartsWith("34") || cardNumber.StartsWith("37")))
                return CreditCardType.AmericanExpress;

            // Si no coincide con ninguno, retorna null
            return null;
        }
    }
}
