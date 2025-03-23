using System;
using System.IO;
using System.Text.Json;

namespace ConsoleApp
{
    public static class SerializationHelper
    {
        public static void WriteToFile<T>(string filePath, T obj)
        {
            var options = GetOptions();
            string jsonString = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static T ReadFromFile<T>(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            var options = GetOptions();
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        private static JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Converters = { new BankAccountConverter(), new BankConverter(), new TransactionConverter() }
            };
        }
    }

    public class BankAccountConverter : System.Text.Json.Serialization.JsonConverter<BankAccount>
    {
        public override BankAccount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var id = root.GetProperty("Id").GetGuid();
                var accountNumber = root.GetProperty("AccountNumber").GetString();
                var ownerName = root.GetProperty("OwnerName").GetString();
                var balance = root.GetProperty("Balance").GetDecimal();
                return new BankAccount(id, accountNumber, ownerName, balance);
            }
        }

        public override void Write(Utf8JsonWriter writer, BankAccount value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Id", value.Id.ToString());
            writer.WriteString("AccountNumber", value.AccountNumber);
            writer.WriteString("OwnerName", value.OwnerName);
            writer.WriteNumber("Balance", value.Balance);
            writer.WriteEndObject();
        }
    }

    public class BankConverter : System.Text.Json.Serialization.JsonConverter<Bank>
    {
        public override Bank Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var id = root.GetProperty("Id").GetGuid();
                var name = root.GetProperty("Name").GetString();
                var bankAccounts = JsonSerializer.Deserialize<BankAccount[]>(root.GetProperty("BankAccounts").GetRawText(), options);
                var transactions = JsonSerializer.Deserialize<Transaction[]>(root.GetProperty("Transactions").GetRawText(), options);
                return new Bank(id, name, bankAccounts, transactions);
            }
        }

        public override void Write(Utf8JsonWriter writer, Bank value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Id", value.Id.ToString());
            writer.WriteString("Name", value.Name);
            writer.WritePropertyName("BankAccounts");
            JsonSerializer.Serialize(writer, value.BankAccounts, options);
            writer.WritePropertyName("Transactions");
            JsonSerializer.Serialize(writer, value.Transactions, options);
            writer.WriteEndObject();
        }
    }

    public class TransactionConverter : System.Text.Json.Serialization.JsonConverter<Transaction>
    {
        public override Transaction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var transactionId = root.GetProperty("TransactionId").GetGuid();
                var senderId = root.GetProperty("SenderId").GetGuid();
                var receiverId = root.GetProperty("ReceiverId").GetGuid();
                var amount = root.GetProperty("Amount").GetDecimal();
                var executionDate = root.GetProperty("ExecutionDate").GetDateTime();
                return new Transaction(transactionId, senderId, receiverId, amount, executionDate);
            }
        }

        public override void Write(Utf8JsonWriter writer, Transaction value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("TransactionId", value.TransactionId.ToString());
            writer.WriteString("SenderId", value.SenderId.ToString());
            writer.WriteString("ReceiverId", value.ReceiverId.ToString());
            writer.WriteNumber("Amount", value.Amount);
            writer.WriteString("ExecutionDate", value.ExecutionDate.Value);
            writer.WriteEndObject();
        }
    }
}
