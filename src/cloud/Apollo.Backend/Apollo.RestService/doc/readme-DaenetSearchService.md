# Daenet Seach Service Documentation

## Table of Contents


 - [Vector Databases](#vector-databases)
     - [What is a Vector](#what-is-a-vector)
     - [What are Vector Databases](#what-are-vector-databases)
     - [How do they work](#how-do-they-work)
  - [Qdrant Vector Database](#qdrant-vector-database)
    - [Create collection](#crate-collection)
    - [Populating](#populating)
    - [Querying](#querying)
    - [Filtering with Query](#filtering-with-query)
  - [Embeddings](#embeddings)
    - [What are embeddings](#what-are-embeddings)
    - [How do embeddings work](#how-do-embeddings-work)
    - [Text Chunk Processing](#text-chunk-processing)
    - [How to calculate Emeddings](#how-to-calculate-emeddings)
  - [Crawler](#crawler)
    - [What is a Crawler](#what-is-a-crawler)
    - [How to build a web crawler](#how-to-build-a-web-crawler)
    - [Directory Crawler](#directory-crawler)
    - [Running a website crawler in docker container](#running-a-website-crawler-in-docker-container)

## Vector Databases

 ### What is a Vector

 A Vektor is just an array of numbers:

 
    [0, 1, 2, 3, 4, ....]
 

 But they can represent more complex objects like words, sentences, images, or audio files in a continuous high dimensional space called an Embedding. To put it simply, different objects like lamps, cables and whales are grouped together. That is how embeddings work except they map semantic meaning of words together or similar features in virtually any other data type. 


 These embeddings can then be used for things like recommendation systems, search engines and text generation like ChatGPT. Once you have the embeddings the question becomes where do you store and query them quickly? And that’s when the vector databases come into question.

  ### What are Vector Databases

A vector database indexes and stores vector embeddings for fast retrieval and similarity search.  In Vector Database you have arrays of numbers clustered together based on similarity, which can be queried with low latency. This makes it an ideal choice for AI driven applications.

### How do they work

First it uses clever algorithms to calculate the so-called Vector embeddings this is done by Machine learning models. A vector embedding is just a list of numbers that represents the data in a different way.

   unstructured data -> Model -> Vector Embedding

    [0.1 0.2 0.9 0.4 0.7...]

For example you can calculate an embedding for a single word a whole sentence or an image. With that we have numerical data that the computer can understand.

    king -> model -> [embedding]
    man -> model -> [embedding] 
    woman -> model -> [embedding]


One easy possibility we get with vectors is to find similar vectors by calculating the distances and doing a “nearest neighbour search”.

We can take a 2D Vector as an example but of course these vectors can have hundrands of dimentions.

Just storing the data as embeddings is not enough to performing a query across thousands of vectors based on its distance metric, which would be extremely slow. Also this is the reason the vectors also need to be indexed.

The indexing process is the second key element of a vector database. An index is a data structure that facilitates the search process. So, the indexing step maps the vectors to a new data structure that will enable faster searching.

    unstructured data -> embedding model -> Index | embeddings

For example, consider a library system where books are arranged randomly. To find a book, you would need to look through each book until you find the one you're looking for, which is inefficient. If the books are indexed in a catalog by title, author, or subject, you can quickly look up a book in the catalog to find its location in the library. In database terms, the catalog serves as the "index," and the books are the data stored in the "table." By using the index, you can significantly reduce the time it takes to find the desired book (or data record), especially in large datasets.)

## Qdrant Vector Database

### Create collection
Before conducting search query with Qdrant we need to create a collection which will be storing all our vectors. And we can compare vectors with using the dot in our collection. (Dot product is basically a scalar product, which returns a single number)

 ```sh
PUT collections/training_collection
 {
   "vectors": {
     "size": 5,
     "distance": "Dot"
   }
}
 ```

 OR as C# code:

 ```sh
 using Qdrant.Client.Grpc;
  
 await client.CreateCollectionAsync(
 	collectionName: "training_collection",
 	vectorsConfig: new VectorParams { Size = 5, Distance = Distance.Dot }
 );
 ```

 ### Populating 

 We can add vectors with payload which are connected to our collection which we created in the step before: (payload is the additional information stored along with vectors)

 ```sh
 PUT collections/training_collection/points
  {
   "points": [
      {
        "id": 1,
        "vector": [0.05, 0.61, 0.76, 0.74],
       "payload": {"city": "Koprivnica"}
     },
     {
       "id": 2,
       "vector": [0.19, 0.81, 0.75, 0.11],
       "payload": {"city": "London"}
     },
     {
       "id": 3,
       "vector": [0.36, 0.55, 0.47, 0.94],
       "payload": {"city": "Zagreb"}
     },
     {
       "id": 4,
       "vector": [0.18, 0.01, 0.85, 0.80],
       "payload": {"city": "Dugo Selo"}
     },
     {
       "id": 5,
       "vector": [0.24, 0.18, 0.22, 0.44],
       "payload": {"city": "Legrad"}
     },
     {
       "id": 6,
       "vector": [0.35, 0.08, 0.11, 0.44],
       "payload": {"city": "Mumbai"}
     }
   ]
 }
 ```

 OR as C# code:

  ```sh
 using Qdrant.Client.Grpc;
   
 var operationInfo = await client.UpsertAsync(
 	collectionName: "training_collection",
  	points: new List<PointStruct>
  	{
  		new()
  		{
  			Id = 1,
 			Vectors = new float[] { 0.05f, 0.61f, 0.76f, 0.74f },
 			Payload = { ["city"] = "Koprivinca" }
 		},
 		new()
 		{
 			Id = 2,
 			Vectors = new float[] { 0.19f, 0.81f, 0.75f, 0.11f },
 			Payload = { ["city"] = "London" }
 		},
 		new()
 		{
 			Id = 3,
 			Vectors = new float[] { 0.36f, 0.55f, 0.47f, 0.94f },
 			Payload = { ["city"] = " Zagreb " }
 		},
 		// Truncated
 	}
 );
  
Console.WriteLine(operationInfo);  
 ```

 ### Querying

 For example now we can query:
 Which vectors are the most similar to the query vector **[0.2, 0.2, 0.9, 0.4]**  with included filter **“Zagreb”**? It should find closest results that includes “Zagreb”


```sh
 POST collections/training_collection/points/search
  {
    "vector": [0.2, 0.2, 0.9, 0.4],
    "filter": {
      "must": [
        {
         "key": "city",
         "match": { "value": "Zagreb" }
       }
     ]
   },
   "limit": 4,
   "with_payload": true
}
```

OR as C# code:

```sh
using static Qdrant.Client.Grpc.Conditions;
  
 var searchResult = await client.SearchAsync(
  	collectionName: "training_collection",
  	vector: new float[] { 0.2f, 0.2f, 0.9f, 0.4f },
  	filter: MatchKeyword("city", "Zagreb"),
  	limit: 4,
  	payloadSelector: true
  );  
  
 Console.WriteLine(searchResult);
```

Resonse:

```sh
 {
    "result": [
     {
        "id": 2,
        "version": 0,
        "score": 0.871,
        "payload": {
          "city": "Zagreb"
       },
       "vector": null
     }
   ],
   "status": "ok",
   "time": 0.001106114
 }  
```

The same thing will be returned as C# code but in different order.

**NOTE**: The score gives you how good of a match the result is. In our example obove result is a 87% match to our query.

### Filtering with Query

Filtering with **must** is equivalent to the operator AND,and the clause only becomes true when both conditions are satisfied.
Filtering with **should** is  is equivalent to the operator OR  and the clause only becomes true when one of the conditions is satisfied.
Filtering with **must_not** is equivalent to the operator (NOT A) AND (NOT B) AND (NOT C) and the clause becomes true if none if the conditions listed inside **should** is satisfied.

Examples for filtering query can be found at Qdrants documentation: https://qdrant.tech/documentation/quick-start/#add-a-filter

## Embeddings

## What are embeddings


I have explained what embeddings are before but for brief review: A vector embedding is essentially a list (or array) of numbers that represents data—like text, images, audio, or any other type of information—in a high-dimensional space. This numerical representation captures important characteristics of the data, allowing machines to understand and process it more effectively.


## How do embeddings work


**NOTE**: Embeddings can vary significantly between different models and platforms, such as OpenAI and Hugging Face.

As an example here I used OpenAI as my model. I uploaded a file words.csv into the collection as a list of words that I want to embed.


```sh
text

red
potatoes
soda
cheese
water
blue
crispy
hamburger
coffee
green
milk
la croix
yellow
chocolate
french fries
latte
cake
brown
cheeseburger
espresso
cheesecake
black
mocha
fizzy
carbon
banana
```

Now we can calculate the word embeddings or to put it simply we convert these words into vectors. In my example I will use OpenAI embedder model for embedding, which is well documented by OpenAI.

For our example instead of using this API request, we will create this function:

### Text and Word Embedding
```sh
     public static async Task Main(string[] args)
     {
         string text = "Your text here"; // Replace with the text you want to embed
         var embedding = await GetEmbeddingAsync(text);
         Console.WriteLine($"Embedding: {embedding}");
     }
  
     public static async Task<float[]> GetEmbeddingAsync(string text, string model = "text-embedding-3-small")
     {
         text = text.Replace("\n", " ");
         var payload = new
         {
             input = new string[] { text },
             model = model
         };
  
         var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
         client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
  
         var response = await client.PostAsync(apiEndpoint, content);
         response.EnsureSuccessStatusCode();
         var jsonResponse = await response.Content.ReadAsStringAsync();
         var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
  
         // Extracting the embedding
         var embedding = data.data[0].embedding.ToObject<float[]>();
         return embedding;
     }
```

This way I can pass a list of words called with the function **GetEmbeddingAsync** and choose a model.


For example i can try embedding the word **cappuccino**:

```sh
// The word you want to get the embedding for
         string word = "cappuccino";
  
         // Call the GetEmbeddingAsync function with the word
         float[] embedding = await GetEmbeddingAsync(word);
  
         // Output the embedding to the console
         Console.WriteLine($"Embedding for '{word}': [{string.Join(", ", embedding)}]");
  ```

The function sends the string and converts this word cappuccino into a vector. But I have a total of 26 words and I want to convert all of them at once and also create a new column with all the vectors right next to them.

Converting sentences into vectors:
```sh
public class Program
 {
     public static async Task Main(string[] args)
     {
         // Example list of texts to be converted to embeddings
         var texts = new List<TextData>
         {
             new TextData { Text = "Hello, world!" },
             new TextData { Text = "How are you today?" },
             new TextData { Text = "This is an example of text embedding." },
             // Add more texts as needed
         };
  
         // Convert each text to an embedding and update the list
         var tasks = texts.Select(async textData =>
         {
             textData.Embedding = await GetEmbeddingAsync(textData.Text, "text-embedding-3-small");
         });
  
         // Await all the tasks to complete
         await Task.WhenAll(tasks);
  
         // Save the updated list to a CSV file
         using (var writer = new StreamWriter("word_embeddings.csv"))
         using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
         {
             csv.WriteRecords(texts);
         }
     }
  
     // Assume GetEmbeddingAsync is defined here as in the previous example
  
     public class TextData
     {
         public string Text { get; set; }
         public float[] Embedding { get; set; }
     }
 }
```

This program is designed for reading text data and their associated embeddings from a CSV file, demonstrating the use of the CsvHelper library for custom data loading and  processing.

```sh
public class Program
 {
     public static void Main(string[] args)
     {
         // Define the path to your CSV file
         string filePath = "word_embeddings.csv";
  
         // Read the CSV file and convert the "embedding" column
         var records = ReadCsvFile(filePath);
  
         // Example of accessing the loaded data
         foreach (var record in records)
         {
             Console.WriteLine($"Text: {record.Text}, Embedding: [{string.Join(", ", record.Embedding)}]");
         }
     }
  
     public static List<TextData> ReadCsvFile(string filePath)
     {
         using (var reader = new StreamReader(filePath))
         using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
         {
             csv.Context.RegisterClassMap<TextDataMap>();
             var records = csv.GetRecords<TextData>().ToList();
             return records;
         }
     }
  
     public class TextData
     {
         public string Text { get; set; }
         public List<double> Embedding { get; set; }
     }
  
     public class TextDataMap : ClassMap<TextData>
     {
         public TextDataMap()
         {
             Map(m => m.Text);
             Map(m => m.Embedding).ConvertUsing(row =>
                 JsonConvert.DeserializeObject<List<double>>(row.GetField("embedding")));
         }
     }
 }
 ```

 Result would be something like this ( we are extracting these from the word.cvs file shown before):

 | Index | Text | embedding |
| :-------------- | :-------------: | --------------: |
| 0    | red    | [-0.02211449,-0.0109130...]    |
| 1    | potatoes   | -[0.025340], -0.04163]    |
| 2    | soda    | [0.01847, -0.25029]    |
| ...   | ...    | ... |


Now when we converted these words into embeddings, we can perform something like similarity searches. For example, I can perform a similarity search for the word cappuccino with the cosine similarity function.

•	The cosine similarity measure is used to find words similar to "cappuccino" by comparing their vectors.

Result:

 | Index | Text | embedding | similarities
| :-------------- | :-------------: | --------------: | 
| 19    | espresso    | [-0.02211449,-0.0109130...]    | 0.675349
| 22    | mocha   | -[0.025340], -0.04163]    | 0.503967
| 8    | coffee    | [0.01847, -0.25029]    | 0.501959
| ...   | ...    | ... | ...


The cosine equation in code would look like this:


```sh
 public class Program
 {
      public static void Main()
      {
          double[] v1 = { 1, 2, 3 };
          double[] v2 = { 4, 5, 6 };
   
         double dotProduct = DotProduct(v1, v2);
         double magnitudeV1 = Magnitude(v1);
         double magnitudeV2 = Magnitude(v2);
  
         double cosineSimilarity = dotProduct / (magnitudeV1 * magnitudeV2);
         Console.WriteLine(cosineSimilarity);
     }
  
     private static double DotProduct(double[] v1, double[] v2)
     {
         double dotProduct = 0;
         for (int i = 0; i < v1.Length; i++)
         {
             dotProduct += v1[i] * v2[i];
         }
         return dotProduct;
     }
  
     private static double Magnitude(double[] vector)
     {
         double sumSquare = 0;
         foreach (double v in vector)
         {
             sumSquare += v * v;
         }
         return Math.Sqrt(sumSquare);
     }
 }
  ```


For calculating embeddings similar approach goes for other models.

### Text Chunk Processing

Step 1: The PDF file is loaded and read.

    •	All the text is extracted and divided into smaller chunks.
    •	Each chunk contains 500 characters and is stored in a list.

Step 2: Embeddings are generated using OpenAI's Text Embedding model.


Step 1

**Parse document**:

```sh
public class Program
{
    public static void Main(string[] args)
    {
        var filePath = "resume-2024.pdf"; //  not actual path
        var fullText = ExtractTextFromPdf(filePath);
        var chunks = DivideTextIntoChunks(fullText, 500);

        // Example: print out the chunks
        foreach (var chunk in chunks)
        {
            Console.WriteLine($"Chunk: {chunk}\n\n---\n");
        }
    }

    private static string ExtractTextFromPdf(string filePath)
    {
        using (var pdf = PdfDocument.Open(filePath))
        {
            var text = string.Empty;

            foreach (var page in pdf.GetPages())
            {
                text += page.Text;
            }

            return text;
        }
    }

    private static List<string> DivideTextIntoChunks(string text, int chunkSize)
    {
        var chunks = new List<string>();

        while (text.Length > chunkSize)
        {
            var lastPeriodIndex = text.Substring(0, chunkSize).LastIndexOf('.');
            if (lastPeriodIndex == -1)
            {
                lastPeriodIndex = chunkSize;
            }
            else
            {
                // Include the period in the chunk
                lastPeriodIndex += 1;
            }

            chunks.Add(text.Substring(0, lastPeriodIndex));
            text = text.Substring(lastPeriodIndex).TrimStart();
        }

        chunks.Add(text); // Add remaining text as the last chunk

        return chunks;
    }
}
 ```

 **Generate embeddings**:

```sh
 public class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiKey = "your_openai_api_key"; // Replace with your actual OpenAI API key--- in reality this wouldn’t be stored here

    public static async Task Main(string[] args)
    {
        // Assuming 'chunks' is a List<string> containing your text chunks
        var chunks = new List<string>(); // Populate this with your chunks

        var points = new List<PointStruct>();
        int i = 0;

        foreach (var chunk in chunks)
        {
            i++;
            var embeddings = await GetEmbeddingAsync(chunk, "text-embedding-3-small");
            points.Add(new PointStruct(id: i, vector: embeddings, payload: new Dictionary<string, string> { { "text", chunk } }));
        }

        // Proceed to use 'points' as needed
    }

    public static async Task<List<float>> GetEmbeddingAsync(string text, string model)
    {
        var requestBody = new
        {
            input = text,
            model = model
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var response = await client.PostAsync("https://api.openai.com/v1/embeddings", content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<OpenAIEmbeddingResponse>(responseBody);

        return data.Data[0].Embedding;
    }

    public struct PointStruct
    {
        public int Id { get; set; }
        public List<float> Vector { get; set; }
        public Dictionary<string, string> Payload { get; set; }

        public PointStruct(int id, List<float> vector, Dictionary<string, string> payload)
        {
            Id = id;
            Vector = vector;
            Payload = payload;
        }
    }

    // Define a minimal structure to capture the necessary part of the OpenAI API response
    private class OpenAIEmbeddingResponse
    {
        public List<EmbeddingData> Data { get; set; }
    }

    private class EmbeddingData
    {
        public List<float> Embedding { get; set; }
    }
}
 ```


  **Data Loading and Database Setup**

•	The next step involves loading the data into the database.

    •	The vectors or points are loaded into Qdrant, a vector database.

•	A new cluster is created and the API key is copied from the platform.

    •	A new collection is built, and the data is sent to the collection using the "upsert" method.


```sh
public class Program
{
    private static readonly HttpClient client = new HttpClient();
    
    public static async Task Main(string[] args)
    {
        var qdrantUrl = Environment.GetEnvironmentVariable("QDRANT_URL");
        var apiKey = Environment.GetEnvironmentVariable("QDRANT_KEY");
        var port = 6333; // Assuming Qdrant is running on the default port

        // Assuming 'points' is already populated with your data
        var points = new PointStruct[] {}; // Populate this with your data

        // Set up the HttpClient instance for Qdrant
        client.BaseAddress = new Uri($"{qdrantUrl}:{port}");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        // Recreate collection
        await RecreateCollection("demo");

        // Upsert points into the collection
        await UpsertPoints("demo", points);
    }

    private static async Task RecreateCollection(string collectionName)
    {
        var payload = new
        {
            name = collectionName,
            vector_size = 1536,
            distance = "Cosine"
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/collections", content);
        response.EnsureSuccessStatusCode();

        Console.WriteLine("Collection recreated successfully.");
    }

    private static async Task UpsertPoints(string collectionName, PointStruct[] points)
    {
        var payload = new
        {
            collection_name = collectionName,
            points // This needs to match the format expected by Qdrant
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/collections/points/upsert", content);
        response.EnsureSuccessStatusCode();

        Console.WriteLine("Points upserted successfully.");
    }

    // Define your PointStruct here, adjusted for the Qdrant API
    public struct PointStruct
    {
        // Structure according to your data and Qdrant's requirements
    }
}
 ```

  **Querying and Similarity Search**


•	Sample queries are run to find the most relevant documents to answer questions.

    o	An embedding of the question is generated.
    o	Similarity search is performed, comparing the question's embedding with the vectors in the Qdrant collection.

•	The limit parameter defines how many similar results will be returned, in this case, just three.


```sh
public class Program
{
	// In a real scenario, these fields wouldn’t be defined as here
    private static readonly HttpClient httpClient = new HttpClient();
    private static readonly string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    private static readonly string qdrantUrl = Environment.GetEnvironmentVariable("QDRANT_URL");
    private static readonly string qdrantApiKey = Environment.GetEnvironmentVariable("QDRANT_KEY");
    private static readonly int port = 6333;

    public static async Task Main(string[] args)
    {
        var query = "How many years of experience does Daniel have?";
        var answer = await CreateAnswerWithContext(query);
        Console.WriteLine(answer);
    }

    private static async Task<string> CreateAnswerWithContext(string query)
    {
        // Step 1: Generate embedding for the query
        var queryEmbedding = await GetEmbeddingAsync(query, "text-embedding-3-small");

        // Step 2: Perform a similarity search in Qdrant
        var searchResult = await SearchInQdrant("demo", queryEmbedding, 3);

        // Step 3: Create the prompt for GPT-4
        var prompt = new StringBuilder();
        prompt.AppendLine("You are a helpful HR assistant who answers questions in brief based on the context below.");
        prompt.AppendLine("Context:");
        foreach (var result in searchResult)
        {
            prompt.AppendLine(result.Payload["text"] + "\n---");
        }
        prompt.AppendLine("Question:" + query + "\n---\n" + "Answer:");

        // Step 4: Get a completion from OpenAI API based on the prompt
        var answer = await GetCompletionFromOpenAi(prompt.ToString());

        return answer;
    }

    // Assuming implementations for GetEmbeddingAsync, SearchInQdrant, and GetCompletionFromOpenAi exist below, similar to the previous examples.
}
 ```

 Answer to my first question would be:
 More than two decades

 Answer to my second question would be:
 four year ago

 ### How to calculate Emeddings:

Embeddings are vectors. So, if we want to understand how close two sentences are to each other, we can calculate the distance between vectors. A smaller distance would be equivalent to a closer semantic meaning.
Different metrics can be used to measure the distance between two vectors:

    •	Euclidean distance (L2),
    •	Manhattant distance (L1),
    •	Dot product,
    •	Cosine distance.


Let’s discuss them. As a simple example, we will be using two 2D vectors.

    vector1 = [1, 4]
    vector2 = [2, 2]

**Euclidean distance (L2)**

The most standard way to define distance between two points (or vectors) is Euclidean distance or L2 norm.

This metric is the most commonly used in day-to-day life, for example, when we are talking about the distance between 2 towns.

L2 distance = √(∑(xᵢ - yᵢ)²)

```sh
using System;

public class Program
{
    public static void Main()
    {
        int[] vector1 = { 1, 4 };
        int[] vector2 = { 2, 2 };

        double euclideanDistance = CalculateEuclideanDistance(vector1, vector2);
        Console.WriteLine($"Euclidean Distance: {euclideanDistance:F4}");
    }

    private static double CalculateEuclideanDistance(int[] vector1, int[] vector2)
    {
        if (vector1.Length != vector2.Length)
            throw new ArgumentException("Vectors must be of equal length.");

        double sum = 0;
        for (int i = 0; i < vector1.Length; i++)
        {
            sum += Math.Pow(vector1[i] - vector2[i], 2);
        }
        return Math.Sqrt(sum);
    }
}
 ```



**Manhattant distance (L1)**

The other commonly used distance is the L1 norm or Manhattan distance.

This distance was called after the island of Manhattan (New York). This island has a grid layout of streets, and the shortest routes between two points in Manhattan will be L1 distance since you need to follow the grid.

L1 distance = ∑ |xᵢ - yᵢ|

```sh
 public class Program
 {
     public static void Main()
     {
         // Vectors that result in a sum of absolute differences of 3
         var vector1 = new List<int> { 1, 4, 5 };
         var vector2 = new List<int> { 2, 2, 5 };
  
         // Calculating the sum of absolute differences
         int sumOfAbsoluteDifferences = vector1.Zip(vector2, (x, y) => Math.Abs(x - y)).Sum();
  
         // Output the result
         Console.WriteLine(sumOfAbsoluteDifferences);
     }
 }
 ```

 **Dot product**

Another way to look at the distance between vectors is to calculate a dot or scalar product. Here’s a formula and we can easily implement it:

->  ->
x ⋅ y  = ∑(xᵢ * yᵢ)



```sh
public class Program
  {
     public static void Main()
      {
          // Vectors that result in a dot product of 11
         var vector1 = new List<int> { 1, 2, 2 };
         var vector2 = new List<int> { 1, 2, 3 };
  
         // Calculating dot product
         int dotProduct = vector1.Zip(vector2, (x, y) => x * y).Sum();
  
         // Output the result
         Console.WriteLine(dotProduct); 
     }
 }
```


**Cosine similarity**

Quite often, cosine similarity is used. Cosine similarity is a dot product normalised by vectors magnitudes (or normes).


The function cosine_similarity expects 2D arrays.

Cosine similarity is equal to the cosine between two vectors. The closer the vectors are, the higher the metric value.

We can even calculate the exact angle between our vectors in degrees. We get results around 30 degrees, and it looks pretty reasonable.

Cosine Similarity = (x • y) / (||x|| * ||y||)

```sh
using System;

public class Program
{
    public static void Main()
    {
        double[] vector1 = { 1, 4 };
        double[] vector2 = { 2, 2 };

        double cosineSimilarity = CalculateCosineSimilarity(vector1, vector2);
        Console.WriteLine($"Cosine Similarity: {cosineSimilarity:F4}");

        double angleInDegrees = Math.Acos(cosineSimilarity) * (180 / Math.PI);
        Console.WriteLine($"Angle in Degrees: {angleInDegrees:F2}");
    }

    private static double CalculateCosineSimilarity(double[] vector1, double[] vector2)
    {
        if (vector1.Length != vector2.Length)
            throw new ArgumentException("Vectors must be of equal length.");

        double dotProduct = 0;
        double normVector1 = 0;
        double normVector2 = 0;

        for (int i = 0; i < vector1.Length; i++)
        {
            dotProduct += vector1[i] * vector2[i];
            normVector1 += Math.Pow(vector1[i], 2);
            normVector2 += Math.Pow(vector2[i], 2);
        }

        normVector1 = Math.Sqrt(normVector1);
        normVector2 = Math.Sqrt(normVector2);

        return dotProduct / (normVector1 * normVector2);
    }
}
```

## Crawler

 ### What is a Crawler
 A web crawler (also know as a spider or a bot) is a program that automatically navigates through internet by visiting web pages, following links, and collecting data.

Its purposes are typically used for indexing or data extraction. With the data obtained, search engines are able to provide relevant suggestions for users queries. 

A web crawler starts by visiting the seed URL, which is a starting point for the crawl.
Then from there the crawler follows links on the seed URL to find new pages to crawl.  The crawler keeps track of the pages he already visited to avoid visiting the same pages multiple times. A Web crawler typically has a specific goal or a task.

They can be programmed to look for specific types of data such as emails and to follow specific links to obtain certain keywords.

**Overview of how it works:**


•	Root or seed URLs

    	The crawler needs somewhere to start; this is provided by a seed file that can contain one or more known URLs from which the crawler can start from. These are added to a queue.

•	URL queue

    	This is a list of URLs that are waiting to be crawled. For this example, only URLs that have never been visited should find their way into the queue. 
•	The crawl

        The top item is taken from the queue and the web page is obtained. From here, its data could be processed or stored in some way.

•	URL parsing and filtering

    The web page is scouted for other links for our crawler to explore, but before they are added to the queue
    they are checked if they have already been crawled or if they are already in the queue. We can also set rules for URLs what we are not interested in.

### How to build a web crawler

In this example I will be using HTML parser called Html Agility Pack. This library will be used to extracting all the links from an HTML page.
The example itself has two main methods, Initialize and Crawl.

```sh
class Program
  {
     private static Seed seed;
      private static Queue queue;
     private static Crawled crawled;
   
      static async Task Main(string[] args)
      {
          Initialize();
         await Crawl();
     }
 } 
```


In this example, these lists are simply text files, however, database storage can also be considered.

```sh
 static void Initialize()
 {
      string path = Directory.GetCurrentDirectory();
      string seedPath = Path.Combine(path, "Seed.txt");
      string queuePath = Path.Combine(path, "Queue.txt");
      string crawledPath = Path.Combine(path, "Crawled.txt");
   
      seed = new(seedPath);
      var seedURLs = seed.Items;
     queue = new(queuePath, seedURLs);
     crawled = new(crawledPath);
 } 
```

The Seed class represents the list of seed URLs. In this case, our root URL from Develepers.de. There is also a property that returns the full list of seed URLs.

```sh
class Seed
{
    /// <summary>
    /// Returns all seed URLs.
    /// </summary>
    public string[] Items
    {
        get => File.ReadAllLines(path);
    }

    private readonly string path;

    public Seed(string path)
    {
        this.path = path;

        string[] seedURLs = new string[]
        {
            "https://developers.de/2023/10/25/how-to-group-files-together-in-visual-studio/"
        };

        using StreamWriter file = File.CreateText(path);

        foreach (string url in seedURLs)
            file.WriteLine(url.ToCleanURL());
    }
}
```


The Queue class functions as a repository for URLs queued up for crawling. Upon instantiation, it takes a collection of initial seed URLs, which it then records to a file.

This class is equipped with several properties that allow for the retrieval of the queue's first element, every element within the queue, and a flag indicating the presence of URLs within the queue.
Additionally, it provides methods to append a new URL to the queue, extract a URL from the queue, and check for the existence of a specific URL within the queue.

```sh
class Queue
{
    /// <summary>
    /// Returns the first item in the queue.
    /// </summary>
    public string Top
    {
        get => File.ReadAllLines(path).First();
    }

    /// <summary>
    /// Returns all items in the queue
    /// </summary>
    public string[] All
    {
        get => File.ReadAllLines(path);
    }

    /// <summary>
    /// Returns a value based on whether there are URLs in the queue.
    /// </summary>
    public bool HasURLs
    {
        get => File.ReadAllLines(path).Length > 0;
    }

    private readonly string path;

    public Queue(string path, string[] seedURLs)
    {
        this.path = path;

        using StreamWriter file = File.CreateText(path);

        foreach (string url in seedURLs)
            file.WriteLine(url.ToCleanURL());
    }

    public async Task Add(string url)
    {
        using StreamWriter file = new(path, append: true);

        await file.WriteLineAsync(url.ToCleanURL());
    }

    public async Task Remove(string url)
    {
        IEnumerable<string> filteredURLs = All.Where(u => u != url);

        await File.WriteAllLinesAsync(path, filteredURLs);
    }

    public bool IsInQueue(string url) => All.Any(u => u == url);
}
```


The Crawled class serves as a catalog for URLs that have been previously crawled. Upon initialization, it either generates a new file or clears the existing one to ensure a fresh start.

This class is designed with key functionalities, including a method to verify if a URL has already been processed and another method to register a URL to the list.

```sh
  class Crawled
  {
      private readonly string path;
   
      public Crawled(string path)
      {
              this.path = path;
              File.Create(path).Close();
      }
  
     public bool HasBeenCrawled(string url) => File.ReadAllLines(path).Any(c => c == url.ToCleanURL());
  
     public async Task Add(string url)
     {
         using StreamWriter file = new(path, append: true);
  
         await file.WriteLineAsync(url.ToCleanURL());
     }
 }
 ```


 Whenever items are added to any of our three lists, they undergo a sanitization process through an extension method named ToCleanURL.

 This method is part of a string extensions class, which is structured as follows:

```sh
 static class StringExtensions
 {
     public static string ToCleanURL(this string str) => str.Trim().ToLower();
 }
 ```

After the Initialize method is invoked and all necessary components have been set up, the Crawl method is executed.

This method contains a loop designed to continuously iterate if there are URLs remaining in the queue to be processed:

```sh
static async Task Crawl()
{
    do
    {
        string url = queue.Top;

        Crawl crawl = new(url);
        await crawl.Start();

        if (crawl.parsedURLs.Count > 0)
            await ProcessURLs(crawl.parsedURLs);

        await PostCrawl(url);

    } while (queue.HasURLs);
}
 ```

The process begins by retrieving the first URL from the queue, which is then handed over to a newly instantiated Crawl object. Following this, the Start method is invoked to commence the crawling operation for the specified URL.

Within the Crawl class, there are three main attributes: one specifies the URL of the webpage currently under examination, another holds the webpage's content, encapsulating all the HTML data retrieved from that page, and the third maintains a collection of URLs discovered on the webpage.

```sh
class Crawl
{
    public readonly string url;
    private string webPage;
    public List<string> parsedURLs;

    public Crawl(string url)
    {
        this.url = url;
        webPage = null;
        parsedURLs = new List<string>();
    }

    public async Task Start()
    {
        await GetWebPage();

        if (!string.IsNullOrWhiteSpace(webPage))
        {
            ParseContent();
            ParseURLs();
        }
    }

    public async Task GetWebPage()
    {
        using HttpClient client = new();

        client.Timeout = TimeSpan.FromSeconds(60);

        string responseBody = await client.GetStringAsync(url);

        if (!string.IsNullOrWhiteSpace(responseBody))
                webPage = responseBody;
    }

    public void ParseURLs()
    {
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(webPage);

        foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
        {
            string hrefValue = link.GetAttributeValue("href", string.Empty);

            if (hrefValue.StartsWith("http"))
                parsedURLs.Add(hrefValue);
        }
    }

    public void ParseContent()
    {
        // Implementation for content parsing could be done here.
        // Html Agility Pack or other libraries can be used to parse and process the HTML content.
    }
}
 ```


Upon initiating the crawl, the HttpClient is utilized to fetch the webpage's contents. Microsoft recommends using HttpClient due to the obsolescence of WebClient and HttpWebRequest. To avoid prolonged wait times on any single page, a timeout is implemented.

If the content is successfully obtained, two key method calls follow. The first is to ParseContent, an initially empty method intended for processing or saving the page's content, such as indexing information for search engine purposes.

The second method, ParseURLs, leverages the Html Agility Pack to extract a collection of anchor tags a href=... from the HTML content retrieved.

Returning to the main program's Crawl method, should any URLs be discovered within the webpage, they are forwarded to another method called ProcessURLs for further handling.

```sh
 static async Task ProcessURLs(List<string> urls)
 {
     foreach (var url in urls)
     {
         if (!crawled.HasBeenCrawled(url) && !queue.IsInQueue(url))
             await queue.Add(url);
     }
}
 ```

This method iterates through each URL and determines whether it has already been crawled or is already in the queue to be crawled. If neither, the URL is added to the queue.

Finally, the PostCrawl method is invoked, which serves the dual purpose of removing the currently processed URL from the queue—since it has already been crawled—and adding it to the list of URLs that have been successfully crawled.

```sh
 static async Task PostCrawl(string url)
 {
     await queue.Remove(url);
  
     await crawled.Add(url);
 }
 ```

 ### Directory Crawler

 A directory crawler would be implemented very very similarly to the previous web crawler.
 Only differences would be in seed path and minor code changes.

 Also it can be realized as here as well:

 ```sh
 namespace DirectoryCrawler
{
    internal class Program
    {
        private static string targetDirectory;
        private static int inaccessibleDirectoryCount;
        private static int accessibleDirectoryCount;

        private static void Main(string[] args)
        {
            Console.Title = $"Directory Crawler {Assembly.GetExecutingAssembly().GetName().Version.ToString(2)}";
            ConsoleHelper.FixEncoding();

            targetDirectory = ParseArgumentsForTargetDirectory(args);
            if (string.IsNullOrEmpty(targetDirectory))
            {
                DisplayUsageInstructions();
                return;
            }

            ValidateTargetDirectory();

            string outputFileName = GenerateOutputFilename();

            var crawlResults = StartCrawlingDirectories(targetDirectory);
            WriteCrawlResultsToFile(crawlResults, outputFileName);
            WriteCrawlSummaryToConsole(crawlResults, outputFileName);
        }

        private static string ParseArgumentsForTargetDirectory(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.StartsWith("/targetdir=", StringComparison.OrdinalIgnoreCase))
                {
                    return arg.Split(new[] { '=' }, 2)[1];
                }
            }
            return null;
        }

        private static void DisplayUsageInstructions()
        {
            Console.WriteLine("\n    ==========================");
            Console.WriteLine($"    | Directory Crawler v{Assembly.GetExecutingAssembly().GetName().Version.ToString(2)} |");
            Console.WriteLine("    ==========================\n");
            Console.WriteLine("    Usage:");
            Console.WriteLine("      DirectoryCrawler.exe /targetdir=\"<PATH>\"");
            Console.WriteLine("\n    Example:");
            Console.WriteLine("      DirectoryCrawler.exe /targetdir=\"C:\\path\\to\\directory\"\n");
        }

        private static void ValidateTargetDirectory()
        {
            if (!Directory.Exists(targetDirectory))
            {
                Console.WriteLine("ERROR: The specified target directory does not exist.");
                Environment.Exit(1);
            }
        }

        private static string GenerateOutputFilename()
        {
            string dirName = new DirectoryInfo(targetDirectory).Name.Replace(" ", "_");
            dirName = dirName.Length > 10 ? dirName.Substring(0, 10) : dirName;

            return $"{dirName}_{DateTime.Now:yyyyMMddHHmmss}";
        }

        private static (List<string> AccessibleDirectories, long CrawlTime, long WriteTime) StartCrawlingDirectories(string path)
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<string> accessibleDirs = GetDirectoriesRecursively(path);
            sw.Stop();
            long crawlTime = sw.ElapsedMilliseconds;

            return (accessibleDirs, crawlTime, 0);
        }

        private static void WriteCrawlResultsToFile((List<string> AccessibleDirectories, long CrawlTime, long WriteTime) crawlResults, string baseFilename)
        {
            string outputFilename = $"{baseFilename}_OUTPUT.txt";
            Stopwatch sw = Stopwatch.StartNew();
            File.WriteAllLines(outputFilename, crawlResults.AccessibleDirectories);
            sw.Stop();
            crawlResults.WriteTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"OUTPUT FILENAME: {outputFilename}");
        }

        private static void WriteCrawlSummaryToConsole((List<string> AccessibleDirectories, long CrawlTime, long WriteTime) crawlResults, string baseFilename)
        {
            string summaryContent = $"TARGETDIR: {targetDirectory}\nCRAWLTIME: {crawlResults.CrawlTime} ms\nWRITETIME: {crawlResults.WriteTime} ms\nDIRCOUNT: {crawlResults.AccessibleDirectories.Count}\nNOTAUTHORIZED_DIRCOUNT: {inaccessibleDirectoryCount}";
            File.WriteAllText($"{baseFilename}_INFO.txt", summaryContent);

            Console.WriteLine("\nCRAWLING INFO\n=============");
            Console.WriteLine($"Crawling time taken: {crawlResults.CrawlTime} ms");
            Console.WriteLine($"Writing to output file time taken: {crawlResults.WriteTime} ms");
            Console.WriteLine($"Accessible directory count: {crawlResults.AccessibleDirectories.Count}");
            Console.WriteLine($"Inaccessible directory count: {inaccessibleDirectoryCount}");
        }

        private static List<string> GetDirectoriesRecursively(string path, string searchPattern = "*")
        {
            var result = new List<string>();
            try
            {
                var directories = Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly).ToList();
                result.AddRange(directories);
                foreach (var directory in directories)
                {
                    result.AddRange(GetDirectoriesRecursively(directory, searchPattern));
                }
            }
            catch (UnauthorizedAccessException)
            {
                inaccessibleDirectoryCount++;
            }
            return result;
        }
    }
}
  ```


  ### Running a website crawler in docker container

Here are detailed steps on how to run a website crawler in docker container:

Step 1: Create a web crawler project.

Step 2: Implement code for the web crawler which has been done previously in this documentation.

Step 3: Ensure your application is ready for employment.

Step 4: Create a Dockerfile in the root of your C# project with the following content:

Single-Stage Build:

```sh
 # Use the official Microsoft .NET Core runtime
  FROM mcr.microsoft.com/dotnet/runtime:latest
   
  # Set the working directory
  WORKDIR /app
   
  # Copy the built application files from the build context to the container
  COPY bin/Release/netcoreapp3.1/publish/ .
   
 # Command to run the application
 ENTRYPOINT ["dotnet", "WebCrawler.dll"]
  ```

Note: This project targets .NET Core 3.1. Adjust the .NET version accordingly.

Multistage Build:

```sh
 # Use the official Microsoft .NET SDK image to build the application
  FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
  WORKDIR /app
   
  # Copy everything and build the release
  COPY . ./
  RUN dotnet restore
  RUN dotnet publish -c Release -o out
   
 # Generate runtime image
 FROM mcr.microsoft.com/dotnet/runtime:5.0
 WORKDIR /app
 COPY --from=build-env /app/out .
 ENTRYPOINT ["dotnet", "WebCrawler.dll"]
  ```
Adjust the .NET SDK and runtime images (5.0 in the example) to match the version you're targeting. Replace WebCrawler.dll with the name of your application's DLL.

Which one to choose:

    •	For Development: Single-stage builds can be quicker and more straightforward, especially when actively developing and testing changes.
    •	For Production: Multistage builds are generally recommended due to their advantages in consistency, security, and image size. They align with best practices for Docker image creation and deployment.

Step 5: Build the Docker Image:

Open command prompt and navigate to the directory containing your project and Dockerfile, and run:

```sh
docker build -t myWebCrawler .
```

This command builds a Docker image named myWebCrawler based on the instructions in your Dockerfile.


Step 6: Runn your application in a Docker Container:

```sh
docker run myWebCrawler
```

**Additional Considerations**
Dependencies: If your application relies(which it will) on external resources or needs environment variables, ensure these are configured within your Dockerfile or passed to the container at runtime using the -e flag with docker run.

For example: Using Environment Variables

Environment variables are key to making Docker containers flexible and configurable across different environments without changing the code or the Docker image itself. You can use them to pass configuration settings to your application, such as database connection strings, API keys, or service URLs.

To use environment variables within your Dockerfile, you can declare them with the ENV instruction:

```sh
 FROM mcr.microsoft.com/dotnet/aspnet:5.0
 WORKDIR /app
 COPY --from=build-env /app/out .
 ENV ConnectionString=DefaultConnectionString
 ENTRYPOINT ["dotnet", "WebCrawler.dll"]
```

**Passing Environment Variables at Runtime**

When running your container, you can override the ConnectionString (or any other environment variable) using the -e flag with docker run:

```sh
docker run -e ConnectionString=ProductionConnectionString myWebCrawler
```


Example: Connecting to a Database

Let's consider an example where your .NET Core application connects to a MongoDB database, and you want to use environment variables to manage the database connection string.

You can access the MongoDB connection string from an environment variable and use it to configure the MongoDB client in your application:


```sh
 using MongoDB.Driver;
  
 var connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString");
 var client = new MongoClient(connectionString);
 var database = client.GetDatabase("myDbName"); 
```

**Dockerfile Configuration**

In your Dockerfile, you can specify a default MongoDB connection string using the ENV instruction. This might point to a local instance for development purposes or be a placeholder value:

```sh
FROM mcr.microsoft.com/dotnet/aspnet:5.0
 WORKDIR /app
 COPY --from=build-env /app/out .
 # Example default connection string (adjust as needed)
 ENV MongoDBConnectionString=mongodb://localhost:27017/myDb
 ENTRYPOINT ["dotnet", "WebCrawler.dll"]
```

**Running the Container with a MongoDB Connection String**

Especially in production or any other environment, you can override the default connection string by passing a new one as an environment variable using the -e flag:

```sh
docker run -e MongoDBConnectionString="mongodb://prodDbHost:27017/myProdDb" WebCrawler
```
