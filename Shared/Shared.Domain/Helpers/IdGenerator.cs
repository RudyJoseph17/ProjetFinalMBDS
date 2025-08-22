using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Helpers
{
    public class IdGenerator
    {
        private static readonly Random _random = new();

        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
        {
            { "IdIdentificationProjet", "XXB-BB-XX-BX" },
        };

        public static string GenererIdPour(string nomEntite)
        {
            if (_patterns.TryGetValue(nomEntite, out var pattern))
            {
                return RandomPattern(pattern);
            }
            throw new ArgumentException($"Aucun format d'ID défini pour '{nomEntite}'", nameof(nomEntite));
        }

        public static string GenererIdAvecPattern(string pattern) => RandomPattern(pattern);

        private static string RandomPattern(string pattern)
        {
            var sb = new StringBuilder();
            foreach (var c in pattern)
            {
                sb.Append(c switch
                {
                    'X' => RandomChar(),
                    'B' => RandomDigit(),
                    _ => c
                });
            }
            return sb.ToString();
        }

        private static char RandomChar() => (char)_random.Next('A', 'Z' + 1);
        private static char RandomDigit() => (char)_random.Next('0', '9' + 1);
    }
}
