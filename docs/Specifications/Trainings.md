# Trainings and Courses Specification

Authors:
- Patric Boscolo
- Ivana Boscolo

Date:
08.10.2021
14.07.2022
19.07.2022

[]: # Language: markdown
[]: # Path: docs\Specifications\Trainings.md

Version:
- 0.0.1 - October 2021
- 0.0.2 - July 2022
- 0.0.3 - July 2022

## Abstract
The purpose of this document is to summarize the data structure for courses and trainings managed in the apollo backend as well as the purpose of trainings and courses. 

## Definitions

### Training Providers
Offering trainings and courses apollo users can enroll in.

### Organisations
An organisation can asess a users skills or competencies and can attest or certify that a individual have learned skills or competencies and met the learnings of a course or training.

### Training Providers
The apollo consortium consits of training providers represented by [bbw](https://www.bbw.de/startseite/), [biwe](https://www.biwe.de/) and [tüv rheinland academy](https://akademie.tuv.com/). They offer voccational trainings in the context of adult education for job seekers, employees and trainees etc. While the scope of this project mainly focuses on the personas as defined in the [personas](Personas/readme.md) document. This document focuses on the products of the training providers.

### Training or Course
A training or course is a document which is unstructured or structered by the corresponding learning management system of the training providers. The course schema or datapoints may vary from provider to provider or even from subject or training or related qualification. So there is no standard information for a particular training or course. **However invite project apollo defines a course or training as a product of the training providers offering the possibility to aquire skills and competencies needed for the job or next step in the career of a potential customer.** 

Therefor a training or course is a set of files or documents describing these products. 

### Purpose of the Training Data Structure
The purpose is to manage courses and trainings and evaluate their relevance. So basically a training consists of several documents and associated information. Therefor a training data structure is something different then the offered course or training itself. While the training description and target audiences as well as competencies aquired are a different process. The idea behind apollo is to identify needs of the market and the apollo audience and the training providers. Therefor documents describing aspects of the course wheter they are imported via the apollo graph api or via the user interface (backend) they typically have a lifecycle where they needed to be managed and maintained. Also courses need to be updated over time to be relevant for the audience. Therefor a management system which allows to import documents, extract necessary and relevant information and support the training providers in updating topics or modules of a course based on feedback from the participants and the labour market are the purpose of the apollo backend. 

In order to stay relevant information like labour market demands and regulatory changes need to be tracked. In order to give advice to the training providers and give the training providers the possiblity to reach out to the participants and/or interested users of courses and inform them about updates or changes. This is a hugh asset of the invite project apollo. 

## Data
TBD

### Training Data Structure
TBD

## Data Aquisition
TBD

## Data Fields
Describes Properties or Edges as well as Nodes for the given training data structure in invite apollo. 

### ApolloId
Internal Id of the invite Project Apollo represented as GUID.

### TrainingProviderClaim
TBD (Authorization) ADFS Claim of the training provider.

### TrainingProviderEditorClaim
TBD (Authorization) ADFS Claim of the training provider.

### ExternalId
Unique Identifier of the training provider. Used by crawlers to check the document state and track changes of the course offered by the training providers. 

### ExternalUrls
The website representing more information about a course or training at the external training provider. Courses may have one ore more Url associated with them from the offering training provider. This information is used by the crawlers to track changes. 

### ExternalDescription
Description of the course or training offered by the training provider. This information is part of the Changeset and is the external description as offered by the training provider.

### LatestUpdate
DateTime Field representing the last update of the course or training. This information is used by the crawlers to track changes.

### Created
DateTime Field representing the creation of the course or training. This information is used by the crawlers to track changes.

### ListOfInternalDocuments
List of documents describing the course or training or other information. These documents are private and are processed by the AI and are maintained by the training providers. Is used for data retrival and extraction of information about a specific course or training.

### ListOfPublicDocuments
Such as PDFs or Course information provided by the training provider. These documents are public and are used to display information about a specific course or training.

### ListOfEscoCompetenciesExtracted
A list of competencies extracted from the course or training. These competencies are used to evaluate the relevance of the course or training for a specific user. ESCO Competencies are extracted via NER and other NLP Modules in apollo. 

### ListOfEscoCompetenciesAssigned
A list of competencies assigend to a course or training by the offering training provider. These competencies are used to evaluate the relevance of the course or training for a specific user. ESCO Competencies represented as a list of competencies than can be assigned.

### Competencies
Returns a list of all ESCO competencies asigned to a course or training. 

### AssignedOccupations
Assigned Occupations are a list of ESCO Occupations asigned by the content manager of a course or training. 

### ExtractedOccupations
Extracted ESCO Occupations are a list which is generated by machine learning and are referenced with a score of probability. 

### Occupations
A List of esco occupations associated with this course. Will return the full list of occupations containing the extracted occupations as well as assigned occupations.

### ChangesExtracted
Is represented as Changeset which contains all changes to the training or course over time. 

### Tags
Is a set of Tags describing the training or course. And delivering additional Informatino for Search and Search Engine Optimization.

### Title
Property that describes the Title of a course or training.

### IsOnlineTraining
Indicates if the course or training is an online training.

### TrainingLocation
Property that describes where the training or course is taking place.

### Duration
Is a property that describes how long the the training or course takes.

### Fulltime
Indicates wheter a training or course is available as fulltime option.

### Parttime
Indicates wheter a training or course is available as parttime option.

### Dates
Available Dates are a list of dates when the training or course is available.

### ListOfModules
List of Modules which are part of a course or training and describe the content of the course or training.

### Shortdescription
Is a short description of the course or training. There is no text limitation for now.

### Summerization
Is a AI based summerization of the course or training. 

### CourseCard
Is a set of Data describing the course or training to users of the apollo app. The set of information is organized and published to the apollo app and public available via the apollo APIs and is approved by the offering training provider.

### UserRatingSummerization
Is a summerization of the user ratings of the course or training.

### ListOfUserFeedback
Short Messages and a scoring given by a user for a specific course. Note only feedback from validated participants are included.

### ListOfCommentsbytheUsers
Comments given by the users for a specific course.

### ListOfDocumentsbytheUsers
Is a list of documents uploaded by the users for a specific course.

### ListOfAnkiCards
Is a list of available Anki cards for a specific course. Are provided by the course or training provider or users of the apollo app.

### ListOfChannels
A list of Messageboards for a specific course. Can be public or private. And are organized by the users themselves for sharing information about the course or related topics. 

### FAQListoftheCourse
A list of Frequently Asked Questions for a specific course. Used to provide information to the users of the apollo app. Used by the apollo chatbot to answer questions about the course or training.

### IsPublished
Indicates wheter a course or training is published to the app or not.

### PublishingDate
Indicates the date when a course or training was published to the app.

### DepricationDate
Indicates the date when a course or training is about to retire or depricated.

### DepricationReason
Description why a course or training is about to retire or be depricated.

### UnpublishingDate
Indicates the date when a course or training is about to retire or be depricated.

### ReplacementId
Id of the course or training that is replacing the current course or training.

### ReplacementIdExternal
Id offered by the training provider of the course or training that is replacing the current course or training.

### AdaptiveCards
Are a list of adaptive cards highlighting one aspect or user story. For example there is a set of date for the course to track changes for booked courses. Or there is a adaptive card which can be used by the user to discuss the course with a manager. Or they maybe a adaptive card to share the relevant information for booking a course with the users hr department.  

### CourseTelemetry
Is a set of information about the course containing page impressions, active users and more telemetry relevant information regarding a course or training.

