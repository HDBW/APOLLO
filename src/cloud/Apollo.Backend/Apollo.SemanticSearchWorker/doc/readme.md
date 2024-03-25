# Apollo Semantic Search Worker Documentation


This README.md provides a concise yet comprehensive guide to understanding and utilizing the export functionality, as well as extending it to support new entities.


The Apollo Semantic Search Worker is a tool designed to export Apollo entities (such as Training, User, Profile, etc.) into a CSV file that is compatible with Daenet Semantic Search.



## How it works

The export process is carried out by the **BlobStorageExporter** class, which takes in an Apollo API instance, the entity name to be exported, and a connection string to the blob storage where the exported CSV file is be stored.

Logging is implemented throughout the process to provide insightful information about the export operation.

## Running Exporter

To run the exporter:

 1.  Specify the entity you wish to export by setting the entity argument or environment variable
 2.  Run the exporter using the command line or by triggering it in your preferred development environment.


 ## Formatters

```sh
    /// <summary>
    /// Converts the training instance into the set of strings.
    /// </summary>
    internal class TrainingFormatter : IEntityFormatter
    {
        Training? t = new Training();

        /// <summary>
        /// Returns Subtitle, TrainingName, ShortDescription and Description of the training.
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public IList<string> FormatObject(object instance)
        {
            Training? training = instance as Training;
            if (training == null)
                throw new ArgumentNullException(nameof(training));

            List<string> list = new List<string>();

            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.SubTitle}");
            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.ShortDescription}");
            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.Description}");
            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.TrainingName}");

            return list;
        }
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
Id|Url|Title|Text
```

  Example how it would be done for a different entity:

  
 ```sh
 using Apollo.Common.Entities;
using System;
using System.Collections.Generic;

namespace Apollo.SemanticSearchWorker
{
    /// <summary>
    /// Converts the user instance into the set of strings.
    /// </summary>
    internal class UserFormatter : IEntityFormatter
    {
        /// <summary>
        /// Returns ObjectId, IdentityProvider, Email, and Name of the user.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IList<string> FormatObject(object instance)
        {
            User? user = instance as User;
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            List<string> list = new List<string>();

            // Basic info
            list.Add($"{user.ObjectId}|{user.ObjectId}|{user.IdentityProvider}|{user.Email}|{user.Name}");

            // Optional info with checks for nullability
            string birthdate = user.Birthdate.HasValue ? user.Birthdate.Value.ToString("yyyy-MM-dd") : "N/A";
            string disabilities = user.Disabilities.HasValue && user.Disabilities.Value ? "Yes" : "No";

            list.Add($"{user.ObjectId}|{user.ObjectId}|{user.IdentityProvider}|Birthdate|{birthdate}");
            list.Add($"{user.ObjectId}|{user.ObjectId}|{user.IdentityProvider}|Disabilities|{disabilities}");

            // Assuming contacts could be important, we format them if they exist.
            if (user.ContactInfos != null && user.ContactInfos.Count > 0)
            {
                foreach (var contact in user.ContactInfos)
                {
                    list.Add($"{user.ObjectId}|{user.ObjectId}|{user.IdentityProvider}|Contact|{contact.Type}: {contact.Value}");
                }
            }

            return list;
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
