using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using projetua3.Domain.Entities;

namespace projetua3.Infrastructure
{
    /// <summary>
    /// Convertisseur JSON personnalise pour serialiser et deserialiser les comptes bancaires
    /// Gere le polymorphisme entre CurrentAccount et SavingsAccount
    /// </summary>
    public class AccountJsonConverter : JsonConverter<Account>
    {
        /// <summary>
        /// Deserialise un compte depuis le format JSON
        /// Determine le type de compte a partir de la propriete AccountType
        /// </summary>
        /// <param name="reader">Lecteur JSON</param>
        /// <param name="typeToConvert">Type a convertir</param>
        /// <param name="options">Options de serialisation</param>
        /// <returns>Instance du compte deserialise</returns>
        public override Account Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var jsonObject = jsonDoc.RootElement;

                if (!jsonObject.TryGetProperty("AccountType", out var accountTypeProperty))
                    throw new JsonException("Propriete 'AccountType' manquante dans le JSON.");

                string accountType = accountTypeProperty.GetString();

                // Creer une copie des options sans ce converter pour eviter la recursion infinie
                var newOptions = new JsonSerializerOptions(options);
                newOptions.Converters.Clear();

                Account account = accountType switch
                {
                    "CurrentAccount" => JsonSerializer.Deserialize<CurrentAccount>(jsonObject.GetRawText(), newOptions),
                    "SavingsAccount" => JsonSerializer.Deserialize<SavingsAccount>(jsonObject.GetRawText(), newOptions),
                    _ => throw new JsonException($"Type de compte inconnu : {accountType}")
                };

                return account;
            }
        }

        /// <summary>
        /// Serialise un compte vers le format JSON
        /// Ajoute automatiquement la propriete AccountType pour identifier le type
        /// </summary>
        /// <param name="writer">Ecrivain JSON</param>
        /// <param name="value">Compte a serialiser</param>
        /// <param name="options">Options de serialisation</param>
        public override void Write(Utf8JsonWriter writer, Account value, JsonSerializerOptions options)
        {
            // Creer une copie des options sans ce converter pour eviter la recursion infinie
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Clear();

            using (var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), newOptions)))
            {
                writer.WriteStartObject();

                // Copier toutes les proprietes existantes
                foreach (var prop in jsonDoc.RootElement.EnumerateObject())
                {
                    prop.WriteTo(writer);
                }

                // Ajouter le discriminateur de type
                writer.WriteString("AccountType", value.GetType().Name);

                writer.WriteEndObject();
            }
        }
    }
}

