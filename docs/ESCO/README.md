ESCO
---

[ESCO (European Skills, Competences, Qualifications and Occupations)](https://ec.europa.eu/) is the European multilingual classification of Skills, Competences and Occupations.  

ESCO works as a dictionary, describing, identifying and classifying professional occupations and skills relevant for the EU labour market and education and training.  Those concepts and the relationships between them can be understood by electronic systems, which allows different online platforms to use ESCO for services like matching jobseekers to jobs on the basis of their skills, suggesting trainings to people who want to reskill or upskill etc.

ESCO provides descriptions of 2942 occupations and 13.485 skills linked to these occupations, translated into 27 languages (all official EU languages plus Icelandic, Norwegian and Arabic). 

TL;DR
-
- ESCO connects the labour market to education and training systems
- Matches people to jobs
- ESCO supports the analysis of labour market data on skills and occupations
- Realtime Data from the [ILO - International Labour Organization](https://www.ilo.org/wcmsp5/groups/public/@dgreports/@dcomm/documents/briefingnote/wcms_740877.pdf) 

- 2942 Occupations
- 13,485 Skills
- 27 Languages
- 514k Skill terms
- 350k Occupational terms
- 500 Transversal skills
- Linked Open Data

Abstract
-
The European Commission, in order to respond to the disruptive changes of the labour market and the current and future skills gaps, is helping people to discover reskilling and upskilling pathways. The European classification of Skills, Competences,Qualifications and Occupations (ESCO) is one concrete implementation of the digital labour market policies put in place by the Commission at European level.

ESCO is meant to be a reference language for employment and education, to create a shared understanding about skills, learning and occupations across borders and languages. It helps to connect people with jobs, education with employment and to analyse information on skills demand.

Through ESCO, the Commission focuses on:

Ensuring transparency and comparability of skills and occupations in Europe,
Strengthening mobility within the EU,
Bridging the gap between education and training systems and the labour market, and
Enhancing the cooperation of Public Employment Services (PES).

Working with ESCO
---

The ESCO data model

In general terms, the data model is structured on the basis of three pillars, represnting a searchable databse in 26 languages.

The pillars are:

- Occupations
    a grouping of jobs involving similar content in terms of tasks, and requiring similar types of skills as defined in the european labour market information database. 
- Knowledge/skills/competences
    - Knowledge
        The body of facts, principles, theories and practices that is related to a field of work or study. Knowledge is described as theorectival and/or factual, and is the outcome of the assimilation of infromation through learning.
    - Skill
        The ability to apply knowledge and use know-how to complete tasks and solve problems. Skills are described as cognitive (involving the use of logical, intuitive and creative thinking) or practical (involving manual dexterity and the use of mehtods, materials, tools and instruments).
        There are around 13492 skill concepts which contain  metadata including the following fields:
        - preferred term
        - Non-prefered terms
        - Hidden terms
        - An explenation of the concept in the form of a description
        - A scope note
        - The skill type ( in the skills pillar, ESCO distinguishes between skill/competence concepts and knowledge concepts)
        - A formal definition
        - The relationship between occupations and skills (i.e. the occupations for which the knowledge, skill or competence is essential and the occupations for which the knowledge, skill or competence is optional)
        - The relationship between knowledge, skills and competences, and other knowledge, skills and competences. This realtionship is categorised as essential or optional.
        - The reusability level, which indicates how widley a knowledge, skill or competenc concept can be applied. 
    - Competence
        The proven ability to use knowledge, skills and personal, social and/or methodological abilities in work or study situations, and in professional and personal development.
    Example: Working as a "civil airline pilot" requires the competence to combine knowledge about "emergency procedures" and "equipment malfunctions" with skills relating to "reading position coordinates" and "following the flight route".
- qualifications

these are interlined in order to show the relations between them, while occupational profiles show wheter skills and competences are essential or optional and what qualifications are relevant for each ESCO occupation. 

ESCO is using [TURTLE](https://www.w3.org/TR/turtle/#bib-N-TRIPLES) to store its data. 


ESCO is using the [Linked Open Data](https://www.linked-open-data.org/) approach to share data on occupations and skills. The data is available on [Open Data Hub](https://opendatahub.ec.europa.eu/). The datastructure is based on the [ESCO schema](https://ec.europa.eu/skills/esco/esco-schema.json). The data is available in: [JSON](https://opendatahub.ec.europa.eu/dataset/ESCO), [CSV](https://opendatahub.ec.europa.eu/dataset/ESCO/download/csv), [RDF](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdf), [XML](https://opendatahub.ec.europa.eu/dataset/ESCO/download/xml), [Turtle](https://opendatahub.ec.europa.eu/dataset/ESCO/download/ttl), [N-Triples](https://opendatahub.ec.europa.eu/dataset/ESCO/download/nt), [N-Quads](https://opendatahub.ec.europa.eu/dataset/ESCO/download/nq), [RDF/XML](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfxml), [RDF/JSON](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfjson), [RDF/Turtle](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfttl), [RDF/N-Triples](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfnt), [RDF/N-Quads](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfnq), [RDF/XML-ABBREV](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfxmlabbrev), [RDF/JSON-LD](https://opendatahub.ec.europa.eu/dataset/ESCO/download/rdfjsonld). 
The native implementation of ESCO is using the [Turtle](https://www.w3.org/TR/turtle/) (Terse RDF Triple Language) W3C standard as data representation. 

This will allow us to build a graph of occupations and skills, and to use it to match people to jobs and to suggest trainings.





