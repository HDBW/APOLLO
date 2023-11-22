# Apollo REST Service operations


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
Request URL and Body
The URL of a POST request for entities looks like this:
{baseUrl}/{EntityName}/query

For example, the URL of the POST request for Product (Article) is:
https://serviceurl/training/query


The request requires a request body to store the query request’s criteria. The criteria used in a query are listed below:
•	*Top* (integer): Number of items return e.g., pageSize. If this parameter is not set, then the number of all documents is counted. This might be a very slow operation that can take time. To prevent a full scan, the caller can specify any number here that will limit the execution time of the scan operation. For example, if there are 30000 matches and Top is set to 200, this method will quickly return 20000 (200*100) as the number of records and will be shown in the response if RequestCount is set to true. If the number of all matches is required, set Top to < 0 (-1 for example) and RequestCount to true. The response will not show any result but will show the number of all matches.
•	*Fields* (string[] array): Specified properties that should be reflected in the result. If this is not specified, all properties are reflected instead.
•	*Skip* (integer): Used for paging indicated by (page - 1) * pageSize. For example, if Top is 10, Skip is 0 and there are 11 matches, the result will show the 1st to 10th matches. If Skip is changed to 1, then the result will show the 2nd to 11th matches.
•	RequestCount (boolean): If set to true, then the response will contain the number of pages and records (items). The number of records equals Top*100 and the number of records equals the number of records/Top.
If set to false, the number of pages is set to -1. 
This argument is used to avoid a double query inside of the backend when several pages are required. To calculate the number of pages, the backend first executes the query and then executes the request to get the number of available items for the given query, which is finally recalculated in the number of pages. In the case of FALSE (default), the second query is not executed.
•	*Filter*: A complex query that defines the criteria of a query. Each criterion is defined in a field. Queried entities must fulfill all the defined fields to be reflected. In other words, between each field is an AND operation. Each field in the filter should contain:
o	FieldName (string): Name of a property in an entity.
o	Operator (integer): Defined the operator. Possible values are 0 for Equals, 1 for Contains (for strings only), 2 for StartsWith (for strings only), 3 for GreaterThan, and 4 for LessThan.
o	Argument (string): The argument of the operation. There can be one or several arguments. If at least one argument is fulfilled, there is a match. In other words, between each argument is an OR operation.
o	Distinct (boolean): If is set to true, the result is returned as distinct.

*Example*
Considering a request body to query Product entities that satisfy these criteria:
•	The fields should be set so that only 2 properties that are DocNo and Width are reflected in the query response.
•	The NumberOfPages and NumberOfRecords (RequestCount) should be reflected.
•	Maximum 10 entities per page (top).
•	Skip the first matched entity, only showing the 2nd matched entity and so on.
•	German language.
•	Only reflected entities that were created after 14/12/2017.
•	The filter should have:
o	DocNo equals (operator 0) to either "005528540" OR "005528541" OR "001234679". The result should not be distinct.
o	AND DocType contains the string “0”. The result should be distinct.

Then the following request body of the Training entity should be of the following:

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


Below is the following response to the such request:
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
•	NumberOfPages: There are fewer than 10 matches, so it took only 1 page.
•	NumberOfRecords: There are 2 matches in total which are Products with DocNo of "005528540", and "005528541 (there is no match for "001234679"). However, because of Skip, the 1st match is not shown in the result.
•	Result: As defined in Fields, only the matched DocNo and Width (with DocumentType as an exception) are shown.
Detail description of the request URL, the schema of the request, the and response for each entity can be found on Swagger.
