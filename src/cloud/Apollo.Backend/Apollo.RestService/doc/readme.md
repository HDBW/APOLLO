# Apollo REST Service operations

## Table of Contents

1. [Apollo REST Service operations](#apollo-rest-service-operations)
2. [Returning a specific entity](#returning-a-specific-entity)
3. [Searching (Querying) entities](#searching-querying-entities)
   - [Request URL](#request-url)
   - [Query Criteria](#query-criteria)
   - [Example Request and Response](#example-request-and-response)
4. [Querying Users](#querying-users)
   - [User Query Criteria](#user-query-criteria)
   - [User Query Examples](#user-query-examples)
   - [Query Structure](#query-structure)
5. [Querying Trainings](#querying-trainings)
   - [Training Query Criteria](#training-query-criteria)
   - [Structure of training query](#structure-of-training-query)
   - [Training Query Examples](#training-query-examples)

## Returning a specific entity 
Every time some specific entity needs to be returned from the service, the CPDM offers specific GET operations for every entity, which is implemented as an HTTP-GET-Method.
To retrieve some entity, the HTTP-GET request has to be formed that uniquely describes the entity. The URL of a GET request for entities looks like this:

~~~
https://{serviceurl}/{EntityName}/{entityId}
~~~

For example, the URL of a GET request for Asset would be:

~~~
https://serviceurl/Training/0000
~~~

The response projects all entity’s properties by default. 

The detailed description of the request URL and the schema of response for each entity can be seen at Swagger.

## Searching (Querying) entities
Every time some specific entities need to be returned from the service if they match certain searching criteria, the CPDM offers POST-operations for every entity, which is implemented as an HTTP-POST-Method.

### Request URL

The URL of a POST request for entities looks like this:
```sh
{baseUrl}/{EntityName}/query
```

For example, the URL of the POST request training is:
~~~
https://serviceurl/training/query
~~~

### Query Criteria
The request requires a request body to store the query request’s criteria. The criteria used in a query are listed below:

-	*Top* (integer): Number of items return e.g., pageSize. If this parameter is not set, then the number of all documents is counted. 
This might be a very slow operation that can take time. To prevent a full scan, the caller can specify any number here that will limit the execution 
time of the scan operation. For example, if there are 30000 matches and Top is set to 200, this method will quickly return 20000 (200*100) 
as the number of records and will be shown in the response if RequestCount is set to true. 
If the number of all matches is required, set Top to < 0 (-1 for example) and RequestCount to true.
The response will not show any result but will show the number of all matches.

-	*Fields* (string[] array): Specified properties that should be reflected in the result. If this is not specified, all properties are reflected instead.
-	*Skip* (integer): Used for paging indicated by (page - 1) * pageSize. For example, if Top is 10, Skip is 0 and there are 11 matches, the result will show the 1st to 10th matches. If Skip is changed to 1, then the result will show the 2nd to 11th matches.

-	RequestCount (boolean): If set to true, then the response will contain the number of pages and records (items). 
The number of records equals Top*100 and the number of records equals the number of records/Top.If set to false, the number of pages is set to -1. 
This argument is used to avoid a double query inside of the backend when several pages are required. To calculate the number of pages, the backend first executes the query and then executes the request to get the number of available items for the given query, which is finally recalculated in the number of pages. In the case of FALSE (default), the second query is not executed.

**Filter**: A complex query that defines the criteria of a query. Each criterion is defined in a field. Queried entities must fulfill all the defined fields to be reflected. In other words, between each field is an AND operation. Each field in the filter should contain:
-	**FieldName (string)**: Name of a property in an entity.
- **Operator (integer)**: Defined the operator. Possible values are 0 for Equals, 1 for Contains (for strings only), 2 for StartsWith (for strings only), 3 for GreaterThan, and 4 for LessThan.
- **Argument (string)**: The argument of the operation. There can be one or several arguments. If at least one argument is fulfilled, there is a match. In other words, between each argument is an OR operation.
- **Distinct (boolean)**: If is set to true, the result is returned as distinct.

### Example Request and Response

>  `*Example*`

Considering a request body to query Product entities that satisfy these criteria:
- 	The fields should be set so that only 2 properties that are DocNo and Width are reflected in the query response.
-	The NumberOfPages and NumberOfRecords (RequestCount) should be reflected.
-	**Maximum** 10 entities per page (top).
-	Skip the first matched entity, only showing the 2nd matched entity and so on.
-	German language.
-	Only reflected entities that were created after 14/12/2017.
-	The filter should have:
-	DocNo equals (operator 0) to either "005528540" **OR** "005528541" **OR** "001234679". The result should not be distinct.
-	AND DocType contains the string “0”. The result should be distinct.

Then the following request body of the Training entity should be of the following:
```sh
{
  "Fields": [
    "TrainingName", "ProviderId"
  ],
  "RequestCount": false,
  "Top":10,
  "Skip": 1,
  "Filter": {
    "Fields": [
      {
        "FieldName": "Description",
        "Operator": 1, // Contains
        "Argument": [
          "Large Language Models", "005528541", "001234679"
        ],
        "Distinct": false
      },
      {
        "FieldName": "DocType",
        "Operator": 1,
        "Argument": [
          "0"
        ],
        "Distinct": true
      }
    ]
  }
}
```

Below is the following response to the such request:
```sh
{
    "Result": [
        {
            "DocNo": "005528540",
            "Width": 0,
            "DocumentType": "Asset"
        }
    ],
    "NumberOfPages": 1,
    "NumberOfRecords": 2
}
```


-	NumberOfPages: There are fewer than 10 matches, so it took only 1 page.
-	NumberOfRecords: There are 2 matches in total which are Products with DocNo of "005528540", and "005528541 (there is no match for "001234679"). 
However, because of Skip, the 1st match is not shown in the result.
-	Result: As defined in Fields, only the matched DocNo and Width (with DocumentType as an exception) are shown.
Detail description of the request URL, the schema of the request, the and response for each entity can be found on **Swagger**.

## Querying Users

Similar to other entities, querying User entities follows the same approach using the POST method.

### User Query criteria

The request body for querying User entities includes the following criteria:
-	Top (integer): Number of items to return.
-	Fields (string[] array): Properties to be reflected in the result.
-	Skip (integer): For paging purposes.
-	RequestCount (boolean): To include the count of pages and records in the response.
-	Filter: Criteria defining the query for user entities.

The filter criteria in the request body includes:
-	FieldName (string): Property name in the User entity.
-	Operator (integer): Operator code (0 for Equals, 1 for Contains - strings only, 2 for StartsWith - strings only, 3 for GreaterThan, and 4 for LessThan).
-	Argument (string[]): Argument(s) for the operation. The OR operation is applied between each argument.
-	Distinct (boolean): To return distinct results.

### User Query Examples

Here's an example request body for querying User entities:
```sh
{
  "Fields": [
    "UserName",
    "Email"
  ],
  "RequestCount": true,
  "Top": 10,
  "Skip": 0,
  "Filter": {
    "Fields": [
      {
        "FieldName": "UserName",
        "Operator": 1,
        "Argument": [
          "testuser1",
          "testuser2"
        ],
        "Distinct": false
      }
    ]
  }
}
```

**Response**

The response to the above query request includes the following structure:
```sh
{
    "Result": [
        {
            "UserName": "testuser1",
            "Email": "testuser1@example.com"
        },
        {
            "UserName": "testuser2",
            "Email": "testuser2@example.com"
        }
    ],
    "NumberOfPages": 1,
    "NumberOfRecords": 2
}
```
-	Result: Contains matched UserName and Email properties.
-	NumberOfPages: Indicates the total number of pages in the response.
-	NumberOfRecords: Shows the total number of records that match the query criteria.

### Query Structure
The Query structure allows specifying query criteria:
```sh
public class Query
{
    public List<string> Fields { get; set; }
    public Filter Filter { get; set; }
    public bool RequestCount { get; set; }
    public int Top { get; set; } = 200;
    public int Skip { get; set; } = 0;
    public SortExpression SortExpression { get; set; }
}
```
This structure allows specifying:
-	Fields to return.
-	Filter criteria using the Filter object.
-	Pagination settings (Top, Skip).
-	Sorting based on a specified field.The internal API method **(_api.QueryTrainings)** uses this information to construct and execute a query against the training data.
The **Filter** class enables setting complex query criteria.

## Querying Trainings

The QueryTrainings method is an API endpoint responsible for querying trainings based on the request. Here's a high-level explanation:
1.	**Request and Handling**:
-	The method QueryTrainings accepts a QueryTrainingsRequest from the API.
-	Inside, it logs the entry and attempts to query the trainings based on the provided request.
2.	**Execution**:
-	It calls an internal API method _api.QueryTrainings(req), passing the request.
-	This internal method likely processes the request parameters (defined in QueryTrainingsRequest), creates a query based on the provided criteria (fields, filters, pagination, sorting), and retrieves the desired trainings.
3.	**Response**:
-	After obtaining the queried trainings, it logs completion and returns a response.
-	The response is a list of QueryTrainingsResponse, typically containing the queried training data.

### Structure of training query

```sh
public class QueryTrainingsRequest : Query
{
    public Query Query { get; set; }
}
```

### Training Query Criteria

The **_api.QueryTrainings(req)** method processes this request, performs the query, and returns the corresponding training data based on the defined criteria.
QueryTrainingsRequest

-	Fields: Specifies which fields of the training entities should be returned in the response. It's a list of field names. For instance, **["TrainingName", "Description"]** indicates you want only these two fields to be included in the response.
-	Filter: Defines the criteria to filter the training entities. In this template, it's set up to filter based on two fields **(FilterField1 and FilterField2)**. 
You can define the field names, operators, and values for filtering. For instance, FilterField1 might be **"TrainingType"** with an operator **Equals** and **FilterValue1** might be "Online".
-	RequestCount: A boolean value indicating whether the response should contain the count of pages and records based on the query. 
If true, it includes this information; otherwise, it's set to -1.
-	Top: Specifies the number of items (trainings) to be returned in the response.
-	Skip: Indicates the number of items to skip before beginning to return items. It's used for pagination.
-	SortExpression: Specifies the field name for sorting and the order (ascending or descending).

### Training Query Examples

**Request Example**

```sh
{
  "Fields": [
    "TrainingName", "Description", "StartDate", "EndDate", 
    "DurationDescription", "Duration", "IsGuaranteed", 
    "TrainingType", "TimeInvestAttendee", "TimeModel"
  ],
  "RequestCount": true,
  "Top": 10,
  "Skip": 0,
  "Filter": {
    "Fields": [
      {
        "FieldName": "StartDate",
        "Operator": 3, // GreaterThan
        "Argument": ["2023-11-20T00:00:00Z"],
        "Distinct": false
      },
      {
        "FieldName": "TrainingType",
        "Operator": 0, // Equals
        "Argument": ["Online", "Offline"],
        "Distinct": false
      }
    ]
  },
  "SortExpression": {
    "FieldName": "StartDate",
    "Order": 1 
  }
}
```

**Response Example**

```sh
{
  "Result": [
    {
      "TrainingName": "Training 1",
      "Description": "Description of Training 1",
      "StartDate": "2023-11-21T08:00:00Z",
      "EndDate": "2023-11-21T16:00:00Z",
      "DurationDescription": "Full-day training",
      "Duration": "08:00:00",
      "IsGuaranteed": true,
      "TrainingType": "Online",
      "TimeInvestAttendee": "04:00:00",
      "TimeModel": "1UE = 45 Minutes"
    },
    {
      "TrainingName": "Training 2",
      "Description": "Description of Training 2",
      "StartDate": "2023-11-22T09:00:00Z",
      "EndDate": "2023-11-22T17:00:00Z",
      "DurationDescription": "Full-day training",
      "Duration": "08:00:00",
      "IsGuaranteed": false,
      "TrainingType": "Offline",
      "TimeInvestAttendee": "05:00:00",
      "TimeModel": "1UE = 45 Minutes"
    }
  ],
  "NumberOfPages": 2,
  "NumberOfRecords": 15
}
```

The QueryTrainingsResponse shows a hypothetical response based on the request:
-	Result: Contains an array of training objects that match the specified criteria in the request's Filter and are filtered based on Fields. 
Each object represents a training entity and includes the requested fields such as **"TrainingName"**, **"Description"**, **"StartDate"**, **"EndDate"**, etc.
-	NumberOfPages: Indicates the total number of pages available based on the applied query criteria.
-	NumberOfRecords: Represents the total count of records (trainings) that satisfy the applied query conditions.This structured response provides filtered training entities based on the criteria provided in the QueryTrainingsRequest, along with metadata regarding the number of pages and total records meeting the criteria.
