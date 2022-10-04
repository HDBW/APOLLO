# Trainings and Courses Specification

Authors:
- Patric Boscolo
- Ivana Boscolo

Co-Authors:
- GPT-3 by OpenAI

Date:
- 08.10.2021
- 14.07.2022
- 19.07.2022
- 23.09.2022
- 04.10.2022

[]: # Language: markdown
[]: # Path: docs\Specifications\Course.md

Version:
- 0.0.1 - October 2021
- 0.0.2 - July 2022
- 0.0.3 - July 2022
- 0.0.4 - September 2022
- 0.0.5 - October 2022

## Abstract
The purpose of this document is to summarize the data structure for courses and trainings managed in the apollo backend as well as the purpose of trainings and courses. 

## Definitions
Courses are structured in segments such as benfits, content, modules, target audience and so on. The course is the main entity and the segments are sub entities. Each of the segmentes have relevant information during the sales funnel of the course. So when a course is viewed by a user he is more interested in the outcome and benefits of the course. When the user is interested in the course he is more interested in the content of the course. When the user is interested in the content of the course he is more interested in the modules of the course. When the user is interested in the modules of the course he is interested if a specific topic or skill is teached in a module.

### Training Providers (TP)
Offering trainings and courses apollo users can enroll in.

### Organizations
An organisation can assess a users skills or competencies and can attest or certify that a individual have learned skills or competencies and met the learnings of a course or training. 

