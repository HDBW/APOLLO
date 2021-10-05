//Import ESCO using CSV files
create index ON :Occupation(ISCOGroup);
create index ON :Occupation(altLabels);
create index ON :Skill(altLabels);
create index ON :ISCOGroup(code);
create index ON :Skill(conceptUri);
create index ON :ISCOGroup(conceptUri);
create index ON :Occupation(conceptUri);
create index ON :Occupation(preferredLabel);
create index ON :Skill(preferredLabel);

//import skills and skillgroups
//skillgroups are also skills
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/skillGroups_de.csv?sv=2020-04-08&st=2021-10-05T06%3A49%3A16Z&se=2024-10-06T06%3A49%3A00Z&sr=b&sp=r&sig=37%2BbH8IYEgZaS9joG4SuawhoB9FwnXKWRXBPH%2Fe0fiQ%3D" as row
create (s:Skill:Skillgroup)
set s = row;
//skills
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/skills_de.csv?sv=2020-04-08&st=2021-10-05T06%3A50%3A23Z&se=2024-10-06T06%3A50%3A00Z&sr=b&sp=r&sig=GBN8zazfP%2Bf42YobxwBjpgN0PkZ%2FlKQTIRYLC2ZZzGg%3D" as row
create (s:Skill)
set s = row;
//add the BROADER_THAN relationship between different skills
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/broaderRelationsSkillPillar.csv?sv=2020-04-08&st=2021-10-05T06%3A51%3A35Z&se=2024-10-06T06%3A51%3A00Z&sr=b&sp=r&sig=CJZ%2BYYeARRYRfi6%2BPnsXZhSNH5gePWsB1B78687HtsA%3D" as row
match (smaller:Skill {conceptUri: row.conceptUri}), (broader:Skill {conceptUri: row.broaderUri})
create (broader)-[:BROADER_THAN]->(smaller);

//import occupations
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/occupations_de.csv?sv=2020-04-08&st=2021-10-05T06%3A52%3A18Z&se=2024-10-06T06%3A52%3A00Z&sr=b&sp=r&sig=1BJBtmLGdHOSCIp9WNsMNiL2kWhJc%2FmAJJYAw3ZJxls%3D" as row
create (o:Occupation)
set o = row;

//import the International Standard Classification for Occupations of the ILO
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/ISCOGroups_de.csv?sv=2020-04-08&st=2021-10-05T06%3A53%3A00Z&se=2024-10-06T06%3A53%3A00Z&sr=b&sp=r&sig=J8YhyBxJQqTtB7cxJtOOL3x3iPUZV0hOB3nNEiMIrTE%3D" as row
create (isco:ISCOGroup)
set isco = row;
//impirt the BROADER_THAN relationships between ISCO groups
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/broaderRelationsOccPillar.csv?sv=2020-04-08&st=2021-10-05T06%3A53%3A56Z&se=2024-10-06T06%3A53%3A00Z&sr=b&sp=r&sig=dCyEEFDvK1yDbWWedTIh8FcJR0VJkee5x60Lc8crYlA%3D" as row
match (smaller:ISCOGroup {conceptUri: row.conceptUri}), (broader:ISCOGroup {conceptUri: row.broaderUri})
create (broader)-[:BROADER_THAN]->(smaller);
//connect the occupations to their ISCOGroup
match (isco:ISCOGroup), (o:Occupation)
where isco.code = o.iscoGroup
create (o)-[:PART_OF_ISCOGROUP]->(isco);

//Connect Skills to Occupations
load csv with headers from "https://pixiclients.blob.core.windows.net/clients/hdbw/occupationSkillRelations.csv?sv=2020-04-08&st=2021-10-05T06%3A54%3A35Z&se=2024-10-06T06%3A54%3A00Z&sr=b&sp=r&sig=kg4Hk2T8fHMUBqmtp0C%2BV5zJ%2BN5L%2B8P%2FNFDUWaMH80s%3D" as row
match (s:Skill {conceptUri: row.skillUri}), (o:Occupation {conceptUri: row.occupationUri})
CREATE (s)-[:RELATED_TO {type: row.relationType}]->(o);

// match ()-[r:RELATED_TO]->()
// return distinct r.type;

//differentiate the different types of relations between occupations and skills
match (a)-[r:RELATED_TO]->(b)
where r.type = "essential"
create (a)-[:ESSENTIAL_FOR]->(b);
match (a)-[r:RELATED_TO]->(b)
where r.type = "optional"
create (a)-[:OPTIONAL_FOR]->(b);
//remove the old relationships
match (a)-[r:RELATED_TO]->(b)
delete r;