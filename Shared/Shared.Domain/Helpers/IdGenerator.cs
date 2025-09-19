using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Shared.Domain.Helpers
{
    public static class IdGenerator
    {
        private static readonly Random _random = new();
        private static readonly string _storeFile = "generated_ids.json";
        private static readonly ReaderWriterLockSlim _lock = new();
        private static readonly Dictionary<string, HashSet<string>> _generatedIds;

        private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
        {
            { "IdIdentificationProjet", "XXB-BB-XX-BX" }
        };

        // Chargement des IDs persistés au démarrage
        static IdGenerator()
        {
            if (File.Exists(_storeFile))
            {
                try
                {
                    var json = File.ReadAllText(_storeFile);
                    _generatedIds = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(json)
                                    ?? new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
                }
                catch
                {
                    _generatedIds = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
                }
            }
            else
            {
                _generatedIds = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Génère un ID unique pour l’entité spécifiée et le stocke persistement.
        /// </summary>
        public static string GenererIdPour(string nomEntite)
        {
            if (!_patterns.TryGetValue(nomEntite, out var pattern))
                throw new ArgumentException($"Aucun format d'ID défini pour '{nomEntite}'", nameof(nomEntite));

            string id;
            int attempts = 0;

            do
            {
                id = RandomPattern(pattern);
                attempts++;
                if (attempts > 1000)
                    throw new InvalidOperationException("Impossible de générer un ID unique après 1000 tentatives.");
            }
            while (!AddId(nomEntite, id));

            return id;
        }

        /// <summary>
        /// Génère un ID aléatoire selon un pattern.
        /// </summary>
        public static string GenererIdAvecPattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("Pattern invalide.", nameof(pattern));

            return RandomPattern(pattern);
        }

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

        /// <summary>
        /// Ajoute l'ID à la collection et persiste le fichier JSON.
        /// Retourne true si l'ID était unique.
        /// </summary>
        private static bool AddId(string nomEntite, string id)
        {
            _lock.EnterWriteLock();
            try
            {
                if (!_generatedIds.TryGetValue(nomEntite, out var set))
                {
                    set = new HashSet<string>();
                    _generatedIds[nomEntite] = set;
                }

                if (set.Contains(id)) return false;

                set.Add(id);

                // Sauvegarde persistente
                File.WriteAllText(_storeFile, JsonSerializer.Serialize(_generatedIds, new JsonSerializerOptions { WriteIndented = true }));

                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
