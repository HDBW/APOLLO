# Apollo Semantic Search Worker Documentation


This README.md provides a concise yet comprehensive guide to understanding and utilizing the export functionality, as well as extending it to support new entities.


The Apollo Semantic Search Worker is a tool designed to export Apollo entities (such as Training, User, Profile, etc.) into a CSV file that is compatible with Daenet Semantic Search.

## Architecture View For CSV File Exporter
![image](https://github.com/HDBW/APOLLO/assets/2386584/01676025-19ec-475a-bbf2-ecbf97ef3f89)


## How it works

The export process is carried out by the **BlobStorageExporter** class, which takes in an Apollo API instance, the entity name to be exported, and a connection string to the blob storage where the exported CSV file will be stored.

### Enhanced Export Process Description

**Initialization:** 

The process begins with logging the start of the export for the specified entity (e.g., "training"). This helps in tracking the commencement of the process.

**Query Construction:**

This query specifies the fields to be fetched (like Id, TrainingName, etc.) and any filters necessary to exclude certain records.
The query is logged to ensure transparency in what data is being requested.

**Blob Storage Preparation:**

Connection to Azure Blob Storage is established, checking and creating the necessary container for the export file if it does not exist.

**Data Fetching and Writing:**

Data is fetched in batches and processed:

- Data retrieval via the Apollo API.
- Each record is formatted into a CSV string using the appropriate formatter (e.g., TrainingFormatter).
- Formatted data is directly written to the blob storage, optimizing memory use and handling large datasets efficiently.
- Progress and batch details are logged to monitor the process and help with troubleshooting.

**Error Handling:**

Throughout the export process, any exceptions are caught and logged with an error message. This includes detailed logging of the exception to help in quick troubleshooting.

### Logging Details

With the updated logging functionalities, users can expect a comprehensive set of logs that provide insights into every stage of the export process. Here’s what users can expect:

**Informational Logs:**

Logs at the start and end of the export provide essential details such as the entity processed, total items exported, and the duration of the process.

**Debug Logs:**
Detailed logs include:

- The specifics of the executed query, showing fields and filters.
- Updates on data processing for each batch, indicating the number of items processed and their sequential status.

**Error Logs:**

Errors during the export process are logged with severity levels, detailing the nature of the error, the error message, and the stack trace for diagnostics.

## Running Exporter

To run the exporter using console:

 1.  After Building whole project repo Debug-> Start debugging will run the project.
 2.  Specify the entity you wish to export by setting the entity argument or environment variable
 3.  To configure command-line arguments in Visual Studio, follow these steps:
        1. Right-click on your project in Solution Explorer and select "Properties".
        2. Navigate to the "Debug" tab.
        3. In the "Application arguments" field, enter the desired command-line arguments for your application.
           For example, if your project file is named "Apollo.SemanticSearchWorker.csproj" and you want to pass "--entity=training" as a 
           command-line argument, you would enter ".\Apollo.SemanticSearchWorker.csproj --entity=training" into this field.

 4.  But By default all necessary environment variable is setup already.
 5.  To configure environment variables in Visual Studio, follow these steps:
        1. Open your project in Visual Studio.
        2. Go to the "Debug" menu.
        3. Select "Project Properties..." (replace "Project" with the name of project, in our case it is, "Apollo.SemanticSearchWorker").
        4. In the project properties window, navigate to the "Debug" tab.
        5. Under the "Environment variables" section, you'll find a list of environment variables available to your application during 
           debugging.
        6. Here, you can add, edit, or remove environment variables as needed.

 6.  Run the exporter using the command line or by triggering it in your preferred development environment.


# Running Exporter in Docker Image
To run the exporter using Docker: 

## Prerequisites
Before running the Docker commands, make sure you have Docker installed on your machine. You can download and install Docker from [here](https://www.docker.com/get-started).

## Building the Docker Image

Navigate to the "Apollo.Backend" directory using the command prompt or terminal:

```
cd C:\Repos\APOLLO\src\cloud\Apollo.Backend
```
Build the Docker image using the following command:
```
docker build -t apollosemanticsearchworker
```
To run the container passing argumant example
```
docker run -d -p 8081:80 --name apollosearchworker2 -e MongoConnStr="Replace with Connection string" -e BlobConnStr="Replace with Connection string" -e entity="training" apollosemanticsearchworker
```
 ## Formatters
Formatters format the responsed data from server before start of stream write operation for CSV in BlobStorage

```sh
 /// <summary>
    /// Converts the training instance into the set of strings.
    /// </summary>
    internal class TrainingFormatter : IEntityFormatter
    {
        private string _cDelimiter = ";";
        Training? t = new Training();

        /// <summary>
        /// Returns:
        /// Trainings:Subtitle, TrainingName, ShortDescription, Description, Content, BenefitList and Prerequisites.
        /// The expected format: Id|Url|Title|Text
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public IList<string> FormatObject(object instance)
        {
            Training? training = instance as Training;
            if (training == null)
                throw new ArgumentNullException(nameof(training));

            List<string> list = new List<string>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(training?.SubTitle?.Replace(_cDelimiter, " ").Trim());
            sb.AppendLine(training?.TrainingName?.Replace(_cDelimiter, " ").Trim());
            sb.AppendLine(training?.ShortDescription?.Replace(_cDelimiter, " ".Trim()));
            sb.AppendLine(training?.Description?.Replace(_cDelimiter, " ".Trim()));
            sb.AppendLine(String.Join(' ', training?.Content?.Select(s => s.Replace(_cDelimiter!, " ".Trim())) ?? Enumerable.Empty<string>()));
            sb.AppendLine(String.Join(' ', training?.BenefitList?.Select(s => s.Replace(_cDelimiter!, " ").Trim()) ?? Enumerable.Empty<string>()));
            sb.AppendLine(String.Join(' ', training?.Prerequisites?.Select(s => s.Replace(_cDelimiter!, " ").Trim()) ?? Enumerable.Empty<string>()));

            list.Add($"{training?.Id}{_cDelimiter}/api/training/{training?.Id}{_cDelimiter}{training?.TrainingName}{_cDelimiter}{sb.ToString().Replace("\r", " ").Replace("\n", " ")}");

            return list;
        }
```

**Purpose**

Converts Training Instances:

 - The formatter takes an instance of the Training class and converts it into a list of strings. Each string contains specific pieces of information about the training, formatted in a way that is useful for the system's needs.


**Basic Requirements**  

- The TrainingFormatter implements an interface named IEntityFormatter, which defines the FormatObject method. This interface enforces a contract that all formatters must follow, making them interchangeable or allowing polymorphic use where different types of entities need to be formatted.

- **Error Handling for Type Mismatch**: The method FormatObject begins by attempting to cast the provided object instance to a Training type. If the cast fails (i.e., if the object is not a Training instance), it throws an ArgumentNullException. This is a basic form of type safety and error handling, ensuring the method only processes objects of the correct type.

- **Extracts and Formats Training Information**: The method extracts various pieces of information from the Training instance, such as its ID, name, subtitle, short description, and full description. Each piece of information is formatted into a string, following a specific pattern. The pattern shown is "{Id}|{Id}|{TrainingName}|{Property}", where {Property} varies depending on the line. This pattern is dictated by the requirements of the system for which this formatter was designed, such as the need to easily split the string later for individual data pieces.

- **Returns a List of Strings**: The formatted strings are added to a list, which is then returned by the method. This list represents the formatted version of the Training instance.

 ## Implementing New Formatters


 To support exporting a new entity, you'll need to implement a new formatter by inheriting from the IEntityFormatter interface.  Each formatter should implement the IEntityFormatter interface, which requires a single method:

```sh
 public IList<string> FormatObject(object entityInstance);
```

This method transforms the entity instance into a list of strings, where each string represents a line in the CSV file. The basic structure for each line should follow the pattern:


 ```sh
Id;Url;Title;Description
```

Example how it would be done for a different entity:

  
 ```sh
using Apollo.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo.SemanticSearchWorker
{
    /// <summary>
    /// Converts the user instance into a set of strings formatted for CSV output.
    /// </summary>
    internal class UserFormatter : IEntityFormatter
    {
        private string _delimiter = ";";

        /// <summary>
        /// Formats the user data into a single line per user.
        /// Expected format: Id|Url|Title|Text
        /// </summary>
        /// <param name="instance">User instance to format.</param>
        /// <returns>A list containing a single string for each user, suitable for CSV output.</returns>
        public IList<string> FormatObject(object instance)
        {
            User? user = instance as User;
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            StringBuilder sb = new StringBuilder();
            sb.Append($"{user.ObjectId}{_delimiter}/api/user/{user.ObjectId}{_delimiter}{user.Name}{_delimiter}");

            sb.Append($"{user.IdentityProvider}{_delimiter}{user.Email}");
            if (user.Birthdate.HasValue)
                sb.Append($"{_delimiter}{user.Birthdate.Value.ToString("yyyy-MM-dd")}");
            else
                sb.Append($"{_delimiter}N/A");

            string disabilities = user.Disabilities.HasValue && user.Disabilities.Value ? "Yes" : "No";
            sb.Append($"{_delimiter}{disabilities}");

            // Optionally, add more user attributes as needed.
            // For instance, format contact information if present.
            if (user.ContactInfos != null && user.ContactInfos.Count > 0)
            {
                foreach (var contact in user.ContactInfos)
                {
                    sb.Append($"{_delimiter}{contact.Type}: {contact.Value}");
                }
            }

            return new List<string> { sb.ToString() };
        }
    }
}
```

## Adding the Formatter to the Export Process

After creating your formatter, you'll need to update the the method **ExportAsync()** in BlobStorageExporter to use this new formatter when exporting the new entity.


Here's an snippet on how you could do this:

```sh
IEntityFormatter formatter;

        // Dynamically select the formatter based on _entityName
        switch (_entityName.ToLower())
        {
            case "training":
                formatter = new TrainingFormatter();
                break;
            case "user":
                formatter = new UserFormatter(); 
                break;
            // Add more cases for other entity types as needed
            default:
                throw new NotSupportedException($"Exporting {_entityName} is not supported.");
```
