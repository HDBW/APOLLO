How to guide importing ESCO with neo4j
---

Make sure you have docker on your machine and docker is up and running.

Install docker for Windows Install Docker Desktop on Windows | Docker Documentation
Install docker for WSL2 Docker Desktop WSL 2 backend | Docker Documentation

From the command line run (WSL2 Ubuntu 20.04)  

docker run \
    --name testneo4j \
    -p7474:7474 -p7687:7687 \
    -d \
    -v $HOME/neo4j/data:/data \
    -v $HOME/neo4j/logs:/logs \
    -v $HOME/neo4j/import:/var/lib/neo4j/import \
    -v $HOME/neo4j/plugins:/plugins \
    --env NEO4J_AUTH=neo4j/test \
    neo4j:latest

default user and password are neo4j


Before you get started you should download the corresponding files from or you use the blobstorage url of azure.

[https://ec.europa.eu/esco/portal/download/](https://ec.europa.eu/esco/portal/download/)

Here is a combination for version 1.03 note current version is 1.08 and draft 1.09 exists.
ESCO - [Download - European Commission (europa.eu)](https://ec.europa.eu/esco/portal/download/e00,e01,e02,e03,e1,ea,et,e12,e1l,e1u,e2b,e2k,e33,e34,e35,j1,ja,jt,j12,j1l,j1u,j2b,j2k,j33,j34,j35)

In neo4j use the [import.cql](https://github.com/HDBW/APOLLO/blob/main/docs/ESCO/importn4j/import.cql) or german [import_de.cql](https://github.com/HDBW/APOLLO/blob/main/docs/ESCO/importn4j/import_de.cql) script and change the url to your csv files. 

