# Piktiv Workshop T1 - part 2

TodoApp is a simple application that stores your own personal TODO list, using Postgres and Redis for storage.

Your mission to create a docker compose that sets up a working environment of these three applications. \
The data in redis and postgresql should also survive a restart of their container, so any todos created should still be alive after a restart.

TodoApp requires the following environment variables: \
REDIS_URL - The URL to Redis (eg. redis or whatever you specify in the docker file) \
DATABASE_HOST - The URL to Postgres (eg. postgres or whatever you specify in the docker file) \
DATABASE_USER - The Postgres user (Postgres has a default user called postgres that you can use) \
DATABASE_PASSWORD - Whatever password you specified when setting up the postgres docker config 

## C# hints
A C# application can be built by using: \
dotnet publish -o {PathToOutput} \
Replace {PathToOutput} with wherever you want to build to and then run your application from that folder
