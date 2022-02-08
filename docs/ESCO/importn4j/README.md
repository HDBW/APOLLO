How to guide importing ESCO with neo4j
---

Make sure you have docker on your machine and docker is up and running.

Install docker for Windows Install Docker Desktop on Windows | Docker Documentation
Install docker for WSL2 Docker Desktop WSL 2 backend | Docker Documentation

From the command line run (WSL2 Ubuntu 20.04)  

## !!! UPDATED !!!:

```
docker run --publish=7474:7474 --publish=7687:7687 --volume=$HOME/neo4j/data:/data --env='NEO4JLABS_PLUGINS=["apoc", "n10s"]' --env=NEO4J_AUTH=neo4j/neo4j neo4j:latest
```

default user and password are neo4j

We ran into issue:

> "KO"	0	0	null	"The following constraint is required for importing RDF. Please run 'CREATE CONSTRAINT n10s_unique_uri ON (r:Resource) ASSERT r.uri IS UNIQUE' and try again."	null

please execute the following commands first neo4j

```
CALL n10s.graphconfig.init();
```

```
CREATE CONSTRAINT n10s_unique_uri ON (r:Resource)
ASSERT r.uri IS UNIQUE;
```

```
CALL n10s.rdf.import.fetch("https://pixiclients.blob.core.windows.net/clients/esco110/esco_v1.1.0.ttl?sv=2020-10-02&st=2022-02-08T11%3A28%3A49Z&se=2023-12-09T11%3A28%3A00Z&sr=b&sp=r&sig=MhFo4eZhQ%2FImhM4yv1BuqZva8MjYV0PxFCg7eJRgABU%3D","Turtle");
```