### Training Providers
The apollo consortium consits of training providers represented by [bbw](https://www.bbw.de/startseite/), [biwe](https://www.biwe.de/) and [tüv rheinland academy](https://akademie.tuv.com/). They offer voccational trainings in the context of adult education for job seekers, employees and trainees etc. While the scope of this project mainly focuses on the personas as defined in the [personas](Personas/readme.md) document. This document focuses on the products of the training providers.

### Training or Course
A training or course is a document which is unstructured or structered by the corresponding learning management system of the training providers. The course schema or datapoints may vary from provider to provider or even from subject or training or related qualification. So there is no standard information for a particular training or course. **However invite project apollo defines a course or training as a product of the training providers offering the possibility to aquire skills and competencies needed for the job or next step in the career of a potential customer.** 

Therefor a training or course is a set of files or documents describing these products. 

### Purpose of the Training Data Structure
The purpose is to manage courses and trainings and evaluate their relevance. So basically a training consists of several documents and associated information. Therefor a training data structure is something different then the offered course or training itself. While the training description and target audiences as well as competencies aquired are a different process. The idea behind apollo is to identify needs of the market and the apollo audience and the training providers. Therefor documents describing aspects of the course wheter they are imported via the apollo graph api or via the user interface (backend) they typically have a lifecycle where they needed to be managed and maintained. Also courses need to be updated over time to be relevant for the audience. Therefor a management system which allows to import documents, extract necessary and relevant information and support the training providers in updating topics or modules of a course based on feedback from the participants and the labour market are the purpose of the apollo backend. 

In order to stay relevant information like labour market demands and regulatory changes need to be tracked. In order to give advice to the training providers and give the training providers the possiblity to reach out to the participants and/or interested users of courses and inform them about updates or changes. This is a hugh asset of the invite project apollo. 

## Data Acquisition

Courses can either be submitted to a public api or in the case of the 1st party providers there are connectors to their learning management systems. 

## Course Process Management

The course management happens in two steps. First the course is imported or submitted into the apollo backend. Second the course is evaluated and the information is extracted and stored in the apollo backend. 

![Course Process Management](/docs/Specifications/process_courses.png)

Via the apollo backend a editor can validate the extracted information and update the information if necessary. The editor can also publish the course to the apollo graph api.

![Courses Management Overview](/docs/Specifications/course_management.png)

## Course Data Analysis

### 1. Course Segments

It is the job of the "machine learning" team to identify segments in the submitted course data.

The segements are:
- Course Metadata
- Target Group
- Benefits
- Descriptions
- Content/Modules
- Prequisites
- Booking Information

### 2. Direct Skill Mapping

There are several approaches to the direct skill mapping. The first approach is to use the course description, modules and the other fields such as title to identify the skills and competencies.
This can be achieved via a machine learning approach or a manual approach. The corresponding techniques used are:

- Named Entity Recognition (NER)
- custom Named Entity Recognition (cNER)
- Topic Modelling
- BiTerm Analysis

### 3. Indirect Skill Mapping

During the analysis of the course data there are typically "PersonType", "Event", "Organization", "Skill" and "Place" entities. A "PersonType" can be a Occupation or a Job Title. Therefor the "PersonType" can be mapped to a list of skills associated to a occupation. Another example is the prerequisites of a course. The prerequisites of a course can be a qualifications. A qualification can be mapped to a list of skills. 

### 4. Linked Entities

However in some cases courses contain domain specific terms which are not part of the ontology. In this case the course data is enriched with the corresponding linked entities. These entities can be linked to the ontology and result in a skill mapping.
For example a ISO 9001:2015 course contains the term "Quality Management System". This term is not part of the ontology. However the term is linked to the ontology via the linked entity "Quality Management System" this references to the transversal skill or skill concept ["manage quality"](https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/35ebe444-9ece-4fbc-a55d-e99ea37267ae) which resluts in a list of skills associated to a concept. 

### 5. Qualifications

Courses can be linked to a qualification or partial qualification. A participant of a course or trainng could earn (by meeting the criterias) a qualification, that can be linked to a list of skills.
The european union is currently evaluating the [learning outcomes project](https://europa.eu/europass/en/find-courses) which helps link training and courses to the ontology of esco. Please note that for each european member state there is a [national qualification Framework (NQF)](https://europa.eu/europass/en/national-qualifications-frameworks-nqfs) which is used to link courses to qualifications. In our case the bmbf is using the [deutsche qualifikationsrahmen](https://www.dqr.de/dqr/de/der-dqr/der-dqr_node.html) which is a national qualification framework. In order to search for a qualification in the dqr the [dqr search](https://www.dqr.de/SiteGlobals/Forms/dqr/de/qualifikationssuche/suche_formular) can be used.

### Course Lifecycle
Note: A course is never deleted, except legal reasons. A course can be unpublished and published again. A course can be updated. A course can have be the successor of course meaning it replaces the old course. Or can be the predecessor of a course meaning it is replaced by a new course. Courses can be depricated.

## API Documentation

### Technical Requirements

![Courses Service Overview](/docs/Specifications/architecture_courses.png)

### Course API - Mobile App

TBD - URL for the course api documentation

### Course API - Backend

TBD - URL for the course api documentation

### Course Endpoints
There are several API Endpoints planned for the apollo backend. 

TBD

## Data
A course consists of several segments. The segments are defined in Course Data Analysis - section 1. Segments. 

### Course Data Structure

Class Diagram of a course and associated services.
RETIRED !!! - The class diagram is currently not supported anymore. The course data structure is replaced by the table below.

## Course Schema
Describes Properties or Edges as well as Nodes for the given training data structure in invite apollo.

### Grpc Service: CourseService

- TBD - App Requirements

### Grpc Message: Course

|     Property  |   Data Type   |  Segment  | Description  | Provided by  | Prototype | UI/UX |
| ------------- | ------------- | ------------- |------------- |------------- |------------- |------------- |
| CourseId  | `String`  | Metadata  | Used as unique identifier | Apollo | YES | NO |
| ProviderId  | `String`  | Metadata  | Used as unique identifier | Training Provider | YES | NO |
| Title  | `String` | Content/Description  | The title of a course  | Training Provider | YES | YES |
| ShortDescription  | `String` | Content/Description  | A short description of the course  | Training Provider | YES | YES |
| Description  | `String` | Content/Description  | description of the course  | Training Provider | YES | YES |
| Benefits  | `List<string>` | Content/Description  | A list or text describing the value propostion of a course | Training Provider | YES | YES |
| TargetGroup  | `String` | Content/Description  | A list of persontypes or occupations as well as text describing the target audience. | Training Provider | YES | YES |
| Instructor  | `String` | Content/Description  | Name of the Instructor | Training Provider | YES | YES |
| Loanoptions  | `List<string>` | Booking  | A List of available student loans as text. | Training Provider | YES | YES |
| Loanoptionsavailable  | `bool` | Booking  | Indicates if a course qulifies for a student loan or credit. | Training Provider | YES | YES |
| loanoptionsurls  | `List<string>` | Booking  | A list of urls for more information about available loan and financing options. | Training Provider | YES | YES |
| Prerequisites  | `List<string>` | Content/Description  | A list of prerequisites for a course. Typically extracted from ML, representing esco qualifications or occupations | Apollo | NO | YES |
| PrerequisitesDescription | `String` | Content/Description  | the original text provided as prerequuisites as the training provider publishes the course. | Training Provider | YES | YES |
| learningobjectives | `List<string>` | Content/Description  | A list of objectives the attendee of a course can expect to learn. | Apollo | NO | YES |
| learningobjectivesDescription | `String` | Content/Description  | the original text provided as learning objectives as the training provider publishes the course. | Training Provider | NO | YES |
| learningoutcomes | `List<string>` | Content/Description  | A list of learning outcomes the attendee of a course can apply after attending the course. | Apollo | NO | YES |
| learningoutcomesDescription | `String` | Content/Description  | the original text provided as learning outcomes as the training provider publishes the course. For Example qualifies for Certification of Specialized by taking exam xxx, course participation certificate. | Training Provider | YES | YES |
| skills | `List<string>` | Content/Description  | A list of skills associated to a training or course. | Apollo | NO | YES |
| escoskills | `List<string>` | Content/Description  | A list of ESCO skills associated to a course. | Apollo | NO | YES |
| occupations | `List<string>` | Content/Description  | A list of ESCO occupations associated to a course. | Apollo | NO | YES |
| presontypes | `List<string>` | Content/Description  | A list of persontypes associated to a course. | Apollo | NO | NO |
| linkedentities | `List<string>` | Content/Description  | A list of linked entities associated to a course. | Apollo | YES | YES |
| qualifications | `List<string>` | Content/Description  | A list of ESCO qualifications associated to a course. | Apollo | NO | YES |
| keyphrases | `String` | Content/Description  | The most used and therefore highlighted statements describing the training or course. Generated by ML. | Apollo | MAYBE | YES |
| summary | `String` | Content/Description  | A summary of the course generated by ML. | Apollo | MAYBE | YES |
| Documents  | `List<Url>` | Content/Description | A list of public available documents such as flyers or additional information regarding the course offered by the training providers. | Training Providers | MAYBE | YES |
| Duration  | `TimeSpan` | Content/Description | The overall timeinvest of a participant needed to attend a course. | Training Providers / Apollo (Autocalculated) | YES | YES |
| Occurance  | `CourseOccurance` | Enumeration | Indicates if a course is available: parttime, fulltime or both. | Training Providers | YES | YES |
| Tags  | `List<string>` | MetaData | Used for SEO optimization or tagging of the courses | Training Providers | NO | NO |
| BookingUrl  | `String` | Booking | The url to the booking page of the course. Typically a Trackingurl provided by the trianing providers. | Training Providers | YES | YES |
| BookingOptions  | `String` | Booking | A list of available booking options. | Training Providers | YES | YES |
| HasGarantuedAppointments  | `bool` | Booking | Indicates if a course has a guaranteed appointment offered by the training provider. Will be autocalculated by the backend. | Training Providers / Apollo | YES | YES |
| CourseAppointments  | `List<CourseAppointment>` | Booking | A list of available appointments for a course offered by the training providers. | Training Providers | YES | YES |
| CourseAbvailability  | `CourseAvailability` | Enumeration | Indicates if a course is available: Unknown, Available, Unavailable | Training Providers | YES | YES |
| CourseLanguages  | `List<string>` | Content/Description | A list of languages the course is available in. | Training Providers | NO | YES |
| Language  | `String` | Content/Description | The language the course description is in. | Apollo | NO | NO |
| CourseMedia  | `List<Url>` | Content/Description | A list of media assets for a course. CDN Url Encoding. | Training Providers / Apollo | MAYBE | YES |
| CourseType  | `CourseType` | Enumeration | Indicates the type of a course: `Unknown, Online, InPerson, OnAndOffline, InHouse, All` | Training Providers | YES | YES |
| TrainingProvider  | `String` | Content/Description | The name of the training provider. | Training Providers | YES | YES |
| TrainingProviderUrl  | `String` | Content/Description | The Url of training provider | Training Providers | YES | YES |
| TrainingProviderLogo  | `String` | Content/Description | The Url of the training provider logo | Training Providers | YES | YES |
| CourseProvider  | `String` | Content/Description | The name of the course provider. | Training Providers | YES | YES |
| CourseProviderUrl  | `String` | Content/Description | The Url of course provider | Training Providers | YES | YES |
| CourseProviderLogo  | `String` | Content/Description | The Url of the course provider logo | Training Providers | YES | YES |
| QualificationProvider  | `String` | Content/Description | The name of the qualification provider. | Training Providers | NO | NO |
| QualificationProviderUrl  | `String` | Content/Description | The Url of qualification provider. NOTE THIS IS TRICKY LET US DOUBLE CHECK THE REQUIREMENTS | Training Providers | NO | NO |
| QualificationProviderLogo  | `String` | Content/Description | The Url of the qualification provider logo | Training Providers | NO | NO |
| PublishingDate  | `DateTime` | Content Management | The date the course was published. | Apollo | YES | YES |
| LastUpdate  | `DateTime` | Content Management | The date the course was last updated. | Apollo | YES | YES |
| DepricationDate  | `DateTime` | Content Management | The date of the course deprication. | Apollo | YES | YES |
| DepricationReason  | `String` | Content Management | The reason for the course deprication. | Apollo | YES | YES |
| UnpublishDate  | `DateTime` | Content Management | The date the course was unpublished. | Apollo | YES | YES |
| CourseSuccessor  | `Course` | None | The Course replacing the current course. | Training Provider / Apollo | YES | YES |
| CourseSuccessorUrl | `String` | None | The Url of the course replacing the current course. NOTE: JSON-LD relevant! | Apollo | NO | NO |
| CoursePredecessor  | `Course` | None | The Course replaced by the current course. | Training Provider / Apollo | YES | YES |
| CoursePredecessorUrl | `String` | None | The Url of the course replaced by the current course. NOTE: JSON-LD relevant! | Apollo | NO | NO |
| FAQ  | `Map<string,string>` | Content/Description | A list of frequently asked questions regarding the course. | Training Providers / Apollo | MAYBE | MAYBE |
| MetaData | `Map<string,string>` | MetaData | A list of meta data associated to the course. | Training Providers / Apollo | NO | NO |
| ParentIds  | `List<string>` | Relations | A course can be part of a other course. (In this case the actual course is a module of a course) Since a module can be part of more than one course this is a reference to a list of Unique Identifiers of the parten courses. | Training Provider / Apollo | YES | YES |
| ChildIds  | `List<string>` | Relations | A course can have modules or courses. Note: Don´t get confused with qualifications since for example to become a architect you need to take several courses. This is not the porpuse of the course to course relation. | Training Providers / Apollo | YES | YES |
| SimilarIds  | `List<string>` | Relations | A list of similar courses. | Apollo | NO | YES |
| RecommendedIds  | `List<string>` | Relations | A list of recommended courses. | Apollo | NO | YES |
| Attributes | `List<Attributes>` | Content | A list of attributes associated to the course. Allowing Training providers to add additional information to a trainnig or course. | Training Providers | NO | NO |

### Grpc Message: Attributes

|     Property  |   Data Type   |  Segment  | Description  | Provided by  | Prototype | UI/UX |
| ------------- | ------------- | ------------- |------------- |------------- |------------- |------------- |
| Attributes | `Map<string,string>` | Content | A list of attributes associated to the course. Allowing Training providers to add additional information to a trainnig or course. | Training Providers | NO | NO |

### Grpc Message: CourseAppointment

|     Property  |   Data Type   |  Segment  | Description  | Provided by  | Prototype | UI/UX |
| ------------- | ------------- | ------------- |------------- |------------- |------------- |------------- |
| AppointmentId  | `String` | TBD | The unique identifier of the appointment. | Apollo | YES | NO |
| Bookingcode  | `String` | TBD | Bookingcode used by the training provider. | Training Provider | YES | YES |
| Summary | `String` | TBD | A short description of the appointment. | Training Provider | YES | YES |
| StartDate  | `DateTime` | TBD | The start date of the appointment. | Training Provider | YES | YES |
| StartTimezone  | `TimeZone` | TBD | The timezone of the appointment. | apollo | YES | YES |
| EndDate  | `DateTime` | TBD | The end date of the appointment. | Training Provider | YES | YES |
| EndTimezone  | `TimeZone` | TBD | The timezone of the appointment. | apollo | YES | YES |
| Recurrence  | `String` | TBD | The recurrence of the appointment. | Training Provider | YES | YES |
| Location  | `String` | TBD | The location of the appointment. | Training Provider | YES | YES |
| IsBookable  | `bool` | TBD | Indicates if the appointment is bookable. | Training Provider | YES | YES |
| IsCancelled  | `bool` | TBD | Indicates if the appointment is cancelled. | Training Provider | YES | YES |
| AppointmentType  | `AppointmentType` | TBD | Indicates the type of the appointment: `Unknown, IsGuaranteed` | Training Provider | YES | YES |
| CourseType | `CourseType` | TBD | Indicates the type of the appointment.  | Training Provider | YES | YES |
| Occurance | `OccuranceType` | TBD | Indicates the type of the appointment is: PartTime, Fulltime | Training Provider | YES | YES |
| Price  | `CoursePrice` | TBD | The price of the appointment. | Training Provider | YES | YES |
| BookingOptions  | `List<string>` | TBD | A list of booking options. | Training Provider | YES | YES |
| BookingUrl  | `String` | TBD | The Url of the booking page. | Training Provider | YES | YES |
| BookingContact  | `BookingContact` | TBD | The contact information for booking. | Training Provider | YES | YES |
| Attributes | `Attributes` | TBD | A list of attributes associated to the appointment. Allowing Training providers to add additional information to a trainnig or course. | Training Providers | NO | NO |

### Grpc Message: BookingContact (nested appointment)
|    Property  |   Data Type   |  Description  | Provided by  | Prototype | UI/UX |
| ------------- | ------------- | ------------- |------------- |------------- |------------- |
| Name  | `String` | The name of the contact person. | Training Provider | YES | YES |
| Email  | `String` | The email of the contact person. | Training Provider | YES | YES |
| Phone  | `String` | The phone number of the contact person. | Training Provider | YES | YES |
| Url  | `String` | The Url of the contact person. | Training Provider | YES | YES |

### Grpc Message: CoursePrice
|    Property  |   Data Type   |  Description  | Provided by  | Prototype | UI/UX |
| ------------- | ------------- | ------------- |------------- |------------- |------------- |
| StartDate  | `DateTime` | The start date of the price. | Training Provider | YES | YES |
| EndDate  | `DateTime` | The end date of the price. Can be null. | Training Provider | YES | YES |
| Price  | `float` | The price of the appointment. | Training Provider | YES | YES |
| Currency  | `string` | The currency of the price. | Training Provider | YES | YES |

### Grpc Message: CourseRequest

NOTE: Backend specific message.

|    Property  |   Data Type   |  Description  | OPTIONAL  |
| ------------- | ------------- | ------------- |------------- |
| CourseId  | `String` | The unique identifier of the course. | YES |
| CourseName  | `String` | The name of the course. | YES |
| Skills  | `List<string>` | A list of skills you are looking for. | YES |
| Occupations  | `List<string>` | A list of occupations you are looking for. | YES |
| QueryParameters  | `Map<key,value>` | Parameters to query for courses | YES |
| Language  | `String` | The language of the course. | YES |

### Grpc Message: CourseResponse

NOTE: Client specific message. Indication for Streaming done in the service definition.

|    Property  |   Data Type   |  Description  | OPTIONAL  |
| ------------- | ------------- | ------------- |------------- |
| Courses  | `List<Course>` | A list of courses. Can be empty. | NO |