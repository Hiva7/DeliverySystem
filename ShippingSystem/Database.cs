using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public class Database
{
    // private readonly string connectionString;
    // private readonly string databaseName;

    private readonly IMongoDatabase database;
    private readonly MongoClient client;

    private readonly Dictionary<string, Dictionary<string, List<string>>> fieldValidations =
        new Dictionary<string, Dictionary<string, List<string>>>()
        {
            {
                "Order",
                new Dictionary<string, List<string>>
                {
                    {
                        "Status",
                        new List<string> { "Pending", "OnRoute", "Complete" }
                    }
                }
            }
        };

    private static bool ValidateDataType(string myCollection, string fieldName, BsonValue fieldValue)
    {
        // Define the expected data types for each field in each collection
        var expectedDataTypes = new Dictionary<string, Dictionary<string, BsonType>>
        {
            {
                "Customer",
                new Dictionary<string, BsonType>
                {
                    { "First_name", BsonType.String },
                    { "Last_name", BsonType.String },
                    { "Contact", BsonType.String }
                }
            },
            {
                "Driver",
                new Dictionary<string, BsonType>
                {
                    { "First_name", BsonType.String },
                    { "Last_name", BsonType.String },
                    { "Employment_date", BsonType.String },
                    { "License_number", BsonType.String },
                    { "Contact", BsonType.String },
                    { "Address", BsonType.String }
                }
            },
            {
                "Location",
                new Dictionary<string, BsonType>
                {
                    { "Name", BsonType.String },
                    { "CordX", BsonType.Decimal128 },
                    { "CordY", BsonType.Decimal128 }
                }
            },
            {
                "Order",
                new Dictionary<string, BsonType>
                {
                    { "Name", BsonType.String },
                    { "Weight", BsonType.Decimal128 },
                    { "Status", BsonType.String },
                    { "Price", BsonType.Decimal128 },
                    { "Customer_id", BsonType.Int32 },
                    { "Location_id", BsonType.Array },
                    { "Driver_id", BsonType.Int32 }
                }
            }
        };

        if (expectedDataTypes.TryGetValue(myCollection, out var fieldDataTypes))
        {
            if (fieldDataTypes.TryGetValue(fieldName, out var expectedDataType))
            {
                return fieldValue.BsonType == expectedDataType;
            }
        }

        // If the field or collection is not defined in the expectedDataTypes dictionary, assume the data type is valid
        return true;
    }

    private bool ValidateField(string collectionName, string fieldName, string fieldValue)
    {
        if (fieldValue == null)
        {
            return false;
        }
        if (
            fieldValidations.ContainsKey(collectionName)
            && fieldValidations[collectionName].ContainsKey(fieldName)
        )
        {
            return fieldValidations[collectionName][fieldName].Contains(fieldValue);
        }
        // If the field is not in the dictionary, it's considered valid
        return true;
    }

    public Database(string connectionString, string databaseName)
    {
        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            if (database is null)
            {
                throw new Exception("Database not found");
            }
        }
        catch (MongoException ex)
        {
            throw new Exception("Error connecting to the database", ex);
        }
    }

    public List<BsonDocument> GetCollection(string myCollection)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }
        var filter = new BsonDocument();
        var documents = collection.Find(filter).ToList();

        if (!documents.Any())
        {
            Console.WriteLine("No documents found in the collection");
        }

        return documents;
    }

    public List<BsonValue> GetField(string myCollection, string myField)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }
        var filter = new BsonDocument();
        var documents = collection.Find(filter).ToList();

        List<BsonValue> fieldValues = new List<BsonValue>();
        foreach (var document in documents)
        {
            if (document.Contains(myField))
            {
                fieldValues.Add(document[myField]);
            }
            else
            {
                throw new Exception("Field not found");
            }
        }

        return fieldValues;
    }

    public int GetLatestID(string myCollection)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }
        var filter = new BsonDocument();
        var documents = collection.Find(filter).ToList();

        BsonDocument latestDocument = documents.LastOrDefault();
        if (latestDocument != null)
        {
            if (latestDocument.ElementCount > 1)
            {
                BsonElement secondElement = latestDocument.ElementAt(1);

                if (secondElement.Value.IsInt32)
                {
                    return secondElement.Value.AsInt32;
                }
            }
        }

        return 0;
    }

    public List<BsonDocument> SearchRecord(string myCollection, string myField, string myQuery)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }

        var filter = Builders<BsonDocument>.Filter.Regex(
            myField,
            new BsonRegularExpression(myQuery, "i")
        );

        var matchingDocuments = collection.Find(filter).ToList();

        return matchingDocuments;
    }


    public void AddRecord(string myCollection, BsonDocument myRecord)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }
        var documents = collection.Find(new BsonDocument()).ToList();

        int latestId = GetLatestID(myCollection);

        if (latestId == 0)
        {
            myRecord.InsertAt(0, new BsonElement("_id", ObjectId.GenerateNewId()));
            myRecord.InsertAt(1, new BsonElement(myCollection + "_id", 1));
        }
        else
        {
            myRecord.InsertAt(0, new BsonElement("_id", ObjectId.GenerateNewId()));
            myRecord.InsertAt(1, new BsonElement(myCollection + "_id", latestId + 1));
        }

        if (documents.Any())
        {
            var firstDocument = documents.FirstOrDefault();
            if (firstDocument != null)
            {
                for (int i = 2; i < firstDocument.ElementCount; i++)
                {
                    if (firstDocument.ElementAt(i).Name != myRecord.ElementAt(i).Name)
                    {
                        throw new Exception(
                            "The names of the parameters do not match the names of the elements in the collection."
                        );
                    }
                }
            }
        }
        foreach (var element in myRecord)
        {
            if (!ValidateField(myCollection, element.Name, element.Value.ToString()))
            {
                throw new Exception($"Invalid value '{element.Value}' for field '{element.Name}'");
            }
            if (!ValidateDataType(myCollection, element.Name, element.Value))
            {
                throw new Exception($"Invalid data type for field '{element.Name}'");
            }
        }

        collection.InsertOne(myRecord);
        Console.WriteLine("New document has been added");
    }

    public void AddRecord(string myCollection, BsonDocument myRecord, BsonDocument relationship)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }
        var documents = collection.Find(new BsonDocument()).ToList();

        foreach (var element in relationship)
        {
            var relatedCollectionName = element.Name.EndsWith("_id")
                ? element.Name.Substring(0, element.Name.Length - 3)
                : element.Name;
            var relatedCollection = database.GetCollection<BsonDocument>(relatedCollectionName);

            if (element.Value.IsBsonArray)
            {
                foreach (var id in element.Value.AsBsonArray)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq(element.Name, id);
                    var relatedDocument = relatedCollection.Find(filter).FirstOrDefault();

                    if (relatedDocument == null)
                    {
                        throw new Exception(
                            $"No document found in {relatedCollectionName} collection with {element.Name}: {id}"
                        );
                    }
                }
            }
            else
            {
                var filter = Builders<BsonDocument>.Filter.Eq(element.Name, element.Value);
                var relatedDocument = relatedCollection.Find(filter).FirstOrDefault();

                if (relatedDocument == null)
                {
                    throw new Exception(
                        $"No document found in {relatedCollectionName} collection with {element.Name}: {element.Value}"
                    );
                }
            }
        }

        int latestId = GetLatestID(myCollection);
        myRecord.AddRange(relationship);

        if (latestId == 0)
        {
            myRecord.InsertAt(0, new BsonElement("_id", ObjectId.GenerateNewId()));
            myRecord.InsertAt(1, new BsonElement(myCollection + "_id", 1));
        }
        else
        {
            myRecord.InsertAt(0, new BsonElement("_id", ObjectId.GenerateNewId()));
            myRecord.InsertAt(1, new BsonElement(myCollection + "_id", latestId + 1));
        }

        if (documents.Any())
        {
            var firstDocument = documents.FirstOrDefault();
            if (firstDocument != null)
            {
                for (int i = 2; i < firstDocument.ElementCount; i++)
                {
                    if (firstDocument.ElementAt(i).Name != myRecord.ElementAt(i).Name)
                    {
                        throw new Exception(
                            "The names of the parameters do not match the names of the elements in the collection."
                        );
                    }
                }
            }
        }
        foreach (var element in myRecord)
        {
            if (!ValidateField(myCollection, element.Name, element.Value.ToString()))
            {
                throw new Exception($"Invalid value '{element.Value}' for field '{element.Name}'");
            }
            if (!ValidateDataType(myCollection, element.Name, element.Value))
            {
                throw new Exception($"Invalid data type for field '{element.Name}'");
            }
        }

        collection.InsertOne(myRecord);
        Console.WriteLine("New document has been added");
    }

    public void DeleteRecord(string myCollection, int id)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }

        var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
        if (firstDocument is null)
        {
            throw new Exception("No documents found in the collection");
        }

        var idName = firstDocument.Names.ElementAt(1);

        var filter = Builders<BsonDocument>.Filter.Eq(idName, id);

        var document = collection.Find(filter).FirstOrDefault();
        if (document is null)
        {
            throw new Exception("Document not found");
        }

        collection.DeleteOne(filter);

        var updateFilter = Builders<BsonDocument>.Filter.Gt(idName, id);
        var update = Builders<BsonDocument>.Update.Inc(idName, -1);
        collection.UpdateMany(updateFilter, update);
        Console.WriteLine("Record has been deleted");
    }

    public void EditRecord(string myCollection, int id, BsonDocument myRecord)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            throw new Exception("Collection not found");
        }
        foreach (var element in myRecord)
        {
            if (element.Name.EndsWith("_id"))
            {
                throw new Exception("myRecord should not contain a field that ends with '_id'");
            }
        }

        var filter = Builders<BsonDocument>.Filter.Eq(myCollection + "_id", id);
        var update = Builders<BsonDocument>.Update;
        var updateDefinition = new List<UpdateDefinition<BsonDocument>>();

        foreach (var element in myRecord)
        {
            if (!ValidateField(myCollection, element.Name, element.Value.ToString()))
            {
                throw new Exception($"Invalid value '{element.Value}' for field '{element.Name}'");
            }
            if (!ValidateDataType(myCollection, element.Name, element.Value))
            {
                throw new Exception($"Invalid data type for field '{element.Name}'");
            }
        }

        foreach (var element in myRecord)
        {
            updateDefinition.Add(update.Set(element.Name, element.Value));
        }

        var result = collection.UpdateOne(filter, update.Combine(updateDefinition));

        if (result.MatchedCount == 0)
        {
            throw new Exception(
                $"No document found in {myCollection} collection with {myCollection}_id: {id}"
            );
        }

        Console.WriteLine("Document has been updated");
    }

    public void CreateCollection(string myCollection)
    {
        var collection = database.GetCollection<BsonDocument>(myCollection);
        if (collection is null)
        {
            Console.WriteLine("Collection has been created");
        }
        else
        {
            Console.WriteLine("Collection already exists");
        }
    }
}


