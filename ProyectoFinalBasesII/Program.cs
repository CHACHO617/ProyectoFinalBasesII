using System;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

class Program
{
    static void Main()
    {
        try
        {
            // Connect to MongoDB
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("ProyectoFinal");

            Console.WriteLine("Connected to MongoDB");

            // Create a collection
            var collection = database.GetCollection<BsonDocument>("Personas");

            // Main loop
            while (true)
            {
                Console.WriteLine("\n-------------------\nChoose an action:");
                Console.WriteLine("1. Create Person");
                Console.WriteLine("2. Read Persons");
                Console.WriteLine("3. Update Person");
                Console.WriteLine("4. Delete Person");
                Console.WriteLine("0. Exit");

                Console.Write("\n-------------------\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        CreatePerson(collection);
                        break;

                    case "2":
                        Console.Clear();
                        ReadPersons(collection);
                        break;

                    case "3":
                        Console.Clear();
                        UpdatePerson(collection);
                        break;

                    case "4":
                        Console.Clear();
                        ReadPersons(collection);
                        DeletePerson(collection);
                        break;

                    case "0":
                        Console.Clear();
                        Console.WriteLine("Exiting the application.");
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CreatePerson(IMongoCollection<BsonDocument> collection)
    {
        // Get input from the user
        Console.Write("Enter cedula: ");
        string cedula = Console.ReadLine();

        // Check if cedula already exists
        if (collection.Find(new BsonDocument("cedula", cedula)).Any())
        {
            Console.WriteLine($"Person with cedula {cedula} already exists.");
            return;
        }

        Console.Write("Enter nombre: ");
        string nombre = Console.ReadLine();

        Console.Write("Enter apellido: ");
        string apellido = Console.ReadLine();

        Console.Write("Enter email: ");
        string email = Console.ReadLine();

        Console.Write("Enter telefono: ");
        string telefono = Console.ReadLine();

        // Create a document
        var document = new BsonDocument
        {
            { "cedula", cedula },
            { "nombre", nombre },
            { "apellido", apellido },
            { "email", email },
            { "telefono", telefono }
        };

        // Insert the document into the collection
        collection.InsertOne(document);

        Console.WriteLine("Person created successfully.");
    }

    static void ReadPersons(IMongoCollection<BsonDocument> collection)
    {
        // Read all documents in the collection
        var persons = collection.Find(new BsonDocument()).ToList();

        Console.WriteLine("Read persons:");

        foreach (var person in persons)
        {
            Console.WriteLine(person);
        }
    }

    static void UpdatePerson(IMongoCollection<BsonDocument> collection)
    {
        // Get input from the user
        Console.Write("Enter cedula of the person to update: ");
        string cedula = Console.ReadLine();

        // Find a person to update
        var filter = Builders<BsonDocument>.Filter.Eq("cedula", cedula);

        // Check if the person exists
        if (!collection.Find(filter).Any())
        {
            Console.WriteLine($"Person with cedula {cedula} not found.");
            return;
        }

        // Get updated information from the user
        Console.Write("Enter new nombre: ");
        string newNombre = Console.ReadLine();

        Console.Write("Enter new apellido: ");
        string newApellido = Console.ReadLine();

        Console.Write("Enter new email: ");
        string newEmail = Console.ReadLine();

        Console.Write("Enter new telefono: ");
        string newTelefono = Console.ReadLine();

        var update = Builders<BsonDocument>.Update
            .Set("nombre", newNombre)
            .Set("apellido", newApellido)
            .Set("email", newEmail)
            .Set("telefono", newTelefono);

        // Update the person
        collection.UpdateOne(filter, update);

        Console.WriteLine("Person updated successfully.");
    }

    static void DeletePerson(IMongoCollection<BsonDocument> collection)
    {
        // Get input from the user
        Console.Write("Enter cedula of the person to delete: ");
        string cedula = Console.ReadLine();

        // Find a person to delete
        var filter = Builders<BsonDocument>.Filter.Eq("cedula", cedula);

        // Check if the person exists
        if (!collection.Find(filter).Any())
        {
            Console.WriteLine($"Person with cedula {cedula} not found.");
            return;
        }

        // Delete the person
        collection.DeleteOne(filter);

        Console.WriteLine("Person deleted successfully.");
    }
}
